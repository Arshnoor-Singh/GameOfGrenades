using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class PlayerSpawnner : MonoBehaviour
{
    PlayerInputManager _playerManager;
   
    public GameObject PlayerPrefab;

    public GameObject[] spawnPoint;
    public GameObject[] playerObjects = new GameObject[4];

    string[] _player = { "Player 1", "Player 2", "Player 3", "Player 4" };
    

    int PlayerCount = 0;

    private void Start()
    {
        spawnPoint[0] = GameObject.Find("Spawn 1");
        spawnPoint[1] = GameObject.Find("Spawn 2");
        spawnPoint[2] = GameObject.Find("Spawn 3");
        spawnPoint[3] = GameObject.Find("Spawn 4");

        _playerManager = GetComponent<PlayerInputManager>(); 
        StartCoroutine(setplayerprefab()); // This is to avoid any early instantiation of player
    }

    IEnumerator setplayerprefab()
    {
        yield return new WaitForSeconds(1f);
        _playerManager.playerPrefab = PlayerPrefab;
    }

    void OnPlayerJoined(PlayerInput player)
    {
        player.transform.position = spawnPoint[PlayerCount].transform.position; //Spawn players in their target points
        player.GetComponent<FragPartyController>().PlayerID = PlayerCount;
        playerObjects[player.playerIndex] = player.transform.parent.gameObject;
        player.transform.parent.tag = _player[PlayerCount]; //sets player tag
        ChangeLayersRecursively(player.transform.parent.transform, _player[PlayerCount]); //sets player layer for the camera to follow
        PlayerCount++;
    }

    void ChangeLayersRecursively(Transform t, string s)
    {
        foreach(Transform child in t)
        {
            if(child.gameObject.tag == "MainCamera")
            {
                int layerTOAdd = LayerMask.NameToLayer(_player[PlayerCount]);
                child.GetComponent<Camera>().cullingMask |= 1 << layerTOAdd;
            }

            if (child.gameObject.tag == "CamFollow")
            {
                child.gameObject.layer = LayerMask.NameToLayer(s);
                
            }

            
            ChangeLayersRecursively(child.transform, s);
        }
    }

    //If players fall off map
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            other.gameObject.GetComponent<FragPartyCharacter>().Kill();        
    }
}
