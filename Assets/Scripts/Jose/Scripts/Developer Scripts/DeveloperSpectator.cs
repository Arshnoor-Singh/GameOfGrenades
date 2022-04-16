/*
 * Authors: José Javier Ortiz Vega
 * Date Created:4/10/2022
 * Date Edited: 4/14/2022
 * Description: Simple movement system FOR DEVELOPER USE ONLY
 *              Can be modified in the editor
 *              
 *              ******************************
 *              ****          FIX         ****
 *              ******************************
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[SelectionBase]
[RequireComponent(typeof(SpawnerScript))]
public class DeveloperSpectator : MonoBehaviour
{
    //Header is called
    [Header("Developer Spectator Settings")]

    //movement settings
    [Tooltip("Change the movement speed of the development camera")]
    [SerializeField] [Range(0f,100f)] public float movementSpeed = 1f; //change the movement speed
    [Tooltip("change the sensitivity of the horizontal view")]
    [SerializeField][Range(0f, 100f)] public float speedHorizontal = 2f; //horizontal view speed
    [Tooltip("change the sensitivity of the vertical view")]
    [SerializeField][Range(0f, 100f)] public float speedVertical = 2f; //vertical view speed

    //cursor variable settings
    [Tooltip("Lock the cursor to the center of the screen")]
    [SerializeField] private bool _lockCursor = true; //lock cursor to the middle of the screen
    [Tooltip("Change visibility of the cursor from true to false")]
    [SerializeField] private bool _seeCursor = false; //lock cursor to the middle of the screen

    //class privates
    private float yaw;
    private float pitch;

    //starts on frame 1
    void Start()
    {
        cursorManipulation();
    }

    //updates every frame
    void Update()
    {
        cameraMovement();
    }

    //moves the camera around the scene
    private void cameraMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        yaw += speedHorizontal * Input.GetAxis("Mouse X");
        pitch -= speedVertical * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw);
        transform.position += transform.TransformDirection(Vector3.forward) * vertical + transform.TransformDirection(Vector3.right) * horizontal;
    }

    //manipulate the cursor to be locked in the screen on game start (Change Later)
    void cursorManipulation()
    {
        if (_lockCursor)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;

        if (_seeCursor)
            Cursor.visible = true;
        else
            Cursor.visible = false;
    }
}
