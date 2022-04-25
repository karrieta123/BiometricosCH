namespace Finger
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
            this.pbHuella = new System.Windows.Forms.PictureBox();
            this.btnPulgares = new System.Windows.Forms.Button();
            this.btnCalibrar = new System.Windows.Forms.Button();
            this.lblMensaje = new System.Windows.Forms.Label();
            this.btncerrar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbHuella)).BeginInit();
            this.SuspendLayout();
            // 
            // pbHuella
            // 
            this.pbHuella.Location = new System.Drawing.Point(16, 12);
            this.pbHuella.Name = "pbHuella";
            this.pbHuella.Size = new System.Drawing.Size(470, 414);
            this.pbHuella.TabIndex = 0;
            this.pbHuella.TabStop = false;
            // 
            // btnPulgares
            // 
            this.btnPulgares.Location = new System.Drawing.Point(194, 485);
            this.btnPulgares.Name = "btnPulgares";
            this.btnPulgares.Size = new System.Drawing.Size(75, 23);
            this.btnPulgares.TabIndex = 3;
            this.btnPulgares.Text = "Pulgares";
            this.btnPulgares.UseVisualStyleBackColor = true;
            this.btnPulgares.Click += new System.EventHandler(this.btnPulgares_Click);
            // 
            // btnCalibrar
            // 
            this.btnCalibrar.Location = new System.Drawing.Point(275, 485);
            this.btnCalibrar.Name = "btnCalibrar";
            this.btnCalibrar.Size = new System.Drawing.Size(75, 23);
            this.btnCalibrar.TabIndex = 4;
            this.btnCalibrar.Text = "Calibrar";
            this.btnCalibrar.UseVisualStyleBackColor = true;
            this.btnCalibrar.Click += new System.EventHandler(this.btnCalibrar_Click);
            // 
            // lblMensaje
            // 
            this.lblMensaje.AutoSize = true;
            this.lblMensaje.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMensaje.Location = new System.Drawing.Point(203, 21);
            this.lblMensaje.Name = "lblMensaje";
            this.lblMensaje.Size = new System.Drawing.Size(0, 18);
            this.lblMensaje.TabIndex = 5;
            this.lblMensaje.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btncerrar
            // 
            this.btncerrar.Location = new System.Drawing.Point(365, 485);
            this.btncerrar.Name = "btncerrar";
            this.btncerrar.Size = new System.Drawing.Size(75, 23);
            this.btncerrar.TabIndex = 6;
            this.btncerrar.Text = "Cerrar";
            this.btncerrar.UseVisualStyleBackColor = true;
            this.btncerrar.Click += new System.EventHandler(this.btncerrar_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 520);
            this.Controls.Add(this.btncerrar);
            this.Controls.Add(this.lblMensaje);
            this.Controls.Add(this.btnCalibrar);
            this.Controls.Add(this.btnPulgares);
            this.Controls.Add(this.pbHuella);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbHuella)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbHuella;
        private System.Windows.Forms.Button btnPulgares;
        private System.Windows.Forms.Button btnCalibrar;
        private System.Windows.Forms.Label lblMensaje;
        private System.Windows.Forms.Button btncerrar;
    }
}

