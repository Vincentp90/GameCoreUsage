using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GameCoreUsage.Helper
{
    public class GameProcess : IDisposable
    {
        private Process process;
        // TODO detect core count
        private int pCoreCount = 6;
        private int vCoreCount = 12;
        private bool disposedValue;

        public int ActiveVCores { get; set; }

        public GameProcess(Process process)
        {
            this.process = process;
            ActiveVCores = vCoreCount;
        }

        // Set affinity to only physical cores (only even or only uneven)
        // TODO CPUs without multithreading
        public void SetPCoresActive(int count)
        {
            // Sum of first alterating bits, converted to decimal: 4^n - 1 / 3 
            // Example: Set first 3 physical cores
            // Bits: 10101
            // Decimal: 1 + 4 + 16 = 21 = 4^3 - 1 / 3
            long affinity = (Pow(4,count) - 1) / 3;
            process.ProcessorAffinity = (System.IntPtr)affinity;
            ActiveVCores = count;
        }

        public long GetAffinity()
        {
            return (long)process.ProcessorAffinity;
        }

        public BitArray GetAffinityBitArray()
        {
            //return new BitArray(new int[] { GetAffinity() });
            return new BitArray(BitConverter.GetBytes(GetAffinity()));
        }

        private long getLongFromBitArray(BitArray bitArray)
        {

            if (bitArray.Length > 64)
                throw new ArgumentException("Argument length shall be at most 64 bits.");

            //long[] array = new long[1];
            //bitArray.CopyTo(array, 0);
            //return array[0];
            var array = new byte[8];
            bitArray.CopyTo(array, 0);
            return BitConverter.ToInt64(array, 0);
        }

        // Add one vcore, physical cores first
        // Return false if all cores where already assigned
        public bool Increase()
        {
            BitArray affinityArray = GetAffinityBitArray();
            for(int i = 0; i < vCoreCount; i += 2)
            {
                if(!affinityArray[i])
                {
                    affinityArray[i] = true;
                    process.ProcessorAffinity = (System.IntPtr)getLongFromBitArray(affinityArray);
                    ActiveVCores++;
                    return true;
                }
            }
            for (int i = 1; i < vCoreCount; i += 2)
            {
                if (!affinityArray[i])
                {
                    affinityArray[i] = true;
                    process.ProcessorAffinity = (System.IntPtr)getLongFromBitArray(affinityArray);
                    ActiveVCores++;
                    return true;
                }
            }
            return false;
        }

        // Remove one core, vcores first
        public bool Decrease()
        {
            BitArray affinityArray = GetAffinityBitArray();
            for (int i = vCoreCount-1; i >= 1; i -= 2)
            {
                if (affinityArray[i])
                {
                    affinityArray[i] = false;
                    process.ProcessorAffinity = (System.IntPtr)getLongFromBitArray(affinityArray);
                    ActiveVCores--;
                    return true;
                }
            }
            for (int i = vCoreCount-2; i >= 1; i -= 2)
            {
                if (affinityArray[i])
                {
                    affinityArray[i] = false;
                    process.ProcessorAffinity = (System.IntPtr)getLongFromBitArray(affinityArray);
                    ActiveVCores--;
                    return true;
                }
            }            
            return false;
        }

        // Set affinity to vCores
        // TODO add flag 'fill physical first'
        public void SetVCoresActive(int count)
        {
            throw new NotImplementedException();
            // todo this is wrong:
            process.ProcessorAffinity = (System.IntPtr)count;
            ActiveVCores = count;
        }

        public static GameProcess Create(string processName)
        {
            Process[] procs = Process.GetProcessesByName(processName);
            try
            {
                if (procs.Length >= 1)
                    return new GameProcess(procs[0]);
                else
                    return null;
            }
            catch (Exception)
            {
                //Also cleanup first process if exception
                if (procs.Length > 0)
                    procs[0].Dispose();
                throw;
            }
            finally
            {
                //Cleanup everything except the first process because this will be used by the created GameProcess
                foreach (Process proc in procs)
                {
                    if (proc == procs[0])
                        continue;
                    proc.Dispose();
                }
            }
        }

        #region Helper
        private static int Pow(int bas, int exp)
        {
            return Enumerable
                  .Repeat(bas, exp)
                  .Aggregate(1, (a, b) => a * b);
        }


        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    process?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~GameProcess()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
