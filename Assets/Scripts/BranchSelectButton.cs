using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


[RequireComponent(typeof(CanvasGroup))]

public class BranchSelectButton : MonoBehaviour
{
    public Text txtBranchMessage;
    public Button btnBranchSelectButton;
    public CanvasGroup canvasGroup;

    public int branchNo;
    public bool isClickable;
    public Ease easeType;

    private GameDirector gameDirector;
    private Sequence sequence;
    public void InitializeBranchSelect(string message, int no, GameDirector director, int count)
    {
        canvasGroup.alpha = 0.0f;
        transform.position = new Vector3(transform.position.x, transform.position.y - (count * 150), transform.position.z);

        sequence = DOTween.Sequence();

        sequence.Append(transform.DOLocalMoveX(1000, 1.0f).SetEase(easeType));
        sequence.Join(canvasGroup.DOFade(1.0f, 1.0f));

        txtBranchMessage.text = message;
        branchNo = no;
        gameDirector = director;

        btnBranchSelectButton.onClick.AddListener(OnClickChooseBranch);
    }

    private void OnClickChooseBranch()
    {
        if(isClickable)
        {
            return;
        }

        isClickable = true;

        gameDirector.InactiveBranchSelectButtons();

        sequence = DOTween.Sequence();

        sequence.Append(transform.DOLocalMoveX(2000, 1.0f).SetEase(easeType))
            .Join(canvasGroup.DOFade(0.0f, 1.0f))
            .AppendCallback(() =>
            {
                Debug.Log("移動終了");
            }
            );
    }
}
