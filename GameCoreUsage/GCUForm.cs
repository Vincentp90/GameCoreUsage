using GameCoreUsage.Helper;
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
        private GameProcess game;
        private BindingSource bindingSource = new BindingSource();
        Dictionary<int, bool> activeCores;

        private const int CORECOUNT = 12;

        public GCUForm()
        {
            InitializeComponent();            
        }

        private void GCUForm_Load(object sender, EventArgs e)
        {
            dataGridCores.DataSource = bindingSource;
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
            game = GameProcess.Create("FactoryGame-Win64-Shipping");
            btnMeasure.Enabled = true;
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

        private void UpdateDataGridCores(BitArray b)
        {
            //this creates 32 length array
            //BitArray b = new BitArray(new int[] { affinity });
            for (int i = 0; i < CORECOUNT; i++)
            {
                activeCores[i] = b[i];
            }
            //TODO fix scuffed https://stackoverflow.com/a/1118992
            bindingSource.DataSource = typeof(Dictionary<int, bool>);
            bindingSource.DataSource = activeCores;
        }

        private void InvokeUpdateDGCores()
        {
            dataGridCores.Invoke(new Action<BitArray>(UpdateDataGridCores), game.GetAffinityBitArray());
        }

        private const int ITERATIONS = 500;
        private const int SLEEPDUR = 410;
        private void btnMeasure_Click(object sender, EventArgs e)
        {
            reader.Start();
            game.SetPCoresActive(2);
            Thread.Sleep(SLEEPDUR);
            UpdateDataGridCores(game.GetAffinityBitArray());
            Task.Factory.StartNew(() =>
            {
                double previousAvg = reader.AverageFrametime;
                double avg;
                int[] coresActiveLog = new int[ITERATIONS];
                Thread.Sleep(SLEEPDUR);
                bool wasIncreased = true;
                for (int i = 0; i < ITERATIONS; i++)
                {
                    avg = reader.AverageFrametime;
                    if (previousAvg > avg * (wasIncreased ? 1.00005 : 0.99995))
                    {
                        game.Increase();
                        wasIncreased = true;
                    }
                    else
                    {
                        game.Decrease();
                        wasIncreased = false;
                    }                        
                    coresActiveLog[i] = game.ActiveVCores;
                    previousAvg = avg;
                    Thread.Sleep(SLEEPDUR);
                    InvokeUpdateDGCores();                   
                }
                //Continue
                //post processing:
                //sum each corecount in coresActiveLog and print
                //print avg active core count
                // take median from active core count or repeat until x sigma core count values are the same
            });            
        }

        private void btnTest1_Click(object sender, EventArgs e)
        {
            game.Increase();
            Thread.Sleep(200);
            UpdateDataGridCores(game.GetAffinityBitArray());
        }

        private void btnTest2_Click(object sender, EventArgs e)
        {
            game.Decrease();
            Thread.Sleep(200);
            UpdateDataGridCores(game.GetAffinityBitArray());
        }
    }
}
