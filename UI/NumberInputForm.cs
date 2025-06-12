using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceDump.UI
{

    // A form that allows the user to input a number value.
    public class NumberInputForm : Form
    {
        public string UserInput { get; private set; }

        public NumberInputForm(string value)
        {
            this.Text = $"Enter {value}";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(300, 150);

            Label label = new Label
            {
                Text = $"Enter {value}",
                Location = new Point(10, 20),
                AutoSize = true
            };

            TextBox textBox = new TextBox
            {
                Location = new Point(100, 18),
                Width = 150
            };

            Button okButton = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new Point(100, 60),
                Width = 60
            };

            okButton.Click += (s, e) =>
            {
                UserInput = textBox.Text;
                this.Close();
            };

            this.Controls.Add(label);
            this.Controls.Add(textBox);
            this.Controls.Add(okButton);

            this.AcceptButton = okButton;
        }
    }
}
