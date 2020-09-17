using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // method called when object is no longer visible by any camera
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    // called during collision event
    // collision object contains information about actual collision as well as target object
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
