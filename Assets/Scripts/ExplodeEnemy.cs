using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeEnemy : MonoBehaviour
{
    public float explosionStrength = 100;
    private void Awake()
    {
        foreach(Transform child in gameObject.transform)
        {
            child.gameObject.GetComponent<Rigidbody2D>().AddExplosionForce(explosionStrength, this.transform.position, 5); ;
        }
    }

}
