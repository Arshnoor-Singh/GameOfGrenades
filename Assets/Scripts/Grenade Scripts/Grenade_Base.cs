using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade_Base : MonoBehaviour
{
    public bool HasCooking = true;
    private bool CookingStarted = false;

    public int MaxGrenades;
    public float Cooldown;

    public float GrenadeVelocity = 0f;
    public float CookingTime = 3f;
    public Vector3 DebugDir;


    Transform grenadeSocket;
    bool Thrown = false;
    // Start is called before the first frame update
    void Start()
    {
        //DEBUG
        CookingStarted = true;
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

    public void Launch (Vector3 dir)
    {
        transform.GetComponent<Rigidbody>().AddForce(dir * GrenadeVelocity, ForceMode.Impulse);
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
