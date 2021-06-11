using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChannelManager : MonoBehaviour
{

    public GameObject player;
    public GameObject ghost; // His name is Steve

    public float lineWidth;

    private List<GameObject> chainedEnemies = new List<GameObject>();
    private LineRenderer lineRenderer;

    void Awake()
    {
        chainedEnemies.Clear();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.widthMultiplier = lineWidth;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVertices();
    }

    public void AddEnemy(GameObject enemy)
    {
        chainedEnemies.Add(enemy);
        if(chainedEnemies.Count == 1) // First enemy hit
        {
            return; 
        }

    }


    public void RemoveEnemy(GameObject enemy = null)
    {
        // If no argument is given, remove oldest object
        if(enemy == null)
        {
            chainedEnemies.Remove(chainedEnemies[0]);
        }
        else
        {
            chainedEnemies.Remove(enemy);
        }

    }

    private void UpdateVertices()
    {
        // Updates the vertices in the line renderer

        int totalPoints = chainedEnemies.Count + 2; // +1 for player, +1 for ghost
        Vector3[] points = new Vector3[totalPoints];

        for(int i = 0; i < totalPoints; i++)
        {
            if(i == 0)
            {
                points[i] = player.transform.position;
            } 
            else if(i == totalPoints - 1)
            {
                points[i] = ghost.transform.position;
            }
            else
            {
                points[i] = chainedEnemies[i-1].transform.position;
            }
        }

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }

    #region events

    public void e_ghostHitEnemy(GameObject enemy)
    {
        AddEnemy(enemy);
    }

    #endregion
}
