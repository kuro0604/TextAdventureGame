﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Title : MonoBehaviour
{
    public bool isNewGameButton;
    public Button btnNewGame;
    public Button btnOption;
    public OptionWindow optionWindow;
    public void LoadMain()
    {
        SceneManager.LoadScene("Game");
    }

    private void Start()
    {
        if (GameData.instance.endingCount > 0)
        {
            isNewGameButton = GameData.instance.LoadCheckEndingData();
            CheckEndingCount();
        }

        if (isNewGameButton == true)
        {
            btnNewGame.gameObject.SetActive(true);
        }
        btnOption.onClick.AddListener(OnClickOpenWindow);

        GameData.instance.LoadReadBranchNos();
    }

    private void CheckEndingCount()
    {
        isNewGameButton = GameData.instance.LoadCheckEndingData();

        if(isNewGameButton == true)
        {
            btnNewGame.gameObject.SetActive(true);
            btnNewGame.onClick.AddListener(OnClickNewGameButton);
        }
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }
    public void OnClickNewGameButton()
    {

    }

    public void OnClickOpenWindow()
    {
        optionWindow.canvasGroup.DOFade(1.0f, 1.0f);
    }
}