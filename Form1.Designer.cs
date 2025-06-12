namespace DeviceDump
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            selectDeviceToolStripMenuItem = new ToolStripMenuItem();
            label1 = new Label();
            labelSelectedPhysicalDevice = new Label();
            listBoxHexDump = new ListBox();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.BackgroundImageLayout = ImageLayout.None;
            menuStrip1.Items.AddRange(new ToolStripItem[] { optionsToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.RenderMode = ToolStripRenderMode.System;
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { selectDeviceToolStripMenuItem });
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(61, 20);
            optionsToolStripMenuItem.Text = "Options";
            // 
            // selectDeviceToolStripMenuItem
            // 
            selectDeviceToolStripMenuItem.Name = "selectDeviceToolStripMenuItem";
            selectDeviceToolStripMenuItem.Size = new Size(143, 22);
            selectDeviceToolStripMenuItem.Text = "Select Device";
            selectDeviceToolStripMenuItem.DropDownOpening += SelectDeviceToolStripMenuItem_DropDownOpening;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 33);
            label1.Name = "label1";
            label1.Size = new Size(91, 15);
            label1.TabIndex = 1;
            label1.Text = "Physical Device:";
            // 
            // labelSelectedPhysicalDevice
            // 
            labelSelectedPhysicalDevice.AutoSize = true;
            labelSelectedPhysicalDevice.ForeColor = SystemColors.ControlText;
            labelSelectedPhysicalDevice.Location = new Point(101, 33);
            labelSelectedPhysicalDevice.Name = "labelSelectedPhysicalDevice";
            labelSelectedPhysicalDevice.Size = new Size(34, 15);
            labelSelectedPhysicalDevice.TabIndex = 2;
            labelSelectedPhysicalDevice.Text = "none";
            // 
            // listBoxHexDump
            // 
            listBoxHexDump.FormattingEnabled = true;
            listBoxHexDump.ItemHeight = 15;
            listBoxHexDump.Location = new Point(9, 121);
            listBoxHexDump.Name = "listBoxHexDump";
            listBoxHexDump.Size = new Size(779, 319);
            listBoxHexDump.TabIndex = 3;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(800, 450);
            Controls.Add(listBoxHexDump);
            Controls.Add(labelSelectedPhysicalDevice);
            Controls.Add(label1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Device Dumper";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem selectDeviceToolStripMenuItem;
        private Label label1;
        private Label labelSelectedPhysicalDevice;
        private ListBox listBoxHexDump;
    }
}
