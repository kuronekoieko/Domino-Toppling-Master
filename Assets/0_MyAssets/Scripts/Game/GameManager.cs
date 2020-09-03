using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
/// <summary>
/// 3D空間の処理の管理
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField] Color[] dominoColors;
    DominoController[] dominoControllers;
    public static GameManager i;
    [NonSerialized] public bool isClicked;
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
    }

    private void Update()
    {
        ClearCheck();
    }

    void ClearCheck()
    {
        bool isClear = dominoControllers.All(d => d.isToppled);
        if (isClear == false) return;
        if (Variables.screenState != ScreenState.Game) return;
        Variables.screenState = ScreenState.Clear;
        Debug.Log("clear");
    }
}
