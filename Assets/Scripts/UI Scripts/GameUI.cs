using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameUI : MonoBehaviour
{
    public Image lifeBarBack, lifeBarGreen;
    public Image weapon0, weapon1, weapon2, weapon3;
    List<Image> weaponsList = new List<Image>();

    const int MAX_LIFE = 100;
    const int MIN_LIFE = 0;
    const int ANIMATION_LENGTH = 1000;

    public float matchTime;
    float counter = 0;
    float animationStart = 0;

    int currentLife;
    int selectedWeapon;
    int teamScore;
    int enemyScore;

    bool isPaused = false;
    public Text timer;
    UIControls uic;
    public GameObject qm;
    GameObject yesQ;
    GameObject noQ;


    // Start is called before the first frame update
    void Start()
    {
        yesQ = GameObject.Find("YES");
        noQ = GameObject.Find("NO");
        if (lifeBarBack != null)
        {
            lifeBarGreen.rectTransform.sizeDelta = new Vector2(lifeBarBack.rectTransform.sizeDelta.x, lifeBarBack.rectTransform.sizeDelta.y);
        }

        selectedWeapon = 0;

        currentLife = MAX_LIFE;

        weaponsList.Add(weapon0);
        weaponsList.Add(weapon1);
        weaponsList.Add(weapon2);
        weaponsList.Add(weapon3);

        uic = this.GetComponent<UIControls>();
        if(uic == null)
        {
            uic = new UIControls();
        }
        qm.gameObject.SetActive(false);
        changeWeapon(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (uic.p.activeSelf && !qm.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                uic.moveUp();
            }

            if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                uic.moveDown();
            }
        }

        else if(uic.p.activeSelf && qm.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                yesQ.transform.Find("Yes Glow").gameObject.SetActive(false);
                noQ.transform.Find("No Glow").gameObject.SetActive(true);
            }

            else if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                yesQ.transform.Find("Yes Glow").gameObject.SetActive(true);
                noQ.transform.Find("No Glow").gameObject.SetActive(false);
            }

            else if (Input.GetKeyDown(KeyCode.A) && noQ.transform.Find("No Glow").gameObject.activeSelf)
            {
                uic.closeQuitMenu();
            }
        }

        else
        {
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

            if (Input.GetKeyDown(KeyCode.Space))
            {
                changeLife(5);
            }

            else if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                changeWeapon(0);
            }

            else if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                changeWeapon(1);
            }

            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                changeWeapon(2);
            }

            else if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                changeWeapon(3);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu();
        }

    }

    void changeLife(int damage)
    {
        currentLife = currentLife - damage;
        float newLife = (currentLife * lifeBarBack.rectTransform.sizeDelta.x) / MAX_LIFE;
        lifeBarGreen.rectTransform.sizeDelta = new Vector2(newLife, lifeBarBack.rectTransform.sizeDelta.y);
        Debug.Log(currentLife);
    }

    void changeWeapon(int theInput)
    {
        if (theInput != selectedWeapon)
        {
            weaponsList[selectedWeapon].GetComponent<Weapon>().changeIsSelected();
            weaponsList[selectedWeapon].GetComponent<Weapon>().Shrink();
            selectedWeapon = theInput;
            weaponsList[selectedWeapon].GetComponent<Weapon>().changeIsSelected();
            weaponsList[selectedWeapon].GetComponent<Weapon>().Grow();
        }
        else
        {
            weaponsList[selectedWeapon].GetComponent<Weapon>().Grow();
        }
        Debug.Log(selectedWeapon);
    }

    void pauseMenu()
    {
        uic.p.SetActive(!uic.p.activeSelf);
        uic.resetMenuCounter();
    }
}
