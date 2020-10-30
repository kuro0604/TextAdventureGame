using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GameData : MonoBehaviour
{
    public static GameData instance;
    public Scenario scenarioSO;


    public int endingCount;
    public List<int> endingNos = new List<int>();
    private string ENDING = "ending_";

    public float WordSpeed;
    public float BGM_Volume = 0.1f;
    public float SE_Volume = 0.2f;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        CreateScenarioDataList();
    }

    private void CreateScenarioDataList()
    {
        

        
        
        foreach (Scenario.Param scenarioData in scenarioSO.sheets[0].list)
        {
            scenarioData.messages = scenarioData.messageString.Split(',').ToArray();

            scenarioData.charaTypes = scenarioData.charaNoString.Split(',').Select(x => (CHARA_NAME_TYPE)Enum.Parse(typeof(CHARA_NAME_TYPE), x)).ToArray();

            scenarioData.branchs = scenarioData.branchString.Split(',').Select(x => int.Parse(x)).ToArray();
            
            scenarioData.branchMessages = scenarioData.branchMessageString.Split(',').ToArray();

            List<string> strList = scenarioData.displayCharaString.Split('/').ToList();

            int i = 0;

       　   scenarioData.displayCharas = new Dictionary<int, CHARA_NAME_TYPE[]>();

            foreach (string str in strList)
            {
                CHARA_NAME_TYPE[] displayChara = str.Split(',').Select(x => (CHARA_NAME_TYPE)Enum.Parse(typeof(CHARA_NAME_TYPE), x)).ToArray();

                scenarioData.displayCharas.Add(i, displayChara);

                i++;
            }
        }
        Debug.Log("Create ScenarioDataList");
    }

    public void SaveEndingData(int endingNo)
    {
        PlayerPrefs.SetInt(ENDING + endingNo.ToString(), endingNo);
        PlayerPrefs.Save();
    }

    public bool LoadCheckEndingData()
    {
        for (int i = 1; i < endingCount +1 ; i++)
        {
            if (PlayerPrefs.HasKey(ENDING + i.ToString()))
                {
                endingNos.Add(PlayerPrefs.GetInt(ENDING + i.ToString(), 0));
                }
        }
        return endingCount == endingNos.Count ? true : false;
    }

}
