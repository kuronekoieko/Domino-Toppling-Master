using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// ゲーム画面
/// ゲーム中に表示するUIです
/// あくまで例として実装してあります
/// ボタンなどは適宜編集してください
/// </summary>
public class GameCanvasManager : BaseCanvasManager
{
    [SerializeField] Text stageNumText;
    [SerializeField] Button retryButton;
    [SerializeField] TutrialHandController tutrialHandController;
    public static GameCanvasManager i;
    public override void OnStart()
    {
        i = this;
        base.SetScreenAction(thisScreen: ScreenState.Game);

        this.ObserveEveryValueChanged(currentSceneBuildIndex => Variables.currentSceneBuildIndex)
            .Subscribe(currentSceneBuildIndex => { ShowStageNumText(); })
            .AddTo(this.gameObject);

        gameObject.SetActive(true);
        retryButton.onClick.AddListener(base.ReLoadScene);
        tutrialHandController.OnStart();
    }

    public override void OnUpdate()
    {
        if (!base.IsThisScreen()) { return; }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            base.ReLoadScene();
        }
    }

    protected override void OnOpen()
    {
        gameObject.SetActive(true);
    }

    protected override void OnClose()
    {
        // gameObject.SetActive(false);
    }

    public override void OnInitialize()
    {
        if (Variables.currentSceneBuildIndex == 1)
        {
            tutrialHandController.TapAnim(tutrialHandController.rectTransform.anchoredPosition);
        }
        else
        {
            tutrialHandController.Kill();
        }
    }

    public void HideTutrial()
    {
        tutrialHandController.Kill();
    }

    void ShowStageNumText()
    {
        stageNumText.text = "LEVEL " + (Variables.currentSceneBuildIndex).ToString("000");
    }
}
