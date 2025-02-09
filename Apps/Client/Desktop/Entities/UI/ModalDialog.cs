using ThePalace.Common.Desktop.Interfaces;

namespace ThePalace.Client.Desktop.Entities
{
    public class ModalDialog<T> : FormBase, IFormResult<T>
    {
        public ModalDialog() { }

        public T Result { get; }
    }
}