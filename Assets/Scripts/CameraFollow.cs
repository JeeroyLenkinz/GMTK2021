using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using UnityEngine;
using DG.Tweening;

public class CameraFollow : MonoBehaviour
{

    public float smoothSpeed = 10f;
    private float oldSmoothSpeed;
    public Vector3 offset;
    public float minZoom;
    public float maxZoom;
    public float zoomLimiter;
    public float severedMultiplier;
    public float movingToGhostZoom;

    public GameObject player;
    public GameObject ghost;

    private Transform target;
    Camera cam;

    private float moveToGhostDur;
    private float newZoom;
    private bool isAnimating;
    private bool isMovingToGhost;

    private enum Status
    {
        PlayerMode,
        GhostMode,
        Severed,
        MoveToGhost
    }
    private Status status;

    private void Start()
    {
        oldSmoothSpeed = smoothSpeed;
        isAnimating = false;
        isMovingToGhost = false;
        status = Status.PlayerMode;
        target = player.transform;
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {

        Move();
        Zoom();
    }

    private float GetDistance()
    {
        var bounds = new Bounds(ghost.transform.position, Vector3.zero);
        // Gets the bounds of ghost and player, then doubles it to hopefully center on ghost
        if (status == Status.PlayerMode)
        {
            return minZoom;
        }
        else if(status == Status.GhostMode)
        {
            bounds = new Bounds(ghost.transform.position, Vector3.zero);
            bounds.Encapsulate(ghost.transform.position);
            bounds.Encapsulate(player.transform.position);
            //bounds.center = ghost.transform.position;
            //return bounds.size.x * 2;
            return bounds.size.x * 2;

        }
        else // Severed
        {
            return minZoom * severedMultiplier;
        }



    }

    private IEnumerator WaitForZoomIn(float duration)
    {
        yield return new WaitForSeconds(duration);
        smoothSpeed = 100f;
        isMovingToGhost = true;
        isAnimating = false;
        target = player.transform;
        
    }
    private IEnumerator GhostReachedCoroutine(float duration)
    {
        Tweener tween = cam.DOOrthoSize(minZoom, duration).SetEase(Ease.InOutBack, 0.5f);    // Zoom back out
        yield return new WaitForSeconds(tween.Duration());
        newZoom = minZoom;
        isAnimating = false;
        isMovingToGhost = false;
        smoothSpeed = oldSmoothSpeed;
        status = Status.PlayerMode;
    }

    private void Move()
    {
        // Lead the with the camera if possible

        if (!isAnimating)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            transform.position = smoothedPosition;
        }

    }

    private void Zoom()
    {
        if (status == Status.MoveToGhost)
        {
            if (!isAnimating && !isMovingToGhost)
            {
                isMovingToGhost = true;
                isAnimating = true;
                cam.gameObject.transform.DOMove(target.position + offset, moveToGhostDur).SetEase(Ease.InOutBack);
                cam.DOOrthoSize(movingToGhostZoom, moveToGhostDur).SetEase(Ease.InOutBack).SetEase(Ease.InOutBack);
                StartCoroutine(WaitForZoomIn(moveToGhostDur));
            }

            return;
        }
        else if (status == Status.PlayerMode)
        {
            newZoom = Mathf.Lerp(newZoom, minZoom, 1f);

        }
        else if (status == Status.Severed)
        {
            newZoom = minZoom * severedMultiplier;

        }
        else
        {
            newZoom = Mathf.Lerp(minZoom, maxZoom, GetDistance() / zoomLimiter);
        }

        if (newZoom < minZoom && status != Status.Severed)
        {
            newZoom = minZoom;
        }

        if(status == Status.GhostMode)
        {
            cam.orthographicSize = newZoom;
        } else
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
        }

    }


    // All of these called by VFX manager
    public void Start_Channel()
    {
        status = Status.GhostMode;
        target = ghost.transform;
    }

    public void Stop_Channel(float moveSpeed)
    {
        moveToGhostDur = moveSpeed;
        status = Status.MoveToGhost;
        target = player.transform;
    }

    public void Severed()
    {
        status = Status.Severed;
        target = player.transform;
    }

    public void GhostReached(float duration)
    {
        isAnimating = true;
        StartCoroutine(GhostReachedCoroutine(duration));
    }

    public void Reattach()
    {
        status = Status.PlayerMode;
        target = player.transform;
    }

}
