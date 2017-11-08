using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XboxCtrlrInput;

public class AbilityScreenManager : MonoBehaviour
{

    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    public GameObject Player1Slot1;
    public GameObject Player1Slot2;
    public GameObject Player1Slot3;


    public GameObject Player2Slot1;
    public GameObject Player2Slot2;
    public GameObject Player2Slot3;

    public GameObject Player3Slot1;
    public GameObject Player3Slot2;
    public GameObject Player3Slot3;

    public GameObject Player4Slot1;
    public GameObject Player4Slot2;
    public GameObject Player4Slot3;

    public Text player1Name;
    public Text player2Name;
    public Text player3Name;
    public Text player4Name;


    void Start()
    {
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



        int controlNum = XCI.GetNumPluggedCtrlrs();
        if (controlNum > 1)
        {
            Clear(Player2Slot1.transform);
            Clear(Player2Slot2.transform);
            Clear(Player2Slot3.transform);
        }
        else
        {
            Disable(Player2Slot1.transform);
            Disable(Player2Slot2.transform);
            Disable(Player2Slot3.transform);
        }


        if (controlNum > 2)
        {
            Clear(Player3Slot1.transform);
            Clear(Player3Slot2.transform);
            Clear(Player3Slot3.transform);
        }
        else
        {
            Disable(Player3Slot1.transform);
            Disable(Player3Slot2.transform);
            Disable(Player3Slot3.transform);
        }


        if (controlNum > 3)
        {
            Clear(Player4Slot1.transform);
            Clear(Player4Slot2.transform);
            Clear(Player4Slot3.transform);
        }
        else
        {
            Disable(Player4Slot1.transform);
            Disable(Player4Slot2.transform);
            Disable(Player4Slot3.transform);

        }



    }
    bool canProceed(string[] playerAbilities)
    {
        for (int i = 0; i < playerAbilities.Length; i++)
        {
            if (playerAbilities[i] == null)
            {
                return false;
            }

            bool Unique = playerAbilities.Distinct().Count() == playerAbilities.Length;
            if (!Unique)
            {
                return false;
            }
        }
        return true;

    }
    public void Play()
    {
        string[] player1Abilities = getAbilities(player1.transform);
        string[] player2Abilities = getAbilities(player2.transform);
        string[] player3Abilities = getAbilities(player3.transform);
        string[] player4Abilities = getAbilities(player4.transform);
        bool proceed = canProceed(player1Abilities);
        proceed = canProceed(player1Abilities);
        proceed = canProceed(player1Abilities);
        if (proceed) {

            PlayerPrefs.SetString("Player1Ability1", player1Abilities[0]);
            PlayerPrefs.SetString("Player1Ability2", player1Abilities[1]);
            PlayerPrefs.SetString("Player1Ability3", player1Abilities[2]);

            PlayerPrefs.SetString("Player2Ability1", player2Abilities[0]);
            PlayerPrefs.SetString("Player2Ability2", player2Abilities[1]);
            PlayerPrefs.SetString("Player2Ability3", player2Abilities[2]);

            PlayerPrefs.SetString("Player3Ability1", player3Abilities[0]);
            PlayerPrefs.SetString("Player3Ability2", player3Abilities[1]);
            PlayerPrefs.SetString("Player3Ability3", player3Abilities[2]);

            PlayerPrefs.SetString("Player4Ability1", player4Abilities[0]);
            PlayerPrefs.SetString("Player4Ability2", player4Abilities[1]);
            PlayerPrefs.SetString("Player4Ability3", player4Abilities[2]);
            
            SceneManager.LoadScene("Main");
           
    }
        
    }

    public void Back()
    {
        SceneManager.LoadScene("Menu");
    }

    string[] getAbilities(Transform transform)
    {
        string[] abilities = new string[3];
        int i = 0;
        foreach (Transform child in transform)
        {
            if (child.childCount > 0) {
                abilities[i] = child.GetChild(0).gameObject.tag;
            }
            i++;
        }
        
        return abilities;
    }


    void Clear(Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    void Disable(Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject.GetComponent<DragHandeler>());
        }
    }
}
