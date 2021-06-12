using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChain : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private GameObject nextAttached;

    public GameObject ghost;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.widthMultiplier = 0.3f;
        lineRenderer.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        nextAttached = ghost;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        UpdateLineRenderer();
    }

    private void UpdateLineRenderer()
    {
        if (nextAttached == null)
        {
            return;
        }
        else
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, nextAttached.transform.position);
        }
    }

    public void Detach()
    {
        nextAttached = null;
        lineRenderer.enabled = false;
    }

    public void AttachNext(GameObject nextObj)
    {
        nextAttached = nextObj;
    }

    public void OnSummon()
    {
        nextAttached = ghost;
        lineRenderer.enabled = true;
    }
}
