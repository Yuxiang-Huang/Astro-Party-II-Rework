using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpManager : MonoBehaviour
{
    public List<KeyValuePair<string, GameObject>> allPowerUps = new List<KeyValuePair<string, GameObject>>();
    public List<GameObject> indicators = new List<GameObject>();
    public List<string> SPU = new List<string>();
    public List<Text> allText = new List<Text>();

    public int powerUpyValue = 30;

    bool allChange;
    public Text allChangeText;

    public GameObject bullet;

    bool isAutoBalance;
    public Text autoBalanceText;

    public int maxPowerUp = 1;
    public Text maxPowerUpText;

    public bool allSameRandomSPU;
    public Text allSameRandomSPUText;

    public GameObject laser;
    public GameObject laserIndicator;
    public Text laserText;

    public GameObject scatterIndicator;
    public Text scatterText;

    public GameObject tripleShotIndicator;
    public Text tripleText;

    public GameObject freezer;
    public GameObject freezerIndicator;
    public Text freezerText;

    public GameObject shieldIndicator;
    public Text shieldText;

    public GameObject mine;
    public GameObject mineIndicator;
    public Text mineText;

    public GameObject bouncyBullet;
    public GameObject BBIndicator;
    public Text BBText;

    public GameObject jousterIndicator;
    public Text jousterText;

    GameManager gameManagerScript;
    ScoreManager scoreManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        allPowerUps.Add(new KeyValuePair<string, GameObject>("Laser Beam", laserIndicator));
        allPowerUps.Add(new KeyValuePair<string, GameObject>("Scatter Shot", scatterIndicator));
        allPowerUps.Add(new KeyValuePair<string, GameObject>("Triple Shot", tripleShotIndicator));
        allPowerUps.Add(new KeyValuePair<string, GameObject>("Freezer", freezerIndicator));
        allPowerUps.Add(new KeyValuePair<string, GameObject>("Shield", shieldIndicator));
        allPowerUps.Add(new KeyValuePair<string, GameObject>("Proximity Mine", mineIndicator));
        allPowerUps.Add(new KeyValuePair<string, GameObject>("Bouncy Bullet", BBIndicator));
        allPowerUps.Add(new KeyValuePair<string, GameObject>("Jouster", jousterIndicator));

        foreach (KeyValuePair<string, GameObject> curr in allPowerUps)
        {
            SPU.Add(curr.Key);
            indicators.Add(curr.Value);
        }
        indicators.Remove(tripleShotIndicator);
        SPU.Remove("Triple Shot");
        SPU.Remove("Shield");

        //set text
        allText.Add(laserText);
        allText.Add(scatterText);
        allText.Add(tripleText);
        allText.Add(freezerText);
        allText.Add(shieldText);
        allText.Add(mineText);
        allText.Add(BBText);
        allText.Add(jousterText);

        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        scoreManagerScript = GameObject.Find("Score Manager").GetComponent<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Auto Balance
    public void setAutoBalance()
    {
        if (isAutoBalance)
        {
            autoBalanceText.text = "Auto Balance: Off";
        }
        else
        {
            autoBalanceText.text = "Auto Balance: On";
        }
        isAutoBalance = !isAutoBalance;
    }

    public void autoBalance()
    {
        if (isAutoBalance)
        {
            //find maxScore
            int maxScore = scoreManagerScript.P1Score;
            maxScore = Mathf.Max(maxScore, scoreManagerScript.P2Score);
            maxScore = Mathf.Max(maxScore, scoreManagerScript.P3Score);
            maxScore = Mathf.Max(maxScore, scoreManagerScript.P4Score);
            maxScore = Mathf.Max(maxScore, scoreManagerScript.P5Score);

            //helper
            foreach (List<GameObject> shipList in gameManagerScript.inGameShips)
            {
                foreach (GameObject ship in shipList)
                {
                    switch (ship.GetComponent<MutualShip>().id)
                    {
                        case 1:
                            getPowerUp(scoreManagerScript.P1Score, maxScore, ship.GetComponent<MutualShip>());
                            break;
                        case 2:
                            getPowerUp(scoreManagerScript.P2Score, maxScore, ship.GetComponent<MutualShip>());
                            break;
                        case 3:
                            getPowerUp(scoreManagerScript.P3Score, maxScore, ship.GetComponent<MutualShip>());
                            break;
                        case 4:
                            getPowerUp(scoreManagerScript.P4Score, maxScore, ship.GetComponent<MutualShip>());
                            break;
                        case 5:
                            getPowerUp(scoreManagerScript.P5Score, maxScore, ship.GetComponent<MutualShip>());
                            break;
                    }
                }
            }
        }
    }

    //for auto balance
    void getPowerUp(int score, int maxScore, MutualShip script)
    {
        if (score < maxScore - 1)
        {
            script.hasShield = true;
        }
        if (score < maxScore - 2)
        {
            script.tripleShot = true;
        }
    }

    //Starting Power Up

    public void setRandomSPUAllSame()
    {
        if (allSameRandomSPU)
        {
            allSameRandomSPUText.text = "Random Starting PowerUp: Off";
        }
        else
        {
            allSameRandomSPUText.text = "Random Starting PowerUp: On";
        }

        allSameRandomSPU = !allSameRandomSPU;
    }

    //PowerUP

    public void AllChange()
    {
        if (allChange)
        {
            //all on
            while (indicators.Count > 0)
            {
                indicators.RemoveAt(0);
            }

            foreach (KeyValuePair<string, GameObject> indicator in allPowerUps)
            {
                indicators.Add(indicator.Value);
            }

            for (int i = 0; i < allText.Count; i++)
            {
                allText[i].text = allPowerUps[i].Key + " On";
            }
            allChangeText.text = "All Off";
        }
        else
        {
            //all off
            while (indicators.Count > 0)
            {
                indicators.RemoveAt(0);
            }

            for (int i = 0; i < allText.Count; i++)
            {
                allText[i].text = allPowerUps[i].Key + " Off";
            }

            allChangeText.text = "All On";
        }
        allChange = !allChange;
    }

    public void increaseUsage()
    {
        maxPowerUp++;
        if (maxPowerUp >= 10)
        {
            maxPowerUp = 1;
        }

        maxPowerUpText.text = "" + maxPowerUp;
    }

    public void decreaseUsage()
    {
        maxPowerUp--;
        if (maxPowerUp <= 0)
        {
            maxPowerUp = 9;
        }

        maxPowerUpText.text = "" + maxPowerUp;
    }


    public void setLaser()
    {
        if (indicators.Contains(laserIndicator))
        {
            indicators.Remove(laserIndicator);
            SPU.Remove("Laser Beam");
            laserText.text = "Laser Beam: Off";
        }
        else
        {
            indicators.Add(laserIndicator);
            SPU.Add("Laser Beam");
            laserText.text = "Laser Beam: On";
        }
    }

    public void setScatterShot()
    {
        if (indicators.Contains(scatterIndicator))
        {
            indicators.Remove(scatterIndicator);
            SPU.Remove("Scatter Shot");
            scatterText.text = "Scatter Shot: Off";
        }
        else
        {
            indicators.Add(scatterIndicator);
            SPU.Add("Scatter Shot");
            scatterText.text = "Scatter Shot: On";
        }
    }

    public void setTripleShot()
    {
        if (indicators.Contains(tripleShotIndicator))
        {
            indicators.Remove(tripleShotIndicator);
            tripleText.text = "Triple Shot: Off";
        }
        else
        {
            indicators.Add(tripleShotIndicator);
            tripleText.text = "Triple Shot: On";
        }
    }

    public void setFreezer()
    {
        if (indicators.Contains(freezerIndicator))
        {
            indicators.Remove(freezerIndicator);
            SPU.Remove("Freezer");
            freezerText.text = "Freezer: Off";
        }
        else
        {
            indicators.Add(freezerIndicator);
            SPU.Add("Freezer");
            freezerText.text = "Freezer: On";
        }
    }

    public void setShield()
    {
        if (indicators.Contains(shieldIndicator))
        {
            indicators.Remove(shieldIndicator);
            shieldText.text = "Shield: Off";
        }
        else
        {
            indicators.Add(shieldIndicator);
            shieldText.text = "Shield: On";
        }
    }

    public void setMine()
    {
        if (indicators.Contains(mineIndicator))
        {
            indicators.Remove(mineIndicator);
            SPU.Remove("Proximity Mine");
            mineText.text = "Proximity Mine: Off";
        }
        else
        {
            indicators.Add(mineIndicator);
            SPU.Add("Proximity Mine");
            mineText.text = "Proximity Mine: On";
        }
    }

    public void setBB()
    {
        if (indicators.Contains(BBIndicator))
        {
            indicators.Remove(BBIndicator);
            SPU.Remove("Bouncy Bullet");
            BBText.text = "Bouncy Bullet: Off";
        }
        else
        {
            indicators.Add(BBIndicator);
            SPU.Add("Bouncy Bullet");
            BBText.text = "Bouncy Bullet: On";
        }
    }

    public void setJouster()
    {
        if (indicators.Contains(jousterIndicator))
        {
            indicators.Remove(jousterIndicator);
            SPU.Remove("Jouster");
            jousterText.text = "Jouster: Off";
        }
        else
        {
            indicators.Add(jousterIndicator);
            SPU.Add("Jouster");
            jousterText.text = "Jouster: On";
        }
    }

    public void dropItem(MutualShip script)
    {
        if (script.shootMode != "normal")
        {
            foreach (KeyValuePair<string, GameObject> curr in allPowerUps)
            {
                if (script.shootMode == curr.Key)
                {
                    GameObject toAdd = Instantiate(curr.Value,
                        new Vector3(script.transform.position.x, powerUpyValue, script.transform.position.z),
                        curr.Value.transform.rotation);
                    gameManagerScript.inGameIndicators.Add(toAdd);
                }
            }
        }
    }
}
