using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    [SerializeField]
    private GameEvent moveToGhostFXDone;

    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void e_Heard_Move_To_Ghost()
    {
        StartCoroutine(MoveToGhostVFX());
    }

    public void e_Heard_Ghost_Reached()
    {
        float effectDur = 0.2f;
        cam.gameObject.GetComponent<CameraFollow>().GhostReached(effectDur);
    }

    private IEnumerator MoveToGhostVFX()
    {
        float effectDur = 0.5f;        // How long the cumulative effect is
        cam.gameObject.GetComponent<CameraFollow>().Stop_Channel(effectDur);
        yield return new WaitForSeconds(effectDur);
        moveToGhostFXDone.Raise();
        yield return null;
    }
}
