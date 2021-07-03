using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace GameCoreUsage.Helper
{
    public static class SharedMemPath
    {
        // https://stackoverflow.com/questions/9407616/how-can-i-call-to-a-c-function-from-c-sharp
        //https://stackoverflow.com/questions/32991274/return-string-from-c-dll-export-function-called-from-c-sharp

        private const string DllFilePath = @"C:\Users\Vincent\Desktop\programming\Indicium-Supra-master\x64\Debug\GCUMemReader.dll";
        private const int BUFLENGTH = 300;

        [DllImport(DllFilePath, CallingConvention = CallingConvention.Cdecl)]
        private extern static void test(byte[] name, byte[] buf, int buflength);

        public static string Test(string name)
        {
            byte[] buf = new byte[BUFLENGTH];
            test(Encoding.UTF8.GetBytes(name), buf, BUFLENGTH);
            return System.Text.Encoding.ASCII.GetString(buf);
        }
    }
}
