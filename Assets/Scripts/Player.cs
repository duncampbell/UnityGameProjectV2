using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour {

    public XboxController xbController;
    public int playerNum { get; private set; }
    PlayerController playerController;
    List<bool[]> playerChoices = new List<bool[]>();

    void Awake() {
        switch (xbController) {
            case XboxController.First: playerNum = 1; break;
            case XboxController.Second: playerNum = 2; break;
            case XboxController.Third: playerNum = 3; break;
            case XboxController.Fourth: playerNum = 4; break;
        }
    }

    void Start() {
        playerController = GetComponent<PlayerController>();
        if (playerNum > GameRoundManager.instance.numPlayers) {
            Destroy(gameObject);
        }
        else if (playerNum > XCI.GetNumPluggedCtrlrs()) {
            GetComponent<AIPlayer>().enabled = true;
            GetComponent<Player>().enabled = false;
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
        /*
        Debug.Log("Player 1 Abilities: "+player1[0] + "," + player1[1] + "," + player1[2]);
        Debug.Log("Player 2 Abilities: " + player2[0] + "," + player2[1] + "," + player2[2]);
        Debug.Log("Player 3 Abilities: " + player3[0] + "," + player3[1] + "," + player3[2]);
        Debug.Log("Player 4 Abilities: " + player4[0] + "," + player4[1] + "," + player4[2]);
        */
        //Debug.Log(XCI.GetNumPluggedCtrlrs() + " Xbox controllers plugged in.");
    }

    void Update() {
        playerController.Move(new Vector3(XCI.GetAxis(XboxAxis.LeftStickX, xbController), 0, XCI.GetAxis(XboxAxis.LeftStickY, xbController)));
        playerController.Aim(new Vector3(XCI.GetAxis(XboxAxis.RightStickX, xbController), 0, XCI.GetAxis(XboxAxis.RightStickY, xbController)));

        if (XCI.GetButtonDown(XboxButton.RightBumper, xbController))
            playerController.Attack();


        if (xbController == XboxController.First)
        {
            UpdatePlayer(0);
        }

        if (xbController == XboxController.Second)
        {
            UpdatePlayer(1);
        }

        if (xbController == XboxController.Third)
        {
            UpdatePlayer(2);
        }

        if (xbController == XboxController.Fourth)
        {
            UpdatePlayer(3);
        }
        

    }



    void UpdatePlayer(int playerNum)
    {
        if (XCI.GetButtonDown(XboxButton.Y, xbController))
        {
            if (playerChoices[playerNum][0])
            {
                playerController.AOE();
            }
        }

        if (XCI.GetButtonDown(XboxButton.X, xbController))
        {
            if (playerChoices[playerNum][1])
            {
                playerController.Force();
            }
        }

        if (XCI.GetButtonDown(XboxButton.DPadUp, xbController))
        {
            if (playerChoices[playerNum][2])
            {
                playerController.ExMine();
            }
        }
        if (XCI.GetButtonDown(XboxButton.DPadDown, xbController))
        {
            if (playerChoices[playerNum][3])
            {
                playerController.TrapMine();
            }
        }

        if (XCI.GetButtonDown(XboxButton.B, xbController))
        {
            if (playerChoices[playerNum][4]) {
                playerController.Heal();
            }
        }
       
        if (XCI.GetButtonDown(XboxButton.LeftBumper, xbController))
        {
            if (playerChoices[playerNum][5])
            {
                playerController.Block();
            }
        }
        if (XCI.GetButtonUp(XboxButton.LeftBumper, xbController))
        {

           if (playerChoices[playerNum][5]) { 
            playerController.StopBlock();
            }
        }
        
        if (XCI.GetButtonDown(XboxButton.A, xbController)) {
            if (playerChoices[playerNum][6])
            {
                playerController.Stream();
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

}