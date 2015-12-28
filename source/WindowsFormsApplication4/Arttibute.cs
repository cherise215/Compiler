using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication4
{

    
    public class SymbolArttribute
    {
        /// <summary>
        /// 属性名
        /// </summary>
        private string name;//属性名
        /// <summary>
        /// 属性值
        /// </summary>
        private string value;//属性值
        public string getName()
        {
            return name;
        }
        public string getValue()
        {
            return value;
        }
        public void  setName(string name)
        {
            this.name = name;

        }
        public void setValue(string value)
        {
            this.value = value;

        }
        public SymbolArttribute(string name,string value)
        {
            this.name = name;
            this.value = value;

        }
    }
}
