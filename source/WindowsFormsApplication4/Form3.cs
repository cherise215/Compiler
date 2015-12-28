using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
namespace WindowsFormsApplication4
{
    public partial class Form3 : Form
    {
        private List<parseResult> rlist;
        private List<ERROR> elist;
        private List<keyInfo> tokenList;
        private Form Form;
        private List<string> threeList;
        public Form3(List<parseResult> rlist, List<ERROR> elist, List<keyInfo> tokenList,List<string>ThreeList)
        {
            InitializeComponent();
            this.rlist = rlist;
            this.elist = elist;
            this.tokenList = tokenList;
            this.threeList = ThreeList;
            if (rlist.Count != 0)
            {
                foreach (parseResult pr in rlist)
                {
                    dataGridView1.Rows.Add(pr.getRow(), pr.getCurKey().getName(),pr.getOutPut(), pr.getStackTop(),pr.getStack());
                }

            }

            if (elist.Count != 0)
            {
                foreach (ERROR e in elist)
                {
                    dataGridView2.Rows.Add(e.getRow(),e.getStrError(),e.getErrorKind());
                }

            }
            if (threeList.Count != 0)
            {
                foreach (string e in threeList)
                {
                    dataGridView3.Rows.Add(e);
                }

            }

            FileStream fs2 = new FileStream("grammarTree.txt", FileMode.Create);
            StreamWriter sw2 = new StreamWriter(fs2, Encoding.Default);
            foreach (parseResult pr in rlist)
            {

                sw2.WriteLine(pr.getOutPut());
            }


            sw2.Flush();
            sw2.Close();
            sw2.Dispose();
        }

        private void Form3_Load(object sender, EventArgs e)
        {


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

            if (!dataGridView1.CurrentRow.IsNewRow)
            {
                string row = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                string token = dataGridView1.CurrentRow.Cells[1].Value.ToString();
                string line = "";
                int i = 0;
                foreach (keyInfo k in tokenList)
                {
                    i++;
                    if (k.getRow().ToString().Equals(row))
                        line += k.getName() + " ";


                }
                richTextBox1.Text = line;
                HighLightText(token);
            }

         //  HighLightText(token);
            

           //MessageBox.Show(row + "");
           //Form.Show();
        }
        private string GaoL = null;

        private void HighLightText(string s)
        {
            string text=richTextBox1.Text.ToString();
            string [] ss=text.Split(' ');

              
                richTextBox1.SelectAll();

                richTextBox1.SelectionBackColor = Color.White;
                int i = 0;
                foreach (string term in ss)
                {

                    if (term.Equals(s))
                    {
                        richTextBox1.Select(i, term.Length);
                        richTextBox1.SelectionBackColor = Color.Yellow;
                        richTextBox1.SelectionFont = new Font("楷体", 10, FontStyle.Bold);
                    }
                    i += term.Length+1;
                }
               // richTextBox1.Refresh();

           
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (!dataGridView2.CurrentRow.IsNewRow)
            {
                string row = dataGridView2.CurrentRow.Cells[0].Value.ToString();
                string token = dataGridView2.CurrentRow.Cells[1].Value.ToString();
                string line = "";
                int i = 0;
                foreach (keyInfo k in tokenList)
                {
                    i++;
                    if (k.getRow().ToString().Equals(row))
                        line += k.getName() + " ";


                }
                richTextBox2.Text = line;
                HighLightText2(token);
            }

        }
        private void HighLightText2(string s)
        {
            string text = richTextBox2.Text.ToString();
            string[] ss = text.Split(' ');


            richTextBox2.SelectAll();

            richTextBox2.SelectionBackColor = Color.White;
            int i = 0;
            foreach (string term in ss)
            {

                if (term.Equals(s))
                {
                    richTextBox2.Select(i, term.Length);
                    richTextBox2.SelectionBackColor = Color.Yellow;
                    richTextBox2.SelectionFont = new Font("楷体", 10, FontStyle.Bold);
                }
                i += term.Length + 1;
            }
            // richTextBox1.Refresh();


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }



        

    }
}
