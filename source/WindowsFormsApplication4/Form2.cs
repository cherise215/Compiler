using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication4
{
    public partial class Form2 : Form
    {
        private string text;
        private HashSet<NonTerminalSymbol> nl = new HashSet<NonTerminalSymbol>();
        private List<keyInfo> tokenList;
        private GrammarAnalysis g;
        public Form2()
        {
        

            InitializeComponent();
            listView1.Columns.Add("编号", 50, HorizontalAlignment.Left);
            listView1.Columns.Add("左部", 100, HorizontalAlignment.Left);
            listView1.Columns.Add("产生式", 250, HorizontalAlignment.Left);
            listView1.Columns.Add("select集合", 150, HorizontalAlignment.Left);
            listView1.Columns.Add("follow集合", 150, HorizontalAlignment.Left);
            listView1.Columns.Add("first集合", 100, HorizontalAlignment.Left);
            listView2.Columns.Add("栈符号", 100, HorizontalAlignment.Left);
            listView2.Columns.Add("输入符号", 200, HorizontalAlignment.Left);
            listView2.Columns.Add("操作", 300, HorizontalAlignment.Left);
          
           
          

              

           

           

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Reset();
            openFileDialog1.Filter = "文本文件(*.txt)|*.txt|所有文件|*";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.Multiselect = false;
            openFileDialog1.Title = "打开文件";
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                GrammarEditor.LoadFile(openFileDialog1.FileName, RichTextBoxStreamType.PlainText);

                /* isopenFile = true;
                 istextChanged = false;
                 this.tsBtnY.Text = Path.GetFileName(openFileDialog1.FileName) + "-源程序";
                 fileAddress = openFileDialog1.FileName;*/
            }
        }

       

        private void button2_Click(object sender, EventArgs e)
        {

           string s= GrammarEditor.Text;
            int i=0;

            listView1.Items.Clear();
            listView2.Items.Clear();
            listBox1.DataSource = null;
            listBox3.DataSource = null;
              nl = new HashSet<NonTerminalSymbol>();

           if (s.Trim() != "")
           {
              g=new GrammarAnalysis(s);

               //MessageBox.Show("sdjhj");
               int count = 1;
              foreach (Grammar gm in g.getGrammarFromFile())
               {
                
                  

                  // MessageBox.Show(gm.getRightPart().Count+"");

                   ListViewItem lvi = new ListViewItem();
                   lvi.SubItems[0].Text = count+ "";
                   lvi.SubItems.Add(gm.getLeftPart());
                   lvi.SubItems.Add(gm.getmGrammar());
                   string selectset="";
               
                 foreach (string sm in gm.getSelect() )
                  {

                      selectset+=sm+" ";
                  }
                   lvi.SubItems.Add(selectset);
                   string df = "";
               
                  foreach (string ss in g.mapGet(gm.getLeftPart()).getFollowList())
                   {
                       df+=ss + " ";
                   }
                   lvi.SubItems.Add(df);
                   df = "";
                   foreach (string sr in g.mapGet(gm.getLeftPart()).getFirst())
                   {
                       df += sr + " ";
                   }
               
                  lvi.SubItems.Add(df);
                  
                       count++;
                        listView1.Items.Add(lvi); 
                  
                       
                   }


               


              listBox1.DataSource = g.getNquene();
              listBox3.DataSource = g.getTquene();
              nl = g.getNlist();

              FileStream fs2 = new FileStream("expression.txt", FileMode.Create);
              StreamWriter sw2 = new StreamWriter(fs2, Encoding.Default);
              foreach (NonTerminalSymbol n in nl)
              {

                  HashSet<Table> t = n.getTable();

                  foreach (Table tt in t)
                  {
                      string item1 = tt.getName();
                      List<string> s2 = tt.getExpression();
                      string item2 = "";
                      foreach (string ss3 in s2)
                      {
                          item2 += ss3 + " ";

                      }
                 
                    sw2.WriteLine(n.getValue() + "#" + item1 + "#" + item2);
                


                  }
              }

             
              sw2.Flush();
              sw2.Close();
              sw2.Dispose();
                 
               }
           
           else
           {
               MessageBox.Show("无法进行文法分析。文法为空");
           }
           
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            

            
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           
            String i = (String)listBox1.SelectedItem;
            // MessageBox.Show(i);
            listView2.Items.Clear();

           
            foreach (NonTerminalSymbol n in nl)
            {
                if (n.getValue().Equals(i))
                {
                    HashSet<Table> t = n.getTable();

                    foreach (Table tt in t)
                    {
                        string item1 = tt.getName();
                        List<string> s2 = tt.getExpression();
                        string item2 = "";
                        foreach (string s in s2)
                        {
                            item2 += s + " ";

                        }
                        ListViewItem lvi = new ListViewItem();
                        lvi.SubItems[0].Text = n.getValue();

                        lvi.SubItems.Add(item1);
                        lvi.SubItems.Add(item2);
                        listView2.Items.Add(lvi);
                      


                    }
                    break;

                }
                else
                    continue;
            }

           
            

        }

        private void button3_Click_1(object sender, EventArgs e)
        {

            if (GrammarEditor.Text.TrimEnd() != "")
            {
                string s = GrammarEditor.Text.TrimEnd();
                Form f = new Form1(s);
                f.Show();
            }
            else
            {
                MessageBox.Show("文法不能为空！");
            }

            
         

              
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string x = GrammarEditor.Text;
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void 打开文法文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.Reset();
            openFileDialog1.Filter = "文本文件(*.txt)|*.txt|所有文件|*";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            openFileDialog1.Multiselect = false;
            openFileDialog1.Title = "打开文件";
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                GrammarEditor.LoadFile(openFileDialog1.FileName, RichTextBoxStreamType.PlainText);

                /* isopenFile = true;
                 istextChanged = false;
                 this.tsBtnY.Text = Path.GetFileName(openFileDialog1.FileName) + "-源程序";
                 fileAddress = openFileDialog1.FileName;*/
            }
        }

        private void 文法分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string s = GrammarEditor.Text;
            int i = 0;

            listView1.Items.Clear();
            listView2.Items.Clear();
            listBox1.DataSource = null;
            listBox3.DataSource = null;
            nl = new HashSet<NonTerminalSymbol>();

            if (s.Trim() != "")
            {
                g = new GrammarAnalysis(s);

                //MessageBox.Show("sdjhj");
                int count = 1;
                foreach (Grammar gm in g.getGrammarFromFile())
                {



                    // MessageBox.Show(gm.getRightPart().Count+"");

                    ListViewItem lvi = new ListViewItem();
                    lvi.SubItems[0].Text = count + "";
                    lvi.SubItems.Add(gm.getLeftPart());
                    lvi.SubItems.Add(gm.getmGrammar());
                    string selectset = "";

                    foreach (string sm in gm.getSelect())
                    {

                        selectset += sm + " ";
                    }
                    lvi.SubItems.Add(selectset);
                    string df = "";

                    foreach (string ss in g.mapGet(gm.getLeftPart()).getFollowList())
                    {
                        df += ss + " ";
                    }
                    lvi.SubItems.Add(df);
                    df = "";
                    foreach (string sr in g.mapGet(gm.getLeftPart()).getFirst())
                    {
                        df += sr + " ";
                    }

                    lvi.SubItems.Add(df);

                    count++;
                    listView1.Items.Add(lvi);


                }





                listBox1.DataSource = g.getNquene();
                listBox3.DataSource = g.getTquene();
                nl = g.getNlist();

                FileStream fs2 = new FileStream("expression.txt", FileMode.Create);
                StreamWriter sw2 = new StreamWriter(fs2, Encoding.Default);
                foreach (NonTerminalSymbol n in nl)
                {

                    HashSet<Table> t = n.getTable();

                    foreach (Table tt in t)
                    {
                        string item1 = tt.getName();
                        List<string> s2 = tt.getExpression();
                        string item2 = "";
                        foreach (string ss3 in s2)
                        {
                            item2 += ss3 + " ";

                        }

                        sw2.WriteLine(n.getValue() + "#" + item1 + "#" + item2);



                    }
                }


                sw2.Flush();
                sw2.Close();
                sw2.Dispose();

            }

            else
            {
                MessageBox.Show("无法进行文法分析。文法为空");
            }
        }

        private void 句法分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (GrammarEditor.Text.TrimEnd() != "")
            {
                string s = GrammarEditor.Text.TrimEnd();
                Form f = new Form1(s);
                f.Show();
            }
            else
            {
                MessageBox.Show("文法不能为空！");
            }

            
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


    }
}
