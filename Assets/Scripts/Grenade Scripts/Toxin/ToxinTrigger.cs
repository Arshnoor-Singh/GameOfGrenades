using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxinTrigger : MonoBehaviour
{
    public float duration = 10f;
    public float DamageCD;
    private bool PlayerIsIn = false;
    private float Lifetime;
    private float PlayerTimer;
    private FragPartyCharacter FPC;
    public int playerID;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Lifetime += Time.deltaTime;
        if(Lifetime >= duration)
        {
            Destroy(gameObject);
        }

        if(PlayerIsIn)
        {
            PlayerTimer += Time.deltaTime;
        }

        if(PlayerTimer>= DamageCD)
        {
            FPC.Damage(20, playerID);
            PlayerTimer = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            FPC = other.GetComponent<FragPartyCharacter>();
            //EffectThePlayer
            PlayerIsIn = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //StopEffectingThePlayer
            PlayerTimer = 0;
            PlayerIsIn = false;
        }
    }
}
