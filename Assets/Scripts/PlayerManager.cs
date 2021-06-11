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
    private float currentDashTime;
    public float moveSpeed;
    public float dashSpeed;
    public float totalDashTime;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        currentDashTime = totalDashTime;
    }
    
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        Vector2 moveDir = new Vector2(horizontalMove.Value, verticalMove.Value);
        rb.velocity = moveDir * moveSpeed;

        if (isDashing.Value) {
            if (currentDashTime <= 0) {
                isDashing.Value = false;
                currentDashTime = totalDashTime;
                rb.velocity = Vector2.zero;
            }
            else {
                currentDashTime -= Time.fixedDeltaTime;
            }
        }
    }
}
