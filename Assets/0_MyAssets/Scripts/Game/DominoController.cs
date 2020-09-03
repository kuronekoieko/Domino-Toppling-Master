using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    DominoController forwardDomino;
    DominoController backwardDomino;
    public bool isToppled;
    MeshRenderer[] meshRenderers;

    private void Awake()
    {
        meshRenderers = GetComponentsInChildren<MeshRenderer>();
    }
    void Start()
    {
        forwardDomino = GetForwardDomino(isForward: true);
        backwardDomino = GetForwardDomino(isForward: false);
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
        isToppled = true;
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
        if (GameManager.i.isClicked) return;
        GameManager.i.isClicked = true;
        GameCanvasManager.i?.HideTutrial();
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
        rb.AddForceAtPosition(sign * transform.forward * 150f, transform.position + Vector3.up);
    }
}
