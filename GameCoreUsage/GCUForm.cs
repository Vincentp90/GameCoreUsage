using GameCoreUsage.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
    public partial class GCUForm : Form
    {
        private FrametimeReader reader;
        private BindingSource bindingSource = new BindingSource();
        Dictionary<int, bool> activeCores;

        private const int CORECOUNT = 12;

        public GCUForm()
        {
            InitializeComponent();            
        }

        private void GCUForm_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = bindingSource;
            activeCores = new Dictionary<int, bool>();
            for (int i = 0; i < CORECOUNT; i++)
            {
                activeCores.Add(i, true);
            }
            bindingSource.DataSource = activeCores;

        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            reader = new FrametimeReader(this, tbFrametime);

            //this creates 32 length array
            BitArray b = new BitArray(new int[] { 7 });
            for(int i = 0; i < CORECOUNT; i++)
            {
                activeCores[i] = b[i];
            }
            //TODO fix scuffed https://stackoverflow.com/a/1118992
            bindingSource.DataSource = typeof(Dictionary<int, bool>);
            bindingSource.DataSource = activeCores;
        }



        public void UpdateTextbox(TextBox tb, string text)
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
                
        private void btnStart_Click(object sender, EventArgs e)
        {
            reader.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            reader.Stop();
        }

        private void btnTestAffinity_Click(object sender, EventArgs e)
        {
            using (Process myProcess = Process.GetCurrentProcess())
            {
                
                Process[] procs = Process.GetProcessesByName("FactoryGame-Win64-Shipping");
                try
                {
                    if (procs.Length == 1)
                    {
                        Log("ProcessorAffinity: " + myProcess.ProcessorAffinity);
                        procs[0].ProcessorAffinity = (System.IntPtr)7;
                        Thread.Sleep(200);
                        Log("ProcessorAffinity: " + myProcess.ProcessorAffinity);

                        BitArray b = new BitArray(new int[] { 7 });
                        foreach(var core in activeCores)
                        {
                            if (core.Key > b.Length)
                                activeCores[core.Key] = false;
                            else
                                activeCores[core.Key] = b[core.Key];
                        }
                    }
                }
                finally
                {
                    foreach(Process proc in procs)
                    {
                        proc.Dispose();
                    }
                }
            }
        }

        
    }
}
