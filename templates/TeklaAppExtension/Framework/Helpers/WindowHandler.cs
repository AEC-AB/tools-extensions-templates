using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeklaAppExtension.Framework.Helpers;

public static class WindowHandler
{
    public static void ShowWindow<TWindow>(global::System.IServiceProvider serviceProvider, global::System.IntPtr ownerHandle) where TWindow : global::System.Windows.Window
    {
        global::System.Threading.Thread thread = new(() =>
        {
            try
            {
                var window = global::Microsoft.Extensions.DependencyInjection.ServiceProviderServiceExtensions.GetRequiredService<TWindow>(serviceProvider);
                var dispatcher = window.Dispatcher;

                dispatcher.UnhandledException += (sender, e) =>
                {
                    e.Handled = true;
                    window.Close();
                    var message = $"An error occurred while starting the application\n{e.Exception.Message}\n\n{e.Exception.StackTrace}";
                    global::System.Threading.Tasks.Task.Run(() => global::System.Windows.MessageBox.Show(message));
                };
                window.Closed += (sender, e) => { global::System.Windows.Threading.Dispatcher.ExitAllFrames(); };
                window.Show();
                using var keepOnTop = NativeMethods.KeepOnTop(window, ownerHandle);

                global::System.Windows.Threading.Dispatcher.Run();
            }
            catch (global::System.Exception e)
            {
                global::System.Windows.MessageBox.Show($"An error occurred while starting the application\n{e.Message}\n\n{e.StackTrace}");
            }
        });

        thread.Name = global::System.Guid.NewGuid().ToString();
        thread.SetApartmentState(global::System.Threading.ApartmentState.STA);
        thread.Start();
    }


    private class NativeMethods : global::System.IDisposable
    {
        private readonly global::System.IntPtr _teklaMainWindowHandle;
        private readonly global::System.IntPtr _thisHandle;
        private global::System.IntPtr? _lastForegroundWindow;

        public static global::System.IDisposable KeepOnTop(global::System.Windows.Window window, global::System.IntPtr teklaMainWindowHandle)
        {
            var hwndSource = global::System.Windows.PresentationSource.FromVisual(window) as global::System.Windows.Interop.HwndSource;
            if (hwndSource is null)
                throw new global::System.InvalidOperationException("Window handle is null");

            var nativeMethods = new NativeMethods(teklaMainWindowHandle, hwndSource.Handle);
            return nativeMethods;
        }

        public NativeMethods(global::System.IntPtr teklaMainWindowHandle, global::System.IntPtr windowHandle)
        {
            _teklaMainWindowHandle = teklaMainWindowHandle;
            _thisHandle = windowHandle;
            SetAboveWindow();
            global::System.Windows.Media.CompositionTarget.Rendering += (s, e) => SetAboveWindow();
        }

        [global::System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(global::System.IntPtr hWnd);

        [global::System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern global::System.IntPtr GetForegroundWindow();

        private void SetAboveWindow()
        {
            var foregroundWindow = GetForegroundWindow();

            if (_lastForegroundWindow != _thisHandle && _lastForegroundWindow != _teklaMainWindowHandle && foregroundWindow == _teklaMainWindowHandle)
            {
                SetForegroundWindow(_thisHandle);
            }
            _lastForegroundWindow = foregroundWindow;

            if (foregroundWindow == _teklaMainWindowHandle)
            {
                SetWindowPos();
            }
        }

        private void SetWindowPos()
        {
            // Set our window (hwndTarget) to be positioned in front of hWnd
            uint SWP_NOACTIVATE = 0x0010;
            uint SWP_NOMOVE = 0x0002;
            uint SWP_NOSIZE = 0x0001;
            //SetWindowPos(hWndInsertAfter, hWnd, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
            SetWindowPos(_teklaMainWindowHandle, _thisHandle, 0, 0, 0, 0, SWP_NOACTIVATE | SWP_NOMOVE | SWP_NOSIZE);
        }

        [global::System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SetWindowPos(global::System.IntPtr hWnd, global::System.IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        public void Dispose()
        {
            global::System.Windows.Media.CompositionTarget.Rendering -= (s, e) => SetAboveWindow();
            global::System.GC.SuppressFinalize(this);
        }
    }
}
