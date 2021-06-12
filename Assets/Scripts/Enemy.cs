using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool isChained;
    private LineRenderer lineRenderer;
    private GameObject nextAttached;

    private void Awake()
    {
        isChained = false;
        nextAttached = null;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.widthMultiplier = 0.3f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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

    public bool GetIsChained()
    {
        return isChained;
    }

    private void Detach()
    {
        isChained = false;
        nextAttached = null;
        lineRenderer.enabled = false;
    }

    public void AttachNext(GameObject nextObj)
    {
        lineRenderer.enabled = true;
        isChained = true;
        nextAttached = nextObj;
    }

    public void e_ConnectionSevered()
    {
        Detach();
    }

}
