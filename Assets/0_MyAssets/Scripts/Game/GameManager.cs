using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 3D空間の処理の管理
/// </summary>
public class GameManager : MonoBehaviour
{
    DominoController[] dominoControllers;
    private void Awake()
    {
        dominoControllers = FindObjectsOfType<DominoController>();
    }

    private void Start()
    {

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
