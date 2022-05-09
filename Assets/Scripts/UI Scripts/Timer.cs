using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public GameObject TimerObject;
    public float matchTime;
    public Text timer;
    UIControls menu;

    public bool TimerStart = true;

    private void Awake()
    {
        TimerObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        menu = GetComponent<UIControls>();
    }

    // Update is called once per frame
    void Update()
    {
        if (matchTime < 1f)
            TimerStart = false;

        if (!menu.p.activeSelf && TimerStart)
        {
            Debug.Log("timer started");
            matchTime = matchTime - Time.deltaTime;
            int minutes = Mathf.FloorToInt(matchTime / 60);
            int seconds = Mathf.FloorToInt(matchTime % 60);

            if (minutes < 10 && seconds < 10)
            {
                timer.text = "0" + minutes.ToString() + ":0" + seconds.ToString();
            }
            else if (minutes < 10 && seconds >= 10)
            {
                timer.text = "0" + minutes.ToString() + ":" + seconds.ToString();
            }
            else if (minutes >= 10 && seconds < 10)
            {
                timer.text = minutes.ToString() + ":0" + seconds.ToString();
            }
            else if (minutes >= 10 && seconds >= 10)
            {
                timer.text = minutes.ToString() + ":" + seconds.ToString();
            }
        }
    }
}
