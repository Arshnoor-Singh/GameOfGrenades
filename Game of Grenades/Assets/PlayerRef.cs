using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRef : MonoBehaviour
{
    public GameObject player;
    //public GameObject model;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = player.transform.rotation;

        //transform.forward = model.transform.forward;


    }
}
