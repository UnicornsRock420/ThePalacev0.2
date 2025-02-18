namespace ThePalace.Common.Desktop.Entities.Core
{
    public sealed class ApiBinding
    {
        public string Name { get; set; } = null;
        public EventHandler Binding { get; set; } = null;
    }
}