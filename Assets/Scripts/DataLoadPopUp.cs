using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class DataLoadPopUp : MonoBehaviour
{
    public LoadDataSelectButton loadSelectbuttonPrefab; 

    [SerializeField]                                       
    private List<LoadDataSelectButton> loadSelectButtonList = new List<LoadDataSelectButton>();

    [SerializeField]
    private Transform loadSelectButtonTran;               
    [SerializeField]
    private CanvasGroup canvasGroup;                       

    [SerializeField]
    private Button btnClose;

    public void SetUpDataLoadPopUp()
    {
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1.0f, 1.0f);

        btnClose.onClick.AddListener(OnClickClosePopUp);

        
        CreateLoadButtons();
    }

    private void CreateLoadButtons()
    {
        Dictionary<int, string> loadDatas = new Dictionary<int, string>(GameData.instance.GetSaveDatas());

        if (loadDatas.Count == 0)
        {
            Debug.Log("SaveData なし");
            return;
        }

        int i = 0;

        
        foreach (KeyValuePair<int, string> item in loadDatas)
        {
            LoadDataSelectButton loadSelectButton = Instantiate(loadSelectbuttonPrefab, loadSelectButtonTran, false);

            i++;

            loadSelectButton.SetUpLoadSelectButton(item.Key, item.Value, this, i);

            loadSelectButtonList.Add(loadSelectButton);
        }
    }

    
    public void InactiveLoadSelectButtons()
    {

        
        for (int i = 0; i < loadSelectButtonList.Count; i++)
        {

            
            if (loadSelectButtonList[i].isClickable)
            {

                
                loadSelectButtonList[i].isClickable = true;

                
                loadSelectButtonList[i].canvasGroup.DOFade(0.0f, 0.5f);
            }
        }
        
        loadSelectButtonList.Clear();
    }

    
    public void LoadGame()
    {

        // Sequenceの初期化
        Sequence sequence = DOTween.Sequence();

        
        sequence.Append(canvasGroup.DOFade(0, 1.0f));

        sequence.AppendInterval(0.2f).OnComplete(() => {
            
            SceneManager.LoadScene("Game");
        });
    }

    
    private void OnClickClosePopUp()
    {

        
        Sequence sequence = DOTween.Sequence();

        
        sequence.Append(canvasGroup.DOFade(0, 1.0f));

        sequence.AppendInterval(0.2f).OnComplete(() => {
            
            Destroy(gameObject);
        });
    }
}