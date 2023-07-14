using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlManager : MonoBehaviour
{
    public int id;

    public GameObject shipPlayer;
    public GameObject shipBot;
    public GameObject shipBot1;
    public int team;
    public Button PButton;
    public Button teamButtonObject;
    public Button setRotateButton;
    public Button setShootButton;
    public GameObject startingPowerUpButton;

    GameManager gameManagerScript;
    ScoreManager scoreManagerScript;

    public Text PText;
    public Text teamText;
    public Text rotateText;
    public Text shootText;

    // Start is called before the first frame update
    void Start()
    {
        team = id;
        teamText.text = team.ToString();

        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        scoreManagerScript = GameObject.Find("Score Manager").GetComponent<ScoreManager>();

        //Creating ships
        shipBot = Instantiate(gameManagerScript.botShip, new Vector3(0, 0, 0),
            gameManagerScript.playerShip.transform.rotation);
        shipBot.GetComponent<MutualShip>().id = id;
        shipBot.SetActive(false);

        shipBot1 = Instantiate(gameManagerScript.botShip1, new Vector3(0, 0, 0),
            gameManagerScript.playerShip.transform.rotation);
        shipBot1.GetComponent<MutualShip>().id = id;
        shipBot1.SetActive(false);

        shipPlayer = Instantiate(gameManagerScript.playerShip, new Vector3(0, 0, 0),
            gameManagerScript.playerShip.transform.rotation);
        shipPlayer.GetComponent<MutualShip>().id = id;
        shipPlayer.SetActive(false);

        //button color
        Color c = new Color(0, 0, 255);

        switch (id)
        {
            case 1: c = new Color(0, 0, 255); break;
            case 2: c = new Color(255, 0, 0); break;
            case 3: c = new Color(255, 255, 0); break;
            case 4: c = new Color(0, 255, 255); break;
            case 5: c = new Color(0, 255, 0); break;
        }

        setColor(PButton, c);
        setColor(teamButtonObject, c);
        setColor(setRotateButton, c);
        setColor(setShootButton, c);

        teamButtonObject.gameObject.SetActive(false);
        setRotateButton.gameObject.SetActive(false);
        setShootButton.gameObject.SetActive(false);

        //set control
        PlayerController script = shipPlayer.GetComponent<PlayerController>();

        switch (id)
        {
            case 1:
                script.turn = KeyCode.BackQuote;
                script.shoot = KeyCode.Tab;
                break;
            case 2:
                script.turn = KeyCode.Z;
                script.shoot = KeyCode.X;
                break;
            case 3:
                script.turn = KeyCode.N;
                script.shoot = KeyCode.M;
                break;
            case 4:
                script.turn = KeyCode.RightBracket;
                script.shoot = KeyCode.Backslash;
                break;
            case 5:
                script.turn = KeyCode.LeftArrow;
                script.shoot = KeyCode.RightArrow;
                break;
        }

        rotateText.text = script.turn.ToString();
        shootText.text = script.shoot.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setColor(Button button, Color c)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = c;
        colors.selectedColor = c;
        colors.pressedColor = c;
        button.colors = colors;
    }

    public void shipButton()
    {
        GameObject picture = scoreManagerScript.P1;

        switch (id)
        {
            case 1: picture = scoreManagerScript.P1; break;
            case 2: picture = scoreManagerScript.P2; break;
            case 3: picture = scoreManagerScript.P3; break;
            case 4: picture = scoreManagerScript.P4; break;
            case 5: picture = scoreManagerScript.P5; break;
        }

        buttonHelper(shipPlayer, shipBot, shipBot1, PText, team, teamButtonObject,
            setRotateButton, setShootButton, picture, startingPowerUpButton);
    }

    void buttonHelper(GameObject player, GameObject bot, GameObject bot1, Text textPlayer, int team,
        Button teamButton, Button setRotateButton, Button setShootButton, GameObject picture,
            GameObject startingPowerUpButton)
    {
        List<GameObject> ship = gameManagerScript.ships[team-1];
        if (ship.Contains(player))
        {
            textPlayer.text = "Bot1";
            ship.Remove(player);
            ship.Add(bot);

            setRotateButton.gameObject.SetActive(false);
            setShootButton.gameObject.SetActive(false);
        }
        else if (ship.Contains(bot))
        {
            textPlayer.text = "Bot2";
            ship.Remove(bot);
            ship.Add(bot1);
        }
        else if (ship.Contains(bot1))
        {
            textPlayer.text = "Off";
            ship.Remove(bot1);

            teamButton.gameObject.SetActive(false);
            teamButton.gameObject.SetActive(false);
            picture.SetActive(false);
            startingPowerUpButton.SetActive(false);
        }
        else
        {
            textPlayer.text = "P" + player.GetComponent<MutualShip>().id;
            ship.Add(player);

            setRotateButton.gameObject.SetActive(true);
            setShootButton.gameObject.SetActive(true);
            teamButton.gameObject.SetActive(true);
            teamButton.gameObject.SetActive(true);
            picture.gameObject.SetActive(true);
            startingPowerUpButton.SetActive(true);
        }
    }

    public void teamButton()
    {
        GameObject curr = findShip(team, shipPlayer, shipBot);

        gameManagerScript.ships[team-1].Remove(curr);
        team++;

        if (team == 6)
        {
            team = 1;
        }
        gameManagerScript.ships[team-1].Add(curr);

        teamText.text = team.ToString();
    }

    GameObject findShip(int team, GameObject shipTarget, GameObject bot)
    {
        GameObject ans = null;
        List<GameObject> ship = gameManagerScript.ships[team-1];
        if (ship.Contains(shipTarget))
        {
            ans = shipTarget;
        }
        else if (ship.Contains(bot))
        {
            ans = bot;
        }
        return ans;
    }

    public void setRotate()
    {
        KeyCode now = KeyCode.None;

        foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(kcode))
                now = kcode;
        }

        shipPlayer.GetComponent<PlayerController>().turn = now;
        rotateText.text = now.ToString();
    }

    public void setShoot()
    {
        KeyCode now = KeyCode.None;

        foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(kcode))
                now = kcode;
        }

        shipPlayer.GetComponent<PlayerController>().shoot = now;
        shootText.text = now.ToString();
    }
}
