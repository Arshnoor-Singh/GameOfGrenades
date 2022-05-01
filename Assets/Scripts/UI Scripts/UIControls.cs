using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControls : MonoBehaviour
{
    public GameObject quitbutton;
    public GameObject p;
    public GameObject qm;
    GameObject yesQ;
    GameObject noQ;
    public List<GameObject> menus = new List<GameObject>();
    public List<GameObject> optionsM1 = new List<GameObject>();
    public List<GameObject> optionsM2 = new List<GameObject>();
    public List<GameObject> optionsM3 = new List<GameObject>();
    public List<GameObject> optionsM4 = new List<GameObject>();
    public List<GameObject> optionsM5 = new List<GameObject>();
    List<Sprite> SettingsBars = new List<Sprite>();

    public Image overallSp;
    public Image musicSp;
    public Image fxSp;
    public Image brightnessSp;

    public Text overallTxt;
    public Text musicTxt;
    public Text fxTxt;
    public Text brightnessTxt;

    GameObject m1;
    GameObject m2;
    GameObject m3;
    GameObject m4;
    GameObject m5;
    public GameObject dropdowns;

    public int currentMenu = 0;
    int selectedOption = 0;
    int overallIndex = 0;
    int musicIndex = 8;
    int fxIndex = 4;
    int brightnessIndex = 10;

    public bool pauseCheck = false;

    private void Awake()
    {
        p.SetActive(true);
        qm.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        overallTxt.text = overallIndex.ToString();
        musicTxt.text = musicIndex.ToString();
        fxTxt.text = fxIndex.ToString();
        brightnessTxt.text = brightnessIndex.ToString();

        yesQ = GameObject.Find("YES");
        noQ = GameObject.Find("NO");


        dropdowns = GameObject.Find("Dropdowns");

        for (int i = 0; i < 11; i++)
        {
            SettingsBars.Add(Resources.Load<Sprite>("Settings Bars/Setting " + i));
        }

        overallSp.sprite = SettingsBars[overallIndex];
        musicSp.sprite = SettingsBars[musicIndex];
        fxSp.sprite = SettingsBars[fxIndex];
        brightnessSp.sprite = SettingsBars[brightnessIndex];

        m1 = GameObject.Find("First Menu");
        for (int i = 0; i < m1.transform.childCount; i++)
        {
            optionsM1.Add(m1.transform.GetChild(i).gameObject);
            m1.transform.GetChild(i).Find("Glow").gameObject.SetActive(false);

        }
        menus.Add(m1);
        optionsM1[0].gameObject.SetActive(true);

        m2 = GameObject.Find("Audio Menu");
        for (int i = 1; i < m2.transform.childCount; i++)
        {
            optionsM2.Add(m2.transform.GetChild(i).gameObject);
            m2.transform.GetChild(i).Find("Glow").gameObject.SetActive(false);
        }
        menus.Add(m2);
        optionsM2[0].gameObject.SetActive(true);

        m3 = GameObject.Find("Video Menu");
        for (int i = 1; i < m3.transform.childCount; i++)
        {
            optionsM3.Add(m3.transform.GetChild(i).gameObject);
            m3.transform.GetChild(i).Find("Glow").gameObject.SetActive(false);
        }
        menus.Add(m3);
        optionsM3[0].gameObject.SetActive(true);

        m4 = GameObject.Find("Advanced Menu");
        for(int i = 1;i < m4.transform.childCount; i++)
        {
            optionsM4.Add(m4.transform.GetChild(i).gameObject);
            m4.transform.GetChild(i).Find("Glow").gameObject.SetActive(false);
        }
        menus.Add(m4);

        m5 = GameObject.Find("Controls Menu");
        for (int i = 1; i < m5.transform.childCount; i++)
        {
            optionsM5.Add(m5.transform.GetChild(i).gameObject);
            m5.transform.GetChild(i).Find("Glow").gameObject.SetActive(false);
        }
        menus.Add(m5);

        p.SetActive(false);
        qm.SetActive(false);
    }

    public void pauseMenu()
    {
        
        p.SetActive(!p.activeSelf);
        resetMenuCounter();
        Debug.Log("pause function");
        
        if (qm.activeSelf)
        {
            qm.SetActive(false);
        }


    }


    //public void AudioMenu()
    //{
    //    for (int i = 0; i < menus.Count; i++)
    //    {
    //        if (i == currentMenu)
    //        {
    //            menus[i].gameObject.SetActive(true);
    //        }
    //        else
    //        {
    //            menus[i].gameObject.SetActive(false);
    //        }
    //    }
    //}

    public void UIManager(FragPartyInputs _input_)
    {
        if (p.activeSelf)
        {
            for (int i = 0; i < menus.Count; i++)
            {
                if (i == currentMenu)
                {
                    menus[i].gameObject.SetActive(true);
                }
                else
                {
                    menus[i].gameObject.SetActive(false);
                }
            }

            if (currentMenu == 0)
            {
                quitbutton.SetActive(true);
                for (int i = 0; i < optionsM1.Count; i++)
                {
                    if (i == selectedOption)
                    {
                        optionsM1[selectedOption].transform.Find("Glow").gameObject.SetActive(true);
                        if (Input.GetKeyDown(KeyCode.Space) || _input_.UISelect)
                        {
                            quitbutton.SetActive(false);
                            string nom = optionsM1[selectedOption].name;
                            if (nom == "Quit")
                            {
                                Application.Quit();
                                //qm.SetActive(true);
                                //p.SetActive(false);
                                //quitting();
                            }
                            else if (nom == "Back")
                            {
                                p.SetActive(false);
                            }
                            else
                            {
                                changeMenus(i + 1);
                            }
                            _input_.UISelect = false;
                        }
                    }

                    else
                    {
                        optionsM1[i].transform.Find("Glow").gameObject.SetActive(false);
                    }
                }
            }

            if (currentMenu == 1)
            {
                for (int i = 0; i < optionsM2.Count; i++)
                {
                    if (i == selectedOption)
                    {
                        optionsM2[selectedOption].transform.Find("Glow").gameObject.SetActive(true);
                        if (Input.GetKeyDown(KeyCode.Keypad6) || _input_.UINavigateRight)
                        {
                            string nom = optionsM2[selectedOption].name;
                            if (nom != "Quit" && nom != "Back")
                            {
                                int counter = int.Parse(optionsM2[selectedOption].transform.Find("Counter").gameObject.GetComponent<Text>().text);
                                counter++;
                                if (counter < 11)
                                {
                                    optionsM2[selectedOption].transform.Find("Counter").gameObject.GetComponent<Text>().text = counter.ToString();
                                    optionsM2[selectedOption].transform.Find("Bar Value").gameObject.GetComponent<Image>().sprite = SettingsBars[counter];
                                }
                                else
                                {
                                    counter--;
                                }
                            }
                            _input_.UINavigateRight = false;
                        }
                        else if (Input.GetKeyDown(KeyCode.Keypad4) || _input_.UINavigateLeft)
                        {
                            string nom = optionsM2[selectedOption].name;
                            if (nom != "Quit" && nom != "Back")
                            {
                                int counter = int.Parse(optionsM2[selectedOption].transform.Find("Counter").gameObject.GetComponent<Text>().text);
                                counter--;
                                if (counter >= 0)
                                {
                                    optionsM2[selectedOption].transform.Find("Counter").gameObject.GetComponent<Text>().text = counter.ToString();
                                    optionsM2[selectedOption].transform.Find("Bar Value").gameObject.GetComponent<Image>().sprite = SettingsBars[counter];
                                }
                                else
                                {
                                    counter++;
                                }
                            }
                            _input_.UINavigateLeft = false;
                        }
                        else if (Input.GetKeyDown(KeyCode.Space) || _input_.UISelect)
                        {
                            quitbutton.SetActive(true);

                            string nom = optionsM2[selectedOption].name;
                            if (nom == "Quit")
                            {
                                qm.SetActive(true);
                            }
                            else if (nom == "Back")
                            {
                                changeMenus(0);
                            }
                            _input_.UISelect = false;
                            Debug.Log("11111111");
                        }
                    }
                    else
                    {
                        optionsM2[i].transform.Find("Glow").gameObject.SetActive(false);
                        Debug.Log("22222222222");
                    }
                }
            }

            if (currentMenu == 2)
            {
                quitbutton.SetActive(false);

                for (int i = 0; i < optionsM3.Count; i++)
                {
                    if (i == selectedOption)
                    {
                        optionsM3[selectedOption].transform.Find("Glow").gameObject.SetActive(true);
                    }
                    else
                    {
                        optionsM3[i].transform.Find("Glow").gameObject.SetActive(false);
                    }
                }

                string nom = optionsM3[selectedOption].name;
                if ((Input.GetKeyDown(KeyCode.Keypad6) || _input_.UINavigateRight) && nom == "Brightness")
                {
                    int counter = int.Parse(optionsM3[selectedOption].transform.Find("Counter").gameObject.GetComponent<Text>().text);
                    counter++;
                    if (counter < 11)
                    {
                        optionsM3[selectedOption].transform.Find("Counter").gameObject.GetComponent<Text>().text = counter.ToString();
                        optionsM3[selectedOption].transform.Find("Bar Value").gameObject.GetComponent<Image>().sprite = SettingsBars[counter];
                    }
                    else
                    {
                        counter--;
                    }
                    _input_.UINavigateRight = false;
                }

                else if ((Input.GetKeyDown(KeyCode.Keypad4) || _input_.UINavigateLeft) && nom == "Brightness")
                {
                    int counter = int.Parse(optionsM3[selectedOption].transform.Find("Counter").gameObject.GetComponent<Text>().text);
                    counter--;
                    if (counter >= 0)
                    {
                        optionsM3[selectedOption].transform.Find("Counter").gameObject.GetComponent<Text>().text = counter.ToString();
                        optionsM3[selectedOption].transform.Find("Bar Value").gameObject.GetComponent<Image>().sprite = SettingsBars[counter];
                    }
                    else
                    {
                        counter++;
                    }
                    _input_.UINavigateLeft = false;
                }

                else if (Input.GetKeyDown(KeyCode.Space) || _input_.UISelect)
                {
                    if (nom == "Quit")
                    {
                        qm.SetActive(true);
                    }
                    else if (nom == "Back")
                    {
                        changeMenus(0);
                    }
                    quitbutton.SetActive(true);
                    _input_.UISelect = false;
                }

                else if (nom == "Resolution" && (Input.GetKeyDown(KeyCode.Keypad5) || _input_.UISelect))
                {
                    bool tf = dropdowns.transform.GetChild(0).gameObject.activeSelf;
                    dropdowns.transform.GetChild(0).gameObject.SetActive(!tf);
                    _input_.UISelect = false;
                }

                else if (nom == "Window Mode" && (Input.GetKeyDown(KeyCode.Keypad5) || _input_.UISelect))
                {
                    bool tf = dropdowns.transform.GetChild(1).gameObject.activeSelf;
                    dropdowns.transform.GetChild(1).gameObject.SetActive(!tf);
                    _input_.UISelect = false;

                }
            }

            if (currentMenu == 3)
            {
                quitbutton.SetActive(false);
                for (int i = 0; i < optionsM4.Count; i++)
                {
                    string nom = optionsM4[selectedOption].name;
                    if (i == selectedOption)
                    {
                        optionsM4[selectedOption].transform.Find("Glow").gameObject.SetActive(true);
                    }
                    else
                    {
                        optionsM4[i].transform.Find("Glow").gameObject.SetActive(false);
                    }

                    if (Input.GetKeyDown(KeyCode.Space) || _input_.UISelect)
                    {
                        if (nom == "Quit")
                        {
                            qm.SetActive(true);
                        }
                        else if (nom == "Back")
                        {
                            changeMenus(0);
                        }
                        quitbutton.SetActive(true);
                        _input_.UISelect = false;
                    }

                    else if (nom == "Texture Quality" && (Input.GetKeyDown(KeyCode.Keypad5) || _input_.UISelect))
                    {
                        bool tf = dropdowns.transform.GetChild(3).gameObject.activeSelf;
                        dropdowns.transform.GetChild(3).gameObject.SetActive(!tf);
                        _input_.UISelect = false;
                    }

                    else if (nom == "Detail" && (Input.GetKeyDown(KeyCode.Keypad5) || _input_.UISelect))
                    {
                        bool tf = dropdowns.transform.GetChild(4).gameObject.activeSelf;
                        dropdowns.transform.GetChild(4).gameObject.SetActive(!tf);
                        _input_.UISelect = false;
                    }

                    else if (nom == "Colorblind Mode" && (Input.GetKeyDown(KeyCode.Keypad5) || _input_.UISelect))
                    {
                        bool tf = dropdowns.transform.GetChild(5).gameObject.activeSelf;
                        dropdowns.transform.GetChild(5).gameObject.SetActive(!tf);
                        _input_.UISelect = false;
                    }
                }
            }

            if (currentMenu == 4)
            {
                quitbutton.SetActive(false);
                for (int i = 0; i < optionsM5.Count; i++)
                {
                    string nom = optionsM5[selectedOption].name;
                    if (i == selectedOption)
                    {
                        optionsM5[selectedOption].transform.Find("Glow").gameObject.SetActive(true);
                    }
                    else
                    {
                        optionsM5[i].transform.Find("Glow").gameObject.SetActive(false);
                    }

                    if (Input.GetKeyDown(KeyCode.Space) || _input_.UISelect)
                    {

                        if (nom == "Quit")
                        {
                            qm.SetActive(true);
                        }
                        else if (nom == "Back")
                        {
                            changeMenus(0);
                        }
                        quitbutton.SetActive(true);
                        _input_.UISelect = false;
                    }
                }
            }
           
        }

        if (p.activeSelf && qm.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Keypad6) || _input_.UINavigateRight)
            {
                yesQ.transform.Find("Yes Glow").gameObject.SetActive(false);
                noQ.transform.Find("No Glow").gameObject.SetActive(true);
                _input_.UINavigateRight = false;

            }

            else if (Input.GetKeyDown(KeyCode.Keypad4) || _input_.UINavigateLeft)
            {
                yesQ.transform.Find("Yes Glow").gameObject.SetActive(true);
                noQ.transform.Find("No Glow").gameObject.SetActive(false);
                _input_.UINavigateLeft = false;

            }

            else if ((Input.GetKeyDown(KeyCode.Space) || _input_.UISelect) && noQ.transform.Find("No Glow").gameObject.activeSelf)
            {
                qm.SetActive(false);
                p.SetActive(true);
                _input_.UISelect = false;
            }

            else if ((Input.GetKeyDown(KeyCode.Space) || _input_.UISelect) && yesQ.transform.Find("Yes Glow").gameObject.activeSelf)
            {
                Debug.Log("QUIT_GAME");
                Application.Quit();
                _input_.UISelect = false;
            }

        }
    }

    


    public void changeMenus(int n)
    {
        currentMenu = n;
        selectedOption = 0;
    }

    public void resetMenuCounter()
    {
        currentMenu = 0;
        selectedOption = 0;
    }

    public void moveUp()
    {
        int menuSize = 0;
        if(currentMenu == 0)
        {
            menuSize = optionsM1.Count;
        }
        else if (currentMenu == 1)
        {
            menuSize = optionsM2.Count;
        }

        else if (currentMenu == 2)
        {
            menuSize = optionsM3.Count;
        }

        else if (currentMenu == 3)
        {
            menuSize = optionsM4.Count;
        }

        else if (currentMenu == 4)
        {
            menuSize = optionsM5.Count;
        }

        if (menuSize > selectedOption + 1)
        {
            selectedOption++;
        }
        
    }

    public void moveDown()
    {
        int menuSize = 0;
        if (currentMenu == 0)
        {
            menuSize = optionsM1.Count;
        }
        else if (currentMenu == 1)
        {
            menuSize = optionsM2.Count;
        }

        else if (currentMenu == 2)
        {
            menuSize = optionsM3.Count;
        }

        else if (currentMenu == 3)
        {
            menuSize = optionsM4.Count;
        }

        else if (currentMenu == 4)
        {
            menuSize = optionsM5.Count;
        }

        if (0 <= selectedOption - 1)
        {
            selectedOption--;
        }

    }

    public void resetSelectedOption()
    {
        selectedOption = 0;
    }

    public void closeQuitMenu()
    {
        qm.SetActive(false);
    }

    public GameObject darQuitMenu()
    {
        return qm;
    }

    public GameObject darDropdowns()
    {
        return dropdowns;
    }
}
