using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent (typeof(TripMineBaseScript))]
public class TheDonutScript : MonoBehaviour
{
    //calling header
    [Header("The donut settings")]

    //private to class
    [Tooltip("Add the prefab for what will become the tripmine grenade")]
    [SerializeField] private GameObject _theDonut = null; //to call the script specific items ues the following //_theDonut.GetComponent<TripMineBaseScript>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
