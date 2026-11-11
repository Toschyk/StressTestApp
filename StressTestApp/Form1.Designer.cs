namespace StressTestApp
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblMetrics = new System.Windows.Forms.Label();
            this.timerMetrics = new System.Windows.Forms.Timer(this.components);
            this.btnMemoryStress = new System.Windows.Forms.Button();
            this.btnDiskStress = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(15, 17);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Запустить расчет";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(15, 46);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Остановить";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(15, 133);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(189, 23);
            this.progressBar1.TabIndex = 2;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(12, 159);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(68, 13);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Ожидание...";
            // 
            // lblMetrics
            // 
            this.lblMetrics.AutoSize = true;
            this.lblMetrics.Location = new System.Drawing.Point(12, 172);
            this.lblMetrics.Name = "lblMetrics";
            this.lblMetrics.Size = new System.Drawing.Size(112, 13);
            this.lblMetrics.TabIndex = 4;
            this.lblMetrics.Text = "CPU: 0% | RAM: 0 МБ";
            // 
            // timerMetrics
            // 
            this.timerMetrics.Enabled = true;
            this.timerMetrics.Interval = 1000;
            this.timerMetrics.Tick += new System.EventHandler(this.timerMetrics_Tick);
            // 
            // btnMemoryStress
            // 
            this.btnMemoryStress.Location = new System.Drawing.Point(15, 75);
            this.btnMemoryStress.Name = "btnMemoryStress";
            this.btnMemoryStress.Size = new System.Drawing.Size(100, 23);
            this.btnMemoryStress.TabIndex = 5;
            this.btnMemoryStress.Text = "Нагрузка RAM";
            this.btnMemoryStress.UseVisualStyleBackColor = true;
            this.btnMemoryStress.Click += new System.EventHandler(this.btnMemoryStress_Click);
            // 
            // btnDiskStress
            // 
            this.btnDiskStress.Location = new System.Drawing.Point(15, 104);
            this.btnDiskStress.Name = "btnDiskStress";
            this.btnDiskStress.Size = new System.Drawing.Size(120, 23);
            this.btnDiskStress.TabIndex = 6;
            this.btnDiskStress.Text = "Нагрузка диска";
            this.btnDiskStress.UseVisualStyleBackColor = true;
            this.btnDiskStress.Click += new System.EventHandler(this.btnDiskStress_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(221, 214);
            this.Controls.Add(this.btnDiskStress);
            this.Controls.Add(this.btnMemoryStress);
            this.Controls.Add(this.lblMetrics);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblMetrics;
        private System.Windows.Forms.Timer timerMetrics;
        private System.Windows.Forms.Button btnMemoryStress;
        private System.Windows.Forms.Button btnDiskStress;
    }
}

