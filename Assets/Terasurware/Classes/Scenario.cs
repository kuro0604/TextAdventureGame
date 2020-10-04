using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Scenario : ScriptableObject
{	
	public List<Sheet> sheets = new List<Sheet> ();

	[System.SerializableAttribute]
	public class Sheet
	{
		public string name = string.Empty;
		public List<Param> list = new List<Param>();
	}

	[System.SerializableAttribute]
	public class Param
	{
		
		public int senarioNo;
		public string messageString;
		public string charaNoString;
		public string branchString;
		public string displayCharaString;
		public int backgroundImageNo;
		public int bgmNo;
		public string branchMessageString;
		public int autoScenarioNo;

		public string[] messages;
		public CHARA_NAME_TYPE[] charaTypes;
		public int[] branchs;
		public Dictionary<int, CHARA_NAME_TYPE[]> displayCharas;
		public string[] branchMessages;
	}
}

