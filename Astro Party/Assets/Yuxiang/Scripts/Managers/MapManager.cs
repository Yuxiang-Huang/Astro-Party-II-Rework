using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    GameManager gameManagerScript;
    CameraManager cameraMangerScript;

    List<GameObject> currMaps = new List<GameObject>();
    List<GameObject> allMaps = new List<GameObject>();
    List<Text> allText = new List<Text>();
    List<string> mapFixedSpawn = new List<string>();

    string allChange = "All Off";
    public Text allChangeText;

    int currMapID;

    public GameObject backButton;

    public GameObject Map0;
    public Text Map0Text;

    public GameObject Map1;
    public Text Map1Text;
    public GameObject Map1rotatingInner;
    public GameObject Map1rotatingOuter;

    public GameObject Map2;
    public Text Map2Text;
    public List<GameObject> Map2rotatingObjects;
    public int velocity;
    public int velocity2;

    public GameObject Map3;
    public Text Map3Text;
    public List<GameObject> Map3ThreeBodyObjects;

    public GameObject Map4;
    public Text Map4Text;

    public GameObject Map5;
    public Text Map5Text;

    public GameObject Map6;
    public Text Map6Text;

    public GameObject Map7;
    public Text Map7Text;

    // Start is called before the first frame update
    void Start()
    {
        gameManagerScript = GameObject.Find("Game Manager").GetComponent<GameManager>();
        cameraMangerScript = GameObject.Find("Main Camera").GetComponent<CameraManager>();

        allMaps.Add(Map0);
        allMaps.Add(Map1);
        allMaps.Add(Map2);
        allMaps.Add(Map3);
        allMaps.Add(Map4);
        allMaps.Add(Map5);
        allMaps.Add(Map6);
        allMaps.Add(Map7);

        foreach (GameObject map in allMaps)
        {
            map.SetActive(false);
        }

        foreach (GameObject map in allMaps)
        {
            //if (map.GetComponent<Map>().mapID == 3)
            currMaps.Add(map);
            mapFixedSpawn.Add("Both");
        }

        allText.Add(Map0Text);
        allText.Add(Map1Text);
        allText.Add(Map2Text);
        allText.Add(Map3Text);
        allText.Add(Map4Text);
        allText.Add(Map5Text);
        allText.Add(Map6Text);
        allText.Add(Map7Text);
    }

    void Update()
    {
        if (currMaps.Count == 0)
        {
            backButton.SetActive(false);
        }
        else
        {
            backButton.SetActive(true);
        }
    }

    public void resetMapNotNext()
    {
        if (currMapID != 0)
        {
            foreach (GameObject currMap in currMaps)
            {
                if (currMap.GetComponent<Map>().mapID == currMapID)
                {
                    currMap.SetActive(false);
                }
            }
        }
    }

    public void resetMap()
    {
        //last map off
        if (currMapID != 0)
        {
            foreach (GameObject currMap in currMaps)
            {
                if (currMap.GetComponent<Map>().mapID == currMapID)
                {
                    currMap.SetActive(false);
                }
            }
        }

        //next map
        GameObject map = currMaps[Random.Range(0, currMaps.Count)];

        currMapID = map.GetComponent<Map>().mapID;

        foreach (GameObject curr in map.GetComponent<Map>().breakables)
        {
            curr.SetActive(true);
        }
        map.SetActive(true);

        gameManagerScript.spawnRadius = map.GetComponent<Map>().radius;

        //call function
        switch (currMapID)
        {
            case 0: reset0(); break;
            case 1: reset1(); break;
            case 2: reset2(); break;
            case 3: reset3(); break;
            case 4: reset4(); break;
            case 5: reset5(); break;
            case 6: reset6(); break;
            case 7: reset7(); break;
        }
    }


    public void AllChange()
    {
        string setString = "";

        //remove all maps
        while (currMaps.Count > 0)
        {
            currMaps.RemoveAt(0);
        }

        if (allChange == "All Off")
        {
            //all Off
            setString = "None";
            allChange = "All Fixed";
        }
        else
        {
           //add all maps
            foreach (GameObject map in allMaps)
            {
                currMaps.Add(map);
            }

            if (allChange == "All Fixed")
            {
                //all Off
                setString = "Fixed";
                allChange = "All Unfixed";
            }

            else if (allChange == "All Unfixed")
            {
                //all Off
                setString = "Unfixed";
                allChange = "All On";
            }

            else if (allChange == "All On")
            {
                //all Off
                setString = "Both";
                allChange = "All Off";
            }
        }

        //set string in mapFixedSpawn and allText
        for (int i = 0; i < mapFixedSpawn.Count; i++)
        {
            mapFixedSpawn[i] = setString;
        }

        foreach (Text mapText in allText)
        {
            mapText.text = setString;
        }

        allChangeText.text = allChange;
    }

    void reset0()
    {
        //nothing
    }

    void reset1()
    {
        if (isFixedSpawned(1))
        {
            //restore
            Map1rotatingInner.GetComponent<Map1RotatingObject>().velocity = 0;
            Map1rotatingOuter.GetComponent<Map1RotatingObject>().velocity = 0;
            Map1rotatingInner.transform.rotation = Quaternion.Euler(0, 0, 0);
            Map1rotatingOuter.transform.rotation = Quaternion.Euler(0, 45, 0);
        }
        else
        {
            //set random velocity
            float posV = Random.Range(0.5f, 1.25f);
            float negV = -Random.Range(0.5f, 1.25f);

            if (Random.Range(0, 2) == 0)
            {
                Map1rotatingInner.GetComponent<Map1RotatingObject>().velocity = negV;
                Map1rotatingOuter.GetComponent<Map1RotatingObject>().velocity = posV;
            }
            else
            {
                Map1rotatingInner.GetComponent<Map1RotatingObject>().velocity = posV;
                Map1rotatingOuter.GetComponent<Map1RotatingObject>().velocity = negV;

            }
        }
    }

    void reset2()
    {
        int radius = 200;

        velocity = Random.Range(125, 175);
        velocity2 = Random.Range(75, 125);

        if (isFixedSpawned(2))
        {
            velocity2 = velocity;
            radius = 300;
        } 

        foreach (GameObject curr in Map2rotatingObjects)
        {
            curr.GetComponent<Map2CircularMotion>().radius = radius;
            curr.GetComponent<Map2CircularMotion>().reset();
            curr.GetComponent<Map2CircularMotion>().velocity = velocity2;
        }
    }

    void reset3()
    {
        foreach (GameObject curr in Map3ThreeBodyObjects)
        {
            curr.GetComponent<Map3ThreeBody>().reset();
        }
    }

    void reset4()
    {
        Map4.GetComponent<Map4LaserBeamControl>().reset();
    }

    void reset5()
    {
        Map5.GetComponent<Map5SpiralBuilder>().reset();
    }

    void reset6()
    {
        Map6.GetComponent<Map6PortalManager>().reset();
        //fix camera for map6
        cameraMangerScript.startLock = true;
    }

    void reset7()
    {
        Map7.GetComponent<Map7FogManager>().reset();
    }

    public void Map0OnOff()
    {
        MapOnOffHelper(Map0, Map0Text);
    }

    public void Map1OnOff()
    {
        MapOnOffHelper(Map1, Map1Text);
    }

    public void Map2OnOff()
    {
        MapOnOffHelper(Map2, Map2Text);
    }

    public void Map3OnOff()
    {
        MapOnOffHelper(Map3, Map3Text);
    }

    public void Map4OnOff()
    {
        MapOnOffHelper(Map4, Map4Text);
    }

    public void Map5OnOff()
    {
        MapOnOffHelper(Map5, Map5Text);
    }

    public void Map6OnOff()
    {
        MapOnOffHelper(Map6, Map6Text);
    }

    public void Map7OnOff()
    {
        MapOnOffHelper(Map7, Map7Text);
    }

    void MapOnOffHelper(GameObject map, Text mapText)
    {
        int id = map.GetComponent<Map>().mapID;

        if (mapFixedSpawn[id] == "Fixed")
        {
            mapFixedSpawn[id] = "Unfixed";
        }

        else if (mapFixedSpawn[id] == "Unfixed")
        {
            mapFixedSpawn[id] = "Both";
        }

        else if (mapFixedSpawn[id] == "Both")
        {
            mapFixedSpawn[id] = "None";
            currMaps.Remove(map);
        }

        else if (mapFixedSpawn[id] == "None")
        {
            mapFixedSpawn[id] = "Fixed";
            currMaps.Add(map);
        }

        mapText.text = mapFixedSpawn[id];
    }

    public bool isFixedSpawned(int id)
    {
        if (mapFixedSpawn[id] == "Both")
        {
            return Random.Range(0, 2) == 1;
        }

        else if (mapFixedSpawn[id] == "Unfixed")
        {
            return false;
        }

        else if (mapFixedSpawn[id] == "Fixed")
        {
            return true;
        }

        return false;
    }
}
