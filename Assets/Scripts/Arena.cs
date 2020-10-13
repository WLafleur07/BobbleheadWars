using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class Arena : MonoBehaviour
{
    public GameObject player;
    public Transform elevator;
    private Animator arenaAnimator;
    private SphereCollider sphereCollider;

    // Start is called before the first frame update
    void Start()
    {
        // getting access to the components and setting them respectively
        arenaAnimator = GetComponent<Animator>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        // gets camera, then disables movement
        // player is made into a child of the platform
        Camera.main.transform.parent.gameObject.GetComponent<CameraMovement>().enabled = false;
        player.transform.parent = elevator.transform;

        // disables player's ability to control marine
        player.GetComponent<PlayerController>().enabled = false;

        SoundManager.Instance.PlayOneShot(SoundManager.Instance.elevatorArrived);
        arenaAnimator.SetBool("OnElevator", true);
    }

    public void ActivatePlatform()
    {
        sphereCollider.enabled = true;
    }
}
