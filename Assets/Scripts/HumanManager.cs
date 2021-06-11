using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanManager : PlayerManager
{
    [SerializeField]
    private GameObject ghost;
    // Start is called before the first frame update
    void Start()
    {
        ghost.SetActive(false);
    }

    public void e_startChanneling() {
        if (!getIsDashing()) {
            disableMovement();
            ghost.transform.position = gameObject.transform.position;
            ghost.SetActive(true);
        }
    }
}
