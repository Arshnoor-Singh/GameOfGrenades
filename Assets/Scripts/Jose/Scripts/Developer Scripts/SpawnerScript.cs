/*
 * Authors: José Javier Ortiz Vega
 * Date Created:4/10/2022
 * Date Edited: 4/13/2022
 * Description: Simple object spawner script FOR DEVELOPER USE ONLY
 *              Can be modified in the editor
 *              
 *              ******************************
 *              ****        FINISH        ****
 *              ******************************
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    //calling header
    [Header("Spawner Settings")]

    [Tooltip("GameObject that will be used to spawn other GameObjects in the scene")]
    [SerializeField] private GameObject _objectToSpawn = null;

    //creating basic object
    private DeveloperSpectator _dS; //DeveloperSpecatator object

    //class private
    private bool _hasSomething = false;

    // Start is called before the first frame update
    void Start()
    {
        checker();
    }

    // Update is called once per frame
    void Update()
    {
        if (_hasSomething)
            spawnGameObject();
    }

    //checks if everything is fine
    private void checker ()
    {
        if (_dS != null)
        {
            _hasSomething = true;
        }
        else
        {
            _hasSomething = false;
        }
    }

    //spawn items
    private void spawnGameObject()
    {

    }
}
