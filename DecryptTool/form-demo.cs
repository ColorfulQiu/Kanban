using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MyNamespace
{
   class HelloWorldForm : Form
   {
		Label lblHello;
        Button btnClose;
        public HelloWorldForm()
        {
            this.Text="Hello World Sample";
            btnClose = new Button();
            lblHello = new Label();
            btnClose.Click += new EventHandler(btnClose_Click);
            btnClose.Text="Close";
            btnClose.Location = new Point(10,100);
            btnClose.Size = new Size(200,50);
            lblHello.Text = "Hello World!";
            lblHello.Location = new Point(10, 10);
            lblHello.Size = new Size(200,50);
            SuspendLayout();
            this.Controls.Add(lblHello);
            this.Controls.Add(btnClose);
            ResumeLayout(false);
        }

        void btnClose_Click(object sender, EventArgs args)
        {
            this.Close();
        }
    }
	
	class MainTest {
		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();
		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		const int SW_HIDE = 0;
		const int SW_SHOW = 5;
		static void Main(string[] args) {
			ShowWindow(GetConsoleWindow(), SW_HIDE);
            Application.Run(new HelloWorldForm());
		}
	}
}