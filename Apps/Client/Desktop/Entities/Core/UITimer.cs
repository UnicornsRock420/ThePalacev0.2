using ITimer = ThePalace.Common.Interfaces.Threading.ITimer;
using Timer = System.Windows.Forms.Timer;

namespace ThePalace.Client.Desktop.Entities.Core;

public class UITimer : Timer, ITimer
{
}