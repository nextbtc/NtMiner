﻿using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace NTMiner.Views {
    internal class SafeNativeMethods {
        internal const int GWL_STYLE = -16;
        internal const int WS_VISIBLE = 0x10000000;
        [DllImport("user32.dll")]
        internal static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, ExactSpelling = true, SetLastError = true)]
        internal static extern void MoveWindow(IntPtr hwnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
    }

    public partial class ConsoleWindow : Window {
        public static readonly ConsoleWindow Instance = new ConsoleWindow();
        public Action OnSplashHided;
        private ConsoleWindow() {
            this.Width = AppStatic.MainWindowWidth;
            this.Height = AppStatic.MainWindowHeight;
            if (VirtualRoot.IsGEWin10) {
                // 解决展开子窗口力的Popup时父窗口可能被绘制到子窗口上面的问题，这应该是WPF的BUG。
                this.AllowsTransparency = true;
            }
            else {
                // Win7下需要置为false，否则显示不出控制台窗口
                this.AllowsTransparency = false;
            }
            InitializeComponent();
        }

        private bool _isSplashed = false;
        public void HideSplash() {
            Splash.Visibility = Visibility.Collapsed;
            this.ShowInTaskbar = false;
            IntPtr parent = new WindowInteropHelper(this).Handle;
            IntPtr console = NTMinerConsole.Show();
            SafeNativeMethods.SetParent(console, parent);
            SafeNativeMethods.SetWindowLong(console, SafeNativeMethods.GWL_STYLE, SafeNativeMethods.WS_VISIBLE);
            _isSplashed = true;
            OnSplashHided?.Invoke();
        }

        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        private int _marginLeft, _marginTop, _height;
        public void ReSizeConsoleWindow(int marginLeft, int marginTop, int height) {
            if (!_isSplashed) {
                return;
            }
            if (_marginLeft == marginLeft && _marginTop == marginTop && _height == height) {
                return;
            }
            _marginLeft = marginLeft;
            _marginTop = marginTop;
            _height = height;
            const int paddingLeft = 4;
            const int paddingRight = 5;
            int width = (int)this.ActualWidth - paddingLeft - paddingRight - marginLeft;
            if (width < 0) {
                width = 0;
            }

            IntPtr console = NTMinerConsole.Show();
            SafeNativeMethods.MoveWindow(console, paddingLeft + marginLeft, marginTop, width, height, true);
        }
    }
}
