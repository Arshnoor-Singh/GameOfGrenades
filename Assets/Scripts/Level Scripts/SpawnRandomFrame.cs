using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRandomFrame : MonoBehaviour
{
    public GameObject CentralPivot; // Public prefab object which represents the central tower
    public GameObject[] Frame; // Public array of prefabs which contains the different kinds of frames that act as a sample space through which only 4 will spawn
    private GameObject FrameTemp;// Variable used to run instantiate the frames and parent them to the pivot. Might remove this later since the spinning mechanic might undergo change
    public int FrameDistance; // Distance from the frames body to the center of Pivot
    public float LevelScale = 1;
    //Function that returns random frame game objects
    public GameObject RandomFrame()
    {
        int n = Random.Range(0, Frame.Length);
        return Frame[n];
    }

    public void InstantiatePivot()
    {
        FrameTemp = Instantiate(CentralPivot, transform.position, Quaternion.identity);
        FrameTemp.transform.localScale = new Vector3 (1*LevelScale, 1*LevelScale, 1*LevelScale);
    }

    //Funtion the accepts game objects as one input, and a integer to have thi funtion run in a for loop
    public void InstantiateFrame(GameObject FrameID, int i)
    {
        FrameTemp = Instantiate(FrameID, transform.position, transform.rotation*Quaternion.Euler(0f,i*90f,0f), CentralPivot.transform.parent); // Spawning frame around the central pivot
        //FrameLoc.transform.parent = CentralPivot.transform; // Making the spawned frames the children of the central pivot
        FrameTemp.transform.localScale = new Vector3(1*LevelScale, 1*LevelScale, 1*LevelScale);

    }
    // Start is called before the first frame update
    void Start()
    {
        InstantiatePivot();

        for (int i = 0; i <= 3; i++)
        {
            InstantiateFrame(RandomFrame(), i); // instantiatinig the frame 4 times around the centeral pivot.
        }
                          
     }
   

    // Update is called once per frame
    void Update()
    {
        
    }
}
