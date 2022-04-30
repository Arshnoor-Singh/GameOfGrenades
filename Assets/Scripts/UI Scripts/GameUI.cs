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
    public Text teamScoreTxt;
    public Text enemyScoreTxt;
    UIControls uic;
    public GameObject qm;
    GameObject yesQ;
    GameObject noQ;

    public FragPartyInputs _input;
    ScoreBoard score;
    FragPartyController Team_Name;
    FragPartyCharacter playerhealth;

    // Start is called before the first frame update
    void Start()
    {
        yesQ = GameObject.Find("YES");
        noQ = GameObject.Find("NO");
        teamScore = 0;
        enemyScore = 0;
        _input = transform.parent.GetComponentInChildren<FragPartyInputs>();
        score = FindObjectOfType<ScoreBoard>();
        Team_Name = transform.parent.GetComponentInChildren<FragPartyController>();
        playerhealth = transform.parent.GetComponentInChildren<FragPartyCharacter>();

        if (lifeBarBack != null)
        {
            lifeBarGreen.rectTransform.sizeDelta = new Vector2(lifeBarBack.rectTransform.sizeDelta.x, lifeBarBack.rectTransform.sizeDelta.y);
        }

        selectedWeapon = 0;

        currentLife = playerhealth._currentHealth; 

        weaponsList.Add(weapon0);
        weaponsList.Add(weapon1);
        weaponsList.Add(weapon2);
        weaponsList.Add(weapon3);
        
        uic = FindObjectOfType<UIControls>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Team_Name.Team == "Team_A")
        {
            enemyScoreTxt.text = score.Team_B_Score.ToString(); 
            teamScoreTxt.text = score.Team_A_Score.ToString(); 
        }

        if (Team_Name.Team == "Team_B")
        {
            enemyScoreTxt.text = score.Team_A_Score.ToString(); 
            teamScoreTxt.text = score.Team_B_Score.ToString(); 
        }

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

        else
        {
           
            //Change life
            if (Input.GetKeyDown(KeyCode.Space) || _input.UISelect)
            {
                //changeLifeBar(5);
  
                _input.UISelect = false;
            }

            //Change weapons
            else if ((Input.GetKeyDown(KeyCode.Keypad8) || _input.UINavigateUp) && !uic.p.activeSelf && !uic.qm.activeSelf)
            {
                changeWeapon(0);
                _input.UINavigateUp = false;
            }

            else if ((Input.GetKeyDown(KeyCode.Keypad6) || _input.UINavigateRight) && !uic.p.activeSelf && !uic.qm.activeSelf)
            {
                changeWeapon(2);
                _input.UINavigateRight = false;
            }

            else if ((Input.GetKeyDown(KeyCode.Keypad2) || _input.UINavigateDown) && !uic.p.activeSelf && !uic.qm.activeSelf)
            {
                changeWeapon(3);
                _input.UINavigateDown = false;
            }

            else if ((Input.GetKeyDown(KeyCode.Keypad4) || _input.UINavigateLeft) && !uic.p.activeSelf && !uic.qm.activeSelf)
            {
                changeWeapon(1);
                _input.UINavigateLeft = false;
            }

            changeLifeBar(currentLife);
        }


        uic.UIManager(_input);

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
}
