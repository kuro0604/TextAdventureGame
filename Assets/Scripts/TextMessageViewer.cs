using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;


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

    public Button btnSkip;

    public bool isSkipReadingMessage;
    public bool isReadBranchNo;
    public int currentBranchNo;
    public float skipSpeed = 0f;
    private float currentWordSpeed;





    private int messagesIndex = 0;
    private int wordCount;
    private bool isTapped = false;
    private bool isDisplayedAllMessage = false;

    private IEnumerator waitCoroutine;
    private Tween tween;

    private bool isSkip = false;

    public GameDirector gameDirector;

    public int autoScenarioNo;

    public int endingNo;


    public Button btnOption;
    public GameOptionWindow optionWindow;

    public Button btnAutoPlay;
    public bool isAutoPlay;

    public float WordWaitTime;

    void Start()
    {
        tapIconObj.SetActive(false);

        // ワードスピードの設定
        wordSpeed = GameData.instance.WordSpeed;
        //StartCoroutine(DisplayMessage());

        btnOption.onClick.AddListener(OnClickOpenWindow);

        btnAutoPlay.onClick.AddListener(OnClickAutoPlay);

        btnSkip.onClick.AddListener(OnClickSkipReadingMessage);
        currentWordSpeed = wordSpeed;

    }

    public void SetUpScenarioData(Scenario.Param scenarioData)
    {
        Debug.Log("シナリオ番号 : " + scenarioData.senarioNo  + " のシナリオデータをセット");

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

        if(scenarioData.endingNo != 0)
        {
           endingNo = scenarioData.endingNo;
        }

        messagesIndex = 0;
       // isDisplayedAllMessage = false;

        imgBackground.sprite = Resources.Load<Sprite>("BackGround/" + scenarioData.backgroundImageNo);

        isReadBranchNo = false;
        currentBranchNo = scenarioData.senarioNo;

        foreach (int readBranchNo in GameData.instance.readBranchNoList)
        {
            if (readBranchNo == currentBranchNo)
            {
                isReadBranchNo = true;
            }
        }

        if(!isReadBranchNo)
        {
            isAutoPlay = false;
            currentWordSpeed = wordSpeed;
        }

        SkipMessage();

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

            if(EventSystem.current.currentSelectedGameObject != null)
            {
                return;
            }
            isTapped = true;
        }

        if (Input.GetMouseButtonDown(0) && tween != null)
        {
            if(EventSystem.current.currentSelectedGameObject != null)
            {
                return;
            }
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
            tween = txtMessage.DOText(messages[messagesIndex], messages[messagesIndex].Length * currentWordSpeed).
                SetEase(Ease.Linear).OnComplete(() =>
                {
                    Debug.Log("文字送りで 全文表示 完了");

                    

                    CompleteOneMessage();

                    if(isAutoPlay)
                    {
                        StartCoroutine(NextTouch());
                        Debug.Log("オート再生中");
                    }

                });
            waitCoroutine = WaitTime();
            yield return StartCoroutine(waitCoroutine);

            break;
        }
    }

    /// <summary>
    /// 全文表示までの待機時間(文字数×1文字当たりの表示時間)
    /// タップして全文をまとめて表示した場合にはこの待機時間を停止
    /// </summary>
    private IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(messages[messagesIndex].Length * currentWordSpeed);
    }


    private IEnumerator NextTouch()
    {


        if(!isAutoPlay)
        {
            yield return new WaitUntil(() => isTapped);
            Debug.Log("非オート。タップ待ち");
        }
        else
        {
            yield return new WaitForSeconds(WordWaitTime);
        }

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

            GameData.instance.SaveReadBranchNo(currentBranchNo);

            if (JudgeEnding())
            {
                for (int i = 0; i < displayCharasList.Count; i++)
                {
                    displayCharasList[i].gameObject.SetActive(false);
                }
                GameData.instance.SaveEndingData(endingNo);
                Debug.Log("Ending SaveNo :" + endingNo);
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
        if(endingNo != 0)
        {
            return true;
        }
        return false;

    }

    private void CompleteOneMessage()
    {
        wordCount = messages[messagesIndex].Length;
        tapIconObj.SetActive(true);
    }

    public void OnClickSkip()
    {
        
        isSkip = !isSkip;
        if(isSkip == true)
        {
            Debug.Log("スキップ中");
        }
        else
        {
            Debug.Log("スキップ終了");
        }
    }
    public void OnClickOpenWindow()
    {
        optionWindow.canvasGroup.DOFade(1.0f, 1.0f);
    }

    public void OnClickAutoPlay()
    {
        isAutoPlay = !isAutoPlay;

        if(isAutoPlay)
        {
            btnAutoPlay.image.color = btnAutoPlay.colors.pressedColor;

        }
        else
        {
            btnAutoPlay.image.color = btnAutoPlay.colors.normalColor;
        }
    }

    public void OnClickSkipReadingMessage()
    {
        isSkipReadingMessage = !isSkipReadingMessage;

        if(isSkipReadingMessage)
        {
            btnSkip.image.color = btnSkip.colors.pressedColor;

        }
        else
        {
            btnSkip.image.color = btnSkip.colors.normalColor;
        }

        SkipMessage();
    }

    private void SkipMessage()
    {
        if(isSkipReadingMessage && isReadBranchNo)
        {
            isAutoPlay = true;
            currentWordSpeed = skipSpeed;
            Debug.Log("既読スキップ中");
        }
    }

}
