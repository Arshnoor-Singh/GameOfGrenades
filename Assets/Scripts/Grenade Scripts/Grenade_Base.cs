/*
 * Authors: Tim and José Javier Ortiz Vega
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

[RequireComponent(typeof(Rigidbody))]
public class Grenade_Base : MonoBehaviour
{
    //debug info
    [Header("Debuging purposes")]
    public Vector3 DebugDir;

    //grenade info
    [Header("Grenade Info")]
    public int GrenadeOwner;
    public string grenadeName = "Grenade name";
    public string grenadeDescription = "Grenade Description";
    public bool isTripMine = false;
    public bool HasCooking = true; //this grenade has cooking
    [Range(1,10)] public int MaxGrenades; //Ammounts of grenades that you can have in the inventory
    [Range(0,50)] public float Cooldown; //Replenishment for the grenade
    [Range(0f, 200f)] public float GrenadeVelocity = 0f; //Grenade velocity
    [Range(0f, 20f)] public float CookingTime = 3f; //cooking time (Trip mines will return 0 in cooking time

    //class privates
    private bool CookingStarted = false;
    [HideInInspector] Transform grenadeSocket;
    [HideInInspector] public bool Thrown = false;

    // Start is called before the first frame update
    void Start()
    {
        //DEBUG
        // CookingStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(HasCooking && CookingStarted)
        {
            CookingTime -= Time.deltaTime;
        }

        //if(Thrown == false)
        //{
        //    transform.position = grenadeSocket.position;
        //    transform.rotation = grenadeSocket.rotation;
        //}
    }

    public string getName()
    {
        return name;
    }

    public string getDescription()
    {
        return grenadeDescription;
    }

    public bool getGrenadeIdentification()
    {
        return isTripMine;
    }

    public void Launch (Vector3 dir)
    {
        transform.GetComponent<Rigidbody>().AddForce(dir * GrenadeVelocity, ForceMode.Impulse);
        CookingStarted = true;
        Thrown = true;
    }

    public void StartCooking()
    {
        CookingStarted = true;
    }

    public void SetGrenadeSocket(Transform socketRef)
    {
        grenadeSocket = socketRef;
    }
}
