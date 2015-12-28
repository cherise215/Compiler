using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication4
{
   public  class Symbol
    {
        private int pos;
        private string name;
        private int row;
        private int value;
        private int type;

        public Symbol()
        { 
        }
        public Symbol(int pos, string name, int row)
        {
            this.pos = pos;
            this.name = name;
            this.row = row;

        }
        /// <summary>
        /// 获取内存位置
        /// </summary>
        public int getPos()
        {
           
            return pos;
        }
        /// <summary>
        /// 获取变量值
        /// </summary>
        public int getValue()
        {

            return value;
        }
        /// <summary>
        /// 获取变量种类
        /// </summary>
        public int getType()
        {

            return type;
        }

        /// <summary>
        /// 获取标识符名称
        /// </summary>
        public string getName()
        {
            return name;
        }
        /// <summary>
        /// 获取行数
        /// </summary>
        public int getRow()
        {
            return row;
        }

    }
}
