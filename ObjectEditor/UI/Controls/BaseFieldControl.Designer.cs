namespace TechnosoCommons.Configuration.UI.Controls
{
    partial class BaseFieldControl
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
            components = new System.ComponentModel.Container();
            tableLayoutPanel1 = new TableLayoutPanel();
            label1 = new Label();
            btnRemove = new Button();
            valueControlPanel = new Panel();
            toolTip1 = new ToolTip(components);
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 0F));
            tableLayoutPanel1.Controls.Add(label1, 0, 0);
            tableLayoutPanel1.Controls.Add(btnRemove, 2, 0);
            tableLayoutPanel1.Controls.Add(valueControlPanel, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(292, 32);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(4, 0);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Padding = new Padding(6);
            label1.Size = new Size(132, 35);
            label1.TabIndex = 3;
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // btnRemove
            // 
            btnRemove.Dock = DockStyle.Fill;
            btnRemove.Enabled = false;
            btnRemove.ForeColor = Color.Red;
            btnRemove.Location = new Point(296, 4);
            btnRemove.Margin = new Padding(4);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(1, 27);
            btnRemove.TabIndex = 5;
            btnRemove.Text = "x";
            btnRemove.UseVisualStyleBackColor = true;
            btnRemove.Click += BtnRemove_Click;
            // 
            // valueControlPanel
            // 
            valueControlPanel.Dock = DockStyle.Fill;
            valueControlPanel.Location = new Point(143, 3);
            valueControlPanel.Name = "valueControlPanel";
            valueControlPanel.Size = new Size(146, 29);
            valueControlPanel.TabIndex = 6;
            // 
            // toolTip1
            // 
            toolTip1.AutoPopDelay = 10000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 100;
            // 
            // BaseFieldControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tableLayoutPanel1);
            Margin = new Padding(4, 3, 4, 3);
            Name = "BaseFieldControl";
            Size = new Size(292, 32);
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnRemove;
        private Panel valueControlPanel;
    }
}
