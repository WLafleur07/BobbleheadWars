using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaWall : MonoBehaviour
{

    private Animator arenaAnimator;

    // Start is called before the first frame update
    void Start()
    {
        // gets the parent GameObject, gets reference to tranform property, then it gets reference to the animator
        GameObject arena = transform.parent.gameObject;
        arenaAnimator = arena.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        arenaAnimator.SetBool("IsLowered", true);
    }

    void OnTriggerExit(Collider other)
    {
        arenaAnimator.SetBool("IsLowered", false);
    }
}
