namespace ObjectEditor.WinForms.Controls
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
            panel1 = new Panel();
            nullLabel = new Label();
            viewControlPanel = new Panel();
            toolTip1 = new ToolTip(components);
            fieldMenu = new ContextMenuStrip(components);
            copyToolStripMenuItem = new ToolStripMenuItem();
            linkToolStripMenuItem = new ToolStripMenuItem();
            pasteToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            setNullToolStripMenuItem = new ToolStripMenuItem();
            createDefaultToolStripMenuItem = new ToolStripMenuItem();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            fieldMenu.SuspendLayout();
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
            tableLayoutPanel1.Controls.Add(panel1, 1, 0);
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
            // panel1
            // 
            panel1.Controls.Add(nullLabel);
            panel1.Controls.Add(viewControlPanel);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(143, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(146, 29);
            panel1.TabIndex = 6;
            // 
            // nullLabel
            // 
            nullLabel.Dock = DockStyle.Fill;
            nullLabel.ForeColor = SystemColors.GrayText;
            nullLabel.Location = new Point(0, 0);
            nullLabel.Name = "nullLabel";
            nullLabel.Size = new Size(146, 29);
            nullLabel.TabIndex = 0;
            nullLabel.Text = "null";
            nullLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // valueControlPanel
            // 
            viewControlPanel.Dock = DockStyle.Fill;
            viewControlPanel.Location = new Point(0, 0);
            viewControlPanel.Name = "valueControlPanel";
            viewControlPanel.Size = new Size(146, 29);
            viewControlPanel.TabIndex = 6;
            // 
            // toolTip1
            // 
            toolTip1.AutoPopDelay = 10000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 100;
            // 
            // fieldMenu
            // 
            fieldMenu.Items.AddRange(new ToolStripItem[] { copyToolStripMenuItem, linkToolStripMenuItem, pasteToolStripMenuItem, toolStripSeparator1, setNullToolStripMenuItem, createDefaultToolStripMenuItem });
            fieldMenu.Name = "fieldMenu";
            fieldMenu.Size = new Size(181, 142);
            fieldMenu.Opening += FieldMenu_Opening;
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.Size = new Size(180, 22);
            copyToolStripMenuItem.Text = "Copy";
            copyToolStripMenuItem.Click += CopyToolStripMenuItem_Click;
            // 
            // linkToolStripMenuItem
            // 
            linkToolStripMenuItem.Name = "linkToolStripMenuItem";
            linkToolStripMenuItem.Size = new Size(180, 22);
            linkToolStripMenuItem.Text = "Link";
            linkToolStripMenuItem.Click += LinkToolStripMenuItem_Click;
            // 
            // pasteToolStripMenuItem
            // 
            pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            pasteToolStripMenuItem.Size = new Size(180, 22);
            pasteToolStripMenuItem.Text = "Paste";
            pasteToolStripMenuItem.Click += PasteToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(177, 6);
            // 
            // setNullToolStripMenuItem
            // 
            setNullToolStripMenuItem.Name = "setNullToolStripMenuItem";
            setNullToolStripMenuItem.Size = new Size(180, 22);
            setNullToolStripMenuItem.Text = "Set null";
            setNullToolStripMenuItem.Click += SetNullToolStripMenuItem_Click;
            // 
            // createDefaultToolStripMenuItem
            // 
            createDefaultToolStripMenuItem.Name = "createDefaultToolStripMenuItem";
            createDefaultToolStripMenuItem.Size = new Size(180, 22);
            createDefaultToolStripMenuItem.Text = "Create default";
            createDefaultToolStripMenuItem.Click += CreateDefaultToolStripMenuItem_Click;
            // 
            // BaseFieldControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ContextMenuStrip = fieldMenu;
            Controls.Add(tableLayoutPanel1);
            Margin = new Padding(4, 3, 4, 3);
            Name = "BaseFieldControl";
            Size = new Size(292, 32);
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            fieldMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnRemove;
        private Panel panel1;
        private Panel viewControlPanel;
        private Label nullLabel;
        private ContextMenuStrip fieldMenu;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem linkToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem setNullToolStripMenuItem;
        private ToolStripMenuItem createDefaultToolStripMenuItem;
    }
}
