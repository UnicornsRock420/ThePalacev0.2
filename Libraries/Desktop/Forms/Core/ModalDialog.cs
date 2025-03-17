﻿using Lib.Common.Desktop.Interfaces;

namespace Lib.Common.Desktop.Forms.Core;

public class ModalDialog<T> : FormBase, IFormResult<T>
{
    public T Result { get; }
}