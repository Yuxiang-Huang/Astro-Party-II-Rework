using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AllSPUManager : MonoBehaviour
{
    public List<GameObject> SPUPlayers;

    GameManager gameManagerScript;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setRandomAllSPU()
    {
        foreach (GameObject SPU in SPUPlayers)
        {
            SPUManager script = SPU.GetComponent<SPUManager>();
            script.setOnHelper("Random Starting PowerUp", script.RandomSPUText, script.id);
        }
    }

    public void setLaserAllSPU()
    {
        foreach (GameObject SPU in SPUPlayers)
        {
            SPUManager script = SPU.GetComponent<SPUManager>();
            script.setOnHelper("Laser Beam", script.SPULaserText, script.id);
        }
    }

    public void setScatterAllSPU()
    {
        foreach (GameObject SPU in SPUPlayers)
        {
            SPUManager script = SPU.GetComponent<SPUManager>();
            script.setOnHelper("Scatter Shot", script.SPUScatterText, script.id);
        }
    }

    public void setFreezerAllSPU()
    {
        foreach (GameObject SPU in SPUPlayers)
        {
            SPUManager script = SPU.GetComponent<SPUManager>();
            script.setOnHelper("Freezer", script.SPUFreezerText, script.id);
        }
    }

    public void setMineAllSPU()
    {
        foreach (GameObject SPU in SPUPlayers)
        {
            SPUManager script = SPU.GetComponent<SPUManager>();
            script.setOnHelper("Proximity Mine", script.SPUMineText, script.id);
        }
    }

    public void setBBAllSPU()
    {
        foreach (GameObject SPU in SPUPlayers)
        {
            SPUManager script = SPU.GetComponent<SPUManager>();
            script.setOnHelper("Bouncy Bullet", script.SPUBBText, script.id);
        }
    }

    public void setJousterAllSPU()
    {
        foreach (GameObject SPU in SPUPlayers)
        {
            SPUManager script = SPU.GetComponent<SPUManager>();
            script.setOnHelper("Jouster", script.SPUJousterText, script.id);
        }
    }

    public void setTripleAllSPU()
    {
        foreach (List<GameObject> shipList in gameManagerScript.ships)
        {
            foreach (GameObject ship in shipList)
            {
                MutualShip script = ship.GetComponent<MutualShip>();
                script.tripleShot = true;
            }
        }

        foreach (GameObject SPU in SPUPlayers)
        {
            SPUManager SPUScript = SPU.GetComponent<SPUManager>();
            SPUScript.SPUTripleText.text = "Triple Shot: On";
        }
    }

    public void setShieldAllSPU()
    {
        foreach (List<GameObject> shipList in gameManagerScript.ships)
        {
            foreach (GameObject ship in shipList)
            {
                MutualShip script = ship.GetComponent<MutualShip>();
                script.hasShield = true;
            }
        }

        foreach (GameObject SPU in SPUPlayers)
        {
            SPUManager SPUScript = SPU.GetComponent<SPUManager>();
            SPUScript.SPUShieldText.text = "Shield: On";
        }
    }

    public void SPUAllOff()
    {
        //for every ship
        foreach (List<GameObject> shipList in gameManagerScript.ships)
        {
            foreach (GameObject ship in shipList)
            {
                MutualShip script = ship.GetComponent<MutualShip>();
                if (script.shootMode != "normal")
                {
                    //check every SPU Manager
                    foreach (GameObject SPU in SPUPlayers)
                    {
                        SPUManager SPUScript = SPU.GetComponent<SPUManager>();
                        if (SPUScript.id == script.id)
                        {
                            SPUScript.SPUCurrText.text = script.shootMode + ": Off";
                            SPUScript.SPUCurrText = null;
                        }
                    }
                    script.shootMode = "normal";
                    script.hasShield = false;
                    script.tripleShot = false;
                }    
            }
        }

        //switch text of triple and shield to off
        foreach (GameObject SPU in SPUPlayers)
        {
            SPUManager SPUScript = SPU.GetComponent<SPUManager>();
            SPUScript.SPUTripleText.text = "Triple Shot: Off";
            SPUScript.SPUShieldText.text = "Shield: Off";
        }
    }
}
