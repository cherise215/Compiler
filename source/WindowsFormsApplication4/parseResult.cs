using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication4
{
  public   class parseResult
    {
        private keyInfo curKey;
        private string stackTop;
        private string outPut;
        private int row;
        private string stack;

        public parseResult()
        {
           
        }
        public parseResult(keyInfo curKey,string stackTop,string outPut ,int row,string stack)
        {
            this.outPut=outPut;
            this.stackTop = stackTop;
            this.curKey = curKey;
            this.row = row;
            this.stack = stack;

        }
        public void setCurKey(keyInfo curKey)
        {
            this.curKey = curKey;
        }
        public keyInfo getCurKey(){
           return curKey;
        }
        public string getStackTop()
        {
            return stackTop;
        }
        public void setRow(int row)
        {
            this.row = row;
        }
        public int getRow()
        {
            return row;
        }
        public void setStackTop(string stackTop)
        {
            this.stackTop = stackTop;
        }
        public string getOutPut()
        {
            return outPut;
        }
        public void setOutPut(string outPut)
        {
            this.outPut = outPut;
        }
        public string getStack()
        {
            return stack;
        }
        


    }
}
