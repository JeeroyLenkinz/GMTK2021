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
        nextAttached = null;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.widthMultiplier = 0.3f;
    }

    // Start is called before the first frame update
    void Start()
    {
        nextAttached = ghost;
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

    public void Detach()
    {

    }

    public void AttachNext(GameObject nextObj)
    {
        nextAttached = nextObj;
    }
}
