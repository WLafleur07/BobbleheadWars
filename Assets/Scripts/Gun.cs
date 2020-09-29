using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    // set to the position of the barrel
    public Transform launchPosition;
    private AudioSource audioSource;
    public bool isUpgraded;
    public float upgradeTime = 10.0f;
    // keeps track of how long it has been since gun has been upgraded
    private float currentTime;

    private Rigidbody createBullet()
    {
        // 1 creates a GameObject instance for a particular prefab
        GameObject bullet = Instantiate(bulletPrefab) as GameObject;
        // 2 bullets position is set to launcher's position
        bullet.transform.position = launchPosition.position;

        return bullet.GetComponent<Rigidbody>();
    }

    void fireBullet()
    {
        Rigidbody bullet = createBullet();

        // 3 specify its velocity to make the bullet move at a constant rate
        bullet.velocity = transform.parent.forward * 100;

        if (isUpgraded)
        {
            // fires bullet at an angle by adding forward directio to right or left direction and dividing in half
            // right
            Rigidbody bullet2 = createBullet();
            bullet2.velocity = (transform.right + transform.forward / 0.5f) * 100;

            // left
            Rigidbody bullet3 = createBullet();
            bullet3.velocity = ((transform.right * -1) + transform.forward / 0.5f) * 100;
        }
        if (isUpgraded)
        {
            audioSource.PlayOneShot(SoundManager.Instance.upgradedGunFire);
        }
        else
        {
            audioSource.PlayOneShot(SoundManager.Instance.gunFire);
        }
    }

    public void UpgradeGun()
    {
        isUpgraded = true;
        currentTime = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        // gets reference to attached AudioSource
        audioSource = GetComponent<AudioSource>();
      
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

        // increments the time when upgrade
        currentTime += Time.deltaTime;
        if (currentTime > upgradeTime && isUpgraded == true)
        {
            isUpgraded = false;
        }
    }
}
