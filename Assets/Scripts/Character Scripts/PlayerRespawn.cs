using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    FragPartyController PlayerController;
    FragPartyCharacter PlayerCharacter;
    PlayerSpawnner SpawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        PlayerController = GetComponent<FragPartyController>();
        SpawnPoints = FindObjectOfType<PlayerSpawnner>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Respawn()
    {
        PlayerController.transform.position = SpawnPoints.spawnPoint[PlayerController.PlayerID].transform.position;
    }
}
