using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameCoreUsage.Memory
{
    public class FrametimeReader
    {
        public double AverageFrametime { get; set; }

        private const int FRAMETIMES_INT_SIZE = 5000;
        private readonly long intSize = Marshal.SizeOf(typeof(int));

        private CancellationTokenSource source = new CancellationTokenSource();
        private CancellationToken token;

        private GCUForm form;
        private TextBox tbFrametime;

        //TODO fix form <-> frametime dependency
        public FrametimeReader(GCUForm form, TextBox tbFrametime)
        {
            this.form = form;
            this.tbFrametime = tbFrametime;
        }

        public void Start()
        {
            token = source.Token;
            Task.Factory.StartNew(() =>
            {
                ContinuouslyReadFrametime();
            }, token);
        }

        public void Stop()
        {
            source.Cancel();
        }

        private void ContinuouslyReadFrametime()
        {
            using (var mmfIndex = MemoryMappedFile.OpenExisting("SHMINDEX"))
            using (var accessorIndex = mmfIndex.CreateViewAccessor(0, Marshal.SizeOf(typeof(int))))
            using (var mmfFrametimes = MemoryMappedFile.OpenExisting("SHMFRAMETIMES"))
            using (var accessorFrametimes = mmfFrametimes.CreateViewAccessor(0, FRAMETIMES_INT_SIZE * intSize))
            {
                int ownIndex = 0;
                while (!token.IsCancellationRequested)
                {
                    int index;
                    accessorIndex.Read(0, out index);
                    //Log("Index is: " + index);

                    int frametime;
                    int total = 0;
                    int count = 0;
                    for (int j = ownIndex; j % FRAMETIMES_INT_SIZE != index; j++)
                    {
                        accessorFrametimes.Read((j % FRAMETIMES_INT_SIZE) * intSize, out frametime);
                        count++;
                        total += frametime;
                    }

                    if (count > 0)
                    {
                        AverageFrametime = ((double)total / (double)count) / 1000.00;
                        form.UpdateTextbox(tbFrametime, AverageFrametime.ToString());
                    }
                    ownIndex = index;
                    Thread.Sleep(200);
                }
            }
        }

    }
}
