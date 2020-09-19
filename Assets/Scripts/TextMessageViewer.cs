using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TextMessageViewer : MonoBehaviour
{
    public string[] messages;
    public Text txtMessage;
    public float wordSpeed;

    public CHARA_NAME_TYPE[] charaTypes;
    public Text txtCharaType;

    public GameObject tapIconObj;

    private int messagesIndex = 0;
    private int wordCount;
    private bool isTapped = false;
    private bool isDisplayedAllMessage = false;

    private IEnumerator waitCoroutine;
    private Tween tween;
    void Start()
    {
        tapIconObj.SetActive(false);

        StartCoroutine(DisplayMessage());
    }

    
    void Update()
    {
        if(isDisplayedAllMessage)
        {
            return;
        }

        if(Input.GetMouseButtonDown(0) && tween != null)
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

            StartCoroutine(NextTouch());
        }

        if (Input.GetMouseButtonDown(0) && wordCount == messages[messagesIndex].Length)
        {
            isTapped = true;
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

        if(charaTypes[messagesIndex] != CHARA_NAME_TYPE.NO_NAME)
        {
            txtCharaType.text = charaTypes[messagesIndex].ToString();
        }

        while (messages[messagesIndex].Length > wordCount)
        {
            tween = txtMessage.DOText(messages[messagesIndex], messages[messagesIndex].Length * wordSpeed).
                SetEase(Ease.Linear).OnComplete(() =>
                {
                    Debug.Log("文字送りで 全文表示 完了");

                    tapIconObj.SetActive(true);

                });
            waitCoroutine = WaitTime();
            yield return StartCoroutine(waitCoroutine);
        }
        StartCoroutine(NextTouch());
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
        yield return null;
        wordCount = messages[messagesIndex].Length;

        tapIconObj.SetActive(true);

        yield return new WaitUntil(() => isTapped);

        tapIconObj.SetActive(false);

        messagesIndex++;
        wordCount = 0;

        if(messagesIndex < messages.Length)
        {
            StartCoroutine(DisplayMessage());
        }
        else
        {
            isDisplayedAllMessage = true;
            Debug.Log("全メッセージ表示終了");
        }
    }
}
