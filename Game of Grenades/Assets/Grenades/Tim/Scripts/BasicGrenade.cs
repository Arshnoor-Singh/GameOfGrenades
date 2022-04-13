using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGrenade : MonoBehaviour
{
    public GameObject ExplosionTrigger;
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
            Exlode();
        }
    }

    void Exlode()
    {
        Debug.Log("BOOOOOOOOOM");
        Instantiate(ExplosionTrigger, transform.position, Quaternion.identity);
        CanExplode = false;
    }
}
