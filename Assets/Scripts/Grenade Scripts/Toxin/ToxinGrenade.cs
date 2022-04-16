using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxinGrenade : MonoBehaviour
{
    public GameObject toxinEffect;
    private Vector3 spawnDir = new Vector3(0, 1, 0);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //DEBUG
        if (Input.GetKeyDown(KeyCode.G))
        {
            Explode();
        }
    }
    private void OnCollisionEnter(Collision other)
    {
       if(other.gameObject.tag != "Player")
        {
            Explode();
        }

        
    }

    void Explode()
    {
        Instantiate(toxinEffect, transform.position, Quaternion.Euler(spawnDir));
        GameObject.Destroy(gameObject);
    }
}
