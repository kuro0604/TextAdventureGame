using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using System;

public class GameDirector : MonoBehaviour
{
    public List<BranchSelectButton> branchSelectButtonList = new List<BranchSelectButton>();
    public BranchSelectButton BranchSelectButtonPrefab;
    public Transform branchButtonTran;

    public TextMessageViewer textMessageViewer;

    private int currentScenarioNo;

    void Start()
    {
        currentScenarioNo = 0;

        ProcScenarioData(currentScenarioNo);

        //StartCoroutine(CreateBranchSelectButton(3));
    }

    private void ProcScenarioData(int nextScenarioNo)
    {
        currentScenarioNo = nextScenarioNo;

        Scenario.Param scenarioData = GameData.instance.scenarioSO.sheets[0].list.Find(x => x.senarioNo == currentScenarioNo);
        Debug.Log(scenarioData);
        textMessageViewer.SetUpScenarioData(scenarioData);
    }

    public IEnumerator CreateBranchSelectButton(string[] branchMessages)
    {
        for (int i = 0; i < branchMessages.Length; i++)
        {
            BranchSelectButton branchSelectButton = Instantiate(BranchSelectButtonPrefab, branchButtonTran, false);

            branchSelectButton.InitializeBranchSelect(branchMessages[i], i, this, i);
            branchSelectButtonList.Add(branchSelectButton);
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void ChooseBranch(int senarioNo)
    {
        Debug.Log("分岐選択 シナリオ番号 :" + senarioNo);
        ProcScenarioData(senarioNo);
    }
    public void InactiveBranchSelectButtons()
    {
        for (int i = 0; i < branchSelectButtonList.Count; i++)
        {
            if (!branchSelectButtonList[i].isClickable)
            {
                branchSelectButtonList[i].isClickable = true;
                branchSelectButtonList[i].canvasGroup.DOFade(0.0f, 0.5f);

            }
        }
            branchSelectButtonList.Clear();
        
    }
    
}
