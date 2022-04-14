using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeInventory : MonoBehaviour
{
    [SerializeField, Range(0, 3)] private int _activeGrenadeSlot = 0;
    [SerializeField] private GrenadeItem[] _grenadeInventory = new GrenadeItem[4];


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
