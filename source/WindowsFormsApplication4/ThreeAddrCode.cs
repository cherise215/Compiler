using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication4
{
    public class ThreeAddrCode {
	bool hasLabel;// 该三地址码之前是否有标签
	String label;// 该三地址码的标签。只有hasLabel为true时该字段才有效
	private StringBuilder tACode;// 三地址码

	ThreeAddrCode(bool hasLabel, String label) {
		this.hasLabel = hasLabel;
		this.label = label;
		tACode = new StringBuilder();
	}

	/**
	 * 将若干个字符串添加到三地址码的尾部。参数个数可变
	 * 
	 * @param tails
	 */
	void addToTail(List<string> tails) {
		foreach (string str in tails)
			tACode.Append(str);
	}


    }
}
