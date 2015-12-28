using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace WindowsFormsApplication4
{
    public class dict
    {
        private string id;
        private int code;
        public string getId()
        {
            return id;
        }
        public int getCode()
        {
            return code;
        }
        public   void  setCode(int code)
        {
            this.code = code;

        }
        public void setId(string id)
        {
            this.id = id;
        }
        public dict(string id,int code)
        {
            this.id = id;
            this.code = code;
        }

    }
    public class Scanner
    {
        //关键字
        string[] keyWord ={"far","near","pascal","register","asm","cdecl","huge","auto","double","int","struct","break","else","long","switch","case","enum","register","typedef","char","extern","return","union",
　　   "const","float","short","unsigned","continue","for","signed","void","default","goto","sizeof","volatile","do","if","while","static" ,"interrupt","sizeof"};
        string[] sepcialWord = { "NULL"};
        //单界符
        char[] SINGLEArray = { '{', '}', '[', ']', '(', ')', '~', ',', ';', '.', '#','?',':' };
        //运算符
        string[] MathArray = {"<<",">>","<","<=",">",">=", "=", "==", "|", "||","|=","^","^=","&","&&" ,"&=","%","%=","+","++","+=","-","--","-=","->", "/", "/=", "*", "*=","!", "!=","sizeof"};
        //复合
        string[] MutipleArray = {"<<=",">>="};

        string[] TypeArray = { "CONST_INT10","CONST_INT16","CONST_INT8", "CONST_FLOAT","CONST_CHAR","CONST_CHAR*"};
        string[] ConvertArray = {"n","r","0","t","v","b","f","a","\"","'","\\"};
        //是否跳跃

        HashSet<dict> map = new HashSet<dict>();
        public bool isSkip(char temp)
        {
            if (temp == ' ' || temp == '\t' || temp == '\b' || temp == '\n' || temp == '\r')
                return true;
            return false;
        }
        //是否数字
        public bool isDigit(char ch)
        {
            if ((ch >= '0') && (ch <= '9'))
                return true;
            else
                return false;
        }
        //是否字母
        public bool isLetter(char ch)
        {
            if (((ch >= 'A') && (ch <= 'Z')) || ((ch >= 'a') && (ch <= 'z')))
                return true;
            else
                return false;
           
        }
      
        //是否是关键字，返回key对象
        public key isKeyWord(string str)
        {
            int i = 0;
            for (; i < keyWord.Length; i++)
            {
                if (str.Equals(keyWord[i]))//是关键字
                {
                    key temp = new key(keyWord[i],i+257,str);//种别码从255开始
                    return temp;//输出关键字信息（种别码，以及相关信息）
                   
                }
            }
            if (str.Equals("NULL"))
            {
                key temp = new key("NULL", 1+keyWord.Length + 257, "NULL");//种别码从255开始
                return temp;//输出关键字信息（种别码，以及相关信息）
                   
            }

          
            return null
                ;
        }
        //获取界符的key对象
        public key isSingle(char ch)
        {
            int i = 0;

            for (; i < SINGLEArray.Length; i++)
            {
                if (ch==SINGLEArray[i])//是单界符
                {
                    string c = "" + ch;
                    key temp = new key(c, keyWord.Length+i + 258, c);
                    return temp;//输出信息（种别码，以及相关信息）

                }
            }
            return null
                ;
        }
        //获取运算符的key对象
        public key isMathOperator(string ch)
        {
            int i = 0;

            for (; i < MathArray.Length; i++)
            {
                if (ch == MathArray[i])//是界符
                {
                    key temp = new key(ch, keyWord.Length + SINGLEArray.Length + i + 258, ch);
                    return temp;//输出信息（种别码，以及相关信息）

                }
            }
            return null
                ;
        }
       
        //获取特殊运算符的key对象
        public key isMutipleOperator(string ch)
        {
            if (ch == "<<=")
            {
                key temp = new key(ch, MathArray.Length+keyWord.Length + SINGLEArray.Length + 1+ 258, "<<=");
                return temp;//输出信息（种别码，以及相关信息）
            }
            if (ch == ">>=")
            {
                key temp = new key(ch, MathArray.Length + keyWord.Length + SINGLEArray.Length + 2 + 258, ">>=");
                return temp;//输出信息（种别码，以及相关信息）
            }

           
            return null
                ;
        }

        //获得变量类别的种别码
        public int  getTypeCode(string ch)
        {
            int i = 0;

            for (; i < TypeArray.Length; i++)
            {
                if (ch == TypeArray[i])
                {

                    return keyWord.Length + SINGLEArray.Length + MutipleArray.Length + MathArray.Length + i + 258;//输出信息（种别码，以及相关信息）

                }
            }
            return 0
                ;
        }


        public key isConvertOperator(string ch)//获得转义字符的key对象
        {
            int i = 0;

            for (; i < ConvertArray.Length; i++)
            {
                if (ch == ConvertArray[i])//是转义字符
                {
                    key temp = new key(ch, keyWord.Length + SINGLEArray.Length + MutipleArray.Length + MathArray.Length + TypeArray.Length + i + 258, "CONST_CHAR");
                    return temp;//输出信息（种别码，以及相关信息）

                }
            }
            return null
                ;
        }

         public   List<String >writeToken()
        {
           List<string> tempkey = new List<string>();
           map = new HashSet<dict>();
           
          
                int i = 0;
                int code = 0;
                tempkey.Add("标识符"+ " " + 256);
                map.Add(new dict("IDN",256));
                tempkey.Add("------------关键字----------------------");
                for (i=0; i < keyWord.Length; i++)
                {

                    code = i+257;
                    tempkey.Add( keyWord[i]+" "+ code);
                    map.Add(new dict(keyWord[i] , code));
                }
                code++;
                tempkey.Add("NULL"+ " " +code);
                map.Add(new dict( "NULL",code));
               tempkey.Add("------------单个界符----------------------");
             
                for (i=0; i <SINGLEArray.Length; i++)
                {
                    code ++;

                    tempkey.Add(SINGLEArray[i] + " " + code);
                    map.Add(new dict(SINGLEArray[i]+"" , code));
               
               
                       
                }




                tempkey.Add("------------运算符----------------------");

             
                for (i=0; i <MathArray.Length; i++)
                {
                    code ++ ;

                    tempkey.Add(MathArray[i] + " " + code);
                    map.Add(new dict(MathArray[i] + "", code));
                   
                }
                  code++;

                  tempkey.Add("<<=" +" "+code);
                  map.Add(new dict("<<=", code));
                  code++;
                  tempkey.Add(">>=" + " " + code );
                  map.Add(new dict(">>=", code));
                  tempkey.Add("------------常量----------------------");
                 
                for (i=0; i <TypeArray.Length; i++)
                {
                    code ++;

                    tempkey.Add(TypeArray[i] + " " + code);
                    map.Add(new dict(TypeArray[i], code));
                       

                    
                }
                tempkey.Add("------------转义字符----------------------");

               
                for (i=0; i < ConvertArray.Length; i++)
                {

                    code++;
                    string a = "\\" + ConvertArray[i];
                    tempkey.Add(a+ " "+ code);
                  //  map.Add(new dict("CONS", code));

                }
                return tempkey;


               
          
         }
        public HashSet<dict>  getMap()
        {
            return map;

        }

           
      
        

        public Scanner()
        {
            writeToken();
           
        }
    }
}
