using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceDump.UI
{

    public class HexInputForm : Form
    {
        public string UserInput { get; private set; }

        public HexInputForm(string labelText, string description, Func<string, bool>? validate = null)
        {
            if (validate == null)
                throw new ArgumentNullException(nameof(validate));

            this.Text = $"{labelText}";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(350, 180);

            Label label = new Label
            {
                Text = $"{labelText}",
                Location = new Point(10, 20),
                AutoSize = true
            };

            TextBox inputBox = new TextBox
            {
                Location = new Point(130, 18),
                Width = 120
            };

            Label? descriptionLabel = null;
            if (!string.IsNullOrEmpty(description))
            {
                descriptionLabel = new Label
                {
                    Text = description,
                    Location = new Point(10, 50),
                    AutoSize = true,
                    Font = new Font(SystemFonts.DefaultFont, FontStyle.Italic),
                    ForeColor = SystemColors.GrayText
                };
            }

            Button okButton = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new Point(130, 90),
                Width = 60,
                Enabled = false
            };

            inputBox.TextChanged += (s, e) =>
            {
                string text = inputBox.Text;
                if (validate(text))
                {
                    okButton.Enabled = true;
                }
                else
                {
                    okButton.Enabled = false;
                }
            };

            inputBox.KeyPress += (s, e) =>
            {
                inputBox.KeyPress += (s, e) =>
                {
                    if (!char.IsControl(e.KeyChar) &&
                        !char.IsDigit(e.KeyChar) &&
                        !(e.KeyChar >= 'a' && e.KeyChar <= 'f') &&
                        !(e.KeyChar >= 'A' && e.KeyChar <= 'F') &&
                        !(e.KeyChar == 'x' || e.KeyChar == 'X')) // allow typing "0x"
                    {
                        e.Handled = true;
                    }
                };
            };

            okButton.Click += (s, e) =>
            {
                UserInput = inputBox.Text;
                this.Close();
            };

            this.Controls.Add(label);
            this.Controls.Add(inputBox);
            if (descriptionLabel != null)
                this.Controls.Add(descriptionLabel);
            this.Controls.Add(okButton);

            this.AcceptButton = okButton;
        }
    }




}
