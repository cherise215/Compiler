using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication4
{
   public class key
    {
       string name;
        int code;
        string type;
     
    
       
        public key(string name,int code,string type)
        {
            this.name = name;
            this.code=code;
            this.type = type;
        }

        public key()
        {
        }
        /// <summary>
        /// 获取token名
        /// </summary>
          public string getName()
        {
            return name;
        }
          public void  setName(string name)
          {
              this.name=name;
          }

          /// <summary>
          /// 获取种别码
          /// </summary>
          public int getCode()
          {
              return code;
          }
          /// <summary>
          /// 获取种类
          /// </summary>
          public String getType()
          {
              return type;
          }

    }


   public class keyInfo 
   {
       /// <summary>
       /// 词法分析单元
       /// </summary>
        private key k;
        /// <summary>
        /// 所在行数
        /// </summary>
        private int row;
        /// <summary>
        /// 名称
        /// </summary>
        string name;
        /// <summary>
        /// 种别码
        /// </summary>
        int code;
        /// <summary>
        /// 句法分析种别
        /// </summary>
        string type;
        /// <summary>
        /// 值
        /// </summary>
        string value;
        /// <summary>
        /// 属性集合
        /// </summary>
        List<SymbolArttribute> attrList= new List<SymbolArttribute>();

        /// <summary>
        /// 属性名称列
        /// </summary>
        List<string> attrquene = new List<string>(); 

       public void addArttibute(string name,string value)
       {
           bool flag = false;
           foreach (string attrname in attrquene)
           {
               if (attrname.Equals(name))
               {
                   flag = true;
                   break;

               }
              
               
           }
           if (flag)
           {
               foreach (SymbolArttribute atts in attrList)
               {
                   if (atts.getName().Equals(name))
                   {
                       string oldValue = atts.getValue();
                       oldValue += value;
                       atts.setValue(oldValue);

                   }


               }

           }
           else
           {
               SymbolArttribute attr = new SymbolArttribute(name, value);
               attrList.Add(attr);
               attrquene.Add(name);
           }

           

       }
       public string  getArttibute(string name)
       {
           foreach (SymbolArttribute attr in attrList)
           { 
               if(attr.getName().Equals(name))
               {
                   return attr.getValue();

               }
              
           }
               
           return "";

       }
       public void setNewArttibute(string name,string value)
       {
           bool flag = true;
           foreach (SymbolArttribute attr in attrList)
           {
               if (attr.getName().Equals(name))
               {
                   attr.setValue(value);
                   flag = false;
                   break;
               }

           }
           if (flag)
           {
               addArttibute(name, value);
           }

          

       }



       public keyInfo(key k,int row)
       {
           
         this.name =k.getName();
        this.code=k.getCode();
        this.type =k.getType();
        this.row = row;

       }
       public keyInfo(string s)//type
       {

           this.name ="";
           this.code =-1;
           this.type =s;
           this.row = -1;
           

       }

       public key getK()
       {
           return k;
       }
       public string getName()
       {
           return name;
       }

       public int getCode()
       {
           return code;
       }
       public string getValue()
     {
       return value;
      }
       public void setValue(string value)
       {
           this.value = value;

       }
       public String getType()
       {
           return type;
       }
       public int getRow()
       {
           return row;
       }

   }
   
}
