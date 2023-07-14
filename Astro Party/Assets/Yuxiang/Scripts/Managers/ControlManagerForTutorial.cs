using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlManagerForTutorial : MonoBehaviour
{
    public int id;

    public Button PButton;
    public Button setRotateButton;
    public Button setShootButton;

    Tutorial tutorialScript;

    public Text PText;
    public Text rotateText;
    public Text shootText;

    // Start is called before the first frame update
    void Start()
    {
        id = 1;

        //Creating ships
        tutorialScript = GameObject.Find("Tutorial Manager").GetComponent<Tutorial>();

        tutorialScript.playerShip.GetComponent<MutualShip>().id = id;

        //set control
        PlayerController script = tutorialScript.playerShip.GetComponent<PlayerController>();
        script.turn = KeyCode.A;
        script.shoot = KeyCode.S;
      
        rotateText.text = script.turn.ToString();
        shootText.text = script.shoot.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void buttonColorChange()
    {
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
        setColor(setRotateButton, c);
        setColor(setShootButton, c);
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
        id++;
        if (id == 6)
        {
            id = 1;
        }
        tutorialScript.playerShip.GetComponent<MutualShip>().id = id;
        tutorialScript.shipId = id;
        PText.text = "P" + id;
        buttonColorChange();
    }

    public void setRotate()
    {
        KeyCode now = KeyCode.None;

        foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(kcode))
                now = kcode;
        }

        tutorialScript.playerShip.GetComponent<PlayerController>().turn = now;
        tutorialScript.shipRotate = now;
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

        tutorialScript.playerShip.GetComponent<PlayerController>().shoot = now;
        tutorialScript.shipShoot = now;
        shootText.text = now.ToString();
    }
}
