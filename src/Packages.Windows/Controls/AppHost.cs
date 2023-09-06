#region

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Threading;
using Vanara.PInvoke;

#endregion

namespace Packages.Windows.Controls
{
    [TemplatePart(Name = PART_ROOT_NAME, Type = typeof(Border))]
    public class AppHost : Control
    {
        private const string PART_ROOT_NAME = "PART_Root";

        public static readonly DependencyProperty AppPathProperty = DependencyProperty.Register(
            nameof(AppPath), typeof(string), typeof(AppHost),
            new PropertyMetadata(default(string), OnAppPathPropertyChanged));

        private HWND _appHwnd;
        private DispatcherTimer _dispatcherTimer;
        private IntPtr _hWnd;
        private bool _isAppLoaded;

        private Process _process;
        private Window _window;

        static AppHost()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AppHost), new FrameworkPropertyMetadata(typeof(AppHost)));
        }

        public string AppPath
        {
            get => (string)GetValue(AppPathProperty);
            set => SetValue(AppPathProperty, value);
        }

        private static void OnAppPathPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is AppHost host)
            {
                if (e.OldValue is string oldPath && File.Exists(oldPath))
                {
                    host.Loaded -= host.Host_Loaded;
                    host.DetachApp();
                }

                if (e.NewValue is string newPath && File.Exists(newPath))
                {
                    if (host.IsLoaded)
                    {
                        host.AttachApp();
                    }
                    else
                    {
                        host.Loaded += host.Host_Loaded;
                    }
                }
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var rootPart = (Border)GetTemplateChild(PART_ROOT_NAME);
            _hWnd = ((HwndSource)PresentationSource.FromVisual(rootPart))?.Handle ?? IntPtr.Zero;
            _window = this.GetParentUtil<Window>();
            _window.Deactivated += Window_Deactivated;
            _window.Activated += Window_Activated;
            SizeChanged += AppHost_SizeChanged;
            _window.Closing += Window_Closing;
            Unloaded += AppHost_Unloaded;
        }

        private void AppHost_Unloaded(object sender, RoutedEventArgs e)
        {
            DetachApp();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            DetachApp();
        }

        private void AppHost_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizeApp();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            ActivateAppWindow();
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            DeactivateAppWindow();
        }

        private void DeactivateAppWindow()
        {
            User32.SendMessage(_appHwnd, User32.WindowMessage.WM_ACTIVATE, new IntPtr(0), IntPtr.Zero);
        }

        private void AttachApp()
        {
            if (string.IsNullOrWhiteSpace(AppPath) || !File.Exists(AppPath))
            {
                return;
            }

            _process = new Process();
            _process.StartInfo.FileName = AppPath;
            _process.StartInfo.Arguments = $"-parentHWND {_hWnd.ToInt32()} {Environment.NewLine}";
            _process.StartInfo.UseShellExecute = true;
            _process.StartInfo.CreateNoWindow = true;
            _process.Start();
            _process.WaitForInputIdle();
            _isAppLoaded = true;
            User32.EnumChildWindows(_hWnd, WindowEnum, IntPtr.Zero);
            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(200);
            _dispatcherTimer.Tick += InitialResize;
            _dispatcherTimer.Start();
        }

        private void InitialResize(object sender, EventArgs e)
        {
            ResizeApp();
            _dispatcherTimer.Stop();
        }

        private void ResizeApp()
        {
            if (_isAppLoaded)
            {
                var appLeftUpPos = TransformToAncestor(_window).Transform(new Point(0, 0));
                DpiUtils.Init(_window);
                appLeftUpPos.X *= DpiUtils.DpiX;
                appLeftUpPos.Y *= DpiUtils.DpiY;
                User32.MoveWindow(_appHwnd, (int)appLeftUpPos.X, (int)appLeftUpPos.Y,
                    (int)(ActualWidth * DpiUtils.DpiX), (int)(ActualHeight * DpiUtils.DpiY), true);
                ActivateAppWindow();
            }
        }

        private bool WindowEnum(HWND hwnd, IntPtr lparam)
        {
            _appHwnd = hwnd;
            ActivateAppWindow();
            return false;
        }

        private void ActivateAppWindow()
        {
            User32.SendMessage(_appHwnd, User32.WindowMessage.WM_ACTIVATE, new IntPtr(1), IntPtr.Zero);
        }

        private void DetachApp()
        {
            if (_process != null && !_process.HasExited)
            {
                _process.CloseMainWindow();
                while (!_process.HasExited)
                {
                    _process.Kill();
                }
            }
        }

        private void Host_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= Host_Loaded;
            AttachApp();
        }
    }
}