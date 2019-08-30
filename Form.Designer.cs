namespace WebScrapingSelenium {
    partial class Form {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent() {
            this.btnStartMailing = new System.Windows.Forms.Button();
            this.btnFileOpen = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.textBox = new System.Windows.Forms.TextBox();
            this.btnStartFatura = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStartMailing
            // 
            this.btnStartMailing.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold);
            this.btnStartMailing.Location = new System.Drawing.Point(73, 603);
            this.btnStartMailing.Name = "btnStartMailing";
            this.btnStartMailing.Size = new System.Drawing.Size(106, 32);
            this.btnStartMailing.TabIndex = 1;
            this.btnStartMailing.Text = "Robo Mailing";
            this.btnStartMailing.UseVisualStyleBackColor = true;
            this.btnStartMailing.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // btnFileOpen
            // 
            this.btnFileOpen.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFileOpen.Location = new System.Drawing.Point(296, 603);
            this.btnFileOpen.Name = "btnFileOpen";
            this.btnFileOpen.Size = new System.Drawing.Size(106, 32);
            this.btnFileOpen.TabIndex = 2;
            this.btnFileOpen.Text = "Abrir Arquivo";
            this.btnFileOpen.UseVisualStyleBackColor = true;
            this.btnFileOpen.Click += new System.EventHandler(this.BtnFileOpen_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(13, 574);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(461, 23);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar.TabIndex = 3;
            // 
            // textBox
            // 
            this.textBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.textBox.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox.ForeColor = System.Drawing.SystemColors.Window;
            this.textBox.Location = new System.Drawing.Point(12, 12);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ReadOnly = true;
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox.Size = new System.Drawing.Size(462, 556);
            this.textBox.TabIndex = 7;
            // 
            // btnStartFatura
            // 
            this.btnStartFatura.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartFatura.Location = new System.Drawing.Point(185, 603);
            this.btnStartFatura.Name = "btnStartFatura";
            this.btnStartFatura.Size = new System.Drawing.Size(105, 32);
            this.btnStartFatura.TabIndex = 8;
            this.btnStartFatura.Text = "Robo Fatura";
            this.btnStartFatura.UseVisualStyleBackColor = true;
            this.btnStartFatura.Click += new System.EventHandler(this.BtnStartFatura_Click);
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 647);
            this.Controls.Add(this.btnStartFatura);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.btnFileOpen);
            this.Controls.Add(this.btnStartMailing);
            this.Controls.Add(this.progressBar);
            this.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form";
            this.Text = "Robo Mailing";
            this.Load += new System.EventHandler(this.Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnStartMailing;
        private System.Windows.Forms.Button btnFileOpen;
        private System.Windows.Forms.ProgressBar progressBar;
        public System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Button btnStartFatura;
    }
}

