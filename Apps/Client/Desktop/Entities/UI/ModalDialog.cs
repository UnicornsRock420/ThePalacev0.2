using ThePalace.Client.Desktop.Entities.UI;
using ThePalace.Common.Desktop.Interfaces;

namespace ThePalace.Client.Desktop.Entities.Threads.UI
{
    public class ModalDialog<T> : FormBase, IFormResult<T>
    {
        public ModalDialog() { }

        public T Result { get; }
    }
}