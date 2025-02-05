using System;

namespace ThePalace.Client.Desktop.Entities
{
    public sealed class ApiBinding
    {
        public string Name { get; set; } = null;
        public EventHandler Binding { get; set; } = null;
    }
}
