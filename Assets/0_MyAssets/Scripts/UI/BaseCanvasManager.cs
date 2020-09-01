using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.SceneManagement;

public enum ScreenState
{
    Initialize,
    Game,
    Login,
    Clear,
    Failed,
    Home,
    Debug,
}

/// <summary>
/// 画面のスクリプトに継承して使う
/// UniRxでVariables.screenStateを監視し、
/// 画面の開閉処理をやってくれる
/// </summary>
public class BaseCanvasManager : MonoBehaviour
{
    ScreenState thisScreen;

    /// <summary>
    /// OnOpenとOnCloseのイベント発火タイミングを設定する
    /// </summary>
    /// <param name="thisScreen">セットする画面のenumを入れてください</param>
    protected void SetScreenAction(ScreenState thisScreen)
    {
        this.thisScreen = thisScreen;

        this.ObserveEveryValueChanged(screenState => Variables.screenState)
            .Where(screenState => screenState == thisScreen)
            .Subscribe(screenState => OnOpen())
            .AddTo(this.gameObject);

        this.ObserveEveryValueChanged(screenState => Variables.screenState)
            .Buffer(2, 1)
            .Where(screenState => screenState[0] == thisScreen)
            .Where(screenState => screenState[1] != thisScreen)
            .Subscribe(screenState => OnClose())
            .AddTo(this.gameObject);
    }

    public virtual void OnStart()
    {
    }

    public virtual void OnUpdate()
    {
    }

    /// <summary>
    /// 画面が開かれる瞬間だけ呼ばれる
    /// </summary>
    protected virtual void OnOpen()
    {
    }

    /// <summary>
    /// 画面が閉じられる瞬間だけ呼ばれる
    /// </summary>
    protected virtual void OnClose()
    {
    }

    public void OnInitialize()
    {

    }

    protected void ToNextScene()
    {
        if (!IsThisScreen()) { return; }
        Variables.currentStageIndex++;
        SceneManager.LoadScene("GameScene");
    }

    protected void ReLoadScene()
    {
        if (!IsThisScreen()) { return; }
        SceneManager.LoadScene("GameScene");
    }

    protected bool IsThisScreen()
    {
        return Variables.screenState == thisScreen;
    }

    /*
        public readonly ScreenState thisScreen = ScreenState.
        
        public override void OnStart()
        {
            base.SetScreenAction(thisScreen: thisScreen);
        }

        public override void OnUpdate(ScreenState currentScreen)
        {
            if (currentScreen != thisScreen) { return; }

        }

        protected override void OnOpen()
        {
            gameObject.SetActive(true);
        }

        protected override void OnClose()
        {
        }
    */

}
