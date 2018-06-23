using System;
using System.Drawing;
using System.Threading;
using FritzBoxSoap;

namespace TaskBar
{
    partial class Bar
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tx = new System.Windows.Forms.Label();
            this.progressBar1 = new StyledProgressBar();
            this.progressBar2 = new StyledProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.24044F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 67.75956F));
            this.tableLayoutPanel1.Controls.Add(this.tx, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.progressBar1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.progressBar2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(4, 4, 7, 4);
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(194, 37);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.BackColor = Color.Black;
            // 
            // tx
            // 
            this.tx.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tx.AutoSize = true;
            this.tx.ForeColor = System.Drawing.Color.White;
            this.tx.Location = new System.Drawing.Point(66, 18);
            this.tx.Name = "tx";
            this.tx.Size = new System.Drawing.Size(118, 15);
            this.tx.TabIndex = 3;
            this.tx.Text = "tx";
            this.tx.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(7, 7);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(50, 8);
            this.progressBar1.TabIndex = 0;
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(7, 21);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(50, 9);
            this.progressBar2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label1.Location = new System.Drawing.Point(66, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = "rx";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // Bar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.MaximumSize = new System.Drawing.Size(200, 40);
            this.MinimumSize = new System.Drawing.Size(200, 30);
            this.Name = "Bar";
            this.Size = new System.Drawing.Size(200, 40);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
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

                   
                }
            }).Start();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private StyledProgressBar progressBar1;
        private StyledProgressBar progressBar2;
        private System.Windows.Forms.Label tx;
        private System.Windows.Forms.Label label1;
    }
}
