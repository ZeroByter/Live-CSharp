namespace Run_Live_CSharp
{
    partial class CodeOutput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CodeOutput));
            this.codeOutputField = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // codeOutputField
            // 
            this.codeOutputField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.codeOutputField.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.codeOutputField.Location = new System.Drawing.Point(0, 0);
            this.codeOutputField.Name = "codeOutputField";
            this.codeOutputField.Size = new System.Drawing.Size(695, 424);
            this.codeOutputField.TabIndex = 0;
            this.codeOutputField.Text = "";
            // 
            // CodeOutput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 424);
            this.Controls.Add(this.codeOutputField);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CodeOutput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Code Output";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox codeOutputField;
    }
}