using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupGrenade : MonoBehaviour
{
    public GrenadeSpawner spawner;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spawner.CollectGrenade(other.gameObject.GetComponent<GrenadeInventory>());
        }
    }
}
