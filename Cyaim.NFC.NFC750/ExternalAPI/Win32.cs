using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Cyaim.NFC.NFC750.ExternalAPI
{
    public class Win32
    {
        #region kernel32.dll
        [DllImport("kernel32.dll")]
        public static extern void Sleep(int dwMilliseconds);
        #endregion

    }
}
