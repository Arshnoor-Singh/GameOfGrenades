using UnityEngine;

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
        return empty ? 0 : _prefab.GetComponent<Grenade_Base>().Cooldown;
    }

    // get the current time remaining for the grenade to replenish 1 ammo
    public float Timer()
    {
        return empty ? 0 : _timer;
    }
    
    // get the maximum number of grenades for this grenade type
    public int MaxGrenades()
    {
        return empty ? 0 : _prefab.GetComponent<Grenade_Base>().MaxGrenades;
    }

    // get the current number of grenades possessed for this grenade type
    public int CurrentGrenades()
    {
        return empty ? 0 : _count;
    }
    
    // Update the items cooldown time and replenish grenades
    public void UpdateItem()
    {
        // Do nothing if slot is empty
        if (empty) return;
        
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
        if (!empty && CurrentGrenades() > 0)
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