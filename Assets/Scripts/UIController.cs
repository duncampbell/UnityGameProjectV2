using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XboxCtrlrInput;

public class UIController : MonoBehaviour {

    public GameObject PausedMenu;
    public GameObject ControlMenu;
    public XboxController controller;
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;
    public Text player1Name;
    public Text player2Name;
    public Text player3Name;
    public Text player4Name;
    bool paused;
    List<bool[]> playerChoices = new List<bool[]>();

    public GameObject SkillsPanel1;
    public GameObject SkillsPanel2;
    public GameObject SkillsPanel3;
    public GameObject SkillsPanel4;


    // Use this for initialization
    //Sets menus to inactive
    void Start()
    {
        paused = false;
        PausedMenu.SetActive(false);
        ControlMenu.SetActive(false);

        string name = PlayerPrefs.GetString("Player1Name").ToUpper();
        if (name == "")
        {
            player1Name.text = "PLAYER 1";
        }
        else
        {
            player1Name.text = name;
        }

        name = PlayerPrefs.GetString("Player2Name").ToUpper();
        if (name == "")
        {
            player2Name.text = "PLAYER 2";
        }
        else
        {
            player2Name.text = name;
        }

        name = PlayerPrefs.GetString("Player3Name").ToUpper();
        if (name == "")
        {
            player3Name.text = "PLAYER 3";
        }
        else
        {
            player3Name.text = name;
        }

        name = PlayerPrefs.GetString("Player4Name").ToUpper();
        if (name == "")
        {
            player4Name.text = "PLAYER 4";
        }
        else
        {
            player4Name.text = name;
        }


        string[] player1Abilities = new string[] { PlayerPrefs.GetString("Player1Ability1"), PlayerPrefs.GetString("Player1Ability2"), PlayerPrefs.GetString("Player1Ability3") };
        string[] player2Abilities = new string[] { PlayerPrefs.GetString("Player2Ability1"), PlayerPrefs.GetString("Player2Ability2"), PlayerPrefs.GetString("Player2Ability3") };
        string[] player3Abilities = new string[] { PlayerPrefs.GetString("Player3Ability1"), PlayerPrefs.GetString("Player3Ability2"), PlayerPrefs.GetString("Player3Ability3") };
        string[] player4Abilities = new string[] { PlayerPrefs.GetString("Player4Ability1"), PlayerPrefs.GetString("Player4Ability2"), PlayerPrefs.GetString("Player4Ability3") };

        bool[] player1 = PlayerBools(player1Abilities);
        playerChoices.Add(player1);
        bool[] player2 = PlayerBools(player2Abilities);
        playerChoices.Add(player2);
        bool[] player3 = PlayerBools(player3Abilities);
        playerChoices.Add(player3);
        bool[] player4 = PlayerBools(player4Abilities);
        playerChoices.Add(player4);
        

        DeleteChildren(SkillsPanel1.transform, player1);
        DeleteChildren(SkillsPanel2.transform, player2);
        DeleteChildren(SkillsPanel3.transform, player3);
        DeleteChildren(SkillsPanel4.transform, player4);

    }

    void DeleteChildren(Transform transform, bool[] skills)
    {
        foreach (Transform child in transform)
        {
                if (child.gameObject.tag == "AOEAbility")
                {
                
                    if (!skills[0])
                    {
                    GameObject.Destroy(child.gameObject);
                    }
               }
            
                if (child.gameObject.tag == "ShotgunAbility")
                {
                    if (!skills[1])
                    {
                    GameObject.Destroy(child.gameObject);
                }
                }
                
                if (child.gameObject.tag == "MineAbility")
                    {
                    if (!skills[2])
                    {
                    GameObject.Destroy(child.gameObject);
                }
                }
                
                if (child.gameObject.tag == "TrapAbility")
                {
                    if (!skills[3])
                    {
                    GameObject.Destroy(child.gameObject);
                }
                }
                if (child.gameObject.tag == "HealAbility")
                {
                if (!skills[4])
                    {
                    GameObject.Destroy(child.gameObject);
                }
                }
            
                if (child.gameObject.tag == "BlockAbility")
                {
                if (!skills[5])
                    {
                    GameObject.Destroy(child.gameObject);
                }
                }
                
                if (child.gameObject.tag == "FlameAbility")
                {
                if (!skills[6])
                    {
                    GameObject.Destroy(child.gameObject);
                }
                }

                
        }
    }



    bool[] PlayerBools(string[] PlayerAbilities)
    {
        bool[] player = new bool[] { false, false, false, false, false, false, false };


        for (int i = 0; i < PlayerAbilities.Length; i++)
        {
            if (PlayerAbilities[i] == "AOEAbility")
            {
                player[0] = true;
            }
        }

        for (int i = 0; i < PlayerAbilities.Length; i++)
        {
            if (PlayerAbilities[i] == "ShotgunAbility")
            {
                player[1] = true;
            }
        }


        for (int i = 0; i < PlayerAbilities.Length; i++)
        {
            if (PlayerAbilities[i] == "MineAbility")
            {
                player[2] = true;
            }
        }


        for (int i = 0; i < PlayerAbilities.Length; i++)
        {
            if (PlayerAbilities[i] == "TrapAbility")
            {
                player[3] = true;
            }
        }


        for (int i = 0; i < PlayerAbilities.Length; i++)
        {
            if (PlayerAbilities[i] == "HealAbility")
            {
                player[4] = true;
            }
        }


        for (int i = 0; i < PlayerAbilities.Length; i++)
        {
            if (PlayerAbilities[i] == "BlockAbility")
            {
                player[5] = true;
            }
        }

        for (int i = 0; i < PlayerAbilities.Length; i++)
        {
            if (PlayerAbilities[i] == "FlameAbility")
            {
                player[6] = true;
            }
        }


        return player;
    }

    // Update is called once per frame
    //Pause the game if start button is pressed on controller or if bool is true
    void Update()
    {
        
            if (XCI.GetButtonDown(XboxButton.Start, controller))
            {
                paused = !paused;
                if (paused)
                {
                    PausedMenu.SetActive(true);
                }

            }
            if (paused)
            {

                Time.timeScale = 0;
            }
            else
            {
                PausedMenu.SetActive(false);
                ControlMenu.SetActive(false);
                Time.timeScale = 1;

            }
        
    }

    //Resume game and start time scale
    public void Resume()
    {
        paused = false;
    }

    //Show controls menu and get rid of current menu
    public void Controls()
    {
        PausedMenu.SetActive(false);
        ControlMenu.SetActive(true);
    }

    //Quit and go back to main menu
    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }

    //Unpause game and return.
    public void Return()
    {
        PausedMenu.SetActive(true);
        ControlMenu.SetActive(false);
    }

    //Set paused bool to true
    public void Pause()
    {
        paused = !paused;
        if (paused)
        {
            PausedMenu.SetActive(true);
        }
    }
}
