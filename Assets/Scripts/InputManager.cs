using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private FloatReference horizontalMove;
    [SerializeField]
    private FloatReference verticalMove;
    [SerializeField]
    private BoolReference isDashing;
    [SerializeField]
    private GameEvent channelTriggered;
    // Start is called before the first frame update
    void Awake()
    {
        horizontalMove.Value = 0f;
        verticalMove.Value = 0f;
        isDashing.Value = false;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove.Value = Input.GetAxisRaw("Horizontal");
        verticalMove.Value = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("Dash")) {
            isDashing.Value = true;
        }
        if (Input.GetButtonDown("Channel")) {
            channelTriggered.Raise();
        }
    }
}
