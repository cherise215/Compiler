using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication4
{
   public  class SemanticActionAnalysis
    {

       
       public void execute(string  action) {
        
        action.TrimStart('#');
		string[] actions = action.Split(';');
		//this.curLineNum = curLineNum;

		foreach (string aAction in actions) {
            
			
		}
	}

    }
}
