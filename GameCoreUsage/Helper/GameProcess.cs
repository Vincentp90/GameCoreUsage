using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GameCoreUsage.Helper
{
    public class GameProcess
    {
        private Process process;
        // TODO detect core count
        private int pCoreCount = 6;
        private int vCoreCount = 12;


        public GameProcess(Process process)
        {
            this.process = process;
        }

        // Set affinity to only physical cores (only even or only uneven)
        // TODO CPUs without multithreading
        public void SetPCoresActive(int count)
        {
            // Sum of first alterating bits, converted to decimal: 4^n - 1 / 3 
            // Example: Set first 3 physical cores
            // Bits: 10101
            // Decimal: 1 + 4 + 16 = 21 = 4^3 - 1 / 3
            int affinity = (Pow(4,count) - 1) / 3;
            process.ProcessorAffinity = (System.IntPtr)affinity;
        }

        //Test remove after
        public int TestCastAffinity()
        {
            return (int)process.ProcessorAffinity;
        }

        // Add one vcore, physical cores first
        public void Increase()
        {
        }

        // Remove one core, vcores first
        public void Decrease()
        {

        }

        // Set affinity to vCores
        // TODO add flag 'fill physical first'
        public void SetVCoresActive(int count)
        {
            process.ProcessorAffinity = (System.IntPtr)count;
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
        public static int Pow(int bas, int exp)
        {
            return Enumerable
                  .Repeat(bas, exp)
                  .Aggregate(1, (a, b) => a * b);
        }
        #endregion
    }
}
