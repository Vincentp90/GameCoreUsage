using GameCoreUsage.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
            //continue try catch
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
    }
}
