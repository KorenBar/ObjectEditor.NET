namespace TechnosoCommons.UI.Forms
{
    partial class ControllingForm<T>
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
            this.opacityTrackBar = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.opacityTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // opacityTrackBar
            // 
            this.opacityTrackBar.AutoSize = false;
            this.opacityTrackBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.opacityTrackBar.Location = new System.Drawing.Point(0, 0);
            this.opacityTrackBar.Maximum = 100;
            this.opacityTrackBar.Minimum = 20;
            this.opacityTrackBar.Name = "opacityTrackBar";
            this.opacityTrackBar.Size = new System.Drawing.Size(284, 21);
            this.opacityTrackBar.TabIndex = 0;
            this.opacityTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.opacityTrackBar.Value = 100;
            this.opacityTrackBar.ValueChanged += new System.EventHandler(this.opacityTrackBar_ValueChanged);
            // 
            // ControllingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.opacityTrackBar);
            this.Name = "ControllingForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ControllingForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.opacityTrackBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.TrackBar opacityTrackBar;
    }
}