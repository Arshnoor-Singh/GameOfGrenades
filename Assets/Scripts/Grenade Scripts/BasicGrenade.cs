using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGrenade : MonoBehaviour
{
    //public GameObject ExplosionTrigger;
    public GameObject ExplosionEffect;
    public float radious = 5f;
    public float force = 1000f;
    public int damage = 0;

    private Grenade_Base GB;
    private bool CanExplode = true;
    private CharacterController CC;
    // Start is called before the first frame update
    void Start()
    {
        GB = transform.GetComponent<Grenade_Base>();
    }

    // Update is called once per frame
    void Update()
    {
        if(GB.CookingTime <= 0 && CanExplode)
        {
            Explode();
        }

        //DEBUG
        if(Input.GetKeyDown(KeyCode.G))
        {
            Explode();
        }
    }

    void Explode()
    {
        Debug.Log("BOOOOOOOOOM");
        Instantiate(ExplosionEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radious);
        foreach (Collider affectedObjects in colliders)
        {
            Rigidbody rb = affectedObjects.GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radious);
            }

            if(affectedObjects.tag == "Player")
            {
                
            }
        }

        Destroy(gameObject);
    }
}
