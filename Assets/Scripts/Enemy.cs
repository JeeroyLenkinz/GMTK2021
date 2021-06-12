using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    private bool isChained;
    private LineRenderer lineRenderer;
    private GameObject nextAttached;
    private AIPath aiPath;
    [SerializeField]
    private Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerLayer;
    [SerializeField]
    private GameEvent playerHitEvent;
    private Vector2 explosionOrigin;
    [SerializeField]
    private GameObject explodedEnemyPrefab;
    private enum State {
        Moving,
        Attacking,
    }
    private State state;

    private void Awake()
    {
        isChained = false;
        nextAttached = null;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.widthMultiplier = 0.3f;
        aiPath = GetComponent<AIPath>();
        state = State.Moving;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (aiPath.reachedEndOfPath && state == State.Moving) {
            state = State.Attacking;
            aiPath.isStopped = true;
            StartCoroutine(Attack());
        }
        UpdateLineRenderer();
    }

    private void UpdateLineRenderer()
    {
        if (nextAttached == null)
        {
            return;
        }
        else
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, nextAttached.transform.position);
        }
    }

    private IEnumerator Attack() {
        // Begin attack animations
        Debug.Log("Starting windup!");
        // Yield for whatever the duration of the windup is
        yield return new WaitForSeconds(1.5f);
        Debug.Log("Swing!");
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        if (hitPlayers.Length > 0) {
            // Player has been hit
            Debug.Log("Player Hit!");
            playerHitEvent.Raise();
        }
        // Slight cooldown before chasing again
        yield return new WaitForSeconds(1.5f);
        state = State.Moving;
        aiPath.isStopped = false;
    }

    public bool GetIsChained()
    {
        return isChained;
    }

    public void SetExplosionOrigin(Vector2 origin)
    {
        explosionOrigin = transform.InverseTransformPoint(origin);
    }

    private void Detach()
    {
        isChained = false;
        nextAttached = null;
        lineRenderer.enabled = false;
    }

    public void Explode()
    {
        GameObject explosion = Instantiate(explodedEnemyPrefab, transform.position, Quaternion.identity);
        explosion.GetComponent<ExplodeEnemy>().ExplodeMe(explosionOrigin);
        Destroy(this.gameObject);
    }

    public void AttachNext(GameObject nextObj)
    {
        lineRenderer.enabled = true;
        isChained = true;
        nextAttached = nextObj;
    }

    public void e_ConnectionSevered()
    {
        Detach();
    }

    void OnDrawGizmosSelected() {
        if (attackPoint == null) {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
