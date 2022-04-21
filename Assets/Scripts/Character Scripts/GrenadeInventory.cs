/*
*  Author:             Ryan Jenkins
*  Creation Date:      4/19/2022
*  Last Updated:       4/20/2022
*  Description:        This script stores the grenade items collected by the player and manages which grenade is active
*                      to pass the prefab to the controller to be thrown
*/


using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GrenadeInventory : MonoBehaviour
{
    public GrenadeListScriptableObject grenadeList;
    
    [SerializeField, Range(0, 3)] private int _activeGrenadeSlot = 0; // the slot index of the currently selected grenade
    [SerializeField] private GrenadeItem[] _grenadeInventory = new GrenadeItem[4]; // the array of grenade prefabs the player in their inventory

    // Initialize the grenade systems inventory
    public void Init()
    {
        for(int i = 0; i <= 3; ++i)
        {
            _grenadeInventory[i] = new GrenadeItem();
            _grenadeInventory[i].Init(grenadeList.list[0]);
        }
    }

    // Update each grenade item in the inventory
    public void UpdateInventory()
    {
        foreach (var item in _grenadeInventory)
        {
            item.UpdateItem();
        }
    }
    
    // change the active inventory slot to the index provided, returns true if changed
    public bool ChangeSlot(int slotID)
    {
        if (slotID >= 0 && slotID <= 3 && !_grenadeInventory[slotID].empty)
        {
            _activeGrenadeSlot = slotID;
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
        return _grenadeInventory[_activeGrenadeSlot].GetPrefab();
    }

    // Adds a grenade prefab to the inventory in the first available slot or overrides the active slot if full
    public void AddGrenade(GameObject newGrenade)
    {
        bool emptySlotFound = false;
        for (int i = 0; i <= 3; ++i)
        {
            if (_grenadeInventory[i].empty)
            {
                _grenadeInventory[i].Init(newGrenade);
                emptySlotFound = true;
                break;
            }
        }

        if (!emptySlotFound)
        {
            _grenadeInventory[_activeGrenadeSlot].Init(newGrenade);
        }
    }

    // Removes the grenade from the inventory slot and returns a prefab reference or null if unable
    public GameObject RemoveGrenade(int slotID)
    {
        if (slotID >= 0 && slotID <= 3 && !_grenadeInventory[slotID].empty)
        {
            GameObject removedGrenade = _grenadeInventory[slotID].GetPrefab();
            _grenadeInventory[slotID].Clear();
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
        return RemoveGrenade(_activeGrenadeSlot);
    }

    // Attempts to throws the actively selected grenade
    public bool ThrowGrenade(Vector3 position, Quaternion rotation, Vector3 direction)
    {
        if (_grenadeInventory[_activeGrenadeSlot].TryThrow())
        {
            GameObject grenade = Instantiate(_grenadeInventory[_activeGrenadeSlot].GetPrefab(), position, rotation);
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

public class GrenadeItem
{
    private GameObject _prefab;
    private float _timer;
    private int _count;
    public bool empty = true;

    // set the grenade item to the provided grenade type
    public void Init(GameObject prefab)
    {
        _prefab = prefab;
        _timer = Cooldown();
        _count = MaxGrenades();
        empty = false;
    }

    // clear the grenade item to default values
    public void Clear()
    {
        _prefab = null;
        _timer = 0f;
        _count = 0;
        empty = true;
    }

    // get the prefab for this grenade type
    public GameObject GetPrefab()
    {
        return _prefab;
    }
    
    // get the cooldown for the grenade type
    public float Cooldown()
    {
        return _prefab.GetComponent<Grenade_Base>().Cooldown;
    }

    // get the current time remaining for the grenade to replenish 1 ammo
    public float Timer()
    {
        return _timer;
    }
    
    // get the maximum number of grenades for this grenade type
    public int MaxGrenades()
    {
        return _prefab.GetComponent<Grenade_Base>().MaxGrenades;
    }

    // get the current number of grenades possessed for this grenade type
    public int CurrentGrenades()
    {
        return _count;
    }
    
    // Update the items cooldown time and replenish grenades
    public void UpdateItem()
    {
        // check the count against max
        if (_count < MaxGrenades())
        {
            // increment the cooldown timer
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                ++_count;
                _timer = Cooldown();
            }
        }
        else
        {
            _timer = Cooldown();
        }
    }

    // Checks if the actively selected grenade has enough ammo to be thrown and reduces the ammo is there is enough
    public bool TryThrow()
    {
        Debug.Log("Grenade Count: " + _count + ", Grenade Timer: "  +_timer);
        if (CurrentGrenades() > 0)
        {
            --_count;
            return true;
        }
        else
        {
            return false;
        }
    }
}
