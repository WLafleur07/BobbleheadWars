using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Alien : MonoBehaviour
{   
    //  where alien should go
    public Transform target;
    private NavMeshAgent agent;
    // amount of time, in ms for when the alien should update its path
    public float navigationUpdate;
    // tracks how much time has passed since the previous update
    private float navigationTime = 0;
    // creating an OnDestroy event, occurs each call to an alien
    public UnityEvent OnDestroy;
    public Rigidbody head;
    public bool isAlive = true;
    private DeathParticles deathParticles;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            if (target != null)
            {
                navigationTime += Time.deltaTime;
                if (navigationTime > navigationUpdate)
                {
                    agent.destination = target.position;
                    navigationTime = 0;
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (isAlive)
        {
            Die();
            SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDeath);
        }
    }

    // this destroys the alien
    public void Die()
    {
        isAlive = false;
        head.GetComponent<Animator>().enabled = false;
        head.isKinematic = false;
        head.useGravity = true;
        head.GetComponent<SphereCollider>().enabled = true;
        head.gameObject.transform.parent = null;
        head.velocity = new Vector3(0, 6.0f, 3.0f);

        // notifies the listeners, the deletes the game object
        OnDestroy.Invoke();
        OnDestroy.RemoveAllListeners();
        SoundManager.Instance.PlayOneShot(SoundManager.Instance.alienDeath);
        head.GetComponent<SelfDestruct>().Initiate();

        // makes blood splatter when alien dies
        if (deathParticles)
        {
            deathParticles.transform.parent = null;
            deathParticles.Activate();
        }
        Destroy(gameObject);
    }

    public DeathParticles GetDeathParticles()
    {
        if (deathParticles == null)
        {
            // returns the first death particle script found.
            deathParticles = GetComponentInChildren<DeathParticles>();
        }
        return deathParticles;
    }
}
