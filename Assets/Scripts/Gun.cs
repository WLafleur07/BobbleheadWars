using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    // set to the position of the barrel
    public Transform launchPosition;

    void fireBullet()
    {
        // 1 creates a GameObject instance for a particular prefab
        GameObject bullet = Instantiate(bulletPrefab) as GameObject;
        // 2 bullets position is set to launcher's position
        bullet.transform.position = launchPosition.position;
        // 3 specify its velocity to make the bullet move at a constant rate
        bullet.GetComponent<Rigidbody>().velocity =
            transform.parent.forward * 100;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // check with Input Manager if left mouse is held
        if (Input.GetMouseButtonDown(0))
        {
            // check if fireBullet() is being invoked, if not, call InvokeRepeating()
            if(!IsInvoking("fireBullet"))
            {
                // needs a method name, time to start and the repeat rate
                InvokeRepeating("fireBullet", 0f, 0.1f);
            }
        }

        // makes the gun stop firing once user releases the mouse button
        if(Input.GetMouseButtonUp(0))
        {
            CancelInvoke("fireBullet");
        }
    }
}
