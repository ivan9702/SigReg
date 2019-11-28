namespace SVS
{
    public partial class FigureContainer
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
            this.tmLoop = new System.Windows.Forms.Timer(this.components);
            this.tsmiEntrollment = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiDatabase = new System.Windows.Forms.ToolStripMenuItem();
            this.msApplication = new System.Windows.Forms.MenuStrip();
            this.msApplication.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmLoop
            // 
            this.tmLoop.Tick += new System.EventHandler(this.tmLoop_Tick);
            // 
            // tsmiEntrollment
            // 
            this.tsmiEntrollment.Name = "tsmiEntrollment";
            this.tsmiEntrollment.Size = new System.Drawing.Size(43, 20);
            this.tsmiEntrollment.Text = "註冊";
            this.tsmiEntrollment.Click += new System.EventHandler(this.tsmiEntrollment_Click);
            // 
            // tsmiDatabase
            // 
            this.tsmiDatabase.Name = "tsmiDatabase";
            this.tsmiDatabase.Size = new System.Drawing.Size(43, 20);
            this.tsmiDatabase.Text = "登入";
            this.tsmiDatabase.Click += new System.EventHandler(this.tsmiDatabase_Click);
            // 
            // msApplication
            // 
            this.msApplication.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiEntrollment,
            this.tsmiDatabase});
            this.msApplication.Location = new System.Drawing.Point(0, 0);
            this.msApplication.Name = "msApplication";
            this.msApplication.Size = new System.Drawing.Size(939, 24);
            this.msApplication.TabIndex = 0;
            this.msApplication.Text = "應用";
            this.msApplication.Visible = false;
            // 
            // FigureContainer
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(939, 262);
            this.Controls.Add(this.msApplication);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.msApplication;
            this.MaximizeBox = false;
            this.Name = "FigureContainer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            this.Load += new System.EventHandler(this.FigureContainer_Load);
            this.msApplication.ResumeLayout(false);
            this.msApplication.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer tmLoop;
        private System.Windows.Forms.ToolStripMenuItem tsmiEntrollment;
        private System.Windows.Forms.ToolStripMenuItem tsmiDatabase;
        private System.Windows.Forms.MenuStrip msApplication;
    }
}