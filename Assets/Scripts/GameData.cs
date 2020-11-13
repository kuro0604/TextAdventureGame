using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GameData : MonoBehaviour
{
    private string READ_BRANCH_NO = "readBranchNo_";
    public List<int> readBranchNoList = new List<int>();

    public static GameData instance;
    public Scenario scenarioSO;


    public int endingCount;
    public List<int> endingNos = new List<int>();
    private string ENDING = "ending_";

    public float WordSpeed = 0.5f;
    public float BGM_Volume = 0.1f;
    public float SE_Volume = 0.2f;

    public float WordWaitTime = 1.0f;

    public int loadBranchNo;

    private string CURRENT_BRANCH_NO = "currentBranchNo_";
    private string SAVE_TIME_NO = "saveTimeNo_";

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

    public void SaveReadBranchNo(int branchNo)
    {
        PlayerPrefs.SetInt(READ_BRANCH_NO + branchNo.ToString(), branchNo);
        PlayerPrefs.Save();
    }

    public void LoadReadBranchNos()
    {
        for (int i = 0; i < scenarioSO.sheets.Count; i++)
        {
            if (PlayerPrefs.HasKey(READ_BRANCH_NO + i.ToString()))
            {



                Debug.Log("既読シナリオ番号　: " + PlayerPrefs.GetInt(READ_BRANCH_NO + ToString()));
            }
        }

        if (readBranchNoList.Count == 0)
        {
            Debug.Log("既読シナリオなし");
        }

    }

    public void Save(int currentBranchNo)
    {
        PlayerPrefs.SetInt(CURRENT_BRANCH_NO + currentBranchNo.ToString(), currentBranchNo);

        PlayerPrefs.SetString(SAVE_TIME_NO + currentBranchNo.ToString(), DateTime.Now.ToString());

        PlayerPrefs.Save();

        Debug.Log("Save : " + CURRENT_BRANCH_NO + currentBranchNo + " : 時間 : " + DateTime.Now.ToString());
    }

    public Dictionary<int, string> GetSaveDatas()
    {
        Dictionary<int, string> saveDatas = new Dictionary<int, string>();
        for(int i = 0; i < scenarioSO.sheets[0].list.Count; i++)
        {
            if(PlayerPrefs.HasKey(CURRENT_BRANCH_NO + i.ToString()))
            {
                saveDatas.Add(PlayerPrefs.GetInt(CURRENT_BRANCH_NO + i.ToString()), PlayerPrefs.GetString(SAVE_TIME_NO + i.ToString()));

                Debug.Log("保存データ　分岐番号 : " + CURRENT_BRANCH_NO + i.ToString());
                Debug.Log("時間 : " + SAVE_TIME_NO + i.ToString());
            }
        }
        return saveDatas;
    }
}
