using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FragParty
{
    public class GrenadeInventory : MonoBehaviour
    {
        [SerializeField, Range(0, 3)] private int _activeGrenadeSlot = 0;
        [SerializeField] private GameObject[] _grenadeInventory = new GameObject[4];

        // change the active inventory slot to the index provided
        public void ChangeSlot(int slotID)
        {
            
        }

        // cycle to the next inventory slot
        public void NextSlot()
        {
            
        }

        // cycle to the previous inventory slot
        public void PrevSlot()
        {
            
        }
        
        // get the prefab of the active inventory slot
        public void GetGrenade()
        {
            
        }
    }

    public class GrenadeItem
    {
        public int quantity { get { return _quantity; } }
        public float activeTimer { get { return _activeTimer; } }

        private int _quantity;
        private int _maxQuantity;
        private float _maxTimer;
        private float _activeTimer;

        public void DecrementTimer(float time)
        {
            _activeTimer -= time; // decrement the timer
            _activeTimer = Mathf.Round(_activeTimer * 1000f) / 1000f; // round the timer to three places
        }

        public void CheckReplenish()
        {
            if (_activeTimer >= 0 && _quantity < _maxQuantity)
            {
                ++_quantity; // increase grenade quantity by 1
                _activeTimer = _maxTimer; // reset the replenish timer
            }
        }
    }
}
