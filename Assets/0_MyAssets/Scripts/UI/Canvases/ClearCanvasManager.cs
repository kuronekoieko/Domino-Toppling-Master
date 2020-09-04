using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class ClearCanvasManager : BaseCanvasManager
{
    [SerializeField] Button nextButton;
    [SerializeField] Button retryButton;

    public override void OnStart()
    {
        base.SetScreenAction(thisScreen: ScreenState.Clear);

        nextButton.onClick.AddListener(OnClickNextButton);
        retryButton.onClick.AddListener(base.ReLoadScene);
        gameObject.SetActive(false);
    }

    public override void OnUpdate()
    {
        if (!base.IsThisScreen()) { return; }

    }

    protected override void OnOpen()
    {
        UICameraController.i.PlayConfetti();
        nextButton.gameObject.SetActive(!base.IsLastStage);
        retryButton.gameObject.SetActive(base.IsLastStage);
        DOVirtual.DelayedCall(1.5f, () =>
        {
            gameObject.SetActive(true);
        });
    }

    protected override void OnClose()
    {
        gameObject.SetActive(false);
    }

    void OnClickNextButton()
    {
        base.ToNextScene();
        SoundManager.i.PlayOneShot(0);
    }

    void OnClickHomeButton()
    {
        Variables.screenState = ScreenState.Home;
        SoundManager.i.PlayOneShot(0);
    }
}
