using System;
using System.Threading;

namespace UI
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(81, 83);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(100, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(81, 140);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(100, 30);
            this.progressBar2.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.progressBar1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);


            new Thread(() =>
            {
                FritzBoxSoap.FritzBoxSoap soap = new FritzBoxSoap.FritzBoxSoap("192.168.178.1", "-");
                while (true)
                {
                    var currentdl = soap.getCurrentDlSpeed();
                    var currentul = soap.getCurrentUpSpeed();

                    var percdl = soap.getPercentageUsageDownStream();
                    var percul = soap.getPercentageUsageUoStream();

                    this.progressBar1.Maximum = 100;
                    this.progressBar2.Maximum = 100;

                    this.progressBar1.Invoke((Action)delegate
                    {
                        this.progressBar1.Value = System.Convert.ToInt32(percdl);
                    });

                    this.progressBar2.Invoke((Action)delegate
                    {
                        this.progressBar2.Value = System.Convert.ToInt32(percul);
                    });

                    Thread.Sleep(5000);
                }
            }).Start();
        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ProgressBar progressBar2;
    }
}

