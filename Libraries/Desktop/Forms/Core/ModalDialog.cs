﻿using ThePalace.Common.Desktop.Interfaces;

namespace ThePalace.Common.Desktop.Forms.Core;

public class ModalDialog<T> : FormBase, IFormResult<T>
{
    public ModalDialog() { }

    public T Result { get; }
}