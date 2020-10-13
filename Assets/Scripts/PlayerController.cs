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

    // an array of force values for the camera
    public float[] hitForce;

    // grace period after the hero sustains damage
    public float timeBetweenHits = 2.5f;
    // flag for if hero is hit or not
    private bool isHit = false;
    // tracks amount of time during the grace period
    private float timeSinceHit = 0;
    // references the number of times the heor took a hit
    private int hitNumber = -1;

    public Rigidbody marineBody;
    private bool isDead = false;

    private DeathParticles deathParticles;

    // Start is called before the first frame update
    void Start()
    {
        // gets a reference to current component passed into the script
        characterController = GetComponent<CharacterController>();

        deathParticles = gameObject.GetComponentInChildren<DeathParticles>();
    }

    // Update is called once per frame
    void Update()
    {
        // creates new vector3 to store the movement direction
        // then calls SimpleMove() and passes in the moveDirection multiplied by moveSpeed
        // SimpleMove() is a method that autimatically moves the character in the given direction, not allwoing the character to move through obstacles
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        characterController.SimpleMove(moveDirection * moveSpeed);

        // tubulates time since last hit, if that exceeds timeBetweenHits, the player can take no more
        if(isHit)
        {
            timeSinceHit += Time.deltaTime;
            if (timeSinceHit > timeBetweenHits)
            {
                isHit = false;
                timeSinceHit = 0;
            }
        }
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

    void OnTriggerEnter(Collider other)
    {
        Alien alien = other.gameObject.GetComponent<Alien>();
        if (alien != null)
        {
            //1 check if  the colliding object has an alien script
            if (!isHit)
            {
                hitNumber += 1; //2 increases by 1
                // reference to camera shake
                CameraShake cameraShake = Camera.main.GetComponent<CameraShake>();
                if (hitNumber < hitForce.Length) //3 if hitNumber is less than camera shake, hero is still alive.
                {
                    // set the shaking effect
                    cameraShake.intensity = hitForce[hitNumber];
                    // then shake the camera
                    cameraShake.Shake();
                }
                else
                {
                    Die();
                }
                isHit = true; // 4 plays the grunt sound and kills the alien
                SoundManager.Instance.PlayOneShot(SoundManager.Instance.hurt);
            }
            alien.Die();
        }
    }

    public void Die()
    {
        // stops player from moving once dead
        bodyAnimator.SetBool("IsMoving", false);
        // removes GameObject from its parent
        marineBody.transform.parent = null;
        // enabling body to drop and roll
        marineBody.isKinematic = false;
        marineBody.useGravity = true;
        // enable the collider to allow it to drop and roll
        marineBody.gameObject.GetComponent<CapsuleCollider>().enabled = true;
        // prevents the player from firing when dead
        marineBody.gameObject.GetComponent<Gun>().enabled = false;

        // destroys the hinge joint from the head to the body
        Destroy(head.gameObject.GetComponent<HingeJoint>());
        // remove the parent and enable gravity
        head.transform.parent = null;
        head.useGravity = true;
        // plays the death sound and destroys the game object
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.marineDeath);
        deathParticles.Activate();
        Destroy(gameObject);
    }
}
