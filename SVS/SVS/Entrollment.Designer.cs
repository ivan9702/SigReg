namespace SVS
{
    partial class Entrollment
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
            this.btNewSignature = new System.Windows.Forms.Button();
            this.btTemplateSelection = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btDeleteAll = new System.Windows.Forms.Button();
            this.btClearScreen = new System.Windows.Forms.Button();
            this.btDelete = new System.Windows.Forms.Button();
            this.clbNewSignature = new System.Windows.Forms.CheckedListBox();
            this.btTemplateConfirm = new System.Windows.Forms.Button();
            this.btEntrollment = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbTemplateInfo = new System.Windows.Forms.Label();
            this.lbInformation_fixed = new System.Windows.Forms.Label();
            this.lbUserName_fixed = new System.Windows.Forms.Label();
            this.lbAccount_fixed = new System.Windows.Forms.Label();
            this.lbPassword_fixed = new System.Windows.Forms.Label();
            this.lbSex_fixed = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lbUserInfo_fixed = new System.Windows.Forms.Label();
            this.cbSex = new System.Windows.Forms.ComboBox();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbAccount = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tmUpdateFile = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // btNewSignature
            // 
            this.btNewSignature.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btNewSignature.Location = new System.Drawing.Point(40, 20);
            this.btNewSignature.Name = "btNewSignature";
            this.btNewSignature.Size = new System.Drawing.Size(127, 38);
            this.btNewSignature.TabIndex = 5;
            this.btNewSignature.Text = "新筆簽名";
            this.btNewSignature.UseVisualStyleBackColor = true;
            this.btNewSignature.Click += new System.EventHandler(this.btNewSignature_Click);
            // 
            // btTemplateSelection
            // 
            this.btTemplateSelection.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btTemplateSelection.Location = new System.Drawing.Point(78, 20);
            this.btTemplateSelection.Name = "btTemplateSelection";
            this.btTemplateSelection.Size = new System.Drawing.Size(157, 38);
            this.btTemplateSelection.TabIndex = 6;
            this.btTemplateSelection.Text = "樣板挑選";
            this.btTemplateSelection.UseVisualStyleBackColor = true;
            this.btTemplateSelection.Click += new System.EventHandler(this.btTemplateSelection_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.btDeleteAll);
            this.panel1.Controls.Add(this.btClearScreen);
            this.panel1.Controls.Add(this.btDelete);
            this.panel1.Controls.Add(this.btNewSignature);
            this.panel1.Controls.Add(this.clbNewSignature);
            this.panel1.Location = new System.Drawing.Point(778, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(206, 451);
            this.panel1.TabIndex = 4;
            // 
            // btDeleteAll
            // 
            this.btDeleteAll.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btDeleteAll.Location = new System.Drawing.Point(45, 330);
            this.btDeleteAll.Name = "btDeleteAll";
            this.btDeleteAll.Size = new System.Drawing.Size(117, 38);
            this.btDeleteAll.TabIndex = 14;
            this.btDeleteAll.Text = "清除資料";
            this.btDeleteAll.UseVisualStyleBackColor = true;
            this.btDeleteAll.Click += new System.EventHandler(this.btDeleteAll_Click);
            // 
            // btClearScreen
            // 
            this.btClearScreen.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btClearScreen.Location = new System.Drawing.Point(45, 392);
            this.btClearScreen.Name = "btClearScreen";
            this.btClearScreen.Size = new System.Drawing.Size(117, 38);
            this.btClearScreen.TabIndex = 13;
            this.btClearScreen.Text = "清除畫面";
            this.btClearScreen.UseVisualStyleBackColor = true;
            this.btClearScreen.Click += new System.EventHandler(this.btClearScreen_Click);
            // 
            // btDelete
            // 
            this.btDelete.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btDelete.Location = new System.Drawing.Point(45, 267);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(117, 38);
            this.btDelete.TabIndex = 12;
            this.btDelete.Text = "刪除";
            this.btDelete.UseVisualStyleBackColor = true;
            this.btDelete.Click += new System.EventHandler(this.btDelete_Click);
            // 
            // clbNewSignature
            // 
            this.clbNewSignature.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.clbNewSignature.FormattingEnabled = true;
            this.clbNewSignature.Location = new System.Drawing.Point(14, 87);
            this.clbNewSignature.Name = "clbNewSignature";
            this.clbNewSignature.Size = new System.Drawing.Size(174, 174);
            this.clbNewSignature.TabIndex = 11;
            // 
            // btTemplateConfirm
            // 
            this.btTemplateConfirm.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btTemplateConfirm.Location = new System.Drawing.Point(78, 83);
            this.btTemplateConfirm.Name = "btTemplateConfirm";
            this.btTemplateConfirm.Size = new System.Drawing.Size(157, 38);
            this.btTemplateConfirm.TabIndex = 7;
            this.btTemplateConfirm.Text = "樣板確認";
            this.btTemplateConfirm.UseVisualStyleBackColor = true;
            this.btTemplateConfirm.Click += new System.EventHandler(this.btTemplateConfirm_Click);
            // 
            // btEntrollment
            // 
            this.btEntrollment.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btEntrollment.Location = new System.Drawing.Point(78, 145);
            this.btEntrollment.Name = "btEntrollment";
            this.btEntrollment.Size = new System.Drawing.Size(157, 38);
            this.btEntrollment.TabIndex = 8;
            this.btEntrollment.Text = "註冊";
            this.btEntrollment.UseVisualStyleBackColor = true;
            this.btEntrollment.Click += new System.EventHandler(this.btEntrollment_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.lbTemplateInfo);
            this.panel2.Controls.Add(this.lbInformation_fixed);
            this.panel2.Location = new System.Drawing.Point(336, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(436, 451);
            this.panel2.TabIndex = 5;
            // 
            // lbTemplateInfo
            // 
            this.lbTemplateInfo.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.lbTemplateInfo.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbTemplateInfo.Location = new System.Drawing.Point(22, 43);
            this.lbTemplateInfo.Name = "lbTemplateInfo";
            this.lbTemplateInfo.Size = new System.Drawing.Size(394, 387);
            this.lbTemplateInfo.TabIndex = 10;
            this.lbTemplateInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbInformation_fixed
            // 
            this.lbInformation_fixed.AutoSize = true;
            this.lbInformation_fixed.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbInformation_fixed.Location = new System.Drawing.Point(172, 12);
            this.lbInformation_fixed.Name = "lbInformation_fixed";
            this.lbInformation_fixed.Size = new System.Drawing.Size(95, 19);
            this.lbInformation_fixed.TabIndex = 2;
            this.lbInformation_fixed.Text = "Information";
            // 
            // lbUserName_fixed
            // 
            this.lbUserName_fixed.AutoSize = true;
            this.lbUserName_fixed.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbUserName_fixed.Location = new System.Drawing.Point(7, 157);
            this.lbUserName_fixed.Name = "lbUserName_fixed";
            this.lbUserName_fixed.Size = new System.Drawing.Size(109, 19);
            this.lbUserName_fixed.TabIndex = 6;
            this.lbUserName_fixed.Text = "使用者名稱:";
            // 
            // lbAccount_fixed
            // 
            this.lbAccount_fixed.AutoSize = true;
            this.lbAccount_fixed.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbAccount_fixed.Location = new System.Drawing.Point(7, 61);
            this.lbAccount_fixed.Name = "lbAccount_fixed";
            this.lbAccount_fixed.Size = new System.Drawing.Size(52, 19);
            this.lbAccount_fixed.TabIndex = 7;
            this.lbAccount_fixed.Text = "帳號:";
            // 
            // lbPassword_fixed
            // 
            this.lbPassword_fixed.AutoSize = true;
            this.lbPassword_fixed.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbPassword_fixed.Location = new System.Drawing.Point(7, 108);
            this.lbPassword_fixed.Name = "lbPassword_fixed";
            this.lbPassword_fixed.Size = new System.Drawing.Size(52, 19);
            this.lbPassword_fixed.TabIndex = 8;
            this.lbPassword_fixed.Text = "密碼:";
            // 
            // lbSex_fixed
            // 
            this.lbSex_fixed.AutoSize = true;
            this.lbSex_fixed.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbSex_fixed.Location = new System.Drawing.Point(7, 197);
            this.lbSex_fixed.Name = "lbSex_fixed";
            this.lbSex_fixed.Size = new System.Drawing.Size(52, 19);
            this.lbSex_fixed.TabIndex = 9;
            this.lbSex_fixed.Text = "性別:";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.lbUserInfo_fixed);
            this.panel3.Controls.Add(this.cbSex);
            this.panel3.Controls.Add(this.tbUserName);
            this.panel3.Controls.Add(this.tbPassword);
            this.panel3.Controls.Add(this.tbAccount);
            this.panel3.Controls.Add(this.lbAccount_fixed);
            this.panel3.Controls.Add(this.lbSex_fixed);
            this.panel3.Controls.Add(this.lbUserName_fixed);
            this.panel3.Controls.Add(this.lbPassword_fixed);
            this.panel3.Location = new System.Drawing.Point(21, 12);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(311, 241);
            this.panel3.TabIndex = 10;
            // 
            // lbUserInfo_fixed
            // 
            this.lbUserInfo_fixed.AutoSize = true;
            this.lbUserInfo_fixed.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbUserInfo_fixed.Location = new System.Drawing.Point(108, 20);
            this.lbUserInfo_fixed.Name = "lbUserInfo_fixed";
            this.lbUserInfo_fixed.Size = new System.Drawing.Size(85, 19);
            this.lbUserInfo_fixed.TabIndex = 13;
            this.lbUserInfo_fixed.Text = "個人資料";
            // 
            // cbSex
            // 
            this.cbSex.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.cbSex.FormattingEnabled = true;
            this.cbSex.Location = new System.Drawing.Point(122, 194);
            this.cbSex.Name = "cbSex";
            this.cbSex.Size = new System.Drawing.Size(112, 27);
            this.cbSex.TabIndex = 4;
            // 
            // tbUserName
            // 
            this.tbUserName.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbUserName.Location = new System.Drawing.Point(122, 151);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(112, 30);
            this.tbUserName.TabIndex = 3;
            this.tbUserName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbPassword
            // 
            this.tbPassword.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbPassword.Location = new System.Drawing.Point(122, 105);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(112, 30);
            this.tbPassword.TabIndex = 2;
            this.tbPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tbAccount
            // 
            this.tbAccount.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbAccount.Location = new System.Drawing.Point(122, 58);
            this.tbAccount.Name = "tbAccount";
            this.tbAccount.Size = new System.Drawing.Size(112, 30);
            this.tbAccount.TabIndex = 1;
            this.tbAccount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.btEntrollment);
            this.panel4.Controls.Add(this.btTemplateConfirm);
            this.panel4.Controls.Add(this.btTemplateSelection);
            this.panel4.Location = new System.Drawing.Point(21, 259);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(309, 204);
            this.panel4.TabIndex = 12;
            // 
            // tmUpdateFile
            // 
            this.tmUpdateFile.Enabled = true;
            this.tmUpdateFile.Interval = 500;
            this.tmUpdateFile.Tick += new System.EventHandler(this.tmUpdateFile_Tick);
            // 
            // Entrollment
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1012, 499);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Entrollment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SVS-註冊";
            this.Load += new System.EventHandler(this.Entrollment_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btNewSignature;
        private System.Windows.Forms.Button btTemplateSelection;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lbInformation_fixed;
        private System.Windows.Forms.Label lbTemplateInfo;
        private System.Windows.Forms.Label lbUserName_fixed;
        private System.Windows.Forms.Label lbAccount_fixed;
        private System.Windows.Forms.Label lbPassword_fixed;
        private System.Windows.Forms.Label lbSex_fixed;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox cbSex;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbAccount;
        private System.Windows.Forms.Label lbUserInfo_fixed;
        private System.Windows.Forms.Button btEntrollment;
        private System.Windows.Forms.Button btTemplateConfirm;
        private System.Windows.Forms.CheckedListBox clbNewSignature;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btDelete;
        private System.Windows.Forms.Timer tmUpdateFile;
        private System.Windows.Forms.Button btClearScreen;
        private System.Windows.Forms.Button btDeleteAll;
    }
}