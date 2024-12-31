namespace HolePositioning
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonSourcePic = new System.Windows.Forms.Button();
            this.pictureBoxSource = new System.Windows.Forms.PictureBox();
            this.buttonExecute = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSource)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonSourcePic
            // 
            this.buttonSourcePic.Location = new System.Drawing.Point(269, 394);
            this.buttonSourcePic.Name = "buttonSourcePic";
            this.buttonSourcePic.Size = new System.Drawing.Size(75, 23);
            this.buttonSourcePic.TabIndex = 0;
            this.buttonSourcePic.Text = "來源圖";
            this.buttonSourcePic.UseVisualStyleBackColor = true;
            this.buttonSourcePic.Click += new System.EventHandler(this.buttonSourcePic_Click);
            // 
            // pictureBoxSource
            // 
            this.pictureBoxSource.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxSource.Name = "pictureBoxSource";
            this.pictureBoxSource.Size = new System.Drawing.Size(458, 346);
            this.pictureBoxSource.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxSource.TabIndex = 1;
            this.pictureBoxSource.TabStop = false;
            // 
            // buttonExecute
            // 
            this.buttonExecute.Location = new System.Drawing.Point(392, 394);
            this.buttonExecute.Name = "buttonExecute";
            this.buttonExecute.Size = new System.Drawing.Size(75, 23);
            this.buttonExecute.TabIndex = 2;
            this.buttonExecute.Text = "執行";
            this.buttonExecute.UseVisualStyleBackColor = true;
            this.buttonExecute.Click += new System.EventHandler(this.buttonExecute_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(483, 433);
            this.Controls.Add(this.buttonExecute);
            this.Controls.Add(this.pictureBoxSource);
            this.Controls.Add(this.buttonSourcePic);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonSourcePic;
        private System.Windows.Forms.PictureBox pictureBoxSource;
        private System.Windows.Forms.Button buttonExecute;
    }
}

