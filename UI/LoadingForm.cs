using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeviceDump.UI
{
    public partial class LoadingForm : Form
    {
        public LoadingForm(Form parent, string text)
        {
            InitializeComponent();

            Text = "Loading";
            Size = new Size(250, 100);
            ControlBox = false;
            TopMost = true;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            ShowInTaskbar = false;

            Label label = new Label()
            {
                Text = text,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill,
                ForeColor = Color.Black,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 10, FontStyle.Regular)
            };

            Controls.Add(label);

            // Manual centering if parent is provided
            if (parent != null)
            {
                StartPosition = FormStartPosition.Manual;
                int x = parent.Location.X + (parent.Width - Width) / 2;
                int y = parent.Location.Y + (parent.Height - Height) / 2;
                Location = new Point(x, y);
            }
            else
            {
                StartPosition = FormStartPosition.CenterScreen;
            }
        }

    }



}
