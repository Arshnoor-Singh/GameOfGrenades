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

/*
 *  ******************************************
 *  ****           INSTRUCTIONS           ****
 *  ******************************************
 * 
 *  Use the followint:
 *  1. Every class that utilizes this script should hare this as a requirement
 *        [RequireComponent(typeof(TripMineBaseScript))]
 *    
 *      Available functions to call for other classes:
 *      1. Launch(Vector3 dir);
 *      2. getTripMineName();
 *      3. getTripMineDescription();
 *      4. explosionImminent();
 *      5. returnConstantVelocity();
 *      6. gotStuckToAPlayer();
 *      7. getCookingTime();
 *      8. countDown();
 *      9. getDamage();
 *      10. getStack();
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
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
    [Tooltip("Trip mine damage")]
    [SerializeField][Range(0f, 10000f)] public float tripMineDamage = 1f; //the damage of the trip mine
    [Tooltip("Stack that is available for the player in their inventory")]
    [SerializeField] [Range(1, 10)] public int stack = 4; //stack
    [Tooltip("This accounts for the starting velocity of the grenade")]
    [SerializeField][Range(0f, 10000f)] private float _grenadeVelocity = 0f; //grenade velocity
    [Tooltip("Edit the cooking time of the grenade when it gets stuck or when it's collision box sees an enemy player")]
    [SerializeField][Range(0f, 10000f)] public float _cookingTime = 0f; //edit how many times the grenade can bounce before it sticks to something
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
    [Tooltip("Explode the grenade on built in explosion settings (not recommended for trip mine grenades)")]
    [SerializeField] private bool _useBuiltInExplosionSystem = false; //use the base grenade effects for this built in simple system

    //class privates
    [HideInInspector] public bool cookingStarted = false; //starts cooking when stuck in the ground
    [HideInInspector] private bool _isStuckToAPlayer = false; //returns true if it has been stuck to a player

    //class returns
    [HideInInspector] public bool _isAboutToExplode = false; //makes sure that if grenade is about to explode, another script can see this

    //updates per frame
    private void Update()
    {
        if(cookingStarted && _useBuiltInExplosionSystem)
            countDown();
    }

    //on collision stick or check
    private void OnCollisionEnter(Collision collision)
    {
        _bounceCheck(collision);
    }
    //bounce check
    private void _bounceCheck(Collision col)
    {
        if ((sticksToPlayers && col.gameObject.tag == "Player") || (grenadeBounce == 0)) //This needs an eye------------------------
        {
            this._grenadePrephab.GetComponent<Rigidbody>().isKinematic = true;
            _isStuckToAPlayer = true;

            if(_useBuiltInExplosionSystem)
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

    //grenade effects go here--------------
    private void grenadeEffects()
    {
        if((_cookingTime > 0) && (_explodesWithCookingTime))
        {
            cookingStarted = true;
        }
        else if ((_cookingTime <= 0) && (_explodesWithCookingTime))
        {
            _isAboutToExplode = true;

            if (_useBuiltInExplosionSystem)
                Destroy(gameObject);
        }
    }

    //script return events for other scripts to utilize well--------------------------------------------------------------
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

    //return the stack allowed for this grenade
    public int getStack()
    {
        return stack;
    }

    //tells other scripts that the grenade is about to explode and to do all of it's vfx in time
    public bool explosionImminent()
    {
        if (_isAboutToExplode)
            return true;
        else
            return false;
    }

    //return constant velocity bool (Change in another script)
    public bool returnConstantVelocity()
    {
        return constantVelocity;
    }

    //returnn if it got stuck to a player
    public bool gotStuckToAPlayer()
    {
        if (_isStuckToAPlayer)
            return true;
        else
            return false;
    }

    //returns cooking time
    public float getCookingTime()
    {
        return _cookingTime;
    }

    //count down to explode
    public void countDown()
    {
        if (cookingStarted)
        {
            _cookingTime -= Time.deltaTime;

            if (_useBuiltInExplosionSystem)
                grenadeEffects();
        }

        Debug.Log("Time left before destruction: " + ((float)Mathf.Round(_cookingTime * 10f) * 0.1f));
    }

    //returns the damage of the trip mine
    public float getDamage()
    {
        return tripMineDamage;
    }
}
