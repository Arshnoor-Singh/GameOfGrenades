using System.Collections;
using System.Collections.Generic;
using AllIn1VfxToolkit;
using UnityEngine;

public class GrenadeSpawner : MonoBehaviour
{
    [Tooltip("Time in seconds before spawning a new grenade"), Range(1, 120)]
    public float cooldownTimer = 30f;
    private float _timer = 0f;
    private bool _timerActive;
    private bool _grenadeSpawned;

    [Tooltip("The grenade list containing all game grenades")]
    public GrenadeListScriptableObject grenadeList;
    [Tooltip("The target transform where grenades will be spawned")]
    public Transform spawnTarget;
    
    private int _grenadeIndex;
    private GameObject _grenadePrefab;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _timer = 0f;
        _grenadeSpawned = false;
        _timerActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        // if the timer is running, decrement and check if ready to spawn
        if (_timerActive)
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                _timerActive = false;
                SpawnGrenade();
            }
        }
        
        // if no spawned grenade and timer inactive, start the spawn timer
        if (!_grenadeSpawned && !_timerActive)
        {
            _timerActive = true;
            _timer = cooldownTimer;
        }
    }

    void SpawnGrenade()
    {
        // get random grenade from list and display grenade graphic above spawner
        _grenadeIndex = Random.Range(0, grenadeList.list.Length);
        _grenadePrefab = grenadeList.Get(_grenadeIndex);
        
        // Instantiate the grenade on the spawner
        _grenadePrefab = Instantiate(_grenadePrefab, spawnTarget.position, spawnTarget.rotation, spawnTarget); 
        
        // Handling unneeded components
        // Disable rigidbody gravity
        _grenadePrefab.GetComponent<Rigidbody>().useGravity = false;
        // Make rigidbody kinematic
        _grenadePrefab.GetComponent<Rigidbody>().isKinematic = true;
        // Remove colliders
        if (_grenadePrefab.GetComponent<Collider>() != null)
        {
            Collider[] cols = _grenadePrefab.GetComponents<Collider>();
            foreach (var col in cols)
            {
                Destroy(col);
            }
        }
        
        _grenadeSpawned = true;
    }

    public void CollectGrenade(GrenadeInventory playerInventory)
    {
        if (_grenadeSpawned)
        {
            // give grenade to player
            playerInventory.AddGrenade(grenadeList.Get(_grenadeIndex));
            
            // remove grenade graphic from above spawner
            Destroy(_grenadePrefab);
            
            _grenadeSpawned = false;
        }
    }
}
