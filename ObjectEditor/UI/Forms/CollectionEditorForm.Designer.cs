using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnosoCommons.Configuration.UI.Forms
{
    // This file manually written to add and set new controls over the auto-generated designer file of the base form class.
    partial class CollectionEditorForm
    {
        private Button btnAdd = new System.Windows.Forms.Button();

        private void InitializeComponent()
        {
            CustomPanel.Controls.Add(btnAdd); // Add the Add button to the custom panel of the base form
            //
            // btnAdd
            //
            btnAdd.BackColor = System.Drawing.SystemColors.ControlLight;
            btnAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAdd.Location = new System.Drawing.Point(0, 0);
            btnAdd.Margin = new System.Windows.Forms.Padding(0);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new System.Drawing.Size(282, 23);
            btnAdd.TabIndex = 3;
            btnAdd.Text = "+";
            btnAdd.UseVisualStyleBackColor = false;
            btnAdd.Click += new System.EventHandler(this.Add_Click);
        }
    }
}
