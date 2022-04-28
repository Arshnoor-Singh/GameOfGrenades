using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmithGrenadeScript : MonoBehaviour
{
    public GameObject ExplosionEffect;
    public int Damage = 50;
    private Vector3 explosionRotation = new Vector3(0, 0, 0);
    private Grenade_Base GB;
    private bool StartAnim = false;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        GB = GetComponent<Grenade_Base>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GB.Thrown && StartAnim == false)
        {
            // anim.SetTrigger("Rotate");
            StartAnim = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {
            Explode();
            other.transform.GetComponent<FragPartyCharacter>().Damage(Damage,GB.GrenadeOwner);
            other.transform.GetComponent<FragPartyController>().ForceDive();
        }
    }

    private void Explode()
    {
        //Affect the player

        Instantiate(ExplosionEffect, transform.position, Quaternion.Euler(explosionRotation));
        Destroy(gameObject);
    }
}
