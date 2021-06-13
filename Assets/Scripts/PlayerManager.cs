﻿using DG.Tweening;
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
    public float dashCooldown;
    private float currentDashCooldownTimer;

    // Teleport Code
    [SerializeField]
    private BoolReference isTeleporting;
    [SerializeField]
    private LayerMask teleportLayerMask;
    public float teleportAmount;

    private Animator animController;

    bool isAlreadyWalking;
    [HideInInspector]
    public AudioSource audioSource;
    [SerializeField]
    private AudioClip dashSFX;
    [SerializeField]
    private float dashSFXVolume;
    [HideInInspector]
    public Vector3 lastMoveDir;
    [SerializeField]
    private BoolReference canDash;

    private Rigidbody2D rb;
    [SerializeField]
    private SpriteRenderer wandSparkSprite;
    // Start is called before the first frame update
    public void Awake() {
        isAlreadyWalking = false;
        rb = GetComponent<Rigidbody2D>();
        state = State.Normal;
        animController = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        lastMoveDir = new Vector2(1,0);
        currentDashCooldownTimer = 0;
        wandSparkSprite.enabled = true;
    }

    public void Update() {
        currentDashCooldownTimer -= Time.deltaTime;
        if (currentDashCooldownTimer <= 0) {
            canDash.Value = true;
        }
        switch (state) {
            case State.Normal:
                if (moveDir != Vector3.zero) {
                    lastMoveDir = moveDir;
                }
                moveDir = new Vector2(horizontalMove.Value, verticalMove.Value).normalized;
                if (isDashing.Value) {
                    dashDir = moveDir;
                    currentDashSpeed = maxDashSpeed;
                    state = State.Dashing;
                    canDash.Value = false;
                    currentDashCooldownTimer = dashCooldown;
                    audioSource.clip = dashSFX;
                    audioSource.volume = dashSFXVolume;
                    audioSource.Play();
                }
                break;
            case State.Dashing:
                currentDashSpeed -= currentDashSpeed * dashSpeedDropMultiplier * Time.deltaTime;
                if (currentDashSpeed <= moveSpeed) {
                    state = State.Normal;
                    isDashing.Value = false;
                }
                break;
            case State.Unmovable:
                break;
        }

        if (canDash.Value) {
            wandSparkSprite.enabled = true;
        } else {
            wandSparkSprite.enabled = false;
        }

    }
    
    public void FixedUpdate() {
        switch (state) {
            case State.Normal:
                if (moveDir.magnitude == 0) {
                    rb.velocity = Vector2.zero;
                    if (isAlreadyWalking)
                    {
                        isAlreadyWalking = false;
                        animController.SetTrigger("StopWalk");
                        Debug.Log("Stop Walk Cylce");
                    }

                }
                else {
                    rb.velocity = moveDir * moveSpeed;
                    if (!isAlreadyWalking)
                    {
                        Debug.Log("Activate Walk Cylce");
                        animController.SetTrigger("StartWalk");
                        isAlreadyWalking = true;
                    }

                    //Flip Sprite
                    if (rb.velocity.x >= 0) {
                        transform.localScale = new Vector2(3, 3);
                    } else {
                        transform.localScale = new Vector2(-3, 3);
                    }
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
        rb.velocity = Vector2.zero;
        state = State.Unmovable;
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
