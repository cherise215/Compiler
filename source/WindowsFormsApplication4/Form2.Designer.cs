namespace WindowsFormsApplication4
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.GrammarEditor = new System.Windows.Forms.RichTextBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.listView2 = new System.Windows.Forms.ListView();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.listBox3 = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.打开文法文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.句法分析ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.文法分析ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.文法符号表 = new System.Windows.Forms.GroupBox();
            this.终结符一览表 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.menuStrip1.SuspendLayout();
            this.文法符号表.SuspendLayout();
            this.终结符一览表.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // GrammarEditor
            // 
            this.GrammarEditor.Location = new System.Drawing.Point(20, 28);
            this.GrammarEditor.Name = "GrammarEditor";
            this.GrammarEditor.Size = new System.Drawing.Size(1127, 165);
            this.GrammarEditor.TabIndex = 4;
            this.GrammarEditor.Text = "";
            // 
            // listView1
            // 
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(14, 20);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1128, 137);
            this.listView1.TabIndex = 5;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            // 
            // listView2
            // 
            this.listView2.GridLines = true;
            this.listView2.Location = new System.Drawing.Point(275, 20);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(678, 268);
            this.listView2.TabIndex = 8;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.Details;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(22, 20);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(236, 268);
            this.listBox1.TabIndex = 9;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.listBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listBox1_MouseDoubleClick);
            // 
            // listBox3
            // 
            this.listBox3.FormattingEnabled = true;
            this.listBox3.ItemHeight = 12;
            this.listBox3.Location = new System.Drawing.Point(6, 20);
            this.listBox3.Name = "listBox3";
            this.listBox3.Size = new System.Drawing.Size(139, 268);
            this.listBox3.TabIndex = 11;
            this.listBox3.SelectedIndexChanged += new System.EventHandler(this.listBox3_SelectedIndexChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开文法文件ToolStripMenuItem,
            this.句法分析ToolStripMenuItem,
            this.文法分析ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1182, 25);
            this.menuStrip1.TabIndex = 14;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 打开文法文件ToolStripMenuItem
            // 
            this.打开文法文件ToolStripMenuItem.Name = "打开文法文件ToolStripMenuItem";
            this.打开文法文件ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.打开文法文件ToolStripMenuItem.Text = "打开文法文件";
            this.打开文法文件ToolStripMenuItem.Click += new System.EventHandler(this.打开文法文件ToolStripMenuItem_Click);
            // 
            // 句法分析ToolStripMenuItem
            // 
            this.句法分析ToolStripMenuItem.Name = "句法分析ToolStripMenuItem";
            this.句法分析ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.句法分析ToolStripMenuItem.Text = "语法分析";
            this.句法分析ToolStripMenuItem.Click += new System.EventHandler(this.句法分析ToolStripMenuItem_Click);
            // 
            // 文法分析ToolStripMenuItem
            // 
            this.文法分析ToolStripMenuItem.Name = "文法分析ToolStripMenuItem";
            this.文法分析ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.文法分析ToolStripMenuItem.Text = "文法分析";
            this.文法分析ToolStripMenuItem.Click += new System.EventHandler(this.文法分析ToolStripMenuItem_Click);
            // 
            // 文法符号表
            // 
            this.文法符号表.Controls.Add(this.listView1);
            this.文法符号表.Location = new System.Drawing.Point(14, 199);
            this.文法符号表.Name = "文法符号表";
            this.文法符号表.Size = new System.Drawing.Size(1156, 174);
            this.文法符号表.TabIndex = 15;
            this.文法符号表.TabStop = false;
            this.文法符号表.Text = "文法符号表";
            // 
            // 终结符一览表
            // 
            this.终结符一览表.Controls.Add(this.listBox3);
            this.终结符一览表.Location = new System.Drawing.Point(14, 379);
            this.终结符一览表.Name = "终结符一览表";
            this.终结符一览表.Size = new System.Drawing.Size(161, 306);
            this.终结符一览表.TabIndex = 16;
            this.终结符一览表.TabStop = false;
            this.终结符一览表.Text = "终结符一览表";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBox1);
            this.groupBox1.Controls.Add(this.listView2);
            this.groupBox1.Location = new System.Drawing.Point(181, 379);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(974, 306);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "预测分析结果（请点击左侧列表非终结符进行查看，不在右侧表中出现的元素则为EEROR）";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1182, 697);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.终结符一览表);
            this.Controls.Add(this.文法符号表);
            this.Controls.Add(this.GrammarEditor);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form2";
            this.Text = "文法分析器";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.文法符号表.ResumeLayout(false);
            this.终结符一览表.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.RichTextBox GrammarEditor;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ListBox listBox3;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 打开文法文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 文法分析ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 句法分析ToolStripMenuItem;
        private System.Windows.Forms.GroupBox 文法符号表;
        private System.Windows.Forms.GroupBox 终结符一览表;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}