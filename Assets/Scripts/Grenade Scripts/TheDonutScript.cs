/*
 * Authors: José Javier Ortiz Vega
 * Date Created:4/18/2022
 * Date Edited: 4/23/2022
 * Description: Donut Script 
 *              Can be modified in the window editor
 *              
 *              ******************************
 *              ****         DONE         ****
 *              ******************************
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]public class TheDonutScript : MonoBehaviour
{
    //private objects
    private FragPartyController playerController;
    private SlowScript slowingScript;
    private Grenade_Base GB;

    [Header("The donut effects")]
    //effects
    [SerializeField] private GameObject _collisionEffect = null; //_theDonut.GetComponent<TripMineBaseScript>();
    [SerializeField] private GameObject _CollectOrExplodeEffect = null;

    [Header("The donut")]
    //audio clips
    [SerializeField] private AudioClip _collisionSound;
    [SerializeField] private AudioClip _explosionSound;
    [SerializeField] private AudioClip _collectionSound;

    [Header("The donut settings")]
    //base settings
    [Range(0f, 1f)] public float slowAmount = 0.4f;
    [Range(0f, 20f)] public float slowTime = 5f;
    [Range(0, 100)] public int healthToRestore = 20;

    //class privates
    private Grenade_Base _baseScript;
    private AudioSource _audioSource;
    private Vector3 _spawnDir = new Vector3(0, 1, 0);
    private bool startTimer = false;

    //testing variables
    [HideInInspector] public bool CanExplode = true;

    private void Start()
    {
        _baseScript = transform.GetComponent<Grenade_Base>();
        _audioSource = GetComponent<AudioSource>();
        GB = transform.GetComponentInParent<Grenade_Base>();

        //              Debug
        //************************************

        //************************************
        //          End of Debug
    }

    public void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.transform.GetComponent<FragPartyCharacter>().Damage(10, GB.GrenadeOwner);
            StartCoroutine(destroyDonut());
        }
        else
            StartCoroutine(destroyDonut());
    }

    IEnumerator destroyDonut()
    {
        yield return new WaitForSeconds(GB.CookingTime);
        Destroy(gameObject);
    }
}
