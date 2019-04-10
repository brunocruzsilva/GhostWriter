namespace VideoWriter
{
    partial class StartForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxNameActor = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonCreatePost = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxNameActor
            // 
            this.textBoxNameActor.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxNameActor.Location = new System.Drawing.Point(46, 68);
            this.textBoxNameActor.Name = "textBoxNameActor";
            this.textBoxNameActor.Size = new System.Drawing.Size(336, 35);
            this.textBoxNameActor.TabIndex = 1;
            this.textBoxNameActor.Text = "Angelina Jolie";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(40, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(189, 31);
            this.label1.TabIndex = 2;
            this.label1.Text = "Nome do ator";
            // 
            // buttonCreatePost
            // 
            this.buttonCreatePost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(187)))), ((int)(((byte)(155)))));
            this.buttonCreatePost.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonCreatePost.FlatAppearance.BorderSize = 0;
            this.buttonCreatePost.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCreatePost.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCreatePost.ForeColor = System.Drawing.Color.White;
            this.buttonCreatePost.Location = new System.Drawing.Point(46, 129);
            this.buttonCreatePost.Name = "buttonCreatePost";
            this.buttonCreatePost.Size = new System.Drawing.Size(112, 39);
            this.buttonCreatePost.TabIndex = 3;
            this.buttonCreatePost.Text = "CRIAR POST";
            this.buttonCreatePost.UseVisualStyleBackColor = false;
            this.buttonCreatePost.Click += new System.EventHandler(this.buttonCreatePost_Click);
            // 
            // StartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(72)))), ((int)(((byte)(89)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(434, 201);
            this.ControlBox = false;
            this.Controls.Add(this.buttonCreatePost);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxNameActor);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StartForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Ghost Writer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxNameActor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCreatePost;
    }
}

