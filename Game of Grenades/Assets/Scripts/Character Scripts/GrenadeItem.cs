using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeItem : MonoBehaviour
{
    [SerializeField] private GameObject grenadePrefab; // Reference to the grenade prefab to be instantiated for this grenade
    // [SerializeField] private Grenade_Base grenadeBase; // reference to the type of grenade for this grenade item
    [SerializeField] private int _count; // The number of this grenade the player has
    [SerializeField] private float _replenishTime; // The current timer for grenades replenishing
    
    // Initialize the grenade item
    public void Init()
    {
        // _count = grenadeBase.MaxGrenades; // Set current count of grenades to the maximum capacity
        _replenishTime = 0f; // Set the replenish timer for this grenade to 0;
    }

    // Reduces the count of the grenade type
    public bool RemoveGrenade(int amount, bool forceSubtract = false)
    {
        if (_count >= amount) {
            _count -= amount;
            return true; // returns true when count is high enough to reduce by requested quantity
        } else if (_count < amount && forceSubtract) {
            _count = 0; // when not enough grenades to subtract, but forced operation anyways set to 0
            return true;
        } else {
            return false;
        }
        
        
    }

    // Add to the count for this grenade type
    public void AddGrenade(int amount)
    {
        // _count = Mathf.Min(_count + amount, grenadeBase.MaxGrenades); // Add grenades to inventory, but not more than capacity
    }

    // Replenishes the coount of this grenade in inventory
    public void Replenish()
    {
        // // If there is room in the inventory for more of this grenade
        // if (_count < grenadeBase.MaxGrenades) {
        //     _replenishTime += Time.deltaTime;   // Increment the timer by the frame time
        //     
        //     // If the timer has reached the cooldown time
        //     if (_replenishTime >= grenadeBase.Cooldown) {
        //         AddGrenade(1); // Add 1 to the grenade count
        //         _replenishTime -= grenadeBase.Cooldown; // Reduce the replenish time by the Cooldown amount to leave any extra time accrued 
        //     }
        // } 
        //
        // // If this grenade type is at full capacity and the replenish time is not reset to 0, reset it 
        // if (_count >= grenadeBase.MaxGrenades &&  _replenishTime != 0f) {
        //     _replenishTime = 0f;
        // }
        
    }
}
