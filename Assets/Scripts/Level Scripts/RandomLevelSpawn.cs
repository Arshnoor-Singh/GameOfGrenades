using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLevelSpawn : MonoBehaviour
{
    public GameObject[] Level;
    PlayerSpawnner spawnPoints;

    private void Awake()
    {
        Level[Random.Range(0, Level.Length - 1)].SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = FindObjectOfType<PlayerSpawnner>();
        spawnPoints.InitiateSpawnPoints();
    }
}
