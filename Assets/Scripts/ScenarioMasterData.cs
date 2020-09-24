using System.Collections.Generic;
using System;


[Serializable]
public class ScenarioMasterData
{
    public List<ScenarioData> scenario = new List<ScenarioData>();

    [Serializable]
    public class ScenarioData
    {
        public int scenarioNo;
        public string messageString;
        public string charaNoString;
        public string branchString;
        public string backgroundImageNo;
    }

    public string[] messages;
    public CHARA_NAME_TYPE[] charaTypes;
    public int[] branchs;
    public Dictionary<int, CHARA_NAME_TYPE[]> displayCharas;
}
