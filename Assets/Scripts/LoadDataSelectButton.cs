using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadDataSelectButton : MonoBehaviour
{
    [SerializeField]
    private Button btnLoad;


    [SerializeField]
    private Text txtLoadDataInfo;

    [SerializeField]
    private Image imgBackGround;

    [SerializeField]
    private Image imgChara;

    public CanvasGroup canvasGroup;

    public bool isClickable;

    private int branchNo;

    private DataLoadPopUp dataLoadPopUp;

    public void SetUpLoadSelectButton(int branchNo, string date, DataLoadPopUp dataLoadPopUp, int no)
    {
        this.branchNo = branchNo;
        this.dataLoadPopUp = dataLoadPopUp;

        txtLoadDataInfo.text = "セーブ番号 : " + no + " 保存時間 : " + date;

        btnLoad.onClick.AddListener(OnClickLoadButton);

        Scenario.Param scenarioData = GameData.instance.scenarioSO.sheets[0].list.Find((x) => x.senarioNo == this.branchNo);

        imgBackGround.sprite = Resources.Load<Sprite>("BackGround/" + scenarioData.backgroundImageNo);

        if(scenarioData.charaTypes.Length > 0)
        {
            imgChara.sprite = Resources.Load<Sprite>("Charas/Chara_" + (int)scenarioData.charaTypes[0]);
        }
        else
        {
            imgChara.color = new Color(255, 255, 255, 0);
        }
    }
    private void OnClickLoadButton()
    {
        if (isClickable)
        {
            
            return;
        }
        isClickable = true;

        dataLoadPopUp.InactiveLoadSelectButtons();

        GameData.instance.loadBranchNo = branchNo;

        Debug.Log("データロード完了 分岐番号 : " + GameData.instance.loadBranchNo + "からゲーム再開");

        dataLoadPopUp.LoadGame();
    }
}
