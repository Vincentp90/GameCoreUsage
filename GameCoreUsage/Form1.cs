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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameCoreUsage
{
    public partial class Form1 : Form
    {
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
                using (var accessor = mmf.CreateViewAccessor(0, Marshal.SizeOf(typeof(int))))
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

        private void Log(string text)
        {
            tbLog.AppendText(text);
            tbLog.AppendText(Environment.NewLine);
        }
    }
}
