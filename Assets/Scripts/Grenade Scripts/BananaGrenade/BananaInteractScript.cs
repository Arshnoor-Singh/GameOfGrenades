using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaInteractScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //Affest the player
            other.GetComponent<FragPartyController>().ForceSlide();
        }
    }
}
