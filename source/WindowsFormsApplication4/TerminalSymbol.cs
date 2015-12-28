using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication4
{

    public class TerminalSymbol
    {

        private string value;
        private int  code;
        private HashSet <string> first;

        public TerminalSymbol(string value)
        {
            this.value = value;
            this.first = new HashSet<string>();


        }

        public int getCode()
        {
            return code;
        }
        public void setCode(int code)
        {
            this.code=code;
        }



        public string getValue()
        {
            return value;
        }

        public void setValue(string value)
        {
            this.value = value;
        }

        public HashSet<string> getFirst()
        {
            return first;
        }

        public void setFirst(HashSet<string> first)
        {
            this.first = first;
        }

    }
}
