﻿using System.Collections;

namespace Lib.Core.Factories.IO;

public abstract class StreamBase : Disposable
{
    public override void Dispose()
    {
        Close();

        base.Dispose();
        
        GC.SuppressFinalize(this);
    }

    ~StreamBase()
    {
        Dispose();
    }

    protected FileStream _fileStream;
    protected string _pathToFile;

    public bool Open(string pathToFile, bool write = false)
    {
        _pathToFile = pathToFile;

        Close();

        if (write)
        {
            if (File.Exists(_pathToFile))
                _fileStream = new FileStream(_pathToFile, FileMode.Truncate, FileAccess.Write);
            else
                _fileStream = new FileStream(_pathToFile, FileMode.OpenOrCreate, FileAccess.Write);
        }
        else
        {
            if (!File.Exists(_pathToFile)) return false;

            _fileStream = File.Open(_pathToFile, FileMode.Open, FileAccess.Read);
        }

        return true;
    }

    public void Close()
    {
        _fileStream?.Dispose();
        _fileStream = null;
    }
}