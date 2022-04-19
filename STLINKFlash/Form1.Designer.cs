namespace STLINKFlash
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.openstlink = new System.Windows.Forms.Button();
            this.writebutton = new System.Windows.Forms.Button();
            this.readbutton = new System.Windows.Forms.Button();
            this.addrtext = new System.Windows.Forms.TextBox();
            this.lentext = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.coreid = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // openstlink
            // 
            this.openstlink.Location = new System.Drawing.Point(160, 8);
            this.openstlink.Name = "openstlink";
            this.openstlink.Size = new System.Drawing.Size(75, 23);
            this.openstlink.TabIndex = 0;
            this.openstlink.Text = "打开stlink";
            this.openstlink.UseVisualStyleBackColor = true;
            this.openstlink.Click += new System.EventHandler(this.openstlink_Click);
            // 
            // writebutton
            // 
            this.writebutton.Location = new System.Drawing.Point(160, 37);
            this.writebutton.Name = "writebutton";
            this.writebutton.Size = new System.Drawing.Size(75, 23);
            this.writebutton.TabIndex = 0;
            this.writebutton.Text = "写入";
            this.writebutton.UseVisualStyleBackColor = true;
            this.writebutton.Click += new System.EventHandler(this.stlinkop_Click);
            // 
            // readbutton
            // 
            this.readbutton.Location = new System.Drawing.Point(160, 66);
            this.readbutton.Name = "readbutton";
            this.readbutton.Size = new System.Drawing.Size(75, 23);
            this.readbutton.TabIndex = 1;
            this.readbutton.Text = "读取";
            this.readbutton.UseVisualStyleBackColor = true;
            this.readbutton.Click += new System.EventHandler(this.button1_Click);
            // 
            // addrtext
            // 
            this.addrtext.Location = new System.Drawing.Point(42, 10);
            this.addrtext.Name = "addrtext";
            this.addrtext.Size = new System.Drawing.Size(100, 21);
            this.addrtext.TabIndex = 2;
            this.addrtext.Text = "0";
            // 
            // lentext
            // 
            this.lentext.Location = new System.Drawing.Point(42, 37);
            this.lentext.Name = "lentext";
            this.lentext.Size = new System.Drawing.Size(100, 21);
            this.lentext.TabIndex = 2;
            this.lentext.Text = "153600";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "地址";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "长度";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "coreID:";
            // 
            // coreid
            // 
            this.coreid.AutoSize = true;
            this.coreid.Location = new System.Drawing.Point(54, 88);
            this.coreid.Name = "coreid";
            this.coreid.Size = new System.Drawing.Size(11, 12);
            this.coreid.TabIndex = 4;
            this.coreid.Text = "0";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(6, 65);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(148, 23);
            this.progressBar1.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(247, 101);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.coreid);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lentext);
            this.Controls.Add(this.addrtext);
            this.Controls.Add(this.readbutton);
            this.Controls.Add(this.writebutton);
            this.Controls.Add(this.openstlink);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button openstlink;
        private System.Windows.Forms.Button writebutton;
        private System.Windows.Forms.Button readbutton;
        private System.Windows.Forms.TextBox addrtext;
        private System.Windows.Forms.TextBox lentext;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label coreid;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}

