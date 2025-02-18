namespace ThePalace.Common.Desktop.Entities.Core
{
    public partial class ApiBinding
    {
        public string Name { get; set; } = null;
        public EventHandler Binding { get; set; } = null;
    }
}