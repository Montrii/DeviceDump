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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            menuStrip1 = new MenuStrip();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            selectDeviceToolStripMenuItem = new ToolStripMenuItem();
            label1 = new Label();
            labelSelectedPhysicalDevice = new Label();
            richTextBoxHexDump = new RichTextBox();
            label2 = new Label();
            labelSizeInBytes = new Label();
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
            menuStrip1.Size = new Size(707, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            optionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { selectDeviceToolStripMenuItem });
            optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            optionsToolStripMenuItem.Size = new Size(43, 20);
            optionsToolStripMenuItem.Text = "Start";
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
            // richTextBoxHexDump
            // 
            richTextBoxHexDump.Font = new Font("Consolas", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richTextBoxHexDump.Location = new Point(12, 118);
            richTextBoxHexDump.Name = "richTextBoxHexDump";
            richTextBoxHexDump.ReadOnly = true;
            richTextBoxHexDump.Size = new Size(683, 320);
            richTextBoxHexDump.TabIndex = 3;
            richTextBoxHexDump.Text = "";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 57);
            label2.Name = "label2";
            label2.Size = new Size(33, 15);
            label2.TabIndex = 4;
            label2.Text = "Size: ";
            // 
            // labelSizeInBytes
            // 
            labelSizeInBytes.AutoSize = true;
            labelSizeInBytes.ForeColor = SystemColors.ControlText;
            labelSizeInBytes.Location = new Point(40, 57);
            labelSizeInBytes.Name = "labelSizeInBytes";
            labelSizeInBytes.Size = new Size(13, 15);
            labelSizeInBytes.TabIndex = 5;
            labelSizeInBytes.Text = "0";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(707, 450);
            Controls.Add(labelSizeInBytes);
            Controls.Add(label2);
            Controls.Add(richTextBoxHexDump);
            Controls.Add(labelSelectedPhysicalDevice);
            Controls.Add(label1);
            Controls.Add(menuStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
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
        private RichTextBox richTextBoxHexDump;
        private Label label2;
        private Label labelSizeInBytes;
    }
}
