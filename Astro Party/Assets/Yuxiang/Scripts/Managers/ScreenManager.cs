using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenManager : MonoBehaviour
{
    ScoreManager scoreManagerScript;
    GameManager gameManagerScript;

    public GameObject startScreen;
    public GameObject shipScreen;
    public GameObject lastScreen;
    public GameObject infoScreen;
    public GameObject infoScreen1;
    public GameObject infoScreen2;
    public GameObject mapScreen;
    public GameObject powerUpScreen;
    public GameObject AllSPUScreen;

    public GameObject SPUScreen;
    public TextMeshProUGUI P1SPUText;
    public TextMeshProUGUI P2SPUText;
    public TextMeshProUGUI P3SPUText;
    public TextMeshProUGUI P4SPUText;
    public TextMeshProUGUI P5SPUText;

    public GameObject P1PowerUpScreen;
    public GameObject P2PowerUpScreen;
    public GameObject P3PowerUpScreen;
    public GameObject P4PowerUpScreen;
    public GameObject P5PowerUpScreen;

    public GameObject friendlyFireButton;
    public GameObject teamModeText;
    public GameObject soloModeText;

    // Start is called before the first frame update
    void Start()
    {
        //set it for allSPUManager
        P1PowerUpScreen.SetActive(true);
        P2PowerUpScreen.SetActive(true);
        P3PowerUpScreen.SetActive(true);
        P4PowerUpScreen.SetActive(true);
        P5PowerUpScreen.SetActive(true);

        startScreen.SetActive(true);
        shipScreen.SetActive(false);
        lastScreen.SetActive(false);
        infoScreen.SetActive(false);
        infoScreen1.SetActive(false);
        infoScreen2.SetActive(false);
        mapScreen.SetActive(false);
        powerUpScreen.SetActive(false);
        AllSPUScreen.SetActive(false);
        P1PowerUpScreen.SetActive(false);
        P2PowerUpScreen.SetActive(false);
        P3PowerUpScreen.SetActive(false);
        P4PowerUpScreen.SetActive(false);
        P5PowerUpScreen.SetActive(false);
        SPUScreen.SetActive(false);

        scoreManagerScript = GameObject.Find("Score Manager").GetComponent<ScoreManager>();
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void nextToShip()
    {
        startScreen.SetActive(false);
        shipScreen.SetActive(true);
    }

    public void nextToLast()
    {
        shipScreen.SetActive(false);
        lastScreen.SetActive(true);

        //determine game mode to be solo or team

        scoreManagerScript.SoloOrTeam = "solo";

        for (int i = 0; i < gameManagerScript.ships.Count; i++)
        {
            if (gameManagerScript.ships[i].Count > 1)
            {
                scoreManagerScript.SoloOrTeam = "team";
            }
        }

        if (scoreManagerScript.SoloOrTeam == "team")
        {
            friendlyFireButton.SetActive(true);
            teamModeText.SetActive(true);
            soloModeText.SetActive(false);
        }
        else
        {
            friendlyFireButton.SetActive(false);
            soloModeText.SetActive(true);
            teamModeText.SetActive(false);
        }
    }

    public void nextToInfo()
    {
        startScreen.SetActive(false);
        infoScreen.SetActive(true);
    }

    public void backToStart()
    {
        infoScreen.SetActive(false);
        shipScreen.SetActive(false);
        startScreen.SetActive(true);
    }

    public void backToShip()
    {
        lastScreen.SetActive(false);
        shipScreen.SetActive(true);
    }

    public void play()
    {
        lastScreen.SetActive(false);
    }

    public void backToLast()
    {
        powerUpScreen.SetActive(false);
        mapScreen.SetActive(false);
        lastScreen.SetActive(true);
    }

    public void nextToMap()
    {
        lastScreen.SetActive(false);
        mapScreen.SetActive(true);
    }

    public void nextToPowerUp()
    {
        lastScreen.SetActive(false);
        powerUpScreen.SetActive(true);
    }

    public void nextToInfo1()
    {
        infoScreen.SetActive(false);
        infoScreen1.SetActive(true);
    }

    public void backToInfo()
    {
        infoScreen1.SetActive(false);
        infoScreen.SetActive(true);
    }

    public void nextToInfo2()
    {
        infoScreen1.SetActive(false);
        infoScreen2.SetActive(true);
    }

    public void backToInfo1()
    {
        infoScreen2.SetActive(false);
        infoScreen1.SetActive(true);
    }

    //power up

    public void nextToSpecialPowerUp()
    {
        powerUpScreen.SetActive(false);
        AllSPUScreen.SetActive(true);
    }

    public void nextToP1PowerUp()
    {
        powerUpScreen.SetActive(false);
        P1PowerUpScreen.SetActive(true);
    }

    public void nextToP2PowerUp()
    {
        powerUpScreen.SetActive(false);
        P2PowerUpScreen.SetActive(true);
    }

    public void nextToP3PowerUp()
    {
        powerUpScreen.SetActive(false);
        P3PowerUpScreen.SetActive(true);
    }

    public void nextToP4PowerUp()
    {
        powerUpScreen.SetActive(false);
        P4PowerUpScreen.SetActive(true);
    }

    public void nextToP5PowerUp()
    {
        powerUpScreen.SetActive(false);
        P5PowerUpScreen.SetActive(true);
    }

    public void backToPowerUp()
    {
        P1PowerUpScreen.SetActive(false);
        P2PowerUpScreen.SetActive(false);
        P3PowerUpScreen.SetActive(false);
        P4PowerUpScreen.SetActive(false);
        P5PowerUpScreen.SetActive(false);
        AllSPUScreen.SetActive(false);
        powerUpScreen.SetActive(true);
    }

    IEnumerator startingPowerUp()
    {
        foreach (List<GameObject> shipList in gameManagerScript.inGameShips)
        {
            foreach (GameObject ship in shipList)
            {
                switch (ship.GetComponent<MutualShip>().id)
                {
                    case 1:
                        P1SPUText.gameObject.SetActive(true);
                        P1SPUText.text = "P1: " + ship.GetComponent<MutualShip>().shootMode;
                        break;
                    case 2:
                        P2SPUText.gameObject.SetActive(true);
                        P2SPUText.text = "P2: " + ship.GetComponent<MutualShip>().shootMode;
                        break;
                    case 3:
                        P3SPUText.gameObject.SetActive(true);
                        P3SPUText.text = "P3: " + ship.GetComponent<MutualShip>().shootMode;
                        break;
                    case 4:
                        P4SPUText.gameObject.SetActive(true);
                        P4SPUText.text = "P4: " + ship.GetComponent<MutualShip>().shootMode;
                        break;
                    case 5:
                        P5SPUText.gameObject.SetActive(true);
                        P5SPUText.text = "P5: " + ship.GetComponent<MutualShip>().shootMode;
                        break;
                }
            }
        }

        SPUScreen.SetActive(true);

        yield return new WaitForSeconds(2.0f);

        SPUScreen.SetActive(false);
        P1SPUText.gameObject.SetActive(false);
        P2SPUText.gameObject.SetActive(false);
        P3SPUText.gameObject.SetActive(false);
        P4SPUText.gameObject.SetActive(false);
        P5SPUText.gameObject.SetActive(false);
    }
}
