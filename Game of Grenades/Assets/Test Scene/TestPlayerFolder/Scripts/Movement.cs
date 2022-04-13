/*
 * "Simple" :) Moderator Movement Scrip
 * José Javier Ortiz Vega
 * V1.0
 */ 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[SelectionBase]
public class Movement : MonoBehaviour
{
    //Important variables to be set
    [Header("Simple Floating Movement")]
    [Tooltip("Add the camera that the player will use to look around the scene")]
    [SerializeField] private Transform Camera = null;
    [Tooltip("Add the game object to the requirement")]
    [SerializeField] private GameObject BaseObject = null;

    //Mouse Settings
    [Header("Mouse Look Settings")]
    [Space(15)]
    [Range(1f,10f)]
    [Tooltip("Edit the Mouse Sensitivity")]
    [SerializeField] public float MouseSensitivity = 3.5f; //mouse sensitivity
    [Range(-90f,90f)]
    [Tooltip("Starting Location of camera vision")]
    [SerializeField] private float CameraPitch = 0.0f; //camera pitch
    [Tooltip("Set the view of the camera to be of inverted view")]
    [SerializeField] public bool InvertedPitchView = false; //inverted camera pitch
    [Tooltip("Lock the cursor to the center of the screen")]
    [SerializeField] private bool LockCursor = true; //lock cursor to the middle of the screen
    [Tooltip("Change visibility of the cursor from true to false")]
    [SerializeField] private bool SeeCursor = false; //lock cursor to the middle of the screen

    //Movement Settings
    [Header("Movement Settings")]
    [Space(15)]
    [Range(0f,50f)]
    [Tooltip("Edit the base speed of the object")]
    [SerializeField] private float MovementSpeed = 10f;
    [Range(0f, 0.5f)]
    [Tooltip("Edit the smoothness of the movement from sharp movement to smooth movement")]
    [SerializeField] private float SmoothMovement = 0.3f;

    //class privates
    private CharacterController PlayerController = null;

    //Runs at runtime, use to allways verify the location of the camera to be at the desired start location
    void Start()
    {
        Debug.Log("Starting Position: " + Camera.position);
        PlayerController = GetComponent<CharacterController>();
        cursorManipulation();
    }
    
    //Updates at runtime, use to update the movement of the spectator camera and mesh
    void Update()
    {
        UpdateMouseMovement();
        UpdateMovement();
    }

    //Mouse movement=========================
    //Updates Player Rotation Mouse Movement
    void UpdateMouseMovement()
    {
        //variable declaration and Initialization
        Vector2 MouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        //rotate and move the camera
        UpdatePitchView(MouseDelta);
        transform.Rotate(Vector3.up * MouseDelta.x * MouseSensitivity);
    }

    //Increase the camera pitch value to look up or down
    void UpdatePitchView(Vector2 MouseDelta)
    {
        if (InvertedPitchView)
        {
            CameraPitch += MouseDelta.y * MouseSensitivity;
            CameraPitch = Mathf.Clamp(CameraPitch, -90f, 90f);
        }
        else
        {
            CameraPitch -= MouseDelta.y * MouseSensitivity;
            CameraPitch = Mathf.Clamp(CameraPitch, -90f, 90f);
        }

        Camera.localEulerAngles = Vector3.right * CameraPitch;
    }

    //manipulate the cursor to be locked in the screen on game start (Change Later)
    void cursorManipulation()
    {
        if (LockCursor)
            Cursor.lockState = CursorLockMode.Locked;

        if (SeeCursor)
            Cursor.visible = true;
        else
            Cursor.visible = false;
    }

    //Object movement=========================
    //Update the movement of the object
    void UpdateMovement()
    {
        Vector2 InDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        InDirection.Normalize();
        Vector3 Velocity = (transform.forward * InDirection.y + transform.right * InDirection.x) * MovementSpeed;
        PlayerController.Move(Velocity * Time.deltaTime);
    }
}
