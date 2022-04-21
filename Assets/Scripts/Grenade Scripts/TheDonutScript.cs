using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[RequireComponent (typeof(TripMineBaseScript))]
public class TheDonutScript : MonoBehaviour
{
    //calling header
    [Header("The donut settings")]

    //private to class
    [SerializeField] private GameObject _theDonut = null; //to call the script specific items ues the following //_theDonut.GetComponent<TripMineBaseScript>();

    // Update is called once per frame
    void Update()
    {
        slowPlayer();
    }

    //slow players based on the script's 
    private void slowPlayer()
    {
        //...
    }
}
