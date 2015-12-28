using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;


/*
 * 
 * 
 * */

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {
         
         private string text;
         private HashSet<NonTerminalSymbol> nl = new HashSet<NonTerminalSymbol>();
         private List<keyInfo> tokenList;
         private GrammarAnalysis g;
         private myLex Lex;
         public Form1(string  s)
        {
            InitializeComponent();

            listView1.Columns.Add("单词", 120, HorizontalAlignment.Left);
            listView1.Columns.Add("种别码", 50, HorizontalAlignment.Left);
            listView1.Columns.Add("类别", 120, HorizontalAlignment.Left);
            listView1.Columns.Add("行号", 50, HorizontalAlignment.Left);
            listView2.Columns.Add("错误类型", 200, HorizontalAlignment.Left);
            listView2.Columns.Add("错误单词", 200, HorizontalAlignment.Left);
            listView2.Columns.Add("错误行", 200, HorizontalAlignment.Left);
            listView3.Columns.Add("类型", 100, HorizontalAlignment.Left);
            listView3.Columns.Add("变量名称", 100, HorizontalAlignment.Left);
            listView3.Columns.Add("所属类型", 100, HorizontalAlignment.Left);
            listView3.Columns.Add("变量长度", 100, HorizontalAlignment.Left);
            listView3.Columns.Add("内存地址", 100, HorizontalAlignment.Left);
            listView3.Columns.Add("作用域", 100, HorizontalAlignment.Left);
            dataGridView3.ForeColor = Color.Red;
            listView2.ForeColor = Color.Red;
            this.Text = s;

        }



       
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            panel1.Invalidate();
        }

        private void richTextBox1_VScroll(object sender, EventArgs e)
        {
            panel1.Invalidate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            showLineNo();
        }

        private void showLineNo()

        {
            //获得当前坐标信息
            Point p = this.richTextBox1.Location;
            int crntFirstIndex = this.richTextBox1.GetCharIndexFromPosition(p);
            int crntFirstLine = this.richTextBox1.GetLineFromCharIndex(crntFirstIndex);
            Point crntFirstPos = this.richTextBox1.GetPositionFromCharIndex(crntFirstIndex);
            //
            p.Y += this.richTextBox1.Height;
            int crntLastIndex = this.richTextBox1.GetCharIndexFromPosition(p);
            int crntLastLine = this.richTextBox1.GetLineFromCharIndex(crntLastIndex);
            Point crntLastPos = this.richTextBox1.GetPositionFromCharIndex(crntLastIndex);
             //准备画图
            Graphics g = this.panel1.CreateGraphics();
            Font font = new Font(this.richTextBox1.Font,this.richTextBox1.Font.Style);
            SolidBrush brush = new SolidBrush(Color.Green);

             //画图开始

             //刷新画布
            Rectangle rect = this.panel1.ClientRectangle;
            brush.Color = this.panel1.BackColor;
            g.FillRectangle(brush, 0, 0, this.panel1.ClientRectangle.Width,this.panel1.ClientRectangle.Height);
            brush.Color = Color.Green;//重置画笔颜色
             //绘制行号
            int lineSpace = 0;
            if (crntFirstLine != crntLastLine)
            {
                lineSpace = (crntLastPos.Y - crntFirstPos.Y) / (crntLastLine - crntFirstLine);
            }
            else
            {
                lineSpace =(int)( Convert.ToInt32(this.richTextBox1.Font.Size)*1.6 );
            }
            int brushX = this.panel1.ClientRectangle.Width - Convert.ToInt32(font.Size * 3);
            int brushY = crntLastPos.Y+ Convert.ToInt32(font.Size*0.21f);
            for (int i = crntLastLine; i >= 0;i-- )
            {
                g.DrawString((i + 1).ToString(), font, brush, brushX, brushY);
                brushY -= lineSpace;
            }
            g.Dispose();
            font.Dispose();
            brush.Dispose();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private int getRowCount()
        {
          
            string text=richTextBox1.Text;
            int inputIndex=0;
            int row=0;
            while(inputIndex<text.Length)
            {
                if(text[inputIndex]=='\n')
                    row++;
                inputIndex++;
               
            }
            return row;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            showLineNo();
            if (!textBox1.Text.Trim().Equals(""))
            {
                int lineNumber = Convert.ToInt32(textBox1.Text.Trim());

                int start = richTextBox1.GetFirstCharIndexFromLine(lineNumber - 1);

                this.richTextBox1.SelectionStart = start;
                this.richTextBox1.ScrollToCaret();
                this.richTextBox1.Focus();
            }
            else
            {
                MessageBox.Show("请输入行号");
            }
           // textBox2.Text = richTextBox1.Lines[lineNumber];
        }

        private void richTextBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void richTextBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Point p = new Point(e.X,e.Y);
            int crntFirstIndex = this.richTextBox1.GetCharIndexFromPosition(p);
            int lineNumber =richTextBox1.GetLineFromCharIndex(crntFirstIndex);
            //if(lineNumber<richTextBox1.Lines.Length)
               // textBox2.Text=richTextBox1.Lines[lineNumber];
        }
        /*
         *
         */
        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Reset();
            openFileDialog1.Filter = "C程序(*.c)|*.c|所有文件|*";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.Multiselect = false;
            openFileDialog1.Title = "打开文件";
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                richTextBox1.LoadFile(openFileDialog1.FileName, RichTextBoxStreamType.PlainText);

               /* isopenFile = true;
                istextChanged = false;
                this.tsBtnY.Text = Path.GetFileName(openFileDialog1.FileName) + "-源程序";
                fileAddress = openFileDialog1.FileName;*/
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        public string sendText()
        {
            return richTextBox1.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text.TrimEnd().Length == 0)
            {
                MessageBox.Show("输入的为空！请重新输入");
            }
            else
            {
               // MessageBox.Show("haha");
                listView1.Items.Clear();
                listView2.Items.Clear();
               // listView3.Items.Clear();
                dataGridView3.Rows.Clear();
                string a=richTextBox1.Text;
                if (richTextBox1.Text.Length > 100000)
                {
                    MessageBox.Show("文件过大！");
                    return;
                }

                Lex = new myLex();
                Lex.start(richTextBox1.Text);
                foreach (ERROR  em in Lex.getError())
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.SubItems.Clear();
                    lvi.SubItems[0].Text = em.getErrorKind();
                    lvi.SubItems.Add(em.getStrError());
                    lvi.SubItems.Add(em.getRow().ToString());
                    listView2.Items.Add(lvi);
                    dataGridView3.Rows.Add(em.getErrorKind(), em.getStrError(), em.getRow().ToString());
                }

                foreach (keyInfo en in Lex.getTokenList())
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.SubItems.Clear();
                    lvi.SubItems[0].Text = en.getName();
                    lvi.SubItems.Add(en.getCode().ToString());
                    lvi.SubItems.Add(en.getType().ToString());
                    lvi.SubItems.Add(en.getRow().ToString());

                    listView1.Items.Add(lvi);
                }
               
             //   WordAnalysis.writeToken();
               // WordAnalysis.writeWord();
            }
        }

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            openFileDialog1.FileName = "";
            listView1.Items.Clear();
            listView2.Items.Clear();
            listView3.Items.Clear();
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 词法编译ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button2_Click(词法编译ToolStripMenuItem, null);


        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 保存ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
           
        }

        private void 另存为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                    string[] tempArray = richTextBox1.Lines;
                    for (int counter = 0; counter < tempArray.Length; counter++)
                    {
                        sw.WriteLine(tempArray[counter]);
                    }
                    sw.Flush();
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void 查看种别码对应表ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 保存ToolStripMenuItem_Click_2(object sender, EventArgs e)
        {

          //  MessageBox.Show(openFileDialog1.FileName);
                if (openFileDialog1.FileName == "openFileDialog1")
                {
                    try
                    {
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            FileStream fs2 = new FileStream(saveFileDialog1.FileName, FileMode.Create);
                            StreamWriter sw2 = new StreamWriter(fs2, Encoding.Default);
                            string[] tempArray = richTextBox1.Lines;
                            for (int counter = 0; counter < tempArray.Length; counter++)
                            {
                                sw2.WriteLine(tempArray[counter]);
                            }
                            sw2.Flush();
                            sw2.Close();
                            sw2.Dispose();
                            MessageBox.Show("保存成功！");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                else {
                try{
                FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                string[] tempArray2 = richTextBox1.Lines;
                for (int counter = 0; counter < tempArray2.Length; counter++)
                {
                    sw.WriteLine(tempArray2[counter]);
                }
                sw.Flush();
                sw.Close();
                sw.Dispose();
                MessageBox.Show("保存成功！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
                }
        
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            MessageBox.Show("sdj");
        }

        private void 生成种别码映射文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {

          
            
        }

        private void 种别码映射文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    FileStream fs2 = new FileStream(saveFileDialog1.FileName, FileMode.Create);
                    StreamWriter sw2 = new StreamWriter(fs2, Encoding.Default);
                    Scanner sc=new Scanner();
                    List<string> list = sc.writeToken();
                    HashSet<dict> map = sc.getMap();
                    foreach (dict str in map)
                    {
                        sw2.WriteLine(str.getId()+" "+str.getCode());
                    }
                    sw2.Flush();
                    sw2.Close();
                    sw2.Dispose();
                    MessageBox.Show("生成成功！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void 语法分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button3_Click(语法分析ToolStripMenuItem, null);
          
          
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();
            button2_Click(词法编译ToolStripMenuItem, null);
            listView3.Items.Clear();
            dataGridView3.Rows.Clear();
            List<ERROR> el ;
            if (Lex.getTokenList() != null)
            {
                tokenList = Lex.getTokenList();

                if (!Text.TrimEnd().Equals("") && tokenList != null)
                {
                    g = new GrammarAnalysis(Text);
                    g.getGrammarFromFile();
                    g.myParser(tokenList);
                    List<parseResult> rl = g.getResultList();
                     el = g.getErrorList();
                    List<string> threel = g.getThreeList();

                    Form f = new Form3(rl, el, tokenList,threel);
                    f.Show();
                }
            }
            foreach (ERROR em in g.getErrorList())
            {
                ListViewItem lvi = new ListViewItem();
                lvi.SubItems.Clear();
                lvi.SubItems[0].Text = em.getErrorKind();
                lvi.SubItems.Add(em.getStrError());
                lvi.SubItems.Add(em.getRow().ToString());
                listView2.Items.Add(lvi);
                dataGridView3.Rows.Add(em.getErrorKind(), em.getStrError(), em.getRow().ToString());
            }
            
            

                
                List<string> threeli = g.getThreeList();
                dataGridView1.Rows.Clear();
                foreach (string term in threeli)
                {

                      dataGridView1.Rows.Add(term);
                        
                }
               Stack<SymbolTable> sm= g.getST();
               dataGridView2.Rows.Clear();
                foreach (SymbolTable sym in sm)
                {
                    dataGridView2.Rows.Add(sym.getReturnType(), sym.getFun_name(),sym.getOffset());
                 
                    
                }



         

        }

        private void minic语言文法预测分析表ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void listView4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (!dataGridView2.CurrentRow.IsNewRow)
            {
                string funname = dataGridView2.CurrentRow.Cells[1].Value.ToString();
               Stack<SymbolTable> stm= g.getST();
               listView3.Items.Clear();
               foreach (SymbolTable sy in stm)
               {
                   if (sy.getFun_name().Equals(funname))
                   {
                       foreach (Variable va in sy.getParamList())
                       {
                           ListViewItem lvi = new ListViewItem();

                           lvi.SubItems[0].Text = "参数";
                           lvi.SubItems.Add(va.getVarName());
                           lvi.SubItems.Add(va.getType());
                           lvi.SubItems.Add(va.getLength() + "");
                           lvi.SubItems.Add(va.getAddr() + "");
                           lvi.SubItems.Add(funname);
                           listView3.Items.Add(lvi);

                       }
                       foreach (Variable va in sy.getVariableList())
                       {
                           ListViewItem lvi = new ListViewItem();
                           lvi.SubItems.Clear();
                           lvi.SubItems[0].Text = "变量";
                           lvi.SubItems.Add(va.getVarName());
                           lvi.SubItems.Add(va.getType());
                           lvi.SubItems.Add(va.getLength() + "");
                           lvi.SubItems.Add(va.getAddr() + "");
                           lvi.SubItems.Add(funname);
                           listView3.Items.Add(lvi);


                       }


                   }

               }

            }
        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView3_SelectionChanged(object sender, EventArgs e)
        {
            if (!dataGridView3.CurrentRow.IsNewRow)
            {
                string row = dataGridView3.CurrentRow.Cells[2].Value.ToString();
                  showLineNo();
                  int lineNumber = Convert.ToInt32(row);

                    int start = richTextBox1.GetFirstCharIndexFromLine(lineNumber - 1);

                    this.richTextBox1.SelectionStart = start;
                    this.richTextBox1.ScrollToCaret();
                    this.richTextBox1.Focus();
                    HighLightText(lineNumber);
              
               
            }

        }


        private void HighLightText(int row)
        {
            string text = richTextBox1.Text.ToString();
            string[] ss = text.Split('\n');


            richTextBox1.SelectAll();
           

            richTextBox1.SelectionBackColor = Color.Black;
            int count=1;
           int i=0;
            foreach (string term in ss)
            {
                if (count == row)
                {
                    richTextBox1.Select(i, term.Length);
                    richTextBox1.SelectionBackColor = Color.Red;
 
                    break;
                }
                i += term.Length+1;
                count++;
            }
            // richTextBox1.Refresh();


        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

       



    }
}
