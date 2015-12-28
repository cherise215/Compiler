using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication4
{
    public class Table
    {
        private string name;
        private int flag;
        private List<string> expression=new List<string>();
        public Table(string name)
        {
            this.name = name;

        }
        public void setName(string name)
        {
            this.name = name;
        }
        public string getName()
        {
            return name;
        }
        public int getFlag()
        {
            return flag;
        }
        public void setFlag(int flag)
        {
            this.flag=flag;
        }
        public List<string> getExpression()
        {
            return expression;
        }
        public void setExpression( List<string> expression)
        {
            this.expression= expression;
        }
    }
}
