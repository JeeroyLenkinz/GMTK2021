using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public float smoothSpeed = 10f;
    public Vector3 offset;

    public GameObject player;
    public GameObject ghost;

    private Transform target;
    Camera cam;

    private void Start()
    {
        target = player.transform;
        cam = GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;
    }

    public void Start_Channel()
    {
        target = ghost.transform;
    }

    public void Stop_Channel()
    {
        target = player.transform;
    }

}
