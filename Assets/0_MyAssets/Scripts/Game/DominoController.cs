using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum DominoState
{
    Standing,
    Toppling,
    Toppled,
}
public class DominoController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] AddGravity addGravity;
    DominoController forwardDomino;
    DominoController backwardDomino;
    [NonSerialized] public DominoState dominoState;
    MeshRenderer[] meshRenderers;

    private void Awake()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }
    void Start()
    {
        forwardDomino = GetForwardDomino(isForward: true);
        backwardDomino = GetForwardDomino(isForward: false);
        dominoState = DominoState.Standing;
    }

    private void Update()
    {
        CheckTopped();
    }

    void CheckTopped()
    {
        float xAngle = Vector3.Angle(Vector3.up, transform.up);
        if (xAngle < 45) return;
        if (90 < xAngle) return;
        dominoState = DominoState.Toppled;
        addGravity.downForce = 0;
    }

    public void SetColor(Color color)
    {
        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.material.color = color;
        }
    }

    DominoController GetForwardDomino(bool isForward)
    {
        int sign = isForward ? 1 : -1;
        Ray ray = new Ray(transform.position + Vector3.up, transform.forward * sign);
        float distance = 2f;
        bool isHit = Physics.SphereCast(ray.origin, 0.5f, ray.direction, out RaycastHit hit, distance);
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * distance, Color.red);
        if (isHit == false) return null;
        return hit.collider.GetComponent<DominoController>();
    }

    public void OnMouseDownEventTrigger()
    {
        if (dominoState != DominoState.Standing) return;
        if (GameManager.i.tapCountLeft == 0) return;
        if (GameManager.i.gameState != GameState.TapWaiting) return;
        GameManager.i.tapCountLeft--;
        GameCanvasManager.i?.HideTutrial();
        GameManager.i.gameState = GameState.Toppling;

        if (forwardDomino == null)
        {
            Topple(isForward: false);
            return;
        }
        if (backwardDomino == null)
        {
            Topple(isForward: true);
            return;
        }

        Topple(isForward: true);
        backwardDomino.Topple(isForward: false);
    }

    void Topple(bool isForward)
    {
        int sign = isForward ? 1 : -1;
        rb.AddForceAtPosition(sign * transform.forward * 150f * rb.mass, transform.position + Vector3.up);
        dominoState = DominoState.Toppling;
    }

    private void OnCollisionEnter(Collision other)
    {
        PushOtherDomino(other);
    }

    void PushOtherDomino(Collision other)
    {
        var otherDomino = other.gameObject.GetComponent<DominoController>();
        if (otherDomino == null) return;
        if (otherDomino.dominoState != DominoState.Standing) return;
        Vector3 direction = otherDomino.transform.position - this.transform.position;
        float dot = Vector3.Dot(direction, otherDomino.transform.forward);
        otherDomino.Topple(isForward: dot > 0);
    }

}
