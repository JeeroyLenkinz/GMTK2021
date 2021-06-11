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
        horizontalMove.Value = Input.GetAxis("Horizontal");
        verticalMove.Value = Input.GetAxis("Vertical");
        if (Input.GetButtonDown("Dash")) {
            isDashing.Value = true;
        }
    }
}
