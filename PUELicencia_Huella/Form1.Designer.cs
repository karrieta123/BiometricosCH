namespace PUELicencia_Huella
{
    partial class Form1
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
            this.pbhuellas = new System.Windows.Forms.PictureBox();
            this.lblversion = new System.Windows.Forms.Label();
            this.lblerrores = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbhuellas)).BeginInit();
            this.SuspendLayout();
            // 
            // pbhuellas
            // 
            this.pbhuellas.Location = new System.Drawing.Point(12, 12);
            this.pbhuellas.Name = "pbhuellas";
            this.pbhuellas.Size = new System.Drawing.Size(299, 328);
            this.pbhuellas.TabIndex = 0;
            this.pbhuellas.TabStop = false;
            // 
            // lblversion
            // 
            this.lblversion.AutoSize = true;
            this.lblversion.Location = new System.Drawing.Point(13, 359);
            this.lblversion.Name = "lblversion";
            this.lblversion.Size = new System.Drawing.Size(0, 13);
            this.lblversion.TabIndex = 2;
            // 
            // lblerrores
            // 
            this.lblerrores.AutoSize = true;
            this.lblerrores.Font = new System.Drawing.Font("Arial Narrow", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblerrores.ForeColor = System.Drawing.Color.Red;
            this.lblerrores.Location = new System.Drawing.Point(12, 376);
            this.lblerrores.Name = "lblerrores";
            this.lblerrores.Size = new System.Drawing.Size(0, 16);
            this.lblerrores.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 450);
            this.Controls.Add(this.lblerrores);
            this.Controls.Add(this.lblversion);
            this.Controls.Add(this.pbhuellas);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbhuellas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbhuellas;
        private System.Windows.Forms.Label lblversion;
        private System.Windows.Forms.Label lblerrores;
    }
}

