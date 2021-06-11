using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private FloatReference horizontalMove;
    [SerializeField]
    private FloatReference verticalMove;
    [SerializeField]
    private BoolReference isDashing;
    private enum State {
        Normal,
        Dashing,
        Unmovable,
    }
    private State state;
    private Vector3 moveDir;
    private Vector3 dashDir;
    public float moveSpeed;
    private float currentDashSpeed;
    public float maxDashSpeed;
    public float dashSpeedDropMultiplier;

    // Teleport Code
    [SerializeField]
    private BoolReference isTeleporting;
    [SerializeField]
    private LayerMask teleportLayerMask;
    public float teleportAmount;


    private Rigidbody2D rb;
    // Start is called before the first frame update
    public void Awake() {
        rb = GetComponent<Rigidbody2D>();
        state = State.Normal;
    }

    public void Update() {
        switch (state) {
            case State.Normal:
                moveDir = new Vector2(horizontalMove.Value, verticalMove.Value).normalized;
                if (isDashing.Value) {
                    dashDir = moveDir;
                    currentDashSpeed = maxDashSpeed;
                    state = State.Dashing;
                }
                break;
            case State.Dashing:
                currentDashSpeed -= currentDashSpeed * dashSpeedDropMultiplier * Time.fixedDeltaTime;
                if (currentDashSpeed <= moveSpeed) {
                    state = State.Normal;
                    isDashing.Value = false;
                }
                break;
            case State.Unmovable:
                break;
        }

    }
    
    public void FixedUpdate() {
        switch (state) {
            case State.Normal:
                if (moveDir.magnitude == 0) {
                    rb.velocity = Vector2.zero;
                }
                else {
                    rb.velocity = moveDir * moveSpeed;
                }
                
                if (isTeleporting.Value) {
                    Teleport(moveDir);
                }
                break;
            case State.Dashing:
                rb.velocity = dashDir * currentDashSpeed;
                break;
            case State.Unmovable:
                break;
        }
    }

    public void disableMovement() {
        Debug.Log("Set to unmovable!");
        rb.velocity = Vector2.zero;
        state = State.Unmovable;
        Debug.Log("Value of state is: " + state);
    }

    public void enableMovement() {
        state = State.Normal;
    }

    public bool getIsDashing() {
        return isDashing.Value;
    }

    private void Teleport(Vector3 moveDir) {
        Vector3 teleportPosition = transform.position + moveDir * teleportAmount;
        RaycastHit2D raycastHit2d = Physics2D.Raycast(transform.position, moveDir, teleportAmount, teleportLayerMask);
        if (raycastHit2d.collider != null) {
            teleportPosition = raycastHit2d.point;
        }

        rb.MovePosition(teleportPosition);
        isTeleporting.Value = false;
    }
}
