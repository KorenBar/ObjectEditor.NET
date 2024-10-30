namespace ObjectEditor.UI.Forms
{
    partial class ObjectEditorForm
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
            if (disposing)
            {
                SuspendLayout();
                Clear();
                components?.Dispose();
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
            components = new System.ComponentModel.Container();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel1 = new Panel();
            btnOK = new Button();
            btnCancel = new Button();
            btnSave = new Button();
            btnReset = new Button();
            btnApply = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            panel2 = new Panel();
            menuButton = new Button();
            contextMenuStrip1 = new ContextMenuStrip(components);
            resetToolStripMenuItem = new ToolStripMenuItem();
            reloadToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            exportToolStripMenuItem = new ToolStripMenuItem();
            importToolStripMenuItem = new ToolStripMenuItem();
            toolTip1 = new ToolTip(components);
            openFileDialog1 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            loadingPictureBox = new PictureBox();
            tableLayoutPanel1.SuspendLayout();
            panel1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)loadingPictureBox).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel1, 0, 3);
            tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 0, 1);
            tableLayoutPanel1.Controls.Add(panel2, 0, 2);
            tableLayoutPanel1.Controls.Add(menuButton, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 0F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 29F));
            tableLayoutPanel1.Size = new Size(331, 417);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // panel1
            // 
            panel1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            panel1.BackColor = SystemColors.ControlLight;
            panel1.Controls.Add(btnOK);
            panel1.Controls.Add(btnCancel);
            panel1.Controls.Add(btnSave);
            panel1.Controls.Add(btnReset);
            panel1.Controls.Add(btnApply);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 388);
            panel1.Margin = new Padding(0);
            panel1.Name = "panel1";
            panel1.Size = new Size(331, 29);
            panel1.TabIndex = 1;
            // 
            // btnOK
            // 
            btnOK.Dock = DockStyle.Right;
            btnOK.Location = new Point(67, 0);
            btnOK.Margin = new Padding(4, 3, 4, 3);
            btnOK.Name = "btnOK";
            btnOK.Size = new Size(88, 29);
            btnOK.TabIndex = 1;
            btnOK.Text = "OK";
            btnOK.UseVisualStyleBackColor = true;
            btnOK.Click += btnOK_Click;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Dock = DockStyle.Right;
            btnCancel.Location = new Point(155, 0);
            btnCancel.Margin = new Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(88, 29);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.Dock = DockStyle.Left;
            btnSave.Enabled = false;
            btnSave.FlatAppearance.BorderColor = Color.FromArgb(255, 128, 0);
            btnSave.FlatAppearance.BorderSize = 2;
            btnSave.Location = new Point(88, 0);
            btnSave.Margin = new Padding(4, 3, 4, 3);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(88, 29);
            btnSave.TabIndex = 2;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Visible = false;
            btnSave.Click += btnSave_Click;
            // 
            // btnReset
            // 
            btnReset.Dock = DockStyle.Left;
            btnReset.Enabled = false;
            btnReset.Location = new Point(0, 0);
            btnReset.Margin = new Padding(4, 3, 4, 3);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(88, 29);
            btnReset.TabIndex = 0;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Visible = false;
            btnReset.Click += btnReset_Click;
            // 
            // btnApply
            // 
            btnApply.DialogResult = DialogResult.Cancel;
            btnApply.Dock = DockStyle.Right;
            btnApply.Enabled = false;
            btnApply.Location = new Point(243, 0);
            btnApply.Margin = new Padding(4, 3, 4, 3);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(88, 29);
            btnApply.TabIndex = 4;
            btnApply.Text = "Apply";
            btnApply.UseVisualStyleBackColor = true;
            btnApply.Click += btnApply_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 24);
            flowLayoutPanel1.Margin = new Padding(0);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(6);
            flowLayoutPanel1.Size = new Size(331, 364);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 388);
            panel2.Margin = new Padding(0);
            panel2.Name = "panel2";
            panel2.Size = new Size(331, 1);
            panel2.TabIndex = 2;
            // 
            // menuButton
            // 
            menuButton.BackgroundImage = Resources.icons8_menu_dark_gray_32;
            menuButton.BackgroundImageLayout = ImageLayout.Zoom;
            menuButton.Dock = DockStyle.Left;
            menuButton.FlatAppearance.BorderSize = 0;
            menuButton.FlatStyle = FlatStyle.Flat;
            menuButton.Location = new Point(4, 2);
            menuButton.Margin = new Padding(4, 2, 2, 2);
            menuButton.Name = "menuButton";
            menuButton.Size = new Size(32, 20);
            menuButton.TabIndex = 3;
            menuButton.UseVisualStyleBackColor = true;
            menuButton.Click += menuButton_Click;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { resetToolStripMenuItem, reloadToolStripMenuItem, toolStripSeparator1, exportToolStripMenuItem, importToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(111, 98);
            // 
            // resetToolStripMenuItem
            // 
            resetToolStripMenuItem.Image = Resources.icons8_clear_32;
            resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            resetToolStripMenuItem.Size = new Size(110, 22);
            resetToolStripMenuItem.Text = "Reset";
            resetToolStripMenuItem.Click += resetToolStripMenuItem_Click;
            // 
            // reloadToolStripMenuItem
            // 
            reloadToolStripMenuItem.Image = Resources.icons8_refresh_32;
            reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            reloadToolStripMenuItem.Size = new Size(110, 22);
            reloadToolStripMenuItem.Text = "Reload";
            reloadToolStripMenuItem.Click += reloadToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(107, 6);
            // 
            // exportToolStripMenuItem
            // 
            exportToolStripMenuItem.Image = Resources.icons8_export_26;
            exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            exportToolStripMenuItem.Size = new Size(110, 22);
            exportToolStripMenuItem.Text = "Export";
            exportToolStripMenuItem.Click += exportToolStripMenuItem_Click;
            // 
            // importToolStripMenuItem
            // 
            importToolStripMenuItem.Image = Resources.icons8_import_26;
            importToolStripMenuItem.Name = "importToolStripMenuItem";
            importToolStripMenuItem.Size = new Size(110, 22);
            importToolStripMenuItem.Text = "Import";
            importToolStripMenuItem.Click += importToolStripMenuItem_Click;
            // 
            // toolTip1
            // 
            toolTip1.AutoPopDelay = 10000;
            toolTip1.InitialDelay = 500;
            toolTip1.ReshowDelay = 100;
            // 
            // openFileDialog1
            // 
            openFileDialog1.Filter = "XML Files|*.xml;*.config|JSON Files|*.json|All Files|*.*";
            openFileDialog1.FileOk += openFileDialog1_FileOk;
            // 
            // saveFileDialog1
            // 
            saveFileDialog1.Filter = "XML Files|*.xml;*.config";
            saveFileDialog1.FileOk += saveFileDialog1_FileOk;
            // 
            // loadingPictureBox
            // 
            loadingPictureBox.Dock = DockStyle.Fill;
            loadingPictureBox.Image = Resources.infinity_128;
            loadingPictureBox.Location = new Point(0, 0);
            loadingPictureBox.Name = "loadingPictureBox";
            loadingPictureBox.Size = new Size(331, 417);
            loadingPictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
            loadingPictureBox.TabIndex = 3;
            loadingPictureBox.TabStop = false;
            // 
            // ObjectEditorForm
            // 
            AcceptButton = btnOK;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(331, 417);
            Controls.Add(loadingPictureBox);
            Controls.Add(tableLayoutPanel1);
            Margin = new Padding(4, 3, 4, 3);
            MinimumSize = new Size(347, 93);
            Name = "ObjectEditorForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ObjectEditor";
            tableLayoutPanel1.ResumeLayout(false);
            panel1.ResumeLayout(false);
            contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)loadingPictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private PictureBox loadingPictureBox;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem reloadToolStripMenuItem;
        private ToolStripMenuItem resetToolStripMenuItem;
        private Button menuButton;
    }
}