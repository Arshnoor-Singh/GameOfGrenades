/*
 * Authors: José Javier Ortiz Vega
 * Date Created:4/10/2022
 * Date Edited: 4/14/2022
 * Description: This is the base for what will become the trip mine grenade 
 *              Can be modified in the editor
 *              
 *              ******************************
 *              ****         EDIT         ****
 *              ******************************
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TripMineBaseScript : MonoBehaviour
{
    //calling header
    [Header("Trip mine grenade base settings")]

    //Trip mine grenade settings
    [Tooltip("Enter the prephab that this script will spawn")]
    [SerializeField] private GameObject _grenadePrephab = null;
    [Tooltip("Name of the grenade")]
    [SerializeField] public string tripMineName = null; //stores the name of the grenade
    [Tooltip("Name of the grenade")]
    [SerializeField] [Multiline] [TextArea] public string tripMineDescription = null; //description of the grenade
    [Tooltip("Stack that is available for the player in their inventory")]
    [SerializeField] [Range(1, 10)] public int stack = 4; //stack
    [Tooltip("This accounts for the starting velocity of the grenade")]
    [SerializeField][Range(0f, 10000f)] private float _grenadeVelocity = 0f; //grenade velocity
    [Tooltip("Edit the cooking time of the grenade when it gets stuck or when it's collision box sees an enemy player")]
    [SerializeField][Range(0f, 10000f)] private float _cookingTime = 0f; //edit how many times the grenade can bounce before it sticks to something
    [Tooltip("Has constant velocity")]
    [SerializeField] public bool constantVelocity = false; //allow the tripmine to have constant velocity until it hits something
    [Tooltip("Allows the grenade to bounce before sticking to surfaces")]
    [SerializeField] public bool canBounce = false; //allow the grenade to bounce before sticking to surfaces
    [Tooltip("Edit the bounce of the grenade")]
    [SerializeField][Range(0, 15)] public int grenadeBounce = 0; //edit how many times the grenade can bounce before it sticks to something
    [Tooltip("Slow players when they get with by a % of their movement speed")]
    [SerializeField][Range(0f, 1f)] public float slowPlayer = 0f;
    [Tooltip("Sticks to playes, will ignore bounce if it hits players")]
    [SerializeField] public bool sticksToPlayers = true; //allow the grenade to stick to players
    [Tooltip("Explodes with cooking time after it's sticks to something")]
    [SerializeField] private bool _explodesWithCookingTime = true; //explodes wih cooking time

    //class privates
    [HideInInspector] public bool hasCooking = false; //allow the grenade to have cooking while holding it
    [HideInInspector] public bool cookingStarted = false; //starts cooking when stuck in the ground

    //class returns
    [HideInInspector] public bool _isAboutToExplode = false; //makes sure that if grenade is about to explode, another script can see this

    //on collision stick or check
    private void OnCollisionEnter(Collision collision)
    {
        _bounceCheck(collision);
    }
    //bounce check
    private void _bounceCheck(Collision col)
    {
        //if ((!sticksToPlayers && grenadeBounce == 0) && (col.gameObject.tag == "Player"))
        if ((sticksToPlayers && col.gameObject.tag == "Player") || (grenadeBounce == 0)) //fix this-------------
        {
            this._grenadePrephab.GetComponent<Rigidbody>().isKinematic = true;
            grenadeEffects();
        }
        else if (((grenadeBounce == 0) && (!canBounce)) && ((!sticksToPlayers) && (col.gameObject.tag == "Player")))
        {
            _bounceTheGameObject(true);
        }
        else if (canBounce)
        {
            _bounceTheGameObject(false);
        }
    }

    //bounce the grenade
    private void _bounceTheGameObject(bool additionalBounce)
    {
        if (!additionalBounce)
            grenadeBounce--;
        else if (additionalBounce)
            grenadeBounce = grenadeBounce + 2;

        Debug.Log("Bounces left: " + grenadeBounce);
    }

    //grenade effects go here
    private void grenadeEffects()
    {
        if((_cookingTime != 0) && (_explodesWithCookingTime))
        {
            _cookingTime -= Time.deltaTime;
        }
        else if ((_cookingTime == 0) && (_explodesWithCookingTime))
        {
            _isAboutToExplode = true;
            Destroy(gameObject); //add effects here
        }
    }

    //script return events for other scripts to utilize well---------------------------------------------------
    //launches the grenade from the movement script
    public void Launch(Vector3 dir)
    {
        transform.GetComponent<Rigidbody>().AddForce(dir * _grenadeVelocity);
    }

    //returns the trip mine name
    public string getTripMineName()
    {
        return tripMineName;
    }

    //returns the trip mine description
    public string getTripMineDescription()
    {
        return tripMineDescription;
    }

    //tells other scripts that the grenade is about to explode and to do all of it's vfx in time
    public bool explosionImminent()
    {
        if (_isAboutToExplode)
            return true;
        else
            return false;
    }

    //return constant velocity 
    public bool returnConstantVelocity()
    {
        return constantVelocity;
    }
}
