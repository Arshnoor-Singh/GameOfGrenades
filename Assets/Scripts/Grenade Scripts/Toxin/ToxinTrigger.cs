using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxinTrigger : MonoBehaviour
{
    public float duration = 10f;

    private float time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time >= duration)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //EffectThePlayer
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //StopEffectingThePlayer
        }
    }
}
