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
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //***********DEBUG*************
        if(Input.GetKeyDown(KeyCode.W))
        {
            Launch(DebugDir);
        }
        //*****************************

        if(HasCooking && CookingStarted)
        {
            CookingTime -= Time.deltaTime;
        }
    }

    public void Launch (Vector3 dir)
    {
        transform.GetComponent<Rigidbody>().AddForce(dir * GrenadeVelocity);
    }

    public void StartCooking()
    {
        CookingStarted = true;
    }
}
