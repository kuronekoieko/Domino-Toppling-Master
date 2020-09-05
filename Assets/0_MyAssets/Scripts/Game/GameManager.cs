using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public enum GameState
{
    TapWaiting,
    Toppling,
}
public class GameManager : MonoBehaviour
{
    [SerializeField] Color[] dominoColors;
    [SerializeField] int tapCount;
    DominoController[] dominoControllers;
    public static GameManager i;
    [NonSerialized] public int tapCountLeft;
    [NonSerialized] public GameState gameState;
    private void Awake()
    {
        dominoControllers = FindObjectsOfType<DominoController>();
        i = this;
    }

    private void Start()
    {
        foreach (var domino in dominoControllers)
        {
            Color randomColor = dominoColors[UnityEngine.Random.Range(0, dominoColors.Length)];
            domino.SetColor(randomColor);
        }
        tapCountLeft = tapCount;
        gameState = GameState.TapWaiting;
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.TapWaiting:
                break;
            case GameState.Toppling:
                int topplingCount = dominoControllers.Count(d => d.dominoState == DominoState.Toppling);
                if (topplingCount > 0) break;
                gameState = GameState.TapWaiting;
                bool isAllDominoToppled = dominoControllers.All(d => d.dominoState == DominoState.Toppled);
                FailedCheck(isAllDominoToppled);
                ClearCheck(isAllDominoToppled);
                break;
            default:
                break;
        }
    }

    void ClearCheck(bool isAllDominoToppled)
    {
        if (isAllDominoToppled == false) return;
        if (Variables.screenState != ScreenState.Game) return;
        Variables.screenState = ScreenState.Clear;
        Debug.Log(Variables.screenState);
    }

    void FailedCheck(bool isAllDominoToppled)
    {
        if (tapCountLeft > 0) return;
        if (isAllDominoToppled) return;
        if (Variables.screenState != ScreenState.Game) return;
        Variables.screenState = ScreenState.Failed;
        Debug.Log(Variables.screenState);
        LeftDominoBlinkAnim();
    }

    void LeftDominoBlinkAnim()
    {
        var leftDominoes = dominoControllers.Where(d => d.dominoState == DominoState.Standing);

        foreach (var domino in leftDominoes)
        {
            domino.BlinkAnim();
        }
    }
}
