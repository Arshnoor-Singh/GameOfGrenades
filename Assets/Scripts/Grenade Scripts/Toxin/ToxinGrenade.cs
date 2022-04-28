using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxinGrenade : MonoBehaviour
{
    public GameObject toxinEffect;
    private Vector3 spawnDir = new Vector3(0, 1, 0);
    public int bounces = 2;
    private Grenade_Base GB;
    private bool CanExplode = true;

    // Start is called before the first frame update
    void Start()
    {
        GB = transform.GetComponent<Grenade_Base>();
    }

    // Update is called once per frame
    void Update()
    {
        //DEBUG
        if (Input.GetKeyDown(KeyCode.G))
        {
            Explode();
        }

        if (GB.CookingTime <= 0 && CanExplode)
        {
            Explode();
        }
    }
    //private void OnCollisionEnter(Collision other)
    //{
    //   if(other.gameObject.tag != "Player")
    //    {
    //        Explode();
    //    }
    //}

    void Explode()
    {
        GameObject SpawnedTrigger = Instantiate(toxinEffect, transform.position, Quaternion.Euler(spawnDir));
        SpawnedTrigger.GetComponent<ToxinTrigger>().playerID = GB.GrenadeOwner;
        GameObject.Destroy(gameObject);
        CanExplode = false;
    }
}
