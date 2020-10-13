using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathParticles : MonoBehaviour
{
    private ParticleSystem deathParticles;
    private bool didStart = false;

    // Start is called before the first frame update
    void Start()
    {
        deathParticles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        // once particle system stops, deletes the death particles
        if (didStart && deathParticles.isStopped)
        {
            Destroy(gameObject);
        }
    }

    // starts particle system and informs script that it started.
    public void Activate()
    {
        didStart = true;
        deathParticles.Play();
    }

    public void SetDeathFloor(GameObject deathFloor)
    {
        // checks to see if particle system is loaded
        if (deathParticles == null)
        {
            // in case start() doesnt populate the deathParticles, this will
            deathParticles = GetComponent<ParticleSystem>();
        }
        // sets the collision plane
        deathParticles.collision.SetPlane(0, deathFloor.transform);
    }
}
