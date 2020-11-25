using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    
    public Transform player;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float zoom = .96f;

    private void FixedUpdate()
    {
        // Vector3 desiredPosition = player.position + offset;
        // Vector3 smoothedPosition = Vector3.Lerp (transform.position, desiredPosition, smoothSpeed);
        // transform.position = smoothedPosition;

        Vector3 desiredPosition = player.position - player.forward * 10f + offset;
        
        //spring function
        transform.position = transform.position * zoom + desiredPosition * (1.0f - zoom);
        transform.LookAt(player.position + player.forward * 10f);

    }
    
}
