using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 50.0f;
    // variable to store the CharacterController
    private CharacterController characterController;
    public Rigidbody head;
    // layer mask lets you indicate what layers the ray should hit
    public LayerMask layerMask;
    // currentLook is where you want the Marine to stare
    private Vector3 currentLookTarget = Vector3.zero;

    public Animator bodyAnimator;

    // Start is called before the first frame update
    void Start()
    {
        // gets a reference to current component passed into the script
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // creates new vector3 to store the movement direction
        // then calls SimpleMove() and passes in the moveDirection multiplied by moveSpeed
        // SimpleMove() is a method that autimatically moves the character in the given direction, not allwoing the character to move through obstacles
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        characterController.SimpleMove(moveDirection * moveSpeed);
    }

    void FixedUpdate()
    {
        // calculate the movement direction
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // if value equals vector3.zero, marine is standing still
        if (moveDirection == Vector3.zero)
        {
            bodyAnimator.SetBool("IsMoving", false);
        }
        else
        {
            // AddForce gets the head to move
            // provide direction, then mutliply it by the force amount
            // force.acceleration gives a continuous amount of force that ignores the mass
            head.AddForce(transform.right * 150, ForceMode.Acceleration);

            bodyAnimator.SetBool("IsMoving", true);
        }

        // creates empty RaycstHit.
        RaycastHit hit;
        // cast the ray from the main camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.green);

        // Physics.Raycast actually casts a ray
        // pass in ray generate along with hit
        // hit marked as 'out', it can be populated by Physics.Raycast()
        // 1000 indicates the length of the ray
        // layerMask lets the cast know what you are trying to hit
        // QueryTriggerInteraction.Ignore tells the physics engine not to activate triggers
        if (Physics.Raycast(ray, out hit, 1000, layerMask, QueryTriggerInteraction.Ignore))
        {
            // comprises coordinates of the raycast hit
            if (hit.point != currentLookTarget)
            {
                currentLookTarget = hit.point;
            }
        }

        // 1 - Get target position
        Vector3 targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);

        // 2 - calculate the current quaternion, used to determine rotation
        //LookRotation() returns the quaternion for where the marine should turn
        Quaternion rotation = Quaternion.LookRotation(targetPosition - transform.position);

        // 3 - Actual turn by using Lerp(). Lerp is used to change a value smoothly over time
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10.0f);
    }
}
