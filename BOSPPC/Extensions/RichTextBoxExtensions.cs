using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BOSPPC.Extensions
{
	public static class RichTextBoxExtensions
	{
		public static void AppendNewText(this RichTextBox box, string text, Color color)
		{
			box.SelectionStart = box.TextLength;
			box.SelectionLength = 0;

			box.SelectionColor = color;
			box.AppendText("\n" + text + "\n");
			box.SelectionColor = box.ForeColor;
			box.ScrollToCaret();
		}
	}
}
