using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GameCoreUsage.Helper
{
    public static class SharedMemPath
    {
        private const string DllFilePath = @"c:\pathto\mydllfile.dll";

        [DllImport(DllFilePath, CallingConvention = CallingConvention.Cdecl)]
        private extern static string test(string number);

        public static string Test(string name)
        {
            return test(name);
        }
    }
}
