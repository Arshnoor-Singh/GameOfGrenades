using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotRotation : MonoBehaviour
{
    public GameObject CentralPivot;
    public GameObject[] Frame;
    public int FrameRotationSpeed;
    private GameObject FrameLoc;
    public int FrameDistance;
    private float RotationTimer;
    public GameObject RandomFrame()
    {
        int n = Random.Range(0, Frame.Length);
        return Frame[n];
    }
    public void InstantiateFrame(GameObject FrameID, int i)
    {
        FrameLoc = Instantiate(FrameID, transform.position, transform.rotation*Quaternion.Euler(0f,i*90f,0f));
        FrameLoc.transform.parent = CentralPivot.transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i <= 3; i++)
        {
            InstantiateFrame(RandomFrame(), i);
        }
                          
     }
   

    // Update is called once per frame
    void Update()
    {
        CentralPivot.transform.Rotate(Vector3.up * (FrameRotationSpeed * Time.deltaTime));
    }
}
