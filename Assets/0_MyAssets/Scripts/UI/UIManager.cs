using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 画面UIの一括管理
/// GameDirectorと各画面を中継する役割
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject _gameDirector;
    public static GameObject gameDirector;
    BaseCanvasManager[] canvases;

    //Awakeより先に呼ばれる
    //シーンをリロードしても呼ばれない
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void RuntimeInitializeApplication()
    {
       // SceneManager.LoadScene("UIScene");
    }

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        if (gameDirector == null)
        {
            gameDirector = _gameDirector;
            DontDestroyOnLoad(_gameDirector);
        }
    }

    void Start()
    {
        canvases = new BaseCanvasManager[transform.childCount];
        for (int i = 0; i < canvases.Length; i++)
        {
            canvases[i] = transform.GetChild(i).GetComponent<BaseCanvasManager>();
            if (canvases[i] == null) { continue; }
            canvases[i].OnStart();
        }

        SceneManager.sceneLoaded += SceneLoaded;
        SceneManager.LoadScene("GameScene");
    }

    void Update()
    {
        if (Variables.screenState == ScreenState.Initialize)
        {
            Initialize();
        }
        else
        {
            OnUpdate();
        }
    }
    void OnUpdate()
    {
        for (int i = 0; i < canvases.Length; i++)
        {
            if (canvases[i] == null) { continue; }
            canvases[i].OnUpdate();
        }
    }

    void Initialize()
    {
        for (int i = 0; i < canvases.Length; i++)
        {
            if (canvases[i] == null) { continue; }
            canvases[i].OnInitialize();
        }
        Variables.screenState = ScreenState.Game;
    }

    // イベントハンドラー（イベント発生時に動かしたい処理）
    void SceneLoaded(Scene nextScene, LoadSceneMode mode)
    {
        Variables.screenState = ScreenState.Initialize;
    }
}
