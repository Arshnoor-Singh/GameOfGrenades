using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public GameObject PlayerRef;
    public GameObject player;

    float mouseX;
    float mouseY;
    float GamePadX;
    float GamePadY;
    float currYDir = 0, prevYDir = 0;

    // Update is called once per frame
    void Update()
    {
        playerLookAround();
    }

    public void playerLookAround()
    {
        //Mouse
        mouseX = Input.GetAxis("Mouse X");
        mouseY = -(Input.GetAxis("Mouse Y"));

        // Vertical mouse look direction
        currYDir = prevYDir + mouseY;
        currYDir = Mathf.Clamp(currYDir, -20f, 0f);
        //mainCam.transform.Rotate(currYDir - prevYDir, 0, 0);
        prevYDir = currYDir;
        // Horizontal mouse look direction
        PlayerRef.transform.Rotate(Vector3.up * mouseX);
        

        if (Input.GetJoystickNames() != null)
        {
            //Gamepad
            GamePadX = Input.GetAxis("Gamepad X");
            GamePadY = -(Input.GetAxis("Gamepad Y"));

            // Vertical mouse look direction
            currYDir = prevYDir + GamePadY;
            currYDir = Mathf.Clamp(currYDir, -20f, 0f);
            //mainCam.transform.Rotate(currYDir - prevYDir, 0, 0);
            prevYDir = currYDir;
            // Horizontal mouse look direction
            PlayerRef.transform.Rotate(Vector3.up * GamePadX);
        }
    }
}
