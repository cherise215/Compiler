using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication4
{
    public class GrammarAnalysis
    {
        private List<Grammar> grammarList = new List<Grammar>();
        private HashSet<TerminalSymbol> TList = new HashSet<TerminalSymbol>();
        private HashSet<String> Tquene = new HashSet<String>();
        private HashSet<NonTerminalSymbol> NList = new HashSet<NonTerminalSymbol>();
        private HashSet<string> Nquene = new HashSet<string>();
        private HashSet<string> Nullquene = new HashSet<string>();
        private string input;
        private int inputIndex = 0;
        char ch;
        string token;
        private Scanner scanner = new Scanner();
        private HashSet<dict> dt = new HashSet<dict>();
        private HashSet<string> isFollow = new HashSet<string>();
        private HashSet<string> waitFollow = new HashSet<string>();
        public String ERROR = "error";
        public String SYN = "synch";
        private List<ERROR> errorList = new List<ERROR>();
        private List<parseResult> resultList = new List<parseResult>();
        Stack<keyInfo> AttrStack = new Stack<keyInfo>();
        Stack <SymbolTable>  ST = new Stack<SymbolTable>();
        List<string> STname = new List<string>();
        List<string> ThreeList=new List <string>();
        /// <summary>
        /// 获取栈元素
        /// </summary>
        public Stack<SymbolTable> getST()
         {
             ST.Reverse();
             return ST;

         }
        /// <summary>
        /// 获取三地址码列表
        /// </summary>
        public  List<string> getThreeList()
        {
          
            return ThreeList;

        }
        /// <summary>
        /// 获取错误信息
        /// </summary>
        public List<ERROR> getErrorList()
        {
            return errorList;
        }

        /// <summary>
        /// 获取文法分析结果列表
        /// </summary>
        public List<parseResult> getResultList()
        {
            return resultList;
        }


        /// <summary>
        /// 进行语义分析主要功能（包活语法分析）
        /// </summary>
        public GrammarAnalysis(string s)
        {
            input = s + '\n';

            dt = scanner.getMap();


        }
        /// <summary>
        /// 读取一个字符
        /// </summary>
        private void getChar()
        {
            if (inputIndex < input.Length)
            {
                ch = input[inputIndex];
                inputIndex++;
            }
            else
            {
            }

        }
        /// <summary>
        /// 连接字符，缓存
        /// </summary>
        private void concat()
        {
            token += ch;
        }
        /// <summary>
        /// 对文法文件进行分析
        /// </summary>
        public List<Grammar> getGrammarFromFile()
        {
            split();
            initNonterminal();
            initTerminal();
            Tquene.Remove("$");


            initNull();
            Nullquene.Add("$");
            getFirstQuene();
        /*    foreach (Grammar g in grammarList)
            {
                string message = g.getLeftPart() + ":";
                foreach (string s in getStringFirst(g.getRightPart()))
                {
                    message += s + "|";
                }
                //MessageBox.Show(message);
            }

           */
            getFollowList();
            getSelectList();
            getSynch();
            if (!isLL1())
            {
                ;
            }

            else
            {
                //MessageBox.Show("文法符合LL1文法");
                bulidTable();
               // MessageBox.Show("分析表建立成功！");



            }
           // MessageBox.Show("NTS非终结符个数:" + Nquene.Count);
          //  MessageBox.Show("Ts终结符个数" + Tquene.Count);


            



            return grammarList;
        }
        /// <summary>
        /// 记录非终结符的名称
        /// </summary>
        public List<string> getNquene()
        {
            return Nquene.ToList();
        }
        public NonTerminalSymbol mapGet(string s)
        {

            foreach (NonTerminalSymbol nt in NList)
            {
                if (nt.getValue() == s)
                {
                    return nt;
                }

            }
            // if(!NList.Add(new NonTerminalSymbol("s")))
            //MessageBox.Show("succeess");
            return null;

        }
        /// <summary>
        /// 获取所有非终结符的First集合
        /// </summary>
        public void getFirstQuene()
        {
            string Message = "";
            foreach (string s in Nquene)
            {
                HashSet<string> f = getFirst(s);
                NonTerminalSymbol nts = new NonTerminalSymbol(s);
                nts.setFirst(f);
                NList.Add(nts);
                Message += s + ": ";
                foreach (string ff in f)
                {
                    Message += ff + "|";
                }
                //MessageBox.Show(Message);
            }

            // MessageBox.Show(Message);
        }
        /// <summary>
        /// 求一串文法符号的first集合
        /// </summary>
        public HashSet<string> getStringFirst(List<string> right)
        {
            HashSet<string> mFirst = new HashSet<string>();

            if (right.Count() > 0)
            { // 初始化,FIRST(α)= FIRST(X1);   
                string curString = right[0];
                if (Tquene.Contains(curString) && curString != "$")
                    mFirst.Add(curString);

                else
                {
                    mFirst.UnionWith(getFirst(curString));
                }
            }

            if (right.Count() > 0)
            {
                if (Nquene.Contains(right[0]) || right[0] == "$")
                { // 第一个符号为非终结符   
                    if (right.Count() == 1)
                    { // 直接将这个非终结符的first集加入right的first集 
                        if(right[0] != "$")
                          mFirst.UnionWith(mapGet(right[0]).getFirst());
                    }
                    else if (right.Count > 1)
                    {
                        int j = 0;
                        while (j < right.Count - 1 && Nullquene.Contains(right[j]))
                        { // 若Xk→ε,FIRST(α)=FIRST(α)∪FIRST(Xk+1)   
                            String tString = right[j + 1];
                            if (Tquene.Contains(tString))
                            {
                                mFirst.UnionWith(getFirst(tString));
                            }
                            else if (!tString.Equals("$"))
                            {
                                mFirst.UnionWith(mapGet(tString).getFirst());
                            }


                            j++;
                        }
                    }
                }
            }
            if (mFirst.Contains("$")) mFirst.Remove("$");
            return mFirst;
        }






        /// <summary>
        /// 求一次某一个非终结符的Follow集
        /// </summary>
        public int follow(String value)
        {


            int canModify = 0;
            List<Grammar> itor = new List<Grammar>();
            int before = mapGet(value).getFollowList().Count();

            foreach (Grammar g in grammarList)//右部包含该非终结符的所有产生式
            {
                if (g.getRightPart().Contains(value))

                    itor.Add(g);

            }
            foreach (Grammar g in itor)//
            {
                List<string> rlist = g.getRightPart();
                int i = 0;


                foreach (string s in rlist)
                {
                    i++;
                    if (s.Equals(value))
                    {
                        if (i == rlist.Count)//产生式的最右
                        {
                            if (!s.Equals(g.getLeftPart()))
                            {
                                if (!isFollow.Contains(g.getLeftPart()))//左部未求得
                                {

                                    canModify++;
                                    waitFollow.Add(value);
                                    waitFollow.Add(g.getLeftPart());
                                }

                                mapGet(value).unionFollowList(mapGet(g.getLeftPart()).getFollowList());//左部合并
                            }



                        }
                        if (i < rlist.Count)//不是最右
                        {
                            List<string> beta = new List<string>();
                            bool check = true;
                            for (int index = i; index < rlist.Count; index++)
                            {
                                string ri = rlist[i];
                                //MessageBox.Show(i+" :"+ri);
                                beta.Add(ri);
                                if (!Nullquene.Contains(ri))
                                {
                                    check = false;
                                }


                            }
                            // MessageBox.Show("hear");
                            HashSet<string> betafirst = getStringFirst(beta);
                            mapGet(value).unionFollowList(betafirst);
                            if (check == true)
                            {
                                if (!s.Equals(g.getLeftPart()))
                                {
                                    if (!isFollow.Contains(g.getLeftPart()))//左部已经求得
                                    {

                                        canModify++;
                                        waitFollow.Add(value);
                                        waitFollow.Add(g.getLeftPart());
                                    }
                                    mapGet(value).unionFollowList(mapGet(g.getLeftPart()).getFollowList());//左部合并

                                }

                            }

                        }




                    }
                }


            }
            int after = mapGet(value).getFollowList().Count();
            if (canModify == 0)
            {
                isFollow.Add(value);//求解结束
                if (waitFollow.Contains(value))
                {
                    waitFollow.Remove(value);
                }
            }
            return after - before;

        }
        /// <summary>
        /// 获取所有非终结符的Follow集合
        /// </summary>
        public HashSet<NonTerminalSymbol> getFollowList()
        {
            int i = 0;
            bool flag = true;

            HashSet<string> te = new HashSet<string>();
            te.Add("@");
            mapGet(grammarList[0].getLeftPart()).unionFollowList(te);


            while (flag)
            {
                i = 0;
                foreach (string s in Nquene)
                {
                    i += follow(s);
                }
                if (i > 0)
                    flag = true;
                else
                    flag = false;
              //  MessageBox.Show(i+"");
            }
            

            return NList;


        }




        public bool ifExist(List<string> bag, string thing)
        {


            return false;
        }



        public void insertGrammar(string left, List<string> right)
        {



        }
        //扫描左部,左部一定是非终结符
        public void initNonterminal()
        {
            Tquene = new HashSet<String>();
            foreach (Grammar g in grammarList)
            {
                string s = g.getLeftPart();
                Nquene.Add(s);


            }

        }
        //扫描右部，获得终结符
        public void initTerminal()
        {
            Tquene = new HashSet<String>();

            HashSet<dict> map = scanner.getMap();
            foreach (dict str in map)
            {
                Tquene.Add(str.getId());


            }

        }

        //扫描右部，获得可推出空串的非终结符
        public void initNull()
        {
            foreach (string s in Nquene)
            {
                // NonTerminalSymbol ns = new NonTerminalSymbol(s);
                if (canLeadNull(s))
                {
                    Nullquene.Add(s);
                    //  ns.setGenerateNull(true);

                }
                //   NList.Add(ns);


            }
            string message = "";
            foreach (string s in Nullquene)
            {
                message += s + " ";
            }
          //  MessageBox.Show(message);

        }

        public void initNonTerminal()
        {
            foreach (Grammar g in grammarList)
            {
                string s = g.getLeftPart();
                Nquene.Add(s);


            }
        }
        /// <summary>
        /// 判断是否可推出空
        /// </summary>
        public bool canLeadNull(string X)
        {
            // X是终结符，则X不可能推出空串   \\

            if (X.Equals("$"))
            {
                return true;
            }
            if (Tquene.Contains(X))
            {

                return false;
            }
            if (!Nquene.Contains(X))
            {
                MessageBox.Show("文法错误，错误符号:" + X);

                Application.Exit();
                return false;
            }
            // X是非终结符   
            else
            {


                foreach (Grammar g in grammarList)
                {

                    if (X.Equals(g.getLeftPart()))
                    {
                        // 存在一个 X=>$ 的产生式，则说明X可以推出空串   
                        if ("$".Equals(g.getRightPart()[0]))
                        {

                            return true;
                        }

                        // 当前产生式不是 X=>$ ，递归调用   
                        else
                        {
                            bool flag = true;
                            foreach (string s in g.getRightPart())
                            {

                                if (!canLeadNull(s))
                                {
                                    flag = false;
                                    break;
                                }
                            }
                            if (flag == true)
                            {


                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }


        public HashSet<string> initFirst(string value)
        {
            HashSet<string> first = new HashSet<string>();
            foreach (Grammar g in grammarList)
            {
                if (g.getLeftPart().Equals(value))
                {
                    if (Tquene.Contains(g.getRightPart()[0]))
                    {
                        first.Add(g.getRightPart()[0]);


                    }

                }

            }
            return first;
        }
        /// <summary>
        /// 获取某一个符号对应的First集合
        /// </summary>
        public HashSet<string> getFirst(string value)
        {
            HashSet<string> first = new HashSet<string>();
            if (Tquene.Contains(value))
            {
                first.Add(value);
                return first;
            }


            if (Nquene.Contains(value))
            {
                first.UnionWith(initFirst(value));

                foreach (Grammar g in grammarList)
                {
                    if (g.getLeftPart().Equals(value))
                    {
                        string x = g.getRightPart()[0];
                        if (Nquene.Contains(x))
                        {
                            first.UnionWith(getFirst(x));
                        }


                        if (g.getRightPart().Count > 1)
                        {
                            foreach (string next in g.getRightPart())
                            {
                                first.UnionWith(getFirst(next));
                                if (!Nullquene.Contains(next))
                                    break;

                            }
                        }
                    }
                }

            }
            return first;
        }

        /// <summary>
        /// 获取所有文法的select集
        /// </summary>
        public void getSelectList()
        {
            foreach (Grammar g in grammarList)
            {

                HashSet<string> select = new HashSet<string>();
                if (g.getRightPart()[0].Equals("$"))
                {
                    select.UnionWith(mapGet(g.getLeftPart()).getFollowList());
                }
                else
                {
                    select.UnionWith(getStringFirst(g.getRightPart()));
                    bool flag = true;
                    foreach (string s in g.getRightPart())
                    {
                        if (!Nullquene.Contains(s))
                        {
                            flag = false;
                            break;
                        }
                    }
                    if (flag == true)
                    {
                        select.UnionWith(mapGet(g.getLeftPart()).getFollowList());

                    }


                }

                g.setSelect(select);



            }
        }

        //文本分解为文法
        public void split()
        {
            int state = 0;

            //  MessageBox.Show(input);
            List<string> right = new List<string>();//右部

            int number = 0;
            string left = "";
            while (inputIndex < input.Length)
            {
                getChar();


                //   MessageBox.Show(ch+" "+state);
                switch (ch)
                {


                    case '\n':

                        if (state == 3)
                        {
                            right.Add(token);
                            token = "";
                            state = 5;

                        }
                        else
                        {
                            left = "";
                            state = 0;
                        }


                        break;
                    /* case '|':
                         if (state == 3)
                         {
                          
                             state = 4;
                             right.Add(token);
                             token = "";


                         }
                     * */

                    // break;
                    case '-': if (state == 1)
                        {
                            state = 2;
                            left = token;
                            token = "";
                        }
                        else if (state == 3 || state == 4)
                        {
                            state = 3;
                            concat();
                        }
                        break;

                    case '>': if (state == 2)
                        {
                            state = 3;

                        }
                        else if (state == 1)
                        {
                            concat();
                        }
                        else if (state == 4 || state == 3)
                        {
                            state = 3;
                            concat();
                        }
                        else
                        {
                            MessageBox.Show("文法文件书写不正确");
                        }
                        break;
                    default:
                        switch (state)
                        {
                            case 0:
                                state = 1;
                                left = "";
                                right = new List<string>();
                                concat();
                                break;
                            case 1:
                                state = 1;
                                concat();
                                break;
                            case 3:
                                state = 3;
                                concat();
                                break;
                            case 4:
                                state = 3;
                                concat();
                                break;

                        } break;


                }
                if (state == 5)
                {



                    foreach (string ss in right)
                    {
                        Grammar g = new Grammar();
                        List<string> r = new List<string>();
                        List<string> rAction = new List<string>();
                        string miaoshu = "";
                        string[] temp = ss.Split(' ');
                        left = left.Trim();
                        g.setLeftPart(left);
                        miaoshu = left + "->";
                        foreach (string d in temp)
                        {

                            string dd="";
                            if (d.Trim() != "")
                            {
                                 dd = d.Trim();
                                if (!d.StartsWith("#"))
                                {

                                    r.Add(dd);
                                    miaoshu += dd + " ";
                                }
                                rAction.Add(dd);

                            }
                          



                        }
                        g.setRightPart(r);
                        g.setRightPartWithAction(rAction);
                        g.setmGrammar(miaoshu);
                        grammarList.Add(g);

                    }


                    state = 0;
                    left = "";
                }




            }




        }





        /// <summary>
        /// 判断是否有相同元素
        /// </summary>
        public bool checkIfRepeat(string value)
        {
            List<Grammar> glist = new List<Grammar>();

            foreach (Grammar g in grammarList)
            {
                if (g.getLeftPart().Equals(value))
                {
                    glist.Add(g);


                }



            }
            if (glist.Count > 1)
            {
                int i = 0;
                HashSet<string> temp = new HashSet<string>();
                foreach (Grammar g in glist)
                {


                    if (i == 0)
                    {
                        temp = g.getSelect();

                    }
                    else
                    {
                        bool flag = temp.Overlaps(g.getSelect());
                        if (flag == true)
                        {
                            MessageBox.Show("不符合LL1文法");
                            return false;
                        }
                        else
                        {
                            temp.UnionWith(g.getSelect());
                        }
                    }


                }
            }

            return true;




        }
        public bool isLL1()
        {
            foreach (string s in Nquene)
            {
                if (!checkIfRepeat(s))
                    return false;

            }
            return true;
        }

        /// <summary>
        /// 获取预测分析表
        /// </summary>
        public void getAnalysisTable()
        {
            int n1 = Nquene.Count;
            int n2 = Tquene.Count;




        }
        /// <summary>
        /// 获得非终结符的对象列表
        /// </summary>
        public HashSet<NonTerminalSymbol> getNlist()
        {
            return NList;
        }
        /// <summary>
        /// 记录终结符名字
        /// </summary>
        public List<string> getTquene()
        {
            return Tquene.ToList();
        }

        /// <summary>
        /// 求同步记号集
        /// </summary>
        public void getSynch()
        {
            foreach (NonTerminalSymbol nts in NList)
            {
                nts.getSynList().UnionWith(nts.getFollowList()); // 把FOLLOW(A)的元素放入A的同步记号集合
                nts.getSynList().UnionWith(nts.getFirst()); // 把FIRST(A)的元素放入A的同步记号集合
                HashSet<NonTerminalSymbol> higher = nts.getHigherNTS();
                foreach (NonTerminalSymbol j in higher)
                { // 把高层结构的First集加入A的同步记号集
                    nts.getSynList().UnionWith(j.getFirst());
                }


            }
        }

        /// <summary>
        /// 建立预测分析表
        /// </summary>
        public void bulidTable()
        {
            foreach (NonTerminalSymbol nts in NList)
            {
                List<Grammar> gl = new List<Grammar>();

                gl = getGrammarByLeft(nts.getValue());
                foreach (Grammar g in gl)
                {
                    HashSet<string> select = g.getSelect();
                    foreach (string s in select)
                    {
                        Table table = new Table(s);
                        table.setFlag(1);
                        table.setExpression(g.getRightPartWithAction());
                        nts.getTable().Add(table);
                    }

                }

                foreach (string s in nts.getSynList())
                {
                    bool ff = true;
                    foreach (Table t in nts.getTable())
                    {
                        if (t.getName().Equals(s))
                        {
                            ff = false;
                            break;
                        }

                    }
                    if (ff)
                    {
                        Table table = new Table(s);
                        table.setFlag(0);
                        List<string> ll = new List<string>();
                        ll.Add("SYNCH");
                        table.setExpression(ll);
                        nts.getTable().Add(table);
                    }

                }


            }
        }

        /// <summary>
        /// 判断两个非终结符哪个是更高层的结构
        /// </summary>
        public bool isHigher(NonTerminalSymbol nts1,
                NonTerminalSymbol nts2)
        {
            List<Grammar> mexp = getGrammarByLeft(nts1.getValue());
            for (int i = 0; i < mexp.Count; i++)
            {
                List<String> mright = mexp[i].getRightPart();
                for (int j = 0; j < mright.Count; j++)
                {
                    if (mright[j].Equals(nts2.getValue()))
                        return true;
                }
            }
            return false;
        }
   
        public void calcHigherGroup()
        {
            foreach (String s in Nquene)
            {
                NonTerminalSymbol sNonTerminalSymbol = mapGet(s);
                foreach (String r in Nquene)
                {
                    NonTerminalSymbol rNonTerminalSymbol = mapGet(r);
                    if (isHigher(sNonTerminalSymbol, rNonTerminalSymbol))
                    {
                        if (!rNonTerminalSymbol.getHigherNTS().Contains(
                                sNonTerminalSymbol))
                        {
                            rNonTerminalSymbol.getHigherNTS().Add(
                                    sNonTerminalSymbol);
                            rNonTerminalSymbol.getHigherNTS().UnionWith(sNonTerminalSymbol.getHigherNTS());

                        }
                    }
                }
            }
        }


        /// <summary>
        /// 获取文法左部
        /// </summary>
        public List<Grammar> getGrammarByLeft(string left)
        {
            List<Grammar> gl = new List<Grammar>();
            foreach (Grammar g in grammarList)
            {
                if (g.getLeftPart().Equals(left))
                    gl.Add(g);
            }

            return gl;
        }

        /// <summary>
        /// 获取预测分析结果
        /// </summary>
        public List<string> getAction(string s, string curstring)
        {
            NonTerminalSymbol nts = mapGet(s);
            HashSet<Table> table = nts.getTable();
            bool flag = false;//是否是空
            int get = 0;
            List<string> expr = new List<string>();

            foreach (Table t in table)
            {

                if (t.getName().Equals(curstring))
                {
                    expr = t.getExpression();
                    if (t.getFlag() == 1)
                    {
                        get++;
                    }
                    else
                    {

                        return expr;
                    }

                }
            }
            if (get == 0)
            {
                expr.Add("ERROR");
                return expr;
            }
            else if (get == 1)
            {
                return expr;
            }
            else
            {
                foreach (Table t in table)
                {

                    if (t.getName().Equals(curstring))
                    {
                        if (t.getExpression().Contains(t.getName()))
                        {
                            expr = t.getExpression();
                            return expr;
                        }
                        if (!t.getExpression()[0].Equals("$"))
                        {
                            expr = t.getExpression();
                            return expr;
                        }
                        else expr = t.getExpression();

                    }
                }
            }
            return expr;




        }
        public string ListToString(List<string> l)
        {
            string ss = "";
            foreach (string s in l)
            {
                ss += s+" ";
            }
            return ss;

        }
        /// <summary>
        /// 将栈元素变为字符串
        /// </summary>
        public string getstackTostring(Stack<keyInfo> stack)
        {
            string s = "";
            foreach (keyInfo ki in stack)
            {
                s += ki.getType()+" ";
            }
            return s;


        }


        /// <summary>
        /// 从属性栈内得到一个文法符号类
        /// </summary>
        public keyInfo getSymbolFromAttrList(string symbolName)
        {
            foreach(keyInfo ki in AttrStack)
            {
                if (ki.getType().Equals(symbolName))
                {
                    return ki;

                }
            }
            return null;


        }

        //执行一个语义函数方法调用
     

        private String curLabel = "";// 当前的标签
        private bool shouldLabeled = false;// 下一个三地址码前是否应该指定标签

        private int tempCount = 1;// 用于生成临时变量
        private int labelCount = 1;// 用于生成标签
      
        private int curLineNum = 0;



        /// <summary>
        /// 变量引用处理
        /// </summary>
      private string lookIDN(string par,int row) {

          par=par.Substring(par.IndexOf("(") + 1);

          par = par.Trim(')');
          keyInfo ki = getSymbolFromAttrList(par);

          if (!ST.Peek().isIN(ki.getArttibute("name")))
          {
              //变量声明不存在,错误处理
              // MessageBox.Show(ki.getArttibute("name")+"not exist");
              errorList.Add(new ERROR(row, ki.getArttibute("name"), "变量引用不存在"));
              return "";
          }
          return ST.Peek().getVarType(ki.getArttibute("name"));
	    }
      /// <summary>
      /// 生成表达式语句的三地址码
      /// </summary>
      private void gen(string act)
      {
          String par = act.Substring(act.IndexOf("(") + 1);
          par = par.Trim(')');
          string[] pars = par.Split(',');
          if (pars.Length == 4)
          {

              string addr = getSymbolFromAttrList(pars[0]).getArttibute("addr");
              string f1 = getSymbolFromAttrList(pars[1]).getArttibute("name");
              string f2 = getSymbolFromAttrList(pars[3]).getArttibute("name");
              string op = pars[2].Trim('\"');

              string stmt = addr + "=" + f1 + op + f2;
              ThreeList.Add(stmt);

              // MessageBox.Show(stmt);


          }

      }

      /// <summary>
      /// 数组引用处理
      /// </summary>
        public int lookGroup(string act,int row)
        {
            bool flag = true;
                     String par = act.Substring(act.IndexOf("(") + 1);
                              par = par.Trim(')');
                              string[] goups = par.Split(',');
                              if (goups[0].Equals("1")) 
                                  flag=false;
                              string[] pars =goups[1].Split('.');
                             // MessageBox.Show(par+"!!!!!!!"+pars.Length);
                              if (pars.Length == 2)
                              {

                                  string s = getSymbolFromAttrList(pars[0]).getArttibute(pars[1]);
                                  //MessageBox.Show("Group:" +s);
                                  string[] numbers = s.Split(',');
                                  string groupName = numbers[0];
                                  if (numbers.Length > 1)
                                  {
                                      if (ST.Peek().isIN(groupName))
                                      {
                                          List<int> ints = ST.Peek().getArrayWidthByName(groupName);
                                          if (ints != null)
                                          {

                                              if (ints.Count + 1 != numbers.Length)
                                              {
                                                  //错误处理，越界
                                                  //MessageBox.Show("ERROR,数组越界+ints：" + ints.Count + "s" + numbers.Length);
                                                  getSymbolFromAttrList(pars[0]).setNewArttibute("name", "");
                                                  errorList.Add(new ERROR(row, groupName, "数组访问越界！"));
                                                  return 1;

                                              }

                                              else
                                              {
                                                  string n;
                                                  for (int Ai = 1; Ai < numbers.Length; Ai++)
                                                  {
                                                      n = numbers[Ai];
                                                      if (n.Contains('.'))
                                                      {
                                                          errorList.Add(new ERROR(row, n, "数组访问下标不正确！"));
                                                          return 1;
                                                      }
                                                      string type = ST.Peek().getVarType(n);
                                                      if (!type.Equals(""))
                                                      {
                                                          if (type.Contains("["))
                                                          {
                                                              errorList.Add(new ERROR(row, n, "数组访问下标引用不能是数组类型变量！"));
                                                              return 1;
                                                          }
                                                          else if (type.Contains("int"))
                                                          {
                                                              //errorList.Add(new ERROR(row, n, "数组访问下标引用不能是数组类型变量！"));
                                                              //return 1;
                                                          }
                                                          else
                                                          {
                                                              errorList.Add(new ERROR(row, n, "数组访问下标引用必须是整型常数或变量！"));
                                                              getSymbolFromAttrList(pars[0]).setNewArttibute("name", "");
                                                              return 1;

                                                          }



                                                      }



                                                  }


                                                  int baseNumber = ints[0];

                                                  if (ints.Count + 1 == 2)
                                                  {
                                                      string tt = "t" + tempCount;
                                                      tempCount++;
                                                      string stmt = tt + "=" + ints[0] + "*" + numbers[1];
                                                      // MessageBox.Show(stmt);
                                                      if (ST.Peek().getVarType(numbers[1]).Equals(""))
                                                      {
                                                          int anumber = int.Parse(numbers[1]);
                                                          foreach (Variable v in ST.Peek().getVariableList())
                                                          {
                                                              if (v.getVarName().Equals(groupName))
                                                              {
                                                                  int length = v.getLength();
                                                                  if (anumber * ints[0] >= length)
                                                                  {
                                                                      errorList.Add(new ERROR(row, groupName, "下标越界"));
                                                                  }
                                                              }
                                                          }

                                                      }
                                                      ThreeList.Add(stmt);

                                                      string addrs = groupName + "[" + tt + "]";
                                                      if (flag)
                                                      {
                                                          getSymbolFromAttrList(pars[0]).setNewArttibute("name", addrs);

                                                      }
                                                      else
                                                      {
                                                          string tt2 = "t" + tempCount;
                                                          stmt = tt2 + "=" + addrs;
                                                          tempCount++;
                                                          ThreeList.Add(stmt);
                                                          getSymbolFromAttrList(pars[0]).setNewArttibute("name", tt2);
                                                      }

                                                  }


                                                  if (ints.Count + 1 == 3)
                                                  {
                                                      string tt = "t" + tempCount;
                                                      tempCount++;
                                                      int sum1 = ints[0] * ints[1];
                                                      int sum2 = ints[0];

                                                      string stmt = tt + "=" + sum1 + "*" + numbers[1];


                                                      ThreeList.Add(stmt);
                                                      string tt1 = "t" + tempCount;
                                                      tempCount++;
                                                      stmt = tt1 + "=" + sum2 + "*" + numbers[2];
                                                      ThreeList.Add(stmt);
                                                      string tt2 = "t" + tempCount;
                                                      tempCount++;
                                                      stmt = tt2 + "=" + tt + "+" + tt1;
                                                      ThreeList.Add(stmt);


                                                      string addrs = groupName + "[" + tt2 + "]";
                                                      if (flag)
                                                      {
                                                          getSymbolFromAttrList(pars[0]).setNewArttibute("name", addrs
                                                              );
                                                      }
                                                      else
                                                      {
                                                          string tt3 = "t" + tempCount;
                                                          tempCount++;
                                                          stmt = tt3 + "=" + addrs;
                                                          ThreeList.Add(stmt);
                                                          getSymbolFromAttrList(pars[0]).setNewArttibute("name", tt3);
                                                      }

                                                  }

                                                  if (ints.Count + 1 == 4)
                                                  {
                                                      string tt = "t" + tempCount;
                                                      tempCount++;
                                                      int sum1 = ints[0] * ints[1] * ints[2];
                                                      int sum2 = ints[0] * ints[2];
                                                      int sum3 = ints[0];

                                                      string stmt = tt + "=" + sum1 + "*" + numbers[1];
                                                      ThreeList.Add(stmt);
                                                      string tt1 = "t" + tempCount;
                                                      tempCount++;
                                                      stmt = tt1 + "=" + sum2 + "*" + numbers[2];
                                                      ThreeList.Add(stmt);
                                                      string tt2 = "t" + tempCount;
                                                      tempCount++;
                                                      stmt = tt2 + "=" + tt + "+" + tt1;
                                                      ThreeList.Add(stmt);
                                                      string tt3 = "t" + tempCount;
                                                      tempCount++;
                                                      stmt = tt3 + "=" + sum3 + "*" + numbers[3];
                                                      ThreeList.Add(stmt);
                                                      string tt4 = "t" + tempCount;
                                                      tempCount++;
                                                      stmt = tt4 + "=" + tt2 + "+" + tt3;
                                                      ThreeList.Add(stmt);



                                                      string addrs = groupName + "[" + tt4 + "]";
                                                      if (flag)
                                                      {
                                                          getSymbolFromAttrList(pars[0]).setNewArttibute("name", addrs);

                                                      }else
                                                      {
                                                          string st = "t" + tempCount;
                                                          stmt = st + "=" + addrs;
                                                          ThreeList.Add(stmt);
                                                          tempCount++;

                                                          getSymbolFromAttrList(pars[0]).setNewArttibute("name", st);
                                                      }

                                                  }







                                              }
                                          }
                                      }
                                      else
                                      {
                                          //error数组引用不存在
                                          // MessageBox.Show("ERROR,数组引用不存在");// 
                                          getSymbolFromAttrList(pars[0]).setNewArttibute("name", "");
                                          errorList.Add(new ERROR(row, groupName, "该数组未声明无法引用"));
                                          return 2;

                                      }




                                  }
                                  
                              }


                              return 0;
                              

                          
        }
       

 




        /// <summary>
        /// 句法分析器
        /// </summary>
        /// <param name="TokenList">词法分析结果的Token串</param>
        public void myParser(List<keyInfo> TokenList)
        {
            Stack<keyInfo> stack = new Stack<keyInfo>();
          
            int index = 0;
            keyInfo curCharacter = null;
            keyInfo bottom = new keyInfo("@");
            stack.Push(bottom);
            keyInfo start = new keyInfo(grammarList[0].getLeftPart());
            stack.Push(start);
            AttrStack.Push(start);//属性栈压入第一个开始符号
            TokenList.Add(new keyInfo("@"));

            while (!(stack.Peek().getType().Equals(bottom.getType())))
            {




                if (index < TokenList.Count) // 注意下标，否则越界
                    curCharacter = TokenList[index];
               
                if (Tquene.Contains(stack.Peek().getType())
                        || stack.Peek().Equals("@"))
                {
                    if (stack.Peek().getType().Equals(curCharacter.getType()))
                    {
                        if (!stack.Peek().getType().Equals("@"))
                        {

                          
                            if(stack.Peek().getType().Equals("IDN"))
                            {
                                if(getSymbolFromAttrList("IDN")!=null)
                                getSymbolFromAttrList("IDN").addArttibute("name", curCharacter.getName());


                            } if (stack.Peek().getType().Equals("CONST_INT10"))
                            {
                                if (getSymbolFromAttrList("CONST_INT10") != null)
                                    getSymbolFromAttrList("CONST_INT10").addArttibute("name", curCharacter.getName());


                            }
                            if (stack.Peek().getType().Equals("CONST_INT16"))
                            {
                                if (getSymbolFromAttrList("CONST_INT16") != null)
                                    getSymbolFromAttrList("CONST_INT16").addArttibute("name", curCharacter.getName());


                            }
                            if (stack.Peek().getType().Equals("CONST_INT8"))
                            {
                                if (getSymbolFromAttrList("CONST_INT8") != null)
                                    getSymbolFromAttrList("CONST_INT8").addArttibute("name", curCharacter.getName());


                            }
                            if (stack.Peek().getType().Equals("CONST_CHAR"))
                            {
                                if (getSymbolFromAttrList("CONST_CHAR") != null)
                                    getSymbolFromAttrList("CONST_CHAR").addArttibute("name", curCharacter.getName());


                            }
                            if (stack.Peek().getType().Equals("CONST_CHAR*"))
                            {
                                if (getSymbolFromAttrList("CONST_CHAR*") != null)
                                    getSymbolFromAttrList("CONST_CHAR*").addArttibute("name", curCharacter.getName());


                            }
                            if (stack.Peek().getType().Equals("CONST_FLOAT"))
                            {
                                if (getSymbolFromAttrList("CONST_FLOAT") != null)
                                    getSymbolFromAttrList("CONST_FLOAT").addArttibute("name", curCharacter.getName());


                            }
                            if (stack.Peek().getType().Equals("CONST_DOUBLE"))
                            {
                                if (getSymbolFromAttrList("CONST_DOUBLE") != null)
                                    getSymbolFromAttrList("CONST_DOUBLE").addArttibute("name", curCharacter.getName());


                            }










                            stack.Pop();//栈弹出，向前移动指针一步
                            index++;
                        }
                    }
                    else
                    {
                        errorList.Add(new ERROR(curCharacter.getRow(), curCharacter.getName(), "不匹配！当前栈顶符号：" + stack.Pop().getType()));
                    }
                }
                else if (Nquene.Contains(stack.Peek().getType()))
                {
                    List<string> expr = getAction(stack.Peek().getType(), curCharacter.getType());

                    if (expr[0].Equals("ERROR"))
                    {
                        if (index < TokenList.Count - 2)
                        {
                            errorList.Add(new ERROR(curCharacter.getRow(), curCharacter.getName(), "忽略该输入符号，遇到ERROR，当前非终结符：" + stack.Peek().getType()));
                            index++;
                        }
                        else
                        {
                            errorList.Add(new ERROR(curCharacter.getRow(), curCharacter.getName(), "缺少}"));
                            break;
                        }

                      
                        
                    }
                    else if (expr[0].Equals("SYNCH"))
                    {
                        errorList.Add(new ERROR(curCharacter.getRow(), curCharacter.getName(), "遇到SYNCH，从栈顶弹出非终结符：" + stack.Pop().getType()));
                    }

                    else
                    {
                        parseResult pr = new parseResult(curCharacter, stack.Peek().getType(), stack.Peek().getType() + "->" + ListToString(expr), curCharacter.getRow(),getstackTostring(stack));
                        resultList.Add(pr);
                        stack.Pop();
                        for (int j = expr.Count - 1; j > -1; j--)
                        {
                            if (!expr[j].Equals("$"))
                                stack.Push(new keyInfo(expr[j]));

                            if (Nquene.Contains(expr[j]))
                            {
                                AttrStack.Push(new keyInfo(expr[j]));//将非终结符号压入属性栈
                                //MessageBox.Show(expr[j]);
                            }
                            else if ((expr[j][0] >= 'a' && expr[j][0] < 'z')||(expr[j][0] >= 'A' && expr[j][0] <= 'Z'))
                            {
                                AttrStack.Push(new keyInfo(expr[j]));//将终结符的属性压入属性栈
                                //MessageBox.Show("push:"+expr[j]);
                            }

                             
                        }
                    }
                }
                else if(stack.Peek().getType().StartsWith("#"))
                {
                    string rawAciton = stack.Peek().getType();
                    rawAciton =rawAciton.Trim('#');
                    string[] actions = rawAciton.Split(';');//分割动作
                    bool skipflag = true;
                    for (int i = 0; i < actions.Length; i++)
                    {
                      string[] act=actions[i].Split('=');
                      if (act.Length == 1)//语义动作，调用方法
                      {
                          //声明函数
                          if (act[0].Contains("declareFunction"))//
                          {
                              keyInfo ki =getSymbolFromAttrList("Fun");
                             string funname=ki.getArttibute("name");
                              //MessageBox.Show("here"+funname);
                             if (STname.Contains(funname))
                             {
                                 errorList.Add(new ERROR(curCharacter.getRow(), funname, "函数声明重名"));
                             }
                             STname.Add(funname);

                              SymbolTable st=new SymbolTable(funname);//新建符号表
                              st.setReturnType(ki.getArttibute("type"));

                              //处理参数列表

                              string rawParams = ki.getArttibute("params");
                             // MessageBox.Show("rawParams:" + rawParams);
                              if (rawParams.Trim() != "")
                              {
                                  string[] paramList = rawParams.Split('#');

                                  for (int h =paramList.Length-1; h>=1; h--)
                                  {
                                      string newparam = paramList[h];
                                      string[] rr = newparam.Split(':');
                                      if (rr.Length == 2)
                                      {
                                          string ptype = rr[0];
                                          string pname = rr[1];
                                          bool check=st.enterParam(ptype, pname);
                                          if (!check)
                                          {
                                              errorList.Add(new ERROR(curCharacter.getRow(), pname, "参数重复声明，同一个函数内参数不能同名" ));
                                              //变量重复申明参数变量
                                          }
                                      }
                                    


                                  }
                              }


                                  ST.Push(st);
                                
                                 
                             // MessageBox.Show("Fun.params=" + ki.getArttibute("params"));
                              while (AttrStack.Count != 0 && !AttrStack.Peek().getType().Equals("Fun"))
                              {
                                //  MessageBox.Show( "POP:"+AttrStack.Pop().getType());
                                 AttrStack.Pop();
                              
                              }
                            
                              


                               

                          }

                          else    if (act[0].Contains("declareVar"))//
                          {
                              keyInfo ki = getSymbolFromAttrList("dec");
                             
                            //  string store=ki.getArttibute("store");

                              string dectype=  ki.getArttibute("type");
                              string decname= ki.getArttibute("name");
                           //   MessageBox.Show("name"+decname);
                              if (decname.Contains("*"))
                              { 
                                  string []decs=decname.Split('*');
                                  decname = decs[0];
                                  dectype += " ";

                                  for (int m = 1; m < decs.Length; m++)
                                  {
                                      dectype +="[" + decs[m]+"]"+" ";

                                  }
                                 

                              }


                             bool isIn= ST.Peek().enterVariable(dectype,decname);
                             if (!isIn)
                             { 
                                //错误处理：变量声明重复 
                                 errorList.Add(new ERROR(curCharacter.getRow(), decname, "同一个函数内变量不能重复声明"));

                             }


                              while (AttrStack.Count != 0 && !AttrStack.Peek().getType().Equals("dec"))
                              {

                                  //MessageBox.Show( "POP:"+AttrStack.Pop().getType());
                                 AttrStack.Pop();

                              }




                          }
                          else if (act[0].Contains("ASSIGN"))//
                          {
                              keyInfo ki = getSymbolFromAttrList("selectAssign");
                             
                              //MessageBox.Show("Assign_STMT.left=" + ki.getArttibute("left"));
                              //MessageBox.Show("Assign_STMT.op=" + ki.getArttibute("op"));
                              if (!ki.getArttibute("left").Trim().Equals(""))
                              {
                                  
                                  string stmt = ki.getArttibute("left") + "=" + ki.getArttibute("addr");
                                  //MessageBox.Show(stmt);
                                  string temp1=ki.getArttibute("left");
                                    string temp2=ki.getArttibute("addr");
                                  if (ki.getArttibute("left").Contains("["))
                                  {
                                     temp1=ki.getArttibute("left").Substring(0,ki.getArttibute("left").IndexOf("["));
                                     //MessageBox.Show(temp);


                                  }
                                   if (ki.getArttibute("addr").Contains("["))
                                  {
                                     temp2=ki.getArttibute("addr").Substring(0,ki.getArttibute("addr").IndexOf("["));
                                     //MessageBox.Show(temp);


                                  }
                                 
                                    string type1=ST.Peek().getVarType(temp1);
                                    string type2 = ST.Peek().getVarType(temp2);

                                  
                                  
                                    if (type2.Equals(""))
                                    {
                                        if (temp2.Contains("."))
                                        {
                                            type2 = "float";
                                        }
                                        else type2 = "int";
                                    }
                                    
                                   
                                    if ((type2.Contains("float") || type2.Contains("double")) && type1.Contains("int"))
                                    { 
                                        errorList.Add(new ERROR(curCharacter.getRow(),temp1,"无法把将float转为int，可能会造成溢出"));

                                    }
                                  else if(type2.Contains("int")&&type1.Contains("float"))
                                  {
                                      errorList.Add(new ERROR(curCharacter.getRow(),temp1,"警告！正在将int型赋值给float型"));
                                       
                                  }
                                      

                                  
                                  
                                  ThreeList.Add(stmt);
                              }

                              while (AttrStack.Count != 0 && !AttrStack.Peek().getType().Equals("selectAssign"))
                              {
                                //  MessageBox.Show( "POP:"+AttrStack.Pop().getType());
                                  AttrStack.Pop();

                              }




                          }
                          else   if (act[0].Contains("newTemp"))//
                          {
                              skipflag = false;
                             
                              String par = act[0].Substring(act[0].IndexOf("(") + 1);
                              par = par.Trim(')');
                              //getSymbolFromAttrList(par).setNewArttibute("addr", "");
                              getSymbolFromAttrList(par).addArttibute("addr","t"+tempCount);

                              tempCount++;
                             // MessageBox.Show(par);




                          }
                          else if (act[0].Contains("lookIDN"))//
                          {
                              lookIDN(act[0],curCharacter.getRow());


                          }
                           else if (act[0].Contains("newLabel"))//
                          {
                              String par = act[0].Substring(act[0].IndexOf("(") + 1);
                              par = par.Trim(')');
                              string[] pars = par.Split('.');
                              if (pars.Length == 2)
                              {
                                  string labelname = "L" + labelCount;
                                  labelCount++;
                                  getSymbolFromAttrList(pars[0]).addArttibute(pars[1], labelname);
                                 

                              }

                              //MessageBox.Show(par);




                          }
                           else if (act[0].Contains("BUILD"))//
                          {
                              String par = act[0].Substring(act[0].IndexOf("(") + 1);
                              par = par.Trim(')');
                              string[] pars = par.Split(',');
                              if (pars.Length >= 4)
                              {
                                  string stmt ="";
                                  for (int x = 0; x < pars.Length; x++)
                                  {
                                      string v = "";
                                      if (pars[x].Contains('\"'))
                                      {
                                          v = pars[x].Trim('\"');
                                      }
                                      else
                                      { 
                                          string []pp= pars[x].Split('.');
                                          if (pp.Length == 2)
                                          {
                                             // MessageBox.Show("!!" + pp[0] + " " + pp[1]);
                                             v=getSymbolFromAttrList(pp[0]).getArttibute(pp[1]);
                                          }

                                      }
                                      stmt += v+" ";
                                  }
                                
                                  ThreeList.Add(stmt);
                                 // MessageBox.Show(stmt);

                              }

                              //MessageBox.Show(par);




                          }
                          else if (act[0].Contains("label"))//
                          {
                              String par = act[0].Substring(act[0].IndexOf("(") + 1);
                              par = par.Trim(')');
                              string[] pars = par.Split('.');
                              if (pars.Length == 2)
                              {
                                  //MessageBox.Show("pars[0]"+pars[0]+"v"+pars[1]);
                                  string labelname =getSymbolFromAttrList(pars[0]).getArttibute(pars[1])+":";
                                 
                                 
                                  ThreeList.Add(labelname);
                                  //  MessageBox.Show(labelname);

                              }
                              //MessageBox.Show(par);
                          }
                         
                        
                          else if (act[0].Contains("lookGroup"))//
                          {
                     
                              lookGroup(act[0],curCharacter.getRow());
                            

                          }
                              
                          
                           else if (act[0].Contains("gen"))//
                          {

                              gen(act[0]);


                          }




                      }
                      else   //进行传值
                      {
                          string param1 = act[0];
                          string param2 = act[1];
                          if (param2.StartsWith("\""))
                          {
                              string[] param1s = param1.Split('.');
                              if (param1s.Length == 2)
                              {
                                  string symbolName=param1s[0];
                                  string symbolAttrName= param1s[1];
                                  string symbolAttrValue = param2.TrimStart('\"');
                                  symbolAttrValue = symbolAttrValue.TrimEnd('\"');
                                  //MessageBox.Show("afterTrim:" + symbolAttrValue);

                                  
                                  if (getSymbolFromAttrList(symbolName) != null)
                                  {
                                      keyInfo ki = getSymbolFromAttrList(symbolName);
                                      if(symbolAttrValue.Equals("NULL"))
                                      {
                                          ki.setNewArttibute(symbolAttrName,"");

                                      }
                                      else
                                     
                                      ki.addArttibute(symbolAttrName,symbolAttrValue);
                                     //MessageBox.Show(symbolName+ki.getArttibute(symbolAttrName));
                                     
                                    
                                 
                                  }
                              }

                              //直接传值
                          }
                         
                          else 
                          {
                              string[] param1s=param1.Split('.');
                              string[] param2s=param2.Split('.');
                              if (param1s.Length == 2 && param1s.Length == 2)
                              {
                                  string a_name = param1s[0];
                                  string a_value = param1s[1];
                                  string b_name = param2s[0];
                                  string b_value = param2s[1];
                                  if (getSymbolFromAttrList(a_name)!=null&&getSymbolFromAttrList(b_name)!=null)
                                  {
                                      keyInfo ka = getSymbolFromAttrList(a_name);
                                      keyInfo kb = getSymbolFromAttrList(b_name);
                                      string value=kb.getArttibute(b_value);     
                                      ka.addArttibute(a_value, value);

                                     // AttrStack.Pop();
                                     // MessageBox.Show(a_name+" "+a_value + "" + value);
                                    

                                      
                                  }
                                  
                                  
                              }


                          }
                         

                      }
                        
                    }
                    if (skipflag)
                    {
                        AttrStack.Pop();
                    }
                  
                        //此处开始编写语义动作
                    stack.Pop();//动作出分析栈
                }
               


            }

       
          
           
            
            

            




        }
    }
}

