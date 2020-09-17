using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentSpawnTime += Time.deltaTime;
    }
}
