using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject[] spawnPoints;
    public GameObject alien;
    // determine how many aliens on screen
    public int maxAliensOnScreen;
    // total number of aliens player must vanquish
    public int totalAliens;
    // rate at which they spawn
    public float minSpawnTime;
    public float maxSpawnTime;
    // determine how many aliens appear during a spawning event
    public int aliensPerSpawn;
    // track the total number of aliens currently displayed
    private int aliensOnScreen = 0;
    // track the time between spawn events
    private float generatedSpawnTime = 0;
    // track the milliseconds since the last spawn
    private float currentSpawnTime = 0;

    public GameObject upgradePrefab;
    // reference to gun script
    public Gun gun;
    public float upgradeMaxTimeSpawn = 7.5f;

    // tracks if upgrade has spawned
    private bool spawnedUpgrade = false;

    // tracks the time until upgrade spawns
    private float actualUpgradeTime = 0;
    private float currentUpgradeTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        // the time is random number generated min value is max - 3
        actualUpgradeTime = Random.Range(upgradeMaxTimeSpawn - 3.0f, upgradeMaxTimeSpawn);
        // mathf.abs makes sure it's a positive number
        actualUpgradeTime = Mathf.Abs(actualUpgradeTime);
    }

    // Update is called once per frame
    void Update()
    {
        // adds the amount of time from the past frame
        currentUpgradeTime += Time.deltaTime;

        if (currentUpgradeTime > actualUpgradeTime)
        {
            // 1 after random time period passes, check if upgrade has spawned
            if (!spawnedUpgrade)
            {
                // 2 upgrade will appear in one alien position
                int randomNumber = Random.Range(0, spawnPoints.Length - 1);
                GameObject spawnLocation = spawnPoints[randomNumber];

                // 3 handles the business of spawning the upgrade and associating gun with it
                GameObject upgrade = Instantiate(upgradePrefab) as GameObject;
                Upgrade upgradeScript = upgrade.GetComponent<Upgrade>();
                upgradeScript.gun = gun;
                upgrade.transform.position = spawnLocation.transform.position;

                // 4 informs the code that upgrade has been spawned
                spawnedUpgrade = true;

                SoundManager.Instance.PlayOneShot(SoundManager.Instance.powerUpAppear);
            }
        }

        // accumulates amount of time that's passed between each frame
        currentSpawnTime += Time.deltaTime;

        // spawn randomizer
        if(currentSpawnTime > generatedSpawnTime)
        {
            currentSpawnTime = 0;
            // spawn-time randomizer
            generatedSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);

            // stops spawning when the maximum number of aliens are present
            if (aliensPerSpawn > 0 && aliensOnScreen < totalAliens)
            {
                // creates array used to keep track of where you spawn aliens each wave
                List<int> previousSpawnLocations = new List<int>();
                // limits number of aliens you can spawn by the number of spawn points
                if (aliensPerSpawn > spawnPoints.Length)
                {
                    aliensPerSpawn = spawnPoints.Length - 1;
                }

                // if aliensPerSpawn exceeds maximum, then the amount of spawns will reduce
                aliensPerSpawn = (aliensPerSpawn > totalAliens) ?
                    aliensPerSpawn - totalAliens : aliensPerSpawn;

                for (int i = 0; i < aliensPerSpawn; i++)
                {
                    // checks if aliensOnScreen is less than max, then increments the total screen amount
                    if (aliensOnScreen < maxAliensOnScreen)
                    {
                        aliensOnScreen += 1;

                        // 1 generated spawn point number
                        int spawnPoint = -1;

                        // 2 loop runs until it finds a spawn point or the spawn point is no longer -1
                        while (spawnPoint == -1)
                        {
                            // 3 produces a random number as a possible spawn point
                            int randomNumber = Random.Range(0, spawnPoints.Length - 1);

                            // 4 checks the previousSpawnLocations array to see if that number is an active spawn
                            // number is added to the array and the spawnPoint is set, breaking the loop
                            // if it finds a match, the loop iterates again
                            if (!previousSpawnLocations.Contains(randomNumber))
                            {
                                previousSpawnLocations.Add(randomNumber);
                                spawnPoint = randomNumber;
                            }
                        }

                        // grabs the spawn point based on the index that was generated
                        GameObject spawnLocation = spawnPoints[spawnPoint];

                        //  Instantiate() will create an instance of any prefab passed into it
                        // it'll create an object that is type Object, so it must be cast into a GameObject
                        GameObject newAlien = Instantiate(alien) as GameObject;

                        // positions the alien at the spawn point
                        newAlien.transform.position = spawnLocation.transform.position;

                        // gets reference to the Alien script
                        Alien alienScript = newAlien.GetComponent<Alien>();

                        // sets the target to the space marine's current position
                        alienScript.target = player.transform;

                        // rotates the alien towards the hero using the alien's Y-axis position
                        Vector3 targetRotation = new Vector3(player.transform.position.x, newAlien.transform.position.y, player.transform.position.z);
                        newAlien.transform.LookAt(targetRotation);
                    }
                }
            }
        }
    }
}
