using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace kanban.tool.cryptology.form {
	public class StartEncryptForm {
		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();
		[DllImport("user32.dll")]
		static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
		const int SW_HIDE = 0;
		const int SW_SHOW = 5;
		static void Main(string[] args) {
			ShowWindow(GetConsoleWindow(), SW_HIDE);
            Application.Run(new CryptologyForm(CryptologyForm.ENCRYPT_FORM));
		}
	}
}