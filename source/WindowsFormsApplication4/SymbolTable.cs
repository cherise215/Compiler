using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication4
{
    public class SymbolTable
    {
        /// <summary>
        /// 函数名（符号表的唯一标识）
        /// </summary>
       private string fun_name;//函数名
       /// <summary>
       /// 偏移量
       /// </summary>
       private int offset=0;//符号表内偏移地址
      //Sprivate string previous="NULL";//先前创建的表名 
       /// <summary>
       /// 返回值类型
       /// </summary>
       private string returnType="void";
       /// <summary>
       /// 变量列表
       /// </summary>
       private List<Variable> variableList = new List<Variable>();//变量列表
       /// <summary>
       /// 参数列表
       /// </summary>
       private List<Variable> paramList = new List<Variable>();//参数列表

       /// <summary>
       /// 变量名称队列
       /// </summary>
       private List<String> variableQuene= new List<String>();//判断是否已经声明
       /// <summary>
       /// 获取偏移量
       /// </summary>
       public int getOffset()
       {
           return offset;
                
       }
       /// <summary>
       /// </summary>
        public SymbolTable(string fun_name)
       {
           this.fun_name = fun_name;


        }
        /// <summary>
        /// 判断变量是否已声明
        /// </summary>
        public bool isIN(string varname)
        {
            if (variableQuene.Contains(varname))
            {
                return true;
            }
            return false;


        }
        /// <summary>
        /// 获取变量类型
        /// </summary>
        public string getVarType(string varname)
        {
            foreach (Variable va in variableList)
            {
                if (va.getVarName().Equals(varname))
                    return va.getType();
            }
            foreach (Variable v in paramList)
            {
                if (v.getVarName().Equals(varname))
                    return v.getType();
            }
            return "";


        
        }


        /// <summary>
        /// 获取函数名
        /// </summary>
        public string getFun_name()
        {
            return fun_name;
        }
        /// <summary>
        /// 设置返回值类型
        /// </summary>
        public void setReturnType(string returnType)
        {
            this.returnType=returnType;


        }
        /// <summary>
        /// 获取返回值类型
        /// </summary>
        public string  getReturnType()
        {
            return returnType;


        }
        /// <summary>
        /// 获取变量列表
        /// </summary>
        public List<Variable> getVariableList()
        
        {
            return variableList;
        }



        /// <summary>
        /// 添加一个变量
        /// </summary>
       public bool enterVariable(string type,string varName) 
       {



           if (variableQuene.Contains(varName))
           {
               return false;
           }

           Variable va = new Variable(varName, offset, type);
           offset += va.getLength();//偏移地址移动
           variableQuene.Add(varName);
           variableList.Add(va);


           return true;


	   }

       /// <summary>
       /// 添加一个参数
       /// </summary>
       public bool enterParam(string type,string varName)
       {
           if (variableQuene.Contains(varName))
           {
               return false;
           }

           Variable va = new Variable(varName, offset, type);
           offset += va.getLength();//偏移地址移动
           variableQuene.Add(varName);
           paramList.Add(va);


           return true;


       }

       /// <summary>
       /// 获取参数列表
       /// </summary>
       public List<Variable> getParamList()
       {
          
           return paramList;


       }
       /// <summary>
       /// 获取数据类型描述
       /// </summary>
       public List<int> getArrayWidthByName(string name)
       {
           if (variableQuene.Contains(name))
           {

               foreach (Variable va in variableList)
               {
                   if (va.getVarName().Equals(name))
                   {
                       return va.getArrayLength();
                   }
               }
               
           }
           return null;
           
       }
     
     
        
        




    }
}
