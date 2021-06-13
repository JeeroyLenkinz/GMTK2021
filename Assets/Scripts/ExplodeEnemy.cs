using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeEnemy : MonoBehaviour
{
    public float explosionStrength = 100;
    private void Awake()
    {

    }

    public void ExplodeMe(Vector2 origin)
    {
        /*
        foreach (Transform child in gameObject.transform)
        {
            child.gameObject.GetComponent<Rigidbody2D>().AddExplosionForce(explosionStrength, origin, 100);
            //child.GetComponent<Collider2D>().isTrigger = true;
        }
        */
        Destroy(this.gameObject, 4f);
    }
}
