using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;

namespace CheckInSystem.Background_tasks
{
    public static class FullScreenHelpers
    {
        public static void ToggleFullScreenAvalonia(Window? win = null)
        {
            win ??= GetActiveWindow();
            if (win is null) return;

            bool isFull = win.WindowState == WindowState.FullScreen;

            if (isFull)
            {
                win.WindowState = WindowState.Normal;
                win.SystemDecorations = SystemDecorations.Full;
                win.Topmost = false;
            }
            else
            {
                win.WindowState = WindowState.FullScreen;
                win.SystemDecorations = SystemDecorations.None;
                win.Topmost = true;
            }
        }

        private static Window? GetActiveWindow()
        {
            return Avalonia.Application.Current?.ApplicationLifetime switch
            {
                IClassicDesktopStyleApplicationLifetime desktop => desktop.Windows.FirstOrDefault(w => w.IsActive),
                _ => null
            };
        }
    }
}
