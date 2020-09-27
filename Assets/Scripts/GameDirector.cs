using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GameDirector : MonoBehaviour
{
    public TextMessageViewer textMessageViewer;

    private int currentScenarioNo;

    void Start()
    {
        currentScenarioNo = 0;

        ProcScenarioData(currentScenarioNo);
    }


    private void ProcScenarioData(int nextScenarioNo)
    {
        currentScenarioNo = nextScenarioNo;

        Scenario.Param scenarioData = GameData.instance.scenarioSO.sheets[0].list.Find(x => x.senarioNo == currentScenarioNo);

        textMessageViewer.SetUpScenarioData(scenarioData);
    }
}
