using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighlightModeManager : MonoBehaviour
{
    GameManager gameManagerScript;
    ScoreManager scoreManagerScript;
    SpawnManager spawnManagerScript;

    float[] times;
    public TextMeshProUGUI[] PTime;
    List<KeyValuePair<TextMeshProUGUI, float>> data;

    public int totalTime = 60;

    public GameObject crown;
    public GameObject crownPic;
    int crownY = 10;
    float startCrownY;
    float startTimeY;
    public float len;
    public Canvas canvas;
    float updateTime;

    public bool started;

    public GameObject inGameCrown;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        scoreManagerScript = GameObject.Find("Score Manager").GetComponent<ScoreManager>();
        spawnManagerScript = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();

        //set variables
        canvas.gameObject.SetActive(true);
        startTimeY = PTime[0].gameObject.transform.position.y;
        startCrownY = crownPic.gameObject.transform.position.y;
        float scale = canvas.scaleFactor;
        len *= scale;
        canvas.gameObject.SetActive(false);

        times = new float[5];
    }

    // Update is called once per frame
    void Update()
    {
        int ID = -1;

        if (started)
        {
            //solo / team

            //find highlighted ship
            foreach (List<GameObject> shipList in gameManagerScript.inGameShips)
            {
                foreach (GameObject ship in shipList)
                {
                    if (ship.GetComponent<MutualShip>() != null)
                    {
                        if (ship.GetComponent<MutualShip>().highlighed)
                        {
                            ID = ship.GetComponent<MutualShip>().id;
                        }
                    }
                }
            }

            if (ID == -1)
            {
                crownPic.SetActive(false);
                //spawn if no crown
                if (inGameCrown == null)
                {
                    inGameCrown = Instantiate(crown, spawnManagerScript.generateRanPos(crownY), crown.transform.rotation);
                }
            }
            else
            {
                times[ID - 1] -= Time.deltaTime;

                //change time
                PTime[ID - 1].text = "P" + ID + ": " + (int)times[ID-1];

                //check for ending
                if (times[ID - 1] <= 0)
                {
                    reset();
                    displayWinner(ID);
                    gameManagerScript.endRound();
                }

                //Update Crown
                crownPic.SetActive(true);

                //order the time
                if (updateTime <= 0)
                {
                    //update the time values
                    for (int i = data.Count - 1; i >= 0; i--)
                    {
                        if (data[i].Key == PTime[0])
                        {
                            data.Add(new KeyValuePair<TextMeshProUGUI, float>(data[i].Key, times[0]));
                        }
                        else if (data[i].Key == PTime[1])
                        {
                            data.Add(new KeyValuePair<TextMeshProUGUI, float>(data[i].Key, times[1]));
                        }
                        else if (data[i].Key == PTime[2])
                        {
                            data.Add(new KeyValuePair<TextMeshProUGUI, float>(data[i].Key, times[2]));
                        }
                        else if (data[i].Key == PTime[3])
                        {
                            data.Add(new KeyValuePair<TextMeshProUGUI, float>(data[i].Key, times[3]));
                        }
                        else if (data[i].Key == PTime[4])
                        {
                            data.Add(new KeyValuePair<TextMeshProUGUI, float>(data[i].Key, times[4]));
                        }

                        data.RemoveAt(i);
                    }

                    updateTime = 1;

                    TextMeshProUGUI curr = PTime[ID - 1];
                    //to prevent equal case
                    data.Sort((x, y) => x.Value.CompareTo(y.Value + 0.1f));
                    for (int i = 0; i < data.Count; i++)
                    {
                        //set time pos
                        Vector3 pos = data[i].Key.transform.position;
                        data[i].Key.transform.position = new Vector3(pos.x, startTimeY + i * len, pos.z);

                        if (data[i].Key == curr)
                        {
                            //set crown pos
                            pos = crownPic.transform.position;
                            crownPic.transform.position = new Vector3(pos.x, startCrownY + i * len, pos.z);
                        }
                    }
                }
                else
                {
                    updateTime -= Time.deltaTime;
                }
            }
        }
    }

    public void startRound()
    {
        started = true;

        //time text
        times = new float[5];
        for (int i = 0; i < 5; i++)
        {
            times[i] = totalTime;
        }

        data = new List<KeyValuePair<TextMeshProUGUI, float>>();

        for (int i = 0; i < PTime.Length; i++)
        {
            PTime[i].text = "P" + (i + 1) + ": " + (int)times[i];
            PTime[i].gameObject.SetActive(false);
        }

        foreach (List<GameObject> shipList in gameManagerScript.inGameShips)
        {
            foreach (GameObject ship in shipList)
            {
                int i = ship.GetComponent<MutualShip>().id - 1;

                PTime[i].gameObject.SetActive(true);

                //for ordering
                data.Add(new KeyValuePair<TextMeshProUGUI, float>(PTime[i], times[i]));
            }
        }

        //set time pos
        for (int i = 0; i < data.Count; i++)
        {
            Vector3 pos = data[i].Key.transform.position;
            data[i].Key.transform.position = new Vector3(pos.x, startTimeY + i * len, pos.z);
        }

       inGameCrown = Instantiate(crown, spawnManagerScript.generateRanPos(crownY), crown.transform.rotation);
    }

    public void assign(int ID, Vector3 pos, bool suicided)
    {
        if (suicided)
        {
            inGameCrown = Instantiate(crown, new Vector3(pos.x, crownY, pos.z), crown.transform.rotation);
        }
        else
        {
            //assign to the ship that killed it
            foreach (List<GameObject> shipList in gameManagerScript.inGameShips)
            {
                foreach (GameObject ship in shipList)
                {
                    if (ship.GetComponent<MutualShip>() != null)
                    {
                        if (ship.GetComponent<MutualShip>().id == ID)
                        {
                            ship.GetComponent<MutualShip>().highlighed = true;
                        }
                    }
                }
            }
        }
    }

    public void displayWinner(int winner)
    {
        //solo vs team
        switch (winner)
        {
            case 1:
                scoreManagerScript.P1WinText.SetActive(true);
                break;
            case 2:
                scoreManagerScript.P2WinText.SetActive(true);
                break;
            case 3:
                scoreManagerScript.P3WinText.SetActive(true);
                break;
            case 4:
                scoreManagerScript.P4WinText.SetActive(true);
                break;
            case 5:
                scoreManagerScript.P5WinText.SetActive(true);
                break;
        }
        scoreManagerScript.endScreen.SetActive(true);
        scoreManagerScript.pauseText.SetActive(false);
    }

    public void reset()
    {
        started = false;

        Destroy(inGameCrown);
    }
}
