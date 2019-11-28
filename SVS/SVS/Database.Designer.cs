namespace SVS
{
    partial class Database
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btEnter = new System.Windows.Forms.Button();
            this.btExit = new System.Windows.Forms.Button();
            this.lbUid_fixed = new System.Windows.Forms.Label();
            this.lbpass_fixed = new System.Windows.Forms.Label();
            this.tbUid = new System.Windows.Forms.TextBox();
            this.tbPass = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbInfo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tmRun = new System.Windows.Forms.Timer(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btEnter
            // 
            this.btEnter.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btEnter.Location = new System.Drawing.Point(67, 93);
            this.btEnter.Name = "btEnter";
            this.btEnter.Size = new System.Drawing.Size(75, 41);
            this.btEnter.TabIndex = 3;
            this.btEnter.Text = "登入";
            this.btEnter.UseVisualStyleBackColor = true;
            this.btEnter.Click += new System.EventHandler(this.btEnter_Click);
            // 
            // btExit
            // 
            this.btExit.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btExit.Location = new System.Drawing.Point(247, 93);
            this.btExit.Name = "btExit";
            this.btExit.Size = new System.Drawing.Size(75, 41);
            this.btExit.TabIndex = 5;
            this.btExit.Text = "離開";
            this.btExit.UseVisualStyleBackColor = true;
            this.btExit.Click += new System.EventHandler(this.btExit_Click);
            // 
            // lbUid_fixed
            // 
            this.lbUid_fixed.AutoSize = true;
            this.lbUid_fixed.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbUid_fixed.Location = new System.Drawing.Point(86, 13);
            this.lbUid_fixed.Name = "lbUid_fixed";
            this.lbUid_fixed.Size = new System.Drawing.Size(66, 19);
            this.lbUid_fixed.TabIndex = 2;
            this.lbUid_fixed.Text = "帳號：";
            // 
            // lbpass_fixed
            // 
            this.lbpass_fixed.AutoSize = true;
            this.lbpass_fixed.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbpass_fixed.Location = new System.Drawing.Point(86, 48);
            this.lbpass_fixed.Name = "lbpass_fixed";
            this.lbpass_fixed.Size = new System.Drawing.Size(66, 19);
            this.lbpass_fixed.TabIndex = 3;
            this.lbpass_fixed.Text = "密碼：";
            // 
            // tbUid
            // 
            this.tbUid.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbUid.Location = new System.Drawing.Point(183, 10);
            this.tbUid.Name = "tbUid";
            this.tbUid.Size = new System.Drawing.Size(139, 30);
            this.tbUid.TabIndex = 1;
            // 
            // tbPass
            // 
            this.tbPass.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbPass.Location = new System.Drawing.Point(183, 46);
            this.tbPass.Name = "tbPass";
            this.tbPass.PasswordChar = '*';
            this.tbPass.Size = new System.Drawing.Size(139, 30);
            this.tbPass.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.lbInfo);
            this.panel1.Controls.Add(this.btEnter);
            this.panel1.Controls.Add(this.btExit);
            this.panel1.Controls.Add(this.lbUid_fixed);
            this.panel1.Controls.Add(this.lbpass_fixed);
            this.panel1.Controls.Add(this.tbUid);
            this.panel1.Controls.Add(this.tbPass);
            this.panel1.Location = new System.Drawing.Point(133, 116);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(395, 222);
            this.panel1.TabIndex = 6;
            // 
            // lbInfo
            // 
            this.lbInfo.BackColor = System.Drawing.SystemColors.Info;
            this.lbInfo.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbInfo.Location = new System.Drawing.Point(3, 137);
            this.lbInfo.Name = "lbInfo";
            this.lbInfo.Size = new System.Drawing.Size(387, 66);
            this.lbInfo.TabIndex = 7;
            this.lbInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label1.Location = new System.Drawing.Point(41, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(573, 46);
            this.label1.TabIndex = 7;
            this.label1.Text = "Signature Verification System";
            // 
            // tmRun
            // 
            this.tmRun.Tick += new System.EventHandler(this.tmRun_Tick);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button1.Location = new System.Drawing.Point(535, 306);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 32);
            this.button1.TabIndex = 8;
            this.button1.Text = "註冊";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(548, 12);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(47, 16);
            this.radioButton1.TabIndex = 9;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "中文";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(601, 12);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(47, 16);
            this.radioButton2.TabIndex = 10;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "英文";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // Database
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(660, 374);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Name = "Database";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SVS-登入系統";
            this.Load += new System.EventHandler(this.Database_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btEnter;
        private System.Windows.Forms.Button btExit;
        private System.Windows.Forms.Label lbUid_fixed;
        private System.Windows.Forms.Label lbpass_fixed;
        private System.Windows.Forms.TextBox tbUid;
        private System.Windows.Forms.TextBox tbPass;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer tmRun;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
    }
}