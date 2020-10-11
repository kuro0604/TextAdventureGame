using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadMainScript : MonoBehaviour
{
    public void LoadMain()
    {
        SceneManager.LoadScene("Game");
    }
}