using Threading_ITimer = Lib.Common.Interfaces.Threading.ITimer;
using Timer = System.Windows.Forms.Timer;

namespace ThePalace.Client.Desktop.Entities.Core;

public class UITimer : Timer, Threading_ITimer
{
}