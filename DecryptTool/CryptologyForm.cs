using System;
using System.Drawing;
using System.Windows.Forms;

using kanban.tool.encrypt;
using kanban.tool.decrypt;
using kanban.tool.utils;

namespace kanban.tool.cryptology.form
{
   class CryptologyForm : Form
   {
		protected Label _labelHint;
        protected Button _buttonRun;
        protected Label _labelPassword;
        protected TextBox _textBoxPassword;
        protected int formType;

        public readonly static int ENCRYPT_FORM = 0;
        public readonly static int DECRYPT_FORM = 1;

        protected readonly static string ENCRYPTED_FILE_NAME = "encrypted.dat";
        protected readonly static string DECRYPTED_FILE_NAME = "config.json";

        public CryptologyForm(int formType)
        {
            if(formType != ENCRYPT_FORM && formType != DECRYPT_FORM) {
                return;
            }
            this.formType = formType;

            initialComponents();

            setBounds();

            addComponents();
            
            addListeners();
        }

        private void initialComponents() {
            
            _labelHint = new Label();
            _labelPassword = new Label();
            _textBoxPassword = new TextBox();
            _buttonRun = new Button();

            if(formType == CryptologyForm.ENCRYPT_FORM) {
                Text = "Encrypt Configuration";
                _buttonRun.Text = "Encrypt";
                _labelHint.Text = "Input: " + DECRYPTED_FILE_NAME + "\n\n" + "output: " + ENCRYPTED_FILE_NAME;
            } else {
                Text = "Decrypt Configuration";
                _buttonRun.Text = "Decrypt";
                _labelHint.Text = "Input: " + ENCRYPTED_FILE_NAME + "\n\n" + "output: " + DECRYPTED_FILE_NAME;
            }

            _labelPassword.Text = "input password";
        }

        private void setBounds() {

            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterScreen;
            Icon = new Icon("encrypt-icon.ico");

            _labelHint.Location = new Point(70, 30);
            _labelHint.Font = new Font(_labelHint.Font.FontFamily, 8, FontStyle.Regular);
            _labelHint.Size = new Size(_labelHint.PreferredWidth, _labelHint.PreferredHeight);

            _labelPassword.Location = new Point(70, 110);
            _labelPassword.Font = new Font(_labelPassword.Font.FontFamily, 8, FontStyle.Regular);
            _labelPassword.Size = new Size(_labelPassword.PreferredWidth, _labelPassword.PreferredHeight);

            _textBoxPassword.Location = new Point(100, 130);
            _textBoxPassword.Size = new Size(70, 30);

            _buttonRun.Location = new Point(100, 180);
            _buttonRun.Size = new Size(70, 30);
        }

        private void addComponents() {
            SuspendLayout();
            this.Controls.Add(_labelHint);
            this.Controls.Add(_labelPassword);
            this.Controls.Add(_textBoxPassword);
            this.Controls.Add(_buttonRun);
            ResumeLayout(false);
        }

        private void addListeners() {
            _buttonRun.Click += new EventHandler(buttonRunOnClick);
        }

        private void buttonRunOnClick(object sender, EventArgs args)
        {
            string password = _textBoxPassword.Text;
            Console.WriteLine(password);
            string hash = Utils.MD5Digest(password);
            if(hash != "B8C889954A415DDAFCC86A2D997021") {
                MessageBox.Show("Wrong password.", "Authentication failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string key = password;
            if(formType == ENCRYPT_FORM) {
                EncryptTool tool = EncryptTool.getInstance();
                bool ret = tool.encryptFile(DECRYPTED_FILE_NAME, key, ENCRYPTED_FILE_NAME);
                if(!ret) {
                    MessageBox.Show("Please check your srouce file.", "Encryption failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } else if(formType == DECRYPT_FORM) {
                DecryptTool tool = DecryptTool.getInstance();
                bool ret = tool.decryptFile(ENCRYPTED_FILE_NAME, key, DECRYPTED_FILE_NAME);
                if(!ret) {
                    MessageBox.Show("Please check your srouce file and format.", "Decryption failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}