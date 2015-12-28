using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace WindowsFormsApplication4
{
  

/**
 * 代表符号表的一个变量
 * 
 * author :cc
 * 
 */
public class Variable {


    /// <summary>
    /// 变量名
    /// </summary>
    private string varName;// 变量名
    /// <summary>
    /// 变量基本类型
    /// </summary>
	private string Type;// 变量类型
    /// <summary>
    /// 内存分配首地址
    /// </summary>
	private int addr;// 变量的相对地址
    /// <summary>
    /// 变量总长度
    /// </summary>
	private int length;// 变量的长度
    /// <summary>
    /// 变量类型表达式
    /// </summary>
    private List<int> arrayLength=new List<int>();
	
   
   
	public Variable(string varName, int addr, string type) {
		this.varName = varName;
       
        //MessageBox.Show("rawtype" +type);
        if (type.Contains("["))
        {
            this.Type = type;
            this.addr = addr;
            string[] types = type.Split(' ');
             int alength = getLengthByType(types[0]);
             arrayLength.Add(alength);
            for (int n = 1; n < types.Length; n++)
            {
                string num = types[n];
                num=num.Trim('[');
                num= num.Trim(']');
                //MessageBox.Show("num" + num);
                if (!num.Trim().Equals(""))
                {
                    int number = int.Parse(num);
                    if (n > 1)
                    {
                        arrayLength.Add(number) ;

                    }
                    alength *= number;
                  // MessageBox.Show("lenghth" + alength);
                }
               

            }
          
           this.length=alength;
       

        }
        else
        {
            this.Type = type;
            this.addr = addr;

            this.length = getLengthByType(Type);
        }
	
      
	}
    public List<int> getArrayLength()
    {
        return arrayLength;
    }

	public string getVarName() {
		return varName;
	}

	public string getType() {
		return Type;
	}

	public int getAddr() {
		return addr;
	}

	public int getLength() {
		return length;
	}

	
  

	public  int getLengthByType(string type) {
        if (type.Contains("char"))
        {
            return 1;
        }
        else if (type.Contains("short"))
        {
            return 2;
        }
        else if (type.Contains("int") || type.Contains("float"))
        {
            return 4;
        }
       
        else if (type.Equals("double"))
        {
            return 8;
        }
       
        else return 1;



		
	}

}

}
