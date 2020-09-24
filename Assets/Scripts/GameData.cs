using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GameData : MonoBehaviour
{
    public static GameData instance;
    public ScenarioSO scenarioSO;

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
        scenarioSO.scenarioMasterData = new ScenarioMasterData();

        scenarioSO.scenarioMasterData = LoadMasterDataFromJson.LoadScenarioMasterDataFromJson();
        
        foreach (ScenarioMasterData.ScenarioData scenarioData in scenarioSO.scenarioMasterData.scenario)
        {
            scenarioData.messages = scenarioData.messageString.Split(',').ToArray();

            scenarioData.charaTypes = scenarioData.charaNoString.Split(',').Select(x => (CHARA_NAME_TYPE)Enum.Parse(typeof(CHARA_NAME_TYPE), x)).ToArray();

            scenarioData.branchs = scenarioData.branchString.Split(',').Select(x => int.Parse(x)).ToArray();

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
}
