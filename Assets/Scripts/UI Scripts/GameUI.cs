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

    public int currentLife;
    int selectedWeapon;
    int teamScore;
    int enemyScore;

    bool isPaused = false;
    public Text timer;
    Text teamScoreTxt;
    Text enemyScoreTxt;
    UIControls uic;
    public GameObject qm;
    GameObject yesQ;
    GameObject noQ;

    FragPartyInputs _input;
    ScoreBoard score;
    int playerid;
    FragPartyCharacter playerhealth;

    // Start is called before the first frame update
    void Start()
    {
        yesQ = GameObject.Find("YES");
        noQ = GameObject.Find("NO");
        teamScore = 0;
        enemyScore = 0;
        teamScoreTxt = GameObject.Find("TW text").gameObject.GetComponent<Text>();
        enemyScoreTxt = GameObject.Find("EW text").gameObject.GetComponent<Text>();
        _input = FindObjectOfType<FragPartyInputs>();
        score = FindObjectOfType<ScoreBoard>();
        playerid = transform.parent.GetComponentInChildren<FragPartyController>().PlayerID;
        playerhealth = transform.parent.GetComponentInChildren<FragPartyCharacter>();

        if (lifeBarBack != null)
        {
            lifeBarGreen.rectTransform.sizeDelta = new Vector2(lifeBarBack.rectTransform.sizeDelta.x, lifeBarBack.rectTransform.sizeDelta.y);
        }

        selectedWeapon = 0;

        currentLife = playerhealth._currentHealth; //MAX_LIFE;

        weaponsList.Add(weapon0);
        weaponsList.Add(weapon1);
        weaponsList.Add(weapon2);
        weaponsList.Add(weapon3);

        //uic = this.GetComponent<UIControls>();
        //if(uic == null)
        {
            uic = FindObjectOfType<UIControls>(); //new UIControls();
        }
        uic.qm.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerid == 0 || playerid == 1)
            enemyScoreTxt.text = score.Team_B_Score.ToString(); //enemyScore.ToString();
            teamScoreTxt.text = score.Team_A_Score.ToString(); //teamScore.ToString();

        if (playerid == 2 || playerid == 3)
            enemyScoreTxt.text = score.Team_A_Score.ToString(); //enemyScore.ToString();
            teamScoreTxt.text = score.Team_B_Score.ToString(); //teamScore.ToString();

        if (uic.p.activeSelf && !uic.qm.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Keypad2) || _input.UINavigateDown)
            {
                Debug.Log("nav down pressed");
                uic.moveUp();
                _input.UINavigateDown = false;
            }

            if (Input.GetKeyDown(KeyCode.Keypad8) || _input.UINavigateUp)
            {
                uic.moveDown();
                _input.UINavigateUp = false;
            }
        }

        else if(uic.p.activeSelf && qm.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Keypad6) || _input.UINavigateRight)
            {
                yesQ.transform.Find("Yes Glow").gameObject.SetActive(false);
                noQ.transform.Find("No Glow").gameObject.SetActive(true);
                _input.UINavigateRight = false;
            }

            else if (Input.GetKeyDown(KeyCode.Keypad4) || _input.UINavigateLeft)
            {
                yesQ.transform.Find("Yes Glow").gameObject.SetActive(true);
                noQ.transform.Find("No Glow").gameObject.SetActive(false);
                _input.UINavigateLeft = false;
            }

            else if (Input.GetKeyDown(KeyCode.A) && noQ.transform.Find("No Glow").gameObject.activeSelf)
            {
                uic.closeQuitMenu();
            }
        }

        else
        {
            //Time
            //matchTime = matchTime - Time.deltaTime;
            //int minutes = Mathf.FloorToInt(matchTime / 60);
            //int seconds = Mathf.FloorToInt(matchTime % 60);

            //if (minutes < 10 && seconds < 10)
            //{
            //    timer.text = "0" + minutes.ToString() + ":0" + seconds.ToString();
            //}
            //else if (minutes < 10 && seconds >= 10)
            //{
            //    timer.text = "0" + minutes.ToString() + ":" + seconds.ToString();
            //}
            //else if (minutes >= 10 && seconds < 10)
            //{
            //    timer.text = minutes.ToString() + ":0" + seconds.ToString();
            //}
            //else if (minutes >= 10 && seconds >= 10)
            //{
            //    timer.text = minutes.ToString() + ":" + seconds.ToString();
            //}

            //Change life
            if (Input.GetKeyDown(KeyCode.Space) || _input.UISelect)
            {
                //changeLifeBar(5);
                _input.UISelect = false;
            }

            //Change weapons
            else if (Input.GetKeyDown(KeyCode.Keypad8) || _input.UINavigateUp)
            {
                changeWeapon(0);
                _input.UINavigateUp = false;
            }

            else if (Input.GetKeyDown(KeyCode.Keypad6) || _input.UINavigateRight)
            {
                changeWeapon(2);
                _input.UINavigateRight = false;
            }

            else if (Input.GetKeyDown(KeyCode.Keypad2) || _input.UINavigateDown)
            {
                changeWeapon(3);
                _input.UINavigateDown = false;
            }

            else if (Input.GetKeyDown(KeyCode.Keypad4) || _input.UINavigateLeft)
            {
                changeWeapon(1);
                _input.UINavigateLeft = false;
            }

            changeLifeBar(currentLife);
        }
        
        //Activate the pause menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //pauseMenu();
        }

    }

    public void changeLifeBar(int CurrentHealth)
    {
        //currentLife = currentLife - damage;
        float newLife = (currentLife * lifeBarBack.rectTransform.sizeDelta.x) / MAX_LIFE;
        lifeBarGreen.rectTransform.sizeDelta = new Vector2(newLife, lifeBarBack.rectTransform.sizeDelta.y);
        //Debug.Log(currentLife);
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
        //uic.p.SetActive(!uic.p.activeSelf);
        //uic.resetMenuCounter();
    }
}
