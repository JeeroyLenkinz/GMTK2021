using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool isChained;
    private LineRenderer lineRenderer;

    private void Awake()
    {
        isChained = false;
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
        
    }

    public bool GetIsChained()
    {
        return isChained;
    }

    public void SetIsChained(bool val)
    {
        isChained = val;
    }

    public void ChangeLine()
    {

    }
}
