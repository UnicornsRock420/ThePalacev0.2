using System.Collections.Concurrent;
using Lib.Common.Factories.Core;

namespace Lib.Common.Entities.Network;

/// <summary>
///     This stream maintains data only until the data is read, then it is purged from the stream.
/// </summary>
public class BufferStream : Stream
{
    //Maintains the streams' data. The Queue object provides an easy and efficient way to add and remove data
    //Each item in the queue represents each write to the stream. Every call to write translates to an item in the queue
    private readonly ConcurrentQueue<Chunk> _chunks;

    public BufferStream()
    {
        _chunks = new ConcurrentQueue<Chunk>();
    }

    public override bool CanSeek => CanSeekOveride;
    public bool CanSeekOveride { get; set; } = false;

    /// <summary>
    ///     Always returns 0
    /// </summary>
    public override long Position
    {
        //We're always at the start of the stream, because as the stream purges what we've read
        get => 0;
        set
        {
            if (!CanSeek) throw new NotSupportedException($"{GetType().Name} is not seekable");

            _read(null, 0, (int)value);
        }
    }

    public override bool CanWrite => true;

    public override bool CanRead => true;

    public long Count => _chunks?.Count ?? 0;

    public override long Length
    {
        get
        {
            using (var @lock = LockContext.GetLock(_chunks))
            {
                return (_chunks?.Count ?? 0) < 1 ? 0 : _chunks.Sum(b => b.Length - b.Position);
            }
        }
    }

    public byte[] Dequeue()
    {
        if ((_chunks?.Count ?? 0) < 1) return [];

        var chunk = (Chunk?)null;

        using (var @lock = LockContext.GetLock(_chunks))
        {
            _chunks.TryDequeue(out chunk);
        }

        if (chunk == null) return [];

        if (chunk.Position == 0) return chunk.Data;

        var count = chunk.Length - chunk.Position;
        var result = new byte[count];
        Buffer.BlockCopy(chunk.Data, chunk.Position, result, 0, count);

        return result;
    }

    /// <summary>
    ///     Reads up to count bytes from the stream, and removes the read data from the stream.
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    private int _read(byte[] buffer, int offset, int count)
    {
        var iRemainingBytesToRead = count;
        var iTotalBytesRead = 0;

        //Read until we hit the requested count, or until we have nothing left to read
        while (iTotalBytesRead <= count &&
               _chunks.Count > 0)
        {
            var chunk = (Chunk?)null;

            //Get first chunk from the queue
            using (var @lock = LockContext.GetLock(_chunks))
            {
                _chunks.TryPeek(out chunk);
            }

            //Determine how much of the chunk there is left to read
            var iUnreadChunkLength = chunk.Length - chunk.Position;

            //Determine how much of the unread part of the chunk we can actually read
            var iBytesToRead = Math.Min(iUnreadChunkLength, iRemainingBytesToRead);

            if (iBytesToRead > 0)
            {
                if ((buffer?.Length ?? 0) > 0)
                    //Read from the chunk into the buffer
                    Buffer.BlockCopy(chunk.Data, chunk.Position, buffer, offset + iTotalBytesRead, iBytesToRead);

                iTotalBytesRead += iBytesToRead;
                iRemainingBytesToRead -= iBytesToRead;

                //If the entire chunk has been read,  remove it
                if (chunk.Position + iBytesToRead >= chunk.Data.Length)
                    using (var @lock = LockContext.GetLock(_chunks))
                    {
                        _chunks.TryDequeue(out _);
                    }
                else
                    //Otherwise just update the chunk read start index, so we know where to start reading on the next call
                    chunk.Position += iBytesToRead;
            }
            else
            {
                break;
            }
        }

        return iTotalBytesRead;
    }

    /// <summary>
    ///     Reads up to count bytes from the stream, and removes the read data from the stream.
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public override int Read(byte[] buffer, int offset, int count)
    {
        ValidateBufferArgs(buffer, offset, count);

        return _read(buffer, offset, count);
    }

    private void ValidateBufferArgs(byte[]? buffer, int offset, int count)
    {
        if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "offset must be non-negative");
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "count must be non-negative");
        if ((buffer?.Length ?? 0) - offset < count)
            throw new ArgumentException("requested count exceeds available size");
    }

    /// <summary>
    ///     Writes data to the stream
    /// </summary>
    /// <param name="srcBuffer">Data to copy into the stream</param>
    /// <param name="offset"></param>
    /// <param name="count"></param>
    public override void Write(byte[] srcBuffer, int offset, int count)
    {
        ValidateBufferArgs(srcBuffer, offset, count);

        //We don't want to use the buffer passed in, as it could be altered by the caller
        var destBuffer = new byte[count];
        Buffer.BlockCopy(srcBuffer, offset, destBuffer, 0, count);

        //Add the data to the queue
        using (var @lock = LockContext.GetLock(_chunks))
        {
            _chunks.Enqueue(new Chunk(destBuffer));
        }
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        if (!CanSeek) throw new NotSupportedException($"{GetType().Name} is not seekable");

        return origin switch
        {
            SeekOrigin.Begin or SeekOrigin.Current => _read(null, 0, (int)offset),
            SeekOrigin.End => throw new NotSupportedException($"{GetType().Name} is not seekable"),
            _ => 0
        };
    }

    public override void SetLength(long value)
    {
        if (!CanSeek) throw new NotSupportedException($"{GetType().Name} length can not be changed");
        if (value < 1) return;

        using (var @lock = LockContext.GetLock(_chunks))
        {
            _chunks.TryDequeue(out var chunk);
            _chunks.Clear();

            chunk.SetLength(value);

            _chunks.Enqueue(chunk);
        }
    }

    public void Clear()
    {
        using (var @lock = LockContext.GetLock(_chunks))
        {
            _chunks.Clear();
        }
    }

    public override void Flush()
    {
        using (var @lock = LockContext.GetLock(_chunks))
        {
            _chunks.Clear();
        }
    }

    /// <summary>
    ///     Represents a single write into the BufferStream. Each write is a separate chunk
    /// </summary>
    private class Chunk : MemoryStream
    {
        public Chunk(byte[] buffer, int position = 0, int length = 0) : base(buffer, position, length > 0 ? length : buffer.Length, false)
        {
        }

        public new int Length
        {
            get => (int)base.Length;
            set => base.SetLength(value);
        }

        public new int Position
        {
            get => (int)base.Position;
            set => base.Position = value;
        }

        /// <summary>
        ///     Actual Data
        /// </summary>
        public byte[] Data => ToArray();
    }
}