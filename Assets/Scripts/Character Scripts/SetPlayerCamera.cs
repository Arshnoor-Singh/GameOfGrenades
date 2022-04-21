using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class SetPlayerCamera : MonoBehaviour
{
    PlayerInputManager player;

    public GameObject PlayerPrefab;

    public GameObject[] playerObjects;

    string[] _player = { "Player 1", "Player 2", "Player 3", "Player 4" };

    int LayerCount = 0;

    private void Start()
    {
        player = GetComponent<PlayerInputManager>(); 
        StartCoroutine(setplayerprefab()); // This is to avoid any early instanciation of player
    }

    IEnumerator setplayerprefab()
    {
        yield return new WaitForSeconds(1f);
        player.playerPrefab = PlayerPrefab;
    }

    void OnPlayerJoined(PlayerInput player)
    {
        playerObjects[player.playerIndex] = player.transform.parent.gameObject;
        ChangeLayersRecursively(player.transform.parent.transform, _player[LayerCount]);
        LayerCount++;
    }

    void ChangeLayersRecursively(Transform t, string s)
    {
        foreach(Transform child in t)
        {
            if(child.gameObject.tag == "MainCamera")
            // if(child.gameObject.tag == "SplitScreenCamera")
            {
                int layerTOAdd = LayerMask.NameToLayer(_player[LayerCount]);
                child.GetComponent<Camera>().cullingMask |= 1 << layerTOAdd;
            }

            if (child.gameObject.tag == "CamFollow")
            {
                child.gameObject.layer = LayerMask.NameToLayer(s);
                
            }

            
            ChangeLayersRecursively(child.transform, s);
        }
    }
}
