using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaGrenade : MonoBehaviour
{
    public GameObject smallBanana;
    public int countBanana = 6;
    public Vector3 SpawnRange;
    public float maxRangeX = 5;
    public float maxRangeZ = 5;
    public float distanceToCenter = 1f;

    public GameObject ExplosionEffect;
    public float radious = 5f;
    public float force = 1000f;
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
            //Instantiate(gameObject, transform.position, Quaternion.identity);
            Explode();
        }

        if (GB.CookingTime <= 0 && CanExplode)
        {
            Explode();
        }
    }

    void Explode()
    {
        Vector3 curentPos = transform.position;
        //SpawnBananas
        for (int i = 0; i <= countBanana; i++)
        {
            float randX = Random.Range(-maxRangeX, maxRangeX);
            float randZ = Random.Range(-maxRangeZ, maxRangeZ);
            Vector3 SpawnPos = new Vector3(curentPos.x + randX, curentPos.y + distanceToCenter, curentPos.z + randZ);
            Instantiate(smallBanana, SpawnPos, Quaternion.identity);
        }

        //Explode
        Debug.Log("BOOOOOOOOOM");
        Instantiate(ExplosionEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radious);
        foreach (Collider affectedObjects in colliders)
        {
            Rigidbody rb = affectedObjects.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radious);
            }

            if (affectedObjects.tag == "Player")
            {
                //Deal damage
            }
        }
        CanExplode = false;

        Destroy(gameObject);


    }
}
