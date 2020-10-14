using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadMainScript : MonoBehaviour
{
    public bool isNewGameButton;
    public Button btnNewGame;
    public void LoadMain()
    {
        SceneManager.LoadScene("Game");
    }

    private void Start()
    {
        if (GameData.instance.endingCount > 0)
        {
            isNewGameButton = GameData.instance.LoadCheckEndingData();
        }

        if (isNewGameButton == true)
        {
            btnNewGame.gameObject.SetActive(true);
        }
    }
    public void OnClickNewGameButton()
    {

    }
}