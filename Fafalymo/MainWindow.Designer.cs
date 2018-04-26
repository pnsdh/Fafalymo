namespace Fafalymo
{
    partial class MainWindow
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblProgress = new System.Windows.Forms.Label();
            this.progLine = new System.Windows.Forms.ProgressBar();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.progFile = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // lblProgress
            // 
            this.lblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgress.Location = new System.Drawing.Point(12, 65);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(245, 24);
            this.lblProgress.TabIndex = 0;
            this.lblProgress.Text = "0 / 0";
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // progLine
            // 
            this.progLine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progLine.Location = new System.Drawing.Point(12, 41);
            this.progLine.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.progLine.Maximum = 1000;
            this.progLine.Name = "progLine";
            this.progLine.Size = new System.Drawing.Size(245, 20);
            this.progLine.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progLine.TabIndex = 1;
            // 
            // ofd
            // 
            this.ofd.Filter = "ACT_FFXIV_PLUGIN log|*.log";
            this.ofd.Multiselect = true;
            // 
            // progFile
            // 
            this.progFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progFile.Location = new System.Drawing.Point(12, 13);
            this.progFile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.progFile.Maximum = 1000;
            this.progFile.Name = "progFile";
            this.progFile.Size = new System.Drawing.Size(245, 20);
            this.progFile.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progFile.TabIndex = 2;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(269, 98);
            this.Controls.Add(this.progFile);
            this.Controls.Add(this.progLine);
            this.Controls.Add(this.lblProgress);
            this.Font = new System.Drawing.Font("맑은 고딕", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "파파리모";
            this.Shown += new System.EventHandler(this.MainWindow_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.ProgressBar progLine;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.ProgressBar progFile;
    }
}

