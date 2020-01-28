namespace TagRecognition
{
    partial class MainForm
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
            this.LoadImageDialog = new System.Windows.Forms.OpenFileDialog();
            this.btnLoad = new System.Windows.Forms.Button();
            this.lblToLoadImg = new System.Windows.Forms.Label();
            this.btnRecognize = new System.Windows.Forms.Button();
            this.lblResult = new System.Windows.Forms.Label();
            this.lblNowImg = new System.Windows.Forms.Label();
            this.lblAllImg = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.imgBox = new Emgu.CV.UI.ImageBox();
            ((System.ComponentModel.ISupportInitialize)(this.imgBox)).BeginInit();
            this.SuspendLayout();
            // 
            // LoadImageDialog
            // 
            this.LoadImageDialog.Filter = "Image files (*.jpeg; *.jpg)|*.jpeg; *.jpg";
            this.LoadImageDialog.Multiselect = true;
            this.LoadImageDialog.RestoreDirectory = true;
            this.LoadImageDialog.Title = "Load Image";
            // 
            // btnLoad
            // 
            this.btnLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnLoad.Location = new System.Drawing.Point(658, 72);
            this.btnLoad.Margin = new System.Windows.Forms.Padding(4);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(180, 60);
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // lblToLoadImg
            // 
            this.lblToLoadImg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblToLoadImg.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblToLoadImg.Location = new System.Drawing.Point(658, 30);
            this.lblToLoadImg.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblToLoadImg.Name = "lblToLoadImg";
            this.lblToLoadImg.Size = new System.Drawing.Size(180, 30);
            this.lblToLoadImg.TabIndex = 1;
            this.lblToLoadImg.Text = "Please, load your images";
            // 
            // btnRecognize
            // 
            this.btnRecognize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRecognize.Enabled = false;
            this.btnRecognize.Location = new System.Drawing.Point(658, 155);
            this.btnRecognize.Name = "btnRecognize";
            this.btnRecognize.Size = new System.Drawing.Size(180, 60);
            this.btnRecognize.TabIndex = 1;
            this.btnRecognize.Text = "Recognize";
            this.btnRecognize.UseVisualStyleBackColor = true;
            this.btnRecognize.Click += new System.EventHandler(this.btnRecognize_Click);
            // 
            // lblResult
            // 
            this.lblResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblResult.Location = new System.Drawing.Point(658, 295);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(180, 30);
            this.lblResult.TabIndex = 4;
            this.lblResult.Text = "Result:";
            this.lblResult.Visible = false;
            // 
            // lblNowImg
            // 
            this.lblNowImg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNowImg.Location = new System.Drawing.Point(658, 265);
            this.lblNowImg.Name = "lblNowImg";
            this.lblNowImg.Size = new System.Drawing.Size(180, 30);
            this.lblNowImg.TabIndex = 5;
            this.lblNowImg.Text = "Now: ";
            this.lblNowImg.Visible = false;
            // 
            // lblAllImg
            // 
            this.lblAllImg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAllImg.Location = new System.Drawing.Point(658, 235);
            this.lblAllImg.Name = "lblAllImg";
            this.lblAllImg.Size = new System.Drawing.Size(180, 30);
            this.lblAllImg.TabIndex = 6;
            this.lblAllImg.Text = "All: ";
            this.lblAllImg.Visible = false;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Location = new System.Drawing.Point(748, 339);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(90, 30);
            this.btnNext.TabIndex = 7;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Visible = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // imgBox
            // 
            this.imgBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.imgBox.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.imgBox.Location = new System.Drawing.Point(45, 30);
            this.imgBox.Name = "imgBox";
            this.imgBox.Size = new System.Drawing.Size(544, 339);
            this.imgBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imgBox.TabIndex = 2;
            this.imgBox.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 421);
            this.Controls.Add(this.imgBox);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.lblAllImg);
            this.Controls.Add(this.lblNowImg);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.btnRecognize);
            this.Controls.Add(this.lblToLoadImg);
            this.Controls.Add(this.btnLoad);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(550, 425);
            this.Name = "MainForm";
            this.Text = "Tag Recognition";
            ((System.ComponentModel.ISupportInitialize)(this.imgBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog LoadImageDialog;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label lblToLoadImg;
        private System.Windows.Forms.Button btnRecognize;
        private System.Windows.Forms.Label lblResult;
        private System.Windows.Forms.Label lblNowImg;
        private System.Windows.Forms.Label lblAllImg;
        private System.Windows.Forms.Button btnNext;
        private Emgu.CV.UI.ImageBox imgBox;
    }
}

