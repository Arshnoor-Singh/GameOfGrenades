using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBananaScript : MonoBehaviour
{
    public float LiveTime = 15f;
    private float tm;
    private bool WasSpawned = false;
    // Start is called before the first frame update
    void Start()
    {
        WasSpawned = true;
    }

    // Update is called once per frame
    void Update()
    {
        tm += Time.deltaTime;
        if(tm>=LiveTime && WasSpawned)
        {
            Destroy(gameObject);
        }
    }
}
