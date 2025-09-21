﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LightHook
{
    public class WindowsNativeFunc
    {
        #region wow64

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsWow64Process([In] IntPtr process, [Out] out bool wow64Process);
        public static bool IsWin64(Process process)
        {

            IntPtr processHandle;
            bool retVal;

            try
            {
                processHandle = process.Handle;
            }
            catch
            {
                return false;
            }
            IsWow64Process(processHandle, out retVal);
            return !retVal;

        }

        #endregion
    }
}
