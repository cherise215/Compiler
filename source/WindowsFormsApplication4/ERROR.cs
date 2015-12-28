using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication4
{
   public  class ERROR
    {
        /// <summary>
        /// 错误行
        /// </summary>
            int row;
            /// <summary>
            /// 错误词
            /// </summary>
            string strError;
            /// <summary>
            /// 错误信息
            /// </summary>
            string errorKind;
            public ERROR(int row, string strError, string errorKind)
            {
                this.row = row;
                this.strError = strError;
                this.errorKind = errorKind;
            }
            /// <summary>
            /// 获取错误行
            /// </summary>
            public int getRow()
            {
                return row;
            }
            /// <summary>
            /// 获取错误单词
            /// </summary>
            public string getStrError()
            {
                return strError;
            }
            /// <summary>
            /// 获取错误信息
            /// </summary>
            public string getErrorKind()
            {
                return errorKind;
            }
            public void write()
            {
 
            }
    }
}
