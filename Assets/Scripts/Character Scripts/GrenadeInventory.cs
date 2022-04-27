/*
*  Author:             Ryan Jenkins
*  Creation Date:      4/19/2022
*  Last Updated:       4/20/2022
*  Description:        This script stores the grenade items collected by the player and manages which grenade is active
*                      to pass the prefab to the controller to be thrown
*/


using System;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrenadeInventory : MonoBehaviour
{
    public GrenadeListScriptableObject grenadeList;
    
    [Range(0, 3)] public int activeGrenadeSlot = 0; // the slot index of the currently selected grenade
    public GrenadeItem[] grenadeInventory = new GrenadeItem[4]; // the array of grenade prefabs the player in their inventory

    private FragPartyInputs _input;

    private void Start()
    {
        _input = GetComponent<FragPartyInputs>();
    }

    private void Update()
    {
        if (_input.slot_1 && !grenadeInventory[0].empty)
        {
            ChangeSlot(0);
            _input.slot_1 = false;
        }

        if (_input.slot_2 && !grenadeInventory[1].empty)
        {
            ChangeSlot(1);
            _input.slot_2 = false;
        }

        if (_input.slot_3 && !grenadeInventory[2].empty)
        {
            ChangeSlot(2);
            _input.slot_3 = false;
        }

        if (_input.slot_4 && !grenadeInventory[3].empty)
        {
            ChangeSlot(3);
            _input.slot_4 = false;
        }
    }

    // Initialize the grenade systems inventory
    public void Init()
    {
        // Initialize the first inventory slot with the first grenade in the list
        grenadeInventory[0] = new GrenadeItem();
        grenadeInventory[0].Init(grenadeList.list[0]);
        
        // Initialize the remaining inventory slots as empty
        for(int i = 1; i <= 3; ++i)
        {
            grenadeInventory[i] = new GrenadeItem();
            grenadeInventory[i].Clear();
        }
    }

    // Update each grenade item in the inventory
    public void UpdateInventory()
    {
        foreach (var item in grenadeInventory)
        {
            item.UpdateItem();
        }
    }
    
    // change the active inventory slot to the index provided, returns true if changed
    private bool ChangeSlot(int slotID)
    {
        if (slotID >= 0 && slotID <= 3 && !grenadeInventory[slotID].empty)
        {
            activeGrenadeSlot = slotID;
            return true;
        }
        else
        {
            return false;
        }
    }
    
    // Returns the grenade prefab of the active inventory slot
    public GameObject GetGrenade()
    {
        return grenadeInventory[activeGrenadeSlot].GetPrefab();
    }

    // Adds a grenade prefab to the inventory in the first available slot or overrides the active slot if full
    public void AddGrenade(GameObject newGrenade)
    {
        bool emptySlotFound = false;
        for (int i = 0; i <= 3; ++i)
        {
            if (grenadeInventory[i].empty)
            {
                grenadeInventory[i].Init(newGrenade);
                emptySlotFound = true;
                break;
            }
        }

        if (!emptySlotFound)
        {
            grenadeInventory[activeGrenadeSlot].Init(newGrenade);
        }
    }

    // Removes the grenade from the inventory slot and returns a prefab reference or null if unable
    public GameObject RemoveGrenade(int slotID)
    {
        if (slotID >= 0 && slotID <= 3 && !grenadeInventory[slotID].empty)
        {
            GameObject removedGrenade = grenadeInventory[slotID].GetPrefab();
            grenadeInventory[slotID].Clear();
            return removedGrenade;
        }
        else
        {
            return null;
        }
    }

    // removes the grenade in the active inventory slot
    public GameObject RemoveActiveGrenade()
    {
        return RemoveGrenade(activeGrenadeSlot);
    }

    // Attempts to throws the actively selected grenade
    public bool ThrowGrenade(Vector3 position, Quaternion rotation, Vector3 direction)
    {
        if (grenadeInventory[activeGrenadeSlot].TryThrow())
        {
            GameObject grenade = Instantiate(grenadeInventory[activeGrenadeSlot].GetPrefab(), position, rotation);
            Grenade_Base grenadeBase = grenade.GetComponent<Grenade_Base>();
            grenadeBase.StartCooking();
            grenadeBase.Launch(direction);
            return true;
        }
        else
        {
            return false;
        }
    }
}