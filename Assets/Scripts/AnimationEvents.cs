using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{

    Enemy enemyLogic;
    // Start is called before the first frame update
    void Start()
    {
        enemyLogic = GetComponentInParent<Enemy>();
    }

    public void a_SwingDown()
    {
        enemyLogic.a_SwingDown();
    }

    public void a_DoneSwing()
    {
        enemyLogic.a_DoneSwing();
    }

}
