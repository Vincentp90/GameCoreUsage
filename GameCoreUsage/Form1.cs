using GameCoreUsage.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameCoreUsage
{
    public partial class Form1 : Form
    {
        private const int FRAMETIMES_INT_SIZE = 5000;
        private readonly long intSize = Marshal.SizeOf(typeof(int));

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                tbLog.AppendText(SharedMemPath.Test("SHMFRAMETIMES"));
            }
            catch(Exception ex)
            {
                tbLog.AppendText(ex.Message);
                tbLog.AppendText(ex.StackTrace);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                // Create the memory-mapped file.
                //string shmIndexPath = new Uri(SharedMemPath.Test("SHMINDEX")).LocalPath;
                //Log(shmIndexPath);
                //using (var mmf = MemoryMappedFile.CreateFromFile(System.IO.Path.GetFullPath(shmIndexPath), FileMode.Open, "SHMINDEX", 0, MemoryMappedFileAccess.Read))
                using (var mmf = MemoryMappedFile.OpenExisting("SHMINDEX"))
                using (var accessor = mmf.CreateViewAccessor(0, intSize))
                {
                    int bla;
                    accessor.Read(0, out bla);
                    Log("Index is: " + bla);

                    //continue frametime
                }
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                Log(ex.StackTrace);
            }
        }

        //continue frametime
        private void ContinuouslyReadFrametime()
        {
            using (var mmfIndex = MemoryMappedFile.OpenExisting("SHMINDEX"))
            using (var accessorIndex = mmfIndex.CreateViewAccessor(0, Marshal.SizeOf(typeof(int))))
            using (var mmfFrametimes = MemoryMappedFile.OpenExisting("SHMFRAMETIMES"))
            using (var accessorFrametimes = mmfFrametimes.CreateViewAccessor(0, FRAMETIMES_INT_SIZE*intSize))
            {
                int ownIndex = 0;
                while (!token.IsCancellationRequested)
                {                    
                    int index;
                    accessorIndex.Read(0, out index);
                    Log("Index is: " + index);

                    int frametime;
                    int total = 0;
                    int count = 0;
                    for (int j = ownIndex; j % FRAMETIMES_INT_SIZE != index; j++)
                    {
                        accessorFrametimes.Read((j % FRAMETIMES_INT_SIZE)*intSize, out frametime);
                        count++;
                        total += frametime;                        
                    }

                    if (count > 0)
                    {
                        double avg = ((double)total / (double)count) / 1000.00;
                        UpdateTextbox(tbFrametime, avg.ToString());
                    }
                    ownIndex = index;
                    Thread.Sleep(200);
                }
            }
        }

        private void UpdateTextbox(TextBox tb, string text)
        {
            if (tb.InvokeRequired)
            {
                tb.Invoke(new Action<TextBox, string>(UpdateTextbox), tb, text);
                return;
            }
            tb.Text = text;
        }

        private void Log(string text)
        {
            if(tbLog.InvokeRequired)
            {
                tbLog.Invoke(new Action<string>(Log), text);
                return;
            }
            tbLog.AppendText(text);
            tbLog.AppendText(Environment.NewLine);
        }

        private CancellationTokenSource source = new CancellationTokenSource();
        private CancellationToken token;
        
        private void btnStart_Click(object sender, EventArgs e)
        {
            token = source.Token;
            Task.Factory.StartNew(() =>
            {
                ContinuouslyReadFrametime();
            }, token);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            source.Cancel();
        }
    }
}
