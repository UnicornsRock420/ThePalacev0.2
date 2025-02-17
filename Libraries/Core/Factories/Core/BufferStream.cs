namespace ThePalace.Core.Factories.Core
{
    /// <summary>
    /// This stream maintains data only until the data is read, then it is purged from the stream.
    /// </summary>
    public class BufferStream : Stream
    {
        /// <summary>
        /// Represents a single write into the BufferStream. Each write is a seperate chunk
        /// </summary>
        private class Chunk(byte[]? data = null, int position = 0)
        {
            /// <summary>
            /// As we read through the chunk, the start index will increment.When we get to the end of the chunk,
            /// we will remove the chunk
            /// </summary>
            public int Position { get; set; } = position;

            /// <summary>
            /// Actual Data
            /// </summary>
            public byte[] Data { get; } = data;

            public int Length => Data?.Length ?? 0;
        }

        //Maintains the streams data.The Queue object provides an easy and efficient way to add and remove data
        //Each item in the queue represents each write to the stream.Every call to write translates to an item in the queue
        private Queue<Chunk> _chunks;

        public BufferStream() => this._chunks = new();

        /// <summary>
        /// Reads up to count bytes from the stream, and removes the read data from the stream.
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            this.ValidateBufferArgs(buffer, offset, count);

            var iRemainingBytesToRead = count;
            var iTotalBytesRead = 0;

            //Read until we hit the requested count, or until we hav nothing left to read
            while (iTotalBytesRead <= count &&
                this._chunks.Count > 0)
            {
                //Get first chunk from the queue
                var chunk = this._chunks.Peek();

                //Determine how much of the chunk there is left to read
                var iUnreadChunkLength = chunk.Length - chunk.Position;

                //Determine how much of the unread part of the chunk we can actually read
                var iBytesToRead = Math.Min(iUnreadChunkLength, iRemainingBytesToRead);

                if (iBytesToRead > 0)
                {
                    //Read from the chunk into the buffer
                    Buffer.BlockCopy(chunk.Data, chunk.Position, buffer, offset + iTotalBytesRead, iBytesToRead);

                    iTotalBytesRead += iBytesToRead;
                    iRemainingBytesToRead -= iBytesToRead;

                    //If the entire chunk has been read,  remove it
                    if (chunk.Position + iBytesToRead >= chunk.Data.Length)
                    {
                        this._chunks.Dequeue();
                    }
                    else
                    {
                        //Otherwise just update the chunk read start index, so we know where to start reading on the next call
                        chunk.Position += iBytesToRead;
                    }
                }
                else break;
            }

            return iTotalBytesRead;
        }

        private void ValidateBufferArgs(byte[]? buffer, int offset, int count)
        {
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "offset must be non-negative");
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "count must be non-negative");
            if (((buffer?.Length ?? 0) - offset) < count) throw new ArgumentException("requested count exceeds available size");
        }

        /// <summary>
        /// Writes data to the stream
        /// </summary>
        /// <param name="buffer">Data to copy into the stream</param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            this.ValidateBufferArgs(buffer, offset, count);

            //We don't want to use the buffer passed in, as it could be altered by the caller
            var bufSave = new byte[count];
            Buffer.BlockCopy(buffer, offset, bufSave, 0, count);

            //Add the data to the queue
            this._chunks.Enqueue(new Chunk(bufSave));
        }

        public override bool CanSeek => false;
        public bool CanSeekOveride { get; set; } = false;

        /// <summary>
        /// Always returns 0
        /// </summary>
        public override long Position
        {
            //We're always at the start of the stream, because as the stream purges what we've read
            get => 0;
            set
            {
                if (!CanSeekOveride) throw new NotSupportedException(string.Format("{0} is not seekable", this.GetType().Name));

                var iRemainingBytesToRead = (int)value;
                var iTotalBytesRead = 0;

                //Read until we hit the requested count, or until we hav nothing left to read
                while (iTotalBytesRead <= value &&
                    this._chunks.Count > 0)
                {
                    //Get first chunk from the queue
                    var chunk = this._chunks.Peek();

                    //Determine how much of the chunk there is left to read
                    var iUnreadChunkLength = chunk.Length - chunk.Position;

                    //Determine how much of the unread part of the chunk we can actually read
                    var iBytesToRead = Math.Min(iUnreadChunkLength, iRemainingBytesToRead);

                    if (iBytesToRead > 0)
                    {
                        iTotalBytesRead += iBytesToRead;
                        iRemainingBytesToRead -= iBytesToRead;

                        //If the entire chunk has been read,  remove it
                        if (chunk.Position + iBytesToRead >= chunk.Data.Length)
                        {
                            this._chunks.Dequeue();
                        }
                        else
                        {
                            //Otherwise just update the chunk read start index, so we know where to start reading on the next call
                            chunk.Position += iBytesToRead;
                        }
                    }
                    else break;
                }
            }
        }

        public override bool CanWrite => true;

        public override long Seek(long offset, SeekOrigin origin) => this.Position = offset;

        public override void SetLength(long value) =>
            throw new NotSupportedException(string.Format("{0} length can not be changed", this.GetType().Name));

        public override bool CanRead => true;

        public override long Length =>
            (this._chunks?.Count ?? 0) < 1 ? 0 : this._chunks.Sum(b => b.Length - b.Position);

        public override void Flush() { }
    }
}