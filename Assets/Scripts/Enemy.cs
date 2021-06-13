using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.Experimental.Rendering.Universal;
using ScriptableObjectArchitecture;

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
    private Animator animController;
    public GameEvent enemyDestroyedEvent;

    public Light2D pointLight;

    private enum State {
        Moving,
        Attacking,
    }
    private State state;

    private void Awake()
    {
        isChained = false;
        nextAttached = null;
        animController = GetComponentInChildren<Animator>();

        lineRenderer = GetComponentInChildren<LineRenderer>();
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
            animController.SetTrigger("EnemyStartSwing");
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

    public void a_SwingDown()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        if (hitPlayers.Length > 0)
        {
            // Player has been hit
            Debug.Log("Player Hit!");
            playerHitEvent.Raise();
        }
    }

    public void a_DoneSwing()
    {
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
        enemyDestroyedEvent.Raise();
        GameObject explosion = Instantiate(explodedEnemyPrefab, transform.position, Quaternion.identity);
        explosion.GetComponent<ExplodeEnemy>().ExplodeMe(explosionOrigin);
        //Destroy(this.gameObject);
        this.gameObject.SetActive(false);
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

    public Light2D GetLight()
    {
        return pointLight;
    }
}
