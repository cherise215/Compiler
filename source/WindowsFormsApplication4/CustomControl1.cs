using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace WindowsFormsApplication4
{
    public partial class CustomControl1 : RichTextBox
    {
        public CustomControl1()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        
private static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);

//重写OnTextChanged方法：
protected override void OnTextChanged(EventArgs e)
{
            base.OnTextChanged(e);
            SendMessage(base.Handle, 0xB, 0, IntPtr.Zero);
            int sIndex = this.SelectionStart;
            this.SelectAll();
            this.SelectionColor = Color.Black;
            this.Select(sIndex, 0);
            ChangeColor("static", Color.Blue);//调用改变文字颜色的方法
            ChangeColor("void", Color.Blue);
            this.Select(sIndex, 0);
            this.SelectionColor = Color.Black;
            SendMessage(base.Handle, 0xB, 1, IntPtr.Zero);
            this.Refresh();
}

//定义改变文字颜色的私有方法：
public void ChangeColor(string text, Color color)
{
            int s = 0;
            while ((-1 + text.Length - 1) != (s = text.Length - 1 + this.Find(text, s, -1, RichTextBoxFinds.MatchCase | RichTextBoxFinds.WholeWord)))
            {
                        this.SelectionColor = color;
            }
}

    }
}
