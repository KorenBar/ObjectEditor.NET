namespace TechnosoCommons.Configuration.UI.Controls
{
    partial class NumericFieldControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.numericBox = new TechnosoCommons.UI.Controls.NumericBox();
            ((System.ComponentModel.ISupportInitialize)(this.numericBox)).BeginInit();
            this.SuspendLayout();
            // 
            // numericBox
            // 
            this.numericBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.numericBox.Location = new System.Drawing.Point(0, 0);
            this.numericBox.Name = "numericBox";
            this.numericBox.Size = new System.Drawing.Size(120, 20);
            this.numericBox.TabIndex = 0;
            // 
            // NumericFieldControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "NumericFieldControl";
            ((System.ComponentModel.ISupportInitialize)(this.numericBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TechnosoCommons.UI.Controls.NumericBox numericBox;
    }
}
