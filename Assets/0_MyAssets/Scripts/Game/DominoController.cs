using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoController : MonoBehaviour
{
    DominoController forwardDomino;
    DominoController backwardDomino;
    void Start()
    {
        forwardDomino = GetForwardDomino(isForward: true);
        backwardDomino = GetForwardDomino(isForward: false);
        Debug.Log(forwardDomino + " " + backwardDomino);
    }

    private void Update()
    {


    }

    DominoController GetForwardDomino(bool isForward)
    {
        int sign = isForward ? 1 : -1;
        Ray ray = new Ray(transform.position + Vector3.up, transform.forward * sign);
        bool isHit = Physics.SphereCast(ray.origin, 0.5f, ray.direction, out RaycastHit hit, Mathf.Infinity);
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * 10f, Color.red);
        if (isHit == false) return null;
        return hit.collider.GetComponent<DominoController>();
    }

    public void OnMouseDownEventTrigger()
    {


    }
}
