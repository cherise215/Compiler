using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication4
{
   public class myLex
    {
        /// <summary>
        /// 单词表
        /// </summary>
        static List<keyInfo> tokenList = new List<keyInfo>(); //token表
        static List<ERROR> errorList = new List<ERROR>();   //错误描述
        /// <summary>
        /// 符号表
        /// </summary>
        /// 符号表
        static List<Symbol> symbolList = new List<Symbol>();   ///符号表                               ///

        private static int IGNORE_MAX_LENGTH = 200;//注释成功最大长度                                              
        private string[] token; //标识符表
        private int tokenIndex = 0; //标识符表指针
        private long[] constant; //整型常数表
        private int constantIndex = 0; //整型常数表指针
        private double[] dblConstant; //浮点常数表
        private int dblConstantIndex = 0; //浮点常数表指针
        private string[] strConst; //字符串表
        private int strConstIndex; //字符串表指针
        private char[] charConst; //字符表
        private int charConstIndex = 0; //字符表指针
        /// <summary>
        /// 进行词法分析的字符串
        /// </summary>
        private string input; //存放待识别的源程序字符串
        /// <summary>
        /// 进行词法分析的字符串的下标
        /// </summary>
        private int inputIndex = 0; //input字符串指针
        /// <summary>
        /// 当前行数
        /// </summary>
        private int row = 1;
        /// <summary>
        /// 当前字符
        /// </summary>
        private char ch; //存放最新读进的源程序字符
        /// <summary>
        /// 字符读取缓冲区
        /// </summary>
        private string strToken; //存放构成单词符号的字符串
       
        //移动指针，读取下一个字符
        /// <summary>
        /// 指针移动到下一个字符
        /// </summary>
        private void getChar()
        {
            if (inputIndex < input.Length)
            {
                ch = input[inputIndex];
                inputIndex++;
            }
           
        }
        /// <summary>
        /// 判断运算符/界符
        /// </summary>
        private void judgeSeperator(char a,char b)
        {
             key tempWord=new key();
          
             string t2 ="";
            
             string t1 = "";
             t1 += a;
             t1 += b;
             t2 += a;
             t2 += a;
            if (inputIndex < input.Length)
            {
                getChar();
                if (ch==a )
                {
                   // MessageBox.Show(t2+","+ ch);
                    tempWord = sc.isMathOperator(t2);
                    tokenList.Add(new keyInfo(tempWord, row));
                    return;
                   
                   
                }
                else if (ch == b)
                {
                    tempWord = sc.isMathOperator(t1);
                   tokenList.Add(new keyInfo(tempWord, row));
                   return;
                  
                }
               
                else
                {
                    retract();
                    tempWord = sc.isMathOperator(a+"");
                    tokenList.Add(new keyInfo(tempWord, row));
                    return;
                   // continue;
                }
                //tokenList.Add(new keyInfo(tempWord, row));
            }
            else
            {
                tempWord = sc.isMathOperator(a+"");
                tokenList.Add(new keyInfo(tempWord, row));
                return;
               // continue;
            }
          
        }
        /// <summary>
        /// 判断运算符/界符
        /// </summary>
        private void judgeSeperator(char a)
        {
            key tempWord = new key();

           
            string t1 = "";
            t1 += a;
            t1 += "=";
           
            if (inputIndex < input.Length)
            {
                getChar();
                if (ch == '=')
                {
                   // MessageBox.Show(t2 + "," + ch);
                    tempWord = sc.isMathOperator(t1);
                    tokenList.Add(new keyInfo(tempWord, row));
                    return;


                }
               

                else
                {
                    retract();
                    tempWord = sc.isMathOperator(a + "");
                    tokenList.Add(new keyInfo(tempWord, row));
                    return;
                    // continue;
                }
                //tokenList.Add(new keyInfo(tempWord, row));
            }
            else
            {
                tempWord = sc.isMathOperator(a + "");
                tokenList.Add(new keyInfo(tempWord, row));
                return;
                // continue;
            }

        }

        /// <summary>
        /// 判断是否是多个界符
        /// </summary>
        private void judgeMutiple(char a)
        {
            key tempWord = new key();


            string t1 = "";
            t1 += a;
            string t3 = "";
            t3 = t1 + "=";
            t1+=a;
            string t2 = t1;
            t1 += "=";



            if (inputIndex < input.Length)
            {
                getChar();
                if (ch == '=')
                {
                    // MessageBox.Show(t2 + "," + ch);
                    tempWord = sc.isMathOperator(t3);
                    tokenList.Add(new keyInfo(tempWord, row));
                    return;


                }

                 else if(ch==a)
                {
                    if (inputIndex < input.Length)
                    {
                        getChar();
                        if (ch == '=')
                        {
                            tempWord = sc.isMutipleOperator(t1);
                            tokenList.Add(new keyInfo(tempWord, row));
                            return;
                        }
                        else
                        {
                            retract();
                            tempWord = sc.isMathOperator(t2);
                            tokenList.Add(new keyInfo(tempWord, row));
                            return;
                        }
                    }
                    else
                    {

                        tempWord = sc.isMathOperator(t2);
                        tokenList.Add(new keyInfo(tempWord, row));
                        return;
                    }
                    // continue;
                }
                else
                {
                    retract();
                    tempWord = sc.isMathOperator(a + "");
                    tokenList.Add(new keyInfo(tempWord, row));
                    return;
                    // continue;
                }
                //tokenList.Add(new keyInfo(tempWord, row));
            }
            else
            {
                tempWord = sc.isMathOperator(a + "");
                tokenList.Add(new keyInfo(tempWord, row));
                return;
                // continue;
            }

        }
        //字符预读，与上一个字符进行拼接
        /// <summary>
        /// token串读取缓存
        /// </summary>
        private void concat()
        {
            strToken += ch;
        }
        //指针返回上一字符位置
        /// <summary>
        /// 指针回退
        /// </summary>
        private void retract()
        {
            inputIndex--;
        }

        //将strToken中的标识符插入符号表（表中不存在该元素时才插入），返回符号表指针
        /// <summary>
        /// 插入新的标识符到符号表
        /// </summary>
        private void insertId()
        {
            //先在符号表里找该元素
            int i;
            bool notFound = true;
            string s = strToken;
            for (i = 0; notFound && i < token.Length; i++)
            {
                if (token[i] == s) notFound = false;
            }
            //没找到
            if (notFound)
            {
                //插入到符号表内
                token[tokenIndex] = strToken;
                Symbol _symbol=new Symbol(tokenIndex, strToken, row);
                symbolList.Add(_symbol);

                tokenIndex++;
               
            }
          
           
        }

        /// <summary>
        /// 获取错误列表
        /// </summary>
        public  List<ERROR> getError()
        {

            return errorList;
        }
        /// <summary>
        /// 获取词法分析结果
        /// </summary>
        public  List<keyInfo> getTokenList()
        {

            return tokenList;
        }
        /// <summary>
        /// 获取符号表信息
        /// </summary>
        public  List<Symbol> getSymbolList()
        {

            return symbolList;
        }
        //将strToken中的整型常数插入整型常数表（表中不存在该元素时才插入），返回整型常数表指针
        /// <summary>
        /// 插入常量
        /// </summary>
        private int insertConst()
        {
            int i;
            bool notFound = true;
            long s = int.Parse(strToken);
            for (i = 0; notFound && i < constant.Length; i++)
            {
                if (constant[i] == s) notFound = false;
            }
            //没找到
            if (notFound)
            {
                constant[constantIndex] = int.Parse(strToken);
                constantIndex++;
                return (constantIndex - 1);
            }
            //找到了
            else return i - 1;
        }


        //将strToken中的浮点型常数插入浮点常数表（表中不存在该元素时才插入），返回浮点常数表指针
        private int insertDblConst()
        {
            int i;
            bool notFound = true;
            double s = double.Parse(strToken);
            for (i = 0; notFound && i < dblConstant.Length; i++)
            {
                if (dblConstant[i] == s) notFound = false;
            }
            //没找到
            if (notFound)
            {
                dblConstant[dblConstantIndex] = double.Parse(strToken);
                dblConstantIndex++;
                return (dblConstantIndex - 1);
            }
            //找到了
            else return i - 1;
        }

        //将strToken中的字符串插入字符串表（表中不存在该元素时才插入），返回字符串表指针
        /// <summary>
        /// 插入字符串
        /// </summary>
        private int insertString()
        {
            int i;
            bool notFound = true;
            string s = strToken;
            for (i = 0; notFound && i < strConst.Length; i++)
            {
                if (strConst[i] == s) notFound = false;
            }
            //没找到
            if (notFound)
            {
                strConst[strConstIndex] = strToken;
                strConstIndex++;
                return (strConstIndex - 1);
            }
            //找到了
            else return i - 1;
        }

        //将strToken中的字符插入字符表（表中不存在该元素时才插入），返回字符表指针
        /// <summary>
        /// 插入字符型
        /// </summary>
        private int insertChar()
        {
            int i;
            
            bool notFound = true;
            char c = ch;
            for (i = 0; notFound && i < charConst.Length; i++)
            {
                if (charConst[i] == c) notFound = false;
            }
            //没找到
            if (notFound)
            {
                charConst[charConstIndex] = ch;
                charConstIndex++;
                return (charConstIndex - 1);
            }
            //找到了
            else return i - 1;
        }


       
      
        /// <summary>
        /// scanner对象
        /// </summary>
        Scanner sc = new Scanner();
        /// <summary>
        /// 词法分析开始
        /// </summary>
        public void start(string scanInput)
        {
            input = scanInput;
            errorList.Clear();
            tokenList.Clear();
            symbolList.Clear();
            
            token = new string[10000];
            constant = new long[10000];
            dblConstant = new double[10000];
            charConst = new char[10000];
            strConst = new string[10000];
            tokenIndex = 0;
            constantIndex = 0;
            dblConstantIndex = 0;
            strConstIndex = 0;
            charConstIndex = 0;
            inputIndex = 0;

            key tempWord = new key();

            while (inputIndex < input.Length)
            {
                strToken = "";
                getChar();

                if (sc.isSkip(ch))
                {
                    if (ch == '\n')
                    {
                        row++;

                    }
                    continue;
                }
                //单界符号
                else if (sc.isSingle(ch) != null)
                {   tempWord = sc.isSingle(ch);
                    tokenList.Add(new keyInfo(tempWord,row)); 
                    continue;


                }
                /////////////////////////////出现斜杠处理//////////////////////////////////////////
                else if (ch == '/')
                {
                    int state = 1;
                    bool isEnd = false;
                    int book = 0;
                    int startRow = row;
                    while (isEnd == false&&book<IGNORE_MAX_LENGTH)
                    {
                        if (inputIndex >= input.Length)
                        {
                            break;
                        }
                        getChar();
                        //MessageBox.Show(ch + "," + state + "");
                        concat();
                        
                        switch (ch)
                        {
                             
                            case '=':
                                {
                                    if (state == 1)
                                    {
                                        state = 2;
                                        isEnd = true;
                                    }
                                    else if(state==6)
                                        state = 5;
                                   
                                    break;
                                }
                            case '*':
                                {
                                    switch (state)
                                    {
                                        case 1:
                                            {
                                                state = 5; break;
                                            }
                                        case 5:
                                            {
                                                state = 6; break;
                                            }
                                        case 6:
                                            {
                                                state = 6; break;
                                            }
                                        case 3:
                                            {
                                                state = 3; break;
                                            }

                                    }
                                    break;
                                }
                            case '/':
                                {
                                    switch (state)
                                    {
                                        case 1:
                                            {
                                                state = 3; break;
                                            }
                                        case 5:
                                            {
                                                state =5 ; break;
                                            }
                                        case 6:
                                            {
                                                book = 0;
                                                state = 7;
                                                isEnd = true;
                                                break;
                                            }
                                        case 3:
                                            {
                                                state = 3; break;
                                            }

                                    }
                                    break;

                                }
                            case '\n':
                                {
                                    row++;
                                    
                                     switch (state)
                                    {
                                        case 1:
                                            {
                                                state = 8;
                                                isEnd=true;
                                                break;
                                            }
                                        case 3:
                                            {
                                                state =4 ;
                                                isEnd=true;
                                                break;
                                            }
                                        case 5:
                                            {
                                                book++;

                                                state = 5;
                                              
                                                break;
                                            }
                                        case 6:
                                            {
                                                book++;
                                                state = 5; break;
                                            }

                                    }

                                    break;
                                }


                            default:
                                switch (state)
                                    {
                                        case 1:
                                            {
                                                state = 1;
                                                retract();
                                                isEnd=true;
                                                break;
                                            }
                                        case 5:
                                            {
                                                state =5 ;
                                                //isEnd=true;
                                                break;
                                            }
                                        case 6:
                                            {
                                                state = 5;
                                               
                                                break;
                                            }
                                        case 3:
                                            {
                                                state = 3; break;
                                            }

                                    }
                                
                                break;
                        }
                    }
                    if (book == IGNORE_MAX_LENGTH)
                    {
                        errorList.Add(new ERROR(startRow, strToken, "超过注释最大行数"+IGNORE_MAX_LENGTH));
                        continue;
                    }
                    
                    if (state == 1)
                    {
                        tempWord = sc.isMathOperator("/");
                        tokenList.Add(new keyInfo(tempWord, row));


                    }
                    else if (state == 2)
                    {
                        tempWord = sc.isMathOperator("/=");
                        tokenList.Add(new keyInfo(tempWord, row));

                    }
                    else if (state == 4)
                    {
                        continue;
                    }
                     else if (state == 7)
                    {
                        continue;
                    }
                    else if (state == 8)
                    {
                        tempWord = sc.isMathOperator("/");
                        tokenList.Add(new keyInfo(tempWord, row));

                    }
                    else if (state == 6 || state == 5)
                    {
                        errorList.Add(new ERROR(startRow, strToken, "注释未封闭"));
                    }

                    continue;

                }

              
                //////////////////下面处理多状态匹配的界符//////////////////////////////////////////
                else if (ch == '-')
                {
                    if (inputIndex < input.Length)
                    {
                        getChar();
                        if (ch == '-')
                        {
                            tempWord = sc.isMathOperator("--");
                            tokenList.Add(new keyInfo(tempWord, row));
                            continue;
                        }
                        else if (ch == '=')
                        {
                            tempWord = sc.isMathOperator("-=");
                            tokenList.Add(new keyInfo(tempWord, row));
                            continue;
                        }
                        else if (ch == '>')
                        {
                            tempWord = sc.isMathOperator("->");
                            tokenList.Add(new keyInfo(tempWord, row));
                            continue;
                        }
                        else
                        {
                            retract();
                            tempWord = sc.isMathOperator("-");
                            tokenList.Add(new keyInfo(tempWord, row));
                            continue;
                        }
                    }
                    else
                    {
                        tempWord = sc.isMathOperator("-");
                        tokenList.Add(new keyInfo(tempWord, row));
                        continue;
                    }
                }
                else if (ch == '|')
                {
                    judgeSeperator('|', '=');
                    continue;
                }
                else if (ch == '+')
                {
                    judgeSeperator('+', '=');
                    continue;
                }

                else if (ch == '&')
                {
                    judgeSeperator('&', '=');
                    continue;
                }
                else if (ch == '*')
                {
                    judgeSeperator('*');
                    continue;
                }
                else if (ch == '!')
                {
                    judgeSeperator('!');
                    continue;
                }
                else if (ch == '%')
                {
                    judgeSeperator('%');
                    continue;
                }
                else if (ch == '^')
                {
                    judgeSeperator('^');
                    continue;
                }
                else if (ch == '=')
                {
                    judgeSeperator('=');
                    continue;
                }
                else if (ch == '<')
                {
                    judgeMutiple('<');
                    continue;
                }
                else if (ch == '>')
                {
                    judgeMutiple('>');
                    continue;
                }

                //////////////////////下面处理标识符，出现字母或_下划线处理//////////////////////////////////////////////////


                else if (sc.isLetter(ch) || ch == '_')
                {
                    if (input.Length > 1)
                    {
                        while ((sc.isLetter(ch) || sc.isDigit(ch) || (ch == '_')) && inputIndex < input.Length)
                        {
                            concat();
                            getChar();
                        }

                        if (inputIndex < input.Length) retract();
                        else if (inputIndex == input.Length)
                        {
                            if (sc.isLetter(ch) || sc.isDigit(ch) || (ch == '_')) concat();
                            else retract();
                        }
                        if (sc.isKeyWord(strToken) != null)
                        {
                            tempWord = sc.isKeyWord(strToken);
                            tokenList.Add(new keyInfo(tempWord, row));
                            continue;
                        }



                        else
                        {
                            string name = strToken ;
                            key tempkey = new key(name, 256, "IDN");
                            
                            tokenList.Add(new keyInfo(tempkey, row));



                            insertId();
                             

                            continue;
                        }
                    }
                    else
                    {
                        string name =  strToken ;
                        key tempkey = new key(name, 0, "IDN");
                        tokenList.Add(new keyInfo(tempkey, row));
                        continue;
                    }

                }
                //////////////////////下面处理16、8进制数字//////////////////////////////////////////////////
               
                //////////////////////下面处理十进制数字//////////////////////////////////////////////////
                
                else if (sc.isDigit(ch))
                {
                    bool isEnd = false;
                    int state = 1;
                    int isError = 0;
                    int isFalse = 0;
                    bool isTen = true;
                    concat();
                    int isFirst = 0;
                    

                    if (ch == '0')
                    {
                        isError = 0;
                        while (isEnd == false)
                        {
                            isFirst++;
                            if (inputIndex >= input.Length)
                            {
                                break;
                            }
                            getChar();
                           

                            if (ch == '.')
                            {
                                switch (state)
                                {
                                    case 1:
                                       
                                            retract();
                                            isEnd = true;

                                      //  isTen = true;
                                        break;

                                    default:
                                        {
                                            isError++;
                                            break;
                                        }
                                }
                                continue;

                            }
                            else
                            {
                                if (isFirst == 1)
                                {
                                    isTen = false;
                                   
                                } concat();

                            }


                            if (ch == 'x' || ch == 'X')
                            {
                                switch (state)
                                {
                                    case 1: state = 9; break;
                                    default:
                                        {
                                            isError++;
                                            break;
                                        }
                                }
                            }
                            else if (ch >= '0' && ch <= '7')
                            {

                                switch (state)
                                {
                                    case 1: state = 10; break;
                                    case 10: state = 10; break;
                                    case 9: state = 11; break;
                                    case 11: state = 11; break;
                                    default:
                                        {
                                            isError++;
                                            break;
                                        }
                                }
                            }
                            else if ((ch >= 'a' && ch <= 'f')||(ch>='A'&&ch<='F')||ch=='9'||ch=='8')
                            {

                                switch (state)
                                {
                                   
                                    case 9: state = 11; break;
                                    case 11: state = 11; break;
                                    default:
                                        {
                                            isError++;
                                            break;
                                        }
                                }
                            }
                            else if (sc.isLetter(ch) )
                            { 
                                isError++;
                                //break;
                                       
                               
                            }
                            
                            
                            else
                            {
                                strToken = strToken.Substring(0, strToken.Length - 1);
                                retract();
                                isEnd = true;
                               
                            }
                             continue;
                           
                        }

                    
                    }

                   if (isTen==true){
                       isEnd = false;
                       isError = 0;
                    while (isEnd == false)
                    {
                        if (inputIndex >= input.Length)
                        {
                            break;
                        }
                        getChar();
                      
                        concat();
                        if (sc.isDigit(ch))
                        {
                            switch (state)
                            {
                                case 1: state = 1; break;
                                case 2: state = 3; break;
                                case 3: state = 3; break;
                                case 5: state = 6; break;
                                case 6: state = 6; break;
                                case 4: state = 6; break;

                            }
                            continue;
                        }
                   
                        else if (ch == '.')
                        {
                            switch (state)
                            {
                                case 1: state = 2; break;
                                default:
                                    {
                                        isError++;
                                        break;
                                    }

                            }
                            continue;
                        }
                        else if (ch == 'e'||ch=='E')
                        {
                            switch (state)
                            {
                                case 1:
                                    {
                                        state = 4; 
                                        isFalse++;break;
                                    }
                                case 3: state = 4; break;
                                default:
                                    {
                                        retract();
                                        isEnd = true; break;
                                    }

                            }
                            continue;
                        }
                        else if (ch == '+')
                        {
                            switch (state)
                            {
                                case 4: state = 5; break;
                                
                             
                                default:
                                    {
                                        strToken = strToken.Substring(0, strToken.Length - 1);
                                        retract();
                                        isEnd = true; break;
                                    }

                            }
                            continue;
                        }
                        else if (ch == '-')
                        {
                            switch (state)
                            {
                                case 4: state = 5; break;

                                default:
                                    {
                                        strToken = strToken.Substring(0, strToken.Length - 1);
                                        retract();
                                        isEnd = true; break;
                                    }

                            }
                            continue;
                        }
                        else if (sc.isLetter(ch))
                        {
                            isError++;
                            continue;

                        }
                        else
                        {
                            strToken = strToken.Substring(0, strToken.Length - 1);
                            retract();
                            isEnd = true; break;
                        }

                         
                    }
                   }
                    if (isError > 0)
                    {
                        errorList.Add(new ERROR(row, strToken, "数字表示不正确"));
                        continue;
                    }
                   if (state==1)
                    {
                        int tempcode = sc.getTypeCode("CONST_INT10");
                        key temp = new key(strToken + "", tempcode, "CONST_INT10");
                        tokenList.Add(new keyInfo(temp, row));
                        continue;
                    }
                   if (state == 6)
                   {
                       if (isFalse > 0)
                       {
                           int tempcode = sc.getTypeCode("CONST_INT10");
                           key temp = new key(strToken + "", tempcode, "CONST_INT10");
                           tokenList.Add(new keyInfo(temp, row));
                       }
                        else 
                       {
                           int tempcode = sc.getTypeCode("CONST_FLOAT");
                           key temp = new key(strToken + "", tempcode, "CONST_FLOAT");

                           tokenList.Add(new keyInfo(temp, row));
                       }
                       continue;
                   }
                   if (state == 3||state==6)
                   {
                       
                           int tempcode = sc.getTypeCode("CONST_FLOAT");
                           key temp = new key(strToken + "", tempcode, "CONST_FLOAT");
                           tokenList.Add(new keyInfo(temp, row));
                       
                       continue;
                   }
                   if (state == 4)
                   {
                       errorList.Add(new ERROR(row, strToken, "e后必须接+，-，或数字"));
                       continue;
                   }
                   if (state == 5)
                   {
                       errorList.Add(new ERROR(row, strToken, "数字表示不正确"));
                       continue;
                   }
                   if (state == 2)
                   {
                       errorList.Add(new ERROR(row, strToken, ".后必须接数字"));
                       continue;
                   }
                   if (state == 9)
                   {
                       errorList.Add(new ERROR(row, strToken, "不正确的16进制整数表示"));
                       continue;
                   }
                   if (state == 11)
                   {
                       int tempcode = sc.getTypeCode("CONST_INT16");
                       key temp = new key(strToken + "", tempcode, "CONST_INT16");
                       tokenList.Add(new keyInfo(temp, row));
                       continue;
                   }
                   if (state == 10)
                   {
                       int tempcode = sc.getTypeCode("CONST_INT8");
                       key temp = new key(strToken + "", tempcode, "CONST_INT8");
                       tokenList.Add(new keyInfo(temp, row));
                       continue;
                   }
                  
                  
                   




                }

              

                //////////////////////下面处理字符//////////////////////////////////////////////////
                else if (ch == '\'')
                {
                    bool isEnd = false;
                    int state = 1;
                    int isError = 0;
                    int isFalse = 0;
                    while (isEnd == false)
                    {
                        if (inputIndex >= input.Length)
                        {
                            break;
                        }
                        getChar();
                        //MessageBox.Show(ch + "," + state + "");
                        concat();
                        switch (ch)
                        {

                            case '\\':
                                {

                                    if (state == 1)
                                    {
                                        state = 2;

                                    }
                                    else if (state == 2)
                                        state = 5;

                                    break;
                                }

                            case '0':
                            case 'a':
                            case 'v':
                            case 'f':
                            case 'b':
                            case 't':
                            case 'r':
                            case 'n':
                                {
                                    if (state == 2)
                                        state = 5;
                                    else if (state == 1)
                                    {
                                        state = 3;
                                    }
                                    else if (state == 3)
                                        isError++;
                                    break;
                                }
                            case '\'':
                                {
                                    if (state == 2)
                                        state = 5;
                                    else if (state == 1)
                                    {
                                        state = 7;
                                        isEnd = true;
                                    }
                                    else if (state == 3)
                                    {
                                        state = 6;
                                        isEnd = true;
                                    }
                                    else if (state == 5)
                                    {
                                        state = 9;
                                        isEnd = true;
                                    }
                                    break;
                                }
                            case '\n':
                                {
                                    retract();
                                    isEnd = true;
                                    break;
                                }
                            case '+':
                            case '-':
                            case '*':
                            case '/':
                            case '%':
                            case '^':
                            case '@':
                            case '~':
                            case '.':
                            case '|':
                            case '_':
                            case '(':
                            case ')':
                            case '{':
                            case '}':
                            case '#':
                            case '$':
                            case '!':
                            case '`':
                            case '=':
                            case '[':
                            case ']':
                            case ':':
                            case ';':
                            case '<':
                            case '>':
                            case '?':
                                {
                                    if (state == 1)
                                        state = 3;
                                    else if (state == 2)
                                    {
                                        //isEnd = true;
                                        // isError++;
                                        state = 5;
                                        isFalse++;
                                    }
                                    else if (state == 3)
                                    {
                                        retract();
                                        isEnd = true;
                                    }

                                    break;
                                }
                            case '"':
                                {

                                    if (state == 2)
                                        state = 5;
                                    else if (state == 1)
                                        state = 3;
                                    break;
                                }

                            default:
                                {
                                    if (state == 3)
                                    {
                                        isError++;
                                    }
                                    else if (state == 5)
                                    {
                                        isError++;

                                    }
                                    else if (state == 2)
                                    {
                                        state = 5;
                                        isFalse++;

                                    }
                                    else if (state == 1)
                                    {
                                        state = 3;

                                    }
                                    break;
                                }

                        }

                        continue;
                    }
                    if (isFalse > 0)
                    {
                        errorList.Add(new ERROR(row, strToken, "存在无法识别的转义序列"));
                        // continue;

                    }
                    if (isError > 0)
                    {
                        errorList.Add(new ERROR(row, strToken, "字符长度不正确"));
                        continue;


                    }

                    if (state == 9)
                    {
                        tempWord = sc.isConvertOperator(input[inputIndex - 2] + "");
                        if (tempWord != null)
                        {
                            tempWord.setName("\'\\" + input[inputIndex - 2] + "\'");
                            tokenList.Add(new keyInfo(tempWord, row));
                        }
                        //tokenList.Add(new keyInfo(tempWord, row));

                        continue;

                    }

                    else if (state == 6)
                    {

                        char ch2 = input[inputIndex - 2];
                        int tempcode = sc.getTypeCode("CONST_CHAR");
                        key temp = new key(ch2 + "", tempcode, "CONST_CHAR");
                        tokenList.Add(new keyInfo(temp, row));
                        continue;
                    }
                    else if (state == 3 || state == 2 || state == 5 || state == 1)
                    {
                        errorList.Add(new ERROR(row, strToken, "invalid of '"));
                        continue;
                    }
                    else if (state == 7)
                    {
                        errorList.Add(new ERROR(row, strToken, "字符不能为空"));
                        continue;
                    }
                    continue;

                }


                ///////////////////////////////////下面开始判断字符串(通过DFA有限状态机)////////////////////////////////////////////////////////////////////////// 

                else if (ch == '\"')
                {
                    bool isEnd = false;
                    int state = 1;
                    int isError = 0;
                    while (isEnd == false)
                    {
                        if (inputIndex >= input.Length)
                        {
                            break;
                        }
                        getChar();
                        //MessageBox.Show(ch + "," + state + "");
                        concat();
                        switch (ch)
                        {

                            case '\\':
                                {

                                    if (state == 1)
                                    {
                                        state = 2;

                                    }
                                    else if (state == 2)
                                        state = 1;

                                    break;
                                }
                            case '\"':
                                {
                                    if (state == 2)
                                    {
                                        state = 1;
                                    }
                                    else if (state == 1)
                                    {
                                        state = 3;
                                        isEnd = true;
                                    }

                                    break;
                                }

                            case 'a':
                            case 'v':
                            case 'f':
                            case 'b':
                            case 't':
                            case 'r':
                            case 'n':
                            case '0':
                                {
                                    if (state == 2)
                                        state = 1;
                                    else
                                    {
                                        state = 1;
                                    }
                                    break;
                                }
                            case '\'':
                                {
                                    if (state == 2)
                                        state = 1;
                                    else
                                    {
                                        state = 1;
                                        isError++;
                                    }
                                    break;
                                }
                            case '\n':
                                {
                                    retract();
                                    state = 1;
                                    isEnd = true;
                                    break;
                                }
                            default:
                                {
                                    if (state == 2)
                                        isError++;
                                    state = 1;
                                    break;
                                }

                        }
                        continue;
                    }
                    if (isError > 0)
                    {
                        errorList.Add(new ERROR(row, strToken, "字符串有不合法的字符"));
                    }
                    else if (state == 3)
                    {
                        int tempcode = sc.getTypeCode("CONST_CHAR*");
                        strToken = strToken.Substring(0, strToken.Length - 1);
                        key temp = new key(strToken, tempcode, "CONST_CHAR*");
                        tokenList.Add(new keyInfo(temp, row));

                    }
                    else if (state == 1)
                    {
                        errorList.Add(new ERROR(row, strToken, "invalid of \""));
                    }
                    else if (state == 2)
                    {
                        errorList.Add(new ERROR(row, strToken, "字符串内转义字符不合法"));
                    }



                    continue;
                }
                ///////////////////////////////////////////////////////////////////////////////////
                else
                {
                    errorList.Add(new ERROR(row, ch + "", "未定义的字符"));
                    continue;
                }

              
               
            }



           
        }

    }
}



