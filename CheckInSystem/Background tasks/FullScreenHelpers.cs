using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Text;
using System.Threading.Tasks;

namespace CheckInSystem.Background_tasks
{
    class FullScreenHelpers
    {
        public static void ToggleFullScreenWpf(Window? win = null)
        {
            win ??= Application.Current?.Windows
                                      .OfType<Window>()
                                      .FirstOrDefault(w => w.IsActive);
            if (win is null) return;

            bool isFull = win.WindowStyle == WindowStyle.None &&
                          win.WindowState == WindowState.Maximized;

            if (isFull)
            {
                win.WindowStyle = WindowStyle.SingleBorderWindow;
                win.WindowState = WindowState.Normal;
                win.Topmost = false;
            }
            else
            {
                win.WindowStyle = WindowStyle.None;
                win.WindowState = WindowState.Maximized;
                win.Topmost = true;   // keep task-bar under the window
            }
        }
    }
}
