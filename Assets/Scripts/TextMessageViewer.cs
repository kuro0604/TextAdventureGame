using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TextMessageViewer : MonoBehaviour
{
    public string[] messages;
    public int[] branchs;
    public Dictionary<int, CHARA_NAME_TYPE[]> displayCharas;
    public CHARA_NAME_TYPE[] charaTypes;
    
    
    public int bgmNo;
    public string[] branchMessages;

    public float wordSpeed;
    public Text txtCharaType;
    public Text txtMessage;
    public Image imgBackground;
    public List<DisplayChara> displayCharasList;
    public Transform charaTran;

    public GameObject tapIconObj;



    private int messagesIndex = 0;
    private int wordCount;
    private bool isTapped = false;
    private bool isDisplayedAllMessage = false;

    private IEnumerator waitCoroutine;
    private Tween tween;

    public GameDirector gameDirector;

    public int autoScenarioNo;

    void Start()
    {
        tapIconObj.SetActive(false);

        //StartCoroutine(DisplayMessage());
    }

    public void SetUpScenarioData(Scenario.Param scenarioData)
    {
        //Debug.Log("シナリオ番号 : " + scenarioData.senarioNo  + " のシナリオデータをセット");

        messages = new string[scenarioData.charaTypes.Length];
        messages = scenarioData.messages;

        charaTypes = new CHARA_NAME_TYPE[scenarioData.charaTypes.Length];
        charaTypes = scenarioData.charaTypes;

        branchs = new int[scenarioData.branchs.Length];
        branchs = scenarioData.branchs;

        displayCharas = new Dictionary<int, CHARA_NAME_TYPE[]>(scenarioData.displayCharas);

        bgmNo = scenarioData.bgmNo;

        SoundManager.instance.PlayBGM((SoundManager.BGM_Type)bgmNo);

        branchMessages = new string[scenarioData.branchMessages.Length];
        branchMessages = scenarioData.branchMessages;

        autoScenarioNo = scenarioData.autoScenarioNo;

        messagesIndex = 0;
        isDisplayedAllMessage = false;

        imgBackground.sprite = Resources.Load<Sprite>("BackGround/" + scenarioData.backgroundImageNo);

        StartCoroutine(DisplayMessage());
        Debug.Log("シナリオ　再生開始");

    }


    void Update()
    {
        if (isDisplayedAllMessage)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && wordCount == messages[messagesIndex].Length)
        {
            isTapped = true;
        }

        if (Input.GetMouseButtonDown(0) && tween != null)
        {
            tween.Kill();
            tween = null;

            if (waitCoroutine != null)
            {
                StopCoroutine(waitCoroutine);
                waitCoroutine = null;
            }

            txtMessage.text = messages[messagesIndex];

            Debug.Log("文字送りスキップ　1ページまとめて表示");

            CompleteOneMessage();

            StartCoroutine(NextTouch());
        }


    }


    /// <summary>
    /// 一文字ずつ表示
    /// </summary>
    /// 

    private IEnumerator DisplayMessage()
    {
        isTapped = false;

        // 表示テキストをリセット
        txtMessage.text = "";
        txtCharaType.text = "";

        tween = null;

        if (waitCoroutine != null)
        {
            StopCoroutine(waitCoroutine);
            waitCoroutine = null;
        }

        if (charaTypes[messagesIndex] != CHARA_NAME_TYPE.NO_NAME)
        {
            txtCharaType.text = charaTypes[messagesIndex].ToString();
        }

        foreach (DisplayChara chara in displayCharasList)
        {
            chara.gameObject.SetActive(false);
            foreach (KeyValuePair<int, CHARA_NAME_TYPE[]> item in displayCharas)
            {
                if (item.Key == messagesIndex)
                {
                    for (int i = 0; i < item.Value.Length; i++)
                    {
                        if (item.Value[i] == chara.charaNameType)
                        {
                            chara.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }

        while (messages[messagesIndex].Length > wordCount)
        {
            tween = txtMessage.DOText(messages[messagesIndex], messages[messagesIndex].Length * wordSpeed).
                SetEase(Ease.Linear).OnComplete(() =>
                {
                    Debug.Log("文字送りで 全文表示 完了");

                    

                    CompleteOneMessage();

                });
            waitCoroutine = WaitTime();
            yield return StartCoroutine(waitCoroutine);
        }
    }

    /// <summary>
    /// 全文表示までの待機時間(文字数×1文字当たりの表示時間)
    /// タップして全文をまとめて表示した場合にはこの待機時間を停止
    /// </summary>
    private IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(messages[messagesIndex].Length * wordSpeed);
    }


    private IEnumerator NextTouch()
    {


        yield return new WaitUntil(() => isTapped);

        tapIconObj.SetActive(false);

        messagesIndex++;
        wordCount = 0;

        if (messagesIndex < messages.Length)
        {
            StartCoroutine(DisplayMessage());
        }
        else
        {
            isDisplayedAllMessage = true;
            Debug.Log("全メッセージ表示終了");

            if (JudgeEnding())
            {
                for (int i = 0; i < displayCharasList.Count; i++)
                {
                    displayCharasList[i].gameObject.SetActive(false);
                }
            }
            else
            {
                if(branchs[0] == -1)
                {
                    gameDirector.ChooseBranch(autoScenarioNo);
                }
                else
                {
                    StartCoroutine(gameDirector.CreateBranchSelectButton(branchMessages));
                }
                
            }
        }
    }

    private bool JudgeEnding()
    {


        return false;

    }

    private void CompleteOneMessage()
    {
        wordCount = messages[messagesIndex].Length;
        tapIconObj.SetActive(true);
    }
}
