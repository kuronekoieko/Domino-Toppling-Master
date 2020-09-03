using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DominoController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    DominoController forwardDomino;
    DominoController backwardDomino;
    public bool isToppled;
    void Start()
    {
        forwardDomino = GetForwardDomino(isForward: true);
        backwardDomino = GetForwardDomino(isForward: false);
        // Debug.Log(name + " >>>>> " + forwardDomino + " >>>>> " + backwardDomino);
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

    private void OnCollisionEnter(Collision other)
    {
        DominoController domino = other.gameObject.GetComponent<DominoController>();
        if (domino)
        {
            isToppled = true;
        }
    }
}
