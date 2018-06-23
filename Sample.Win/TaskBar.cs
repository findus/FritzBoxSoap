using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSDeskBand.Win;
using CSDeskBand;
using System.Runtime.InteropServices;
using NLog;
using NLog.Config;
using NLog.Targets;
using System.Threading;

namespace TaskBar
{
    /// <summary>
    /// Example winforms deskband. This deskband shows the title of the current focused window.
    /// </summary>
    [ComVisible(true)]
    [Guid("1337FC61-8530-404C-86C1-86CCB8738D06")]
    [CSDeskBandRegistration(Name = "Fritz Box Monitor")]
    public partial class Bar : CSDeskBandWin
    {
        private readonly WinEventDelegate _delegate;
        private readonly IntPtr _hookId;
        private const uint EVENT_SYSTEM_FOREGROUND = 3;
        private const uint WINEVENT_OUTOFCONTEXT = 0;

        public Bar()
        {
            InitializeComponent();
            Options.Horizontal.Width = Options.MinHorizontal.Width = 100;
            _delegate = CallBack;
            _hookId = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, _delegate, 0, 0, WINEVENT_OUTOFCONTEXT);

        }

        protected override void OnClose()
        {
            base.OnClose();
            UnhookWinEvent(_hookId);
        }

        private void CallBack(IntPtr hWinEventHook, uint eventType, IntPtr hWnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
  
        }

        public delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hWnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        [DllImport("user32.dll")]
        static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        public static extern bool UnhookWinEvent(IntPtr hWinEventHook);

        //Get active window title code from stack overflow
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
