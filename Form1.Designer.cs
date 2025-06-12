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
            menuStripToolbar = new MenuStrip();
            optionsToolStripMenuItem = new ToolStripMenuItem();
            selectDeviceToolStripMenuItem = new ToolStripMenuItem();
            label1 = new Label();
            labelSelectedPhysicalDevice = new Label();
            richTextBoxHexDump = new RichTextBox();
            label2 = new Label();
            labelSizeInBytes = new Label();
            label3 = new Label();
            labelBytesRead = new Label();
            label4 = new Label();
            labelDeviceUsed = new Label();
            navigationToolStripMenuItem = new ToolStripMenuItem();
            readBytesFromDeviceToolStripMenuItem = new ToolStripMenuItem();
            menuStripToolbar.SuspendLayout();
            SuspendLayout();
            // 
            // menuStripToolbar
            // 
            menuStripToolbar.BackgroundImageLayout = ImageLayout.None;
            menuStripToolbar.Items.AddRange(new ToolStripItem[] { optionsToolStripMenuItem, navigationToolStripMenuItem });
            menuStripToolbar.Location = new Point(0, 0);
            menuStripToolbar.Name = "menuStripToolbar";
            menuStripToolbar.RenderMode = ToolStripRenderMode.System;
            menuStripToolbar.Size = new Size(707, 24);
            menuStripToolbar.TabIndex = 0;
            menuStripToolbar.Text = "menuStrip1";
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
            label1.Location = new Point(12, 30);
            label1.Name = "label1";
            label1.Size = new Size(91, 15);
            label1.TabIndex = 1;
            label1.Text = "Physical Device:";
            // 
            // labelSelectedPhysicalDevice
            // 
            labelSelectedPhysicalDevice.AutoSize = true;
            labelSelectedPhysicalDevice.ForeColor = SystemColors.ControlText;
            labelSelectedPhysicalDevice.Location = new Point(101, 30);
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
            label2.Location = new Point(12, 49);
            label2.Name = "label2";
            label2.Size = new Size(58, 15);
            label2.TabIndex = 4;
            label2.Text = "Total size:";
            // 
            // labelSizeInBytes
            // 
            labelSizeInBytes.AutoSize = true;
            labelSizeInBytes.ForeColor = SystemColors.ControlText;
            labelSizeInBytes.Location = new Point(70, 49);
            labelSizeInBytes.Name = "labelSizeInBytes";
            labelSizeInBytes.Size = new Size(13, 15);
            labelSizeInBytes.TabIndex = 5;
            labelSizeInBytes.Text = "0";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 69);
            label3.Name = "label3";
            label3.Size = new Size(64, 15);
            label3.TabIndex = 6;
            label3.Text = "Bytes read:";
            // 
            // labelBytesRead
            // 
            labelBytesRead.AutoSize = true;
            labelBytesRead.ForeColor = SystemColors.ControlText;
            labelBytesRead.Location = new Point(74, 69);
            labelBytesRead.Name = "labelBytesRead";
            labelBytesRead.Size = new Size(13, 15);
            labelBytesRead.TabIndex = 7;
            labelBytesRead.Text = "0";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 90);
            label4.Name = "label4";
            label4.Size = new Size(94, 15);
            label4.TabIndex = 8;
            label4.Text = "Device used? -> ";
            // 
            // labelDeviceUsed
            // 
            labelDeviceUsed.AutoSize = true;
            labelDeviceUsed.ForeColor = SystemColors.ControlText;
            labelDeviceUsed.Location = new Point(100, 90);
            labelDeviceUsed.Name = "labelDeviceUsed";
            labelDeviceUsed.Size = new Size(0, 15);
            labelDeviceUsed.TabIndex = 9;
            // 
            // navigationToolStripMenuItem
            // 
            navigationToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { readBytesFromDeviceToolStripMenuItem });
            navigationToolStripMenuItem.Name = "navigationToolStripMenuItem";
            navigationToolStripMenuItem.Size = new Size(77, 20);
            navigationToolStripMenuItem.Text = "Navigation";
            // 
            // readBytesFromDeviceToolStripMenuItem
            // 
            readBytesFromDeviceToolStripMenuItem.Name = "readBytesFromDeviceToolStripMenuItem";
            readBytesFromDeviceToolStripMenuItem.Size = new Size(200, 22);
            readBytesFromDeviceToolStripMenuItem.Text = "Read Bytes From Device";
            readBytesFromDeviceToolStripMenuItem.Click += readBytesFromDeviceToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(707, 450);
            Controls.Add(labelDeviceUsed);
            Controls.Add(label4);
            Controls.Add(labelBytesRead);
            Controls.Add(label3);
            Controls.Add(labelSizeInBytes);
            Controls.Add(label2);
            Controls.Add(richTextBoxHexDump);
            Controls.Add(labelSelectedPhysicalDevice);
            Controls.Add(label1);
            Controls.Add(menuStripToolbar);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStripToolbar;
            Name = "Form1";
            Text = "Device Dumper - by Montri";
            menuStripToolbar.ResumeLayout(false);
            menuStripToolbar.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStripToolbar;
        private ToolStripMenuItem optionsToolStripMenuItem;
        private ToolStripMenuItem selectDeviceToolStripMenuItem;
        private Label label1;
        private Label labelSelectedPhysicalDevice;
        private RichTextBox richTextBoxHexDump;
        private Label label2;
        private Label labelSizeInBytes;
        private Label label3;
        private Label labelBytesRead;
        private Label label4;
        private Label labelDeviceUsed;
        private ToolStripMenuItem navigationToolStripMenuItem;
        private ToolStripMenuItem readBytesFromDeviceToolStripMenuItem;
    }
}
