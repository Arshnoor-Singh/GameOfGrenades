using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoveCharacter : MonoBehaviour
{
    public Vector3 direction = Vector3.zero;
    public float force = 0f;

    private void OnTriggerEnter(Collider collider)
    {
        
        if (collider.gameObject.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<FragPartyController>().AddForce(direction, force);
            Debug.Log("Shove character!");
        }
    }
}
