/*
 * Authors: Tim José Javier Ortiz Vega
 * Date Created:4/18/2022
 * Date Edited: 4/21/2022
 * Description: This will allow to store grenade info 
 *              Can be modified in the window editor
 *              
 *              ******************************
 *              ****         DONE         ****
 *              ******************************
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGrenade : MonoBehaviour
{
    //public GameObject ExplosionTrigger;
    public GameObject ExplosionEffect;
    [Range(0f, 50f)] public float radious = 5f;
    [Range(0f, 200f)] public float force = 1000f;
    [Range(0f, 100f)] public int damage = 0;

    private Grenade_Base GB;
    private bool CanExplode = true;
    private CharacterController CC;

    //sounds
    private AudioSource grenadeAudioSource;
    public AudioClip grenadeAudioClip;

    // Start is called before the first frame update
    void Start()
    {
        grenadeAudioSource.GetComponent<AudioSource>().clip = grenadeAudioClip;
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
        grenadeAudioSource.Play();

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
                affectedObjects.GetComponent<FragPartyCharacter>().Damage(damage, GB.GrenadeOwner);
            }
        }

        Destroy(gameObject);
    }
}
