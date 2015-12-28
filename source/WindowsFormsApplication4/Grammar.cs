using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;




namespace WindowsFormsApplication4
{
    public class Grammar
    {

        /// <summary>
        /// 文法描述
        /// </summary>
	private string mGrammar;//描述
    /// <summary>
    /// 左部
    /// </summary>
	private string leftPart;//左部
    /// <summary>
    /// 文法产生式
    /// </summary>
	private List<string> rightPart;//右部
    private List<string> rightPartWithAction;//右部包括动作
    /// <summary>
    /// select集合
    /// </summary>
	private HashSet<string> select;//select集

	public Grammar(string mGrammar) {
		this.mGrammar = mGrammar;
		
	}
    public Grammar()
    {

        this.leftPart = "";
        this.select = new HashSet<string>();
        this.rightPart = new List<string>();
        this.rightPartWithAction = new List<string>();
    }

    /// <summary>
    /// 获取文法描述
    /// </summary>
	public string getmGrammar() {
		return mGrammar;
	}

	public void setmGrammar(string mGrammar) {
		this.mGrammar = mGrammar;
	}

    /// <summary>
    /// 获取文法左部
    /// </summary>
	public string getLeftPart() {
		return leftPart;
	}

	public void setLeftPart(string leftPart) {
		this.leftPart = leftPart;
	}

    /// <summary>
    /// 获取文法右部
    /// </summary>
	public List<string> getRightPartWithAction() {
		return rightPartWithAction;
	}

	public void setRightPartWithAction(List<string> rightPartWithAction) {
		this.rightPartWithAction = rightPartWithAction;
	}
    public List<string> getRightPart()
    {
        return rightPart;
    }

    public void setRightPart(List<string> rightPart)
    {
        this.rightPart = rightPart;
    }

    /// <summary>
    /// 获取该文法的select集合
    /// </summary>
	public HashSet<string> getSelect() {
		return select;
	}


    
	public void setSelect(HashSet<string> select) {
		this.select = select;
	}
        
       
    }
}
