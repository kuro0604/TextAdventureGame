using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OptionWindow : MonoBehaviour
{
    public Slider WordSpeedSlider;
    public Slider BGMVolumeSlider;
    public Slider SEVolumeSlider;
    public Text SampleText;
    public Button CloseButton;
    public CanvasGroup canvasGroup;
    
    private Tweener tweener;
    

    // Start is called before the first frame update
    void Start()
    {
        WordSpeedSlider.onValueChanged.AddListener(ChangeWordSpeed);
        BGMVolumeSlider.onValueChanged.AddListener(ChangeBGMVolume);
        SEVolumeSlider.onValueChanged.AddListener(ChangeSEVolume);
        CloseButton.onClick.AddListener(OnClickCloseWindow);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 文字スピードの変更
    /// </summary>
    public void ChangeWordSpeed( float value)
    {
        if (GameData.instance.WordSpeed != value)
        {

            GameData.instance.WordSpeed = value;

            tweener = null;
            SampleText.text = "";
            string Word = "あああああ";
            tweener = SampleText.DOText(Word, Word.Length * GameData.instance.WordSpeed).SetEase(Ease.Linear);
            Debug.Log(SampleText.text);
        }
    }

    public void ChangeBGMVolume(float value)
    {
        GameData.instance.BGM_Volume = value;
        //Debug.Log(GameData.instance.BGM_Volume);
    }

    public void ChangeSEVolume(float value)
    {
        GameData.instance.SE_Volume = value;
        Debug.Log(GameData.instance.SE_Volume);
    }

    public void OnClickCloseWindow()
    {
        canvasGroup.DOFade(0, 1.0f);
    }
}
