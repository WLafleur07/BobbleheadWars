using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // follow target is what the camera will follow and movespeed is the speed at which it should move
    public GameObject followTarget;
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // checks if target is available
        if (followTarget != null)
        {
            // Vector3 lerp is called to calculate the required position
            transform.position = Vector3.Lerp(transform.position,
                followTarget.transform.position, Time.deltaTime * moveSpeed);
        }
    }
}
