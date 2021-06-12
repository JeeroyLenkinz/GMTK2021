using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyArt : MonoBehaviour
{
    [SerializeField]
    private AIPath aIPath;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (aIPath.desiredVelocity.x >= 0.01f) {
            transform.localScale = new Vector3(-2.5f, 2.5f, 2.5f);
        } else if (aIPath.desiredVelocity.x <= -0.01f) {
            transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        }
    }
}
