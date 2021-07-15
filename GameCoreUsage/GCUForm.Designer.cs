
namespace GameCoreUsage
{
    partial class GCUForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbLog = new System.Windows.Forms.TextBox();
            this.tbFrametime = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbFPS = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnTestAffinity = new System.Windows.Forms.Button();
            this.btnInit = new System.Windows.Forms.Button();
            this.btnMeasure = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(12, 325);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.Size = new System.Drawing.Size(600, 279);
            this.tbLog.TabIndex = 1;
            // 
            // tbFrametime
            // 
            this.tbFrametime.Location = new System.Drawing.Point(342, 12);
            this.tbFrametime.Name = "tbFrametime";
            this.tbFrametime.Size = new System.Drawing.Size(100, 23);
            this.tbFrametime.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(222, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Avg frametime";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(222, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "FPS";
            // 
            // tbFPS
            // 
            this.tbFPS.Location = new System.Drawing.Point(342, 42);
            this.tbFPS.Name = "tbFPS";
            this.tbFPS.Size = new System.Drawing.Size(100, 23);
            this.tbFPS.TabIndex = 6;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(13, 92);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(117, 26);
            this.btnStart.TabIndex = 7;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(12, 124);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(117, 26);
            this.btnStop.TabIndex = 8;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnTestAffinity
            // 
            this.btnTestAffinity.Location = new System.Drawing.Point(459, 12);
            this.btnTestAffinity.Name = "btnTestAffinity";
            this.btnTestAffinity.Size = new System.Drawing.Size(117, 26);
            this.btnTestAffinity.TabIndex = 9;
            this.btnTestAffinity.Text = "Test affinity";
            this.btnTestAffinity.UseVisualStyleBackColor = true;
            this.btnTestAffinity.Click += new System.EventHandler(this.btnTestAffinity_Click);
            // 
            // btnInit
            // 
            this.btnInit.Location = new System.Drawing.Point(12, 49);
            this.btnInit.Name = "btnInit";
            this.btnInit.Size = new System.Drawing.Size(117, 26);
            this.btnInit.TabIndex = 10;
            this.btnInit.Text = "Init";
            this.btnInit.UseVisualStyleBackColor = true;
            this.btnInit.Click += new System.EventHandler(this.btnInit_Click);
            // 
            // btnMeasure
            // 
            this.btnMeasure.Location = new System.Drawing.Point(222, 124);
            this.btnMeasure.Name = "btnMeasure";
            this.btnMeasure.Size = new System.Drawing.Size(117, 26);
            this.btnMeasure.TabIndex = 11;
            this.btnMeasure.Text = "Measure";
            this.btnMeasure.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(624, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.Size = new System.Drawing.Size(271, 592);
            this.dataGridView1.TabIndex = 12;
            // 
            // GCUForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(907, 616);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnMeasure);
            this.Controls.Add(this.btnInit);
            this.Controls.Add(this.btnTestAffinity);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.tbFPS);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbFrametime);
            this.Controls.Add(this.tbLog);
            this.Name = "GCUForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.GCUForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.TextBox tbFrametime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbFPS;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnTestAffinity;
        private System.Windows.Forms.Button btnInit;
        private System.Windows.Forms.Button btnMeasure;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}

