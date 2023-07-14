using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map6PortalManager : MonoBehaviour
{
    public GameObject portals;

    List<GameObject> portalParentList;
    List<GameObject> portalList;

    public Material blue1;
    public Material red2;
    public Material yellow3;
    public Material cyan4;
    public Material green5;

    public int spawnRadius = 500;

    MapManager mapManagerScript;

    int mapId;

    GameObject pivot;

    private void Awake()
    {
        mapManagerScript = GameObject.Find("Map Manager").GetComponent<MapManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void reset()
    {
        bool fixedSpawn = mapManagerScript.isFixedSpawned(GetComponent<Map>().mapID);

        //clear
        if (portalList != null)
        {
            while (portalList.Count > 0)
            {
                Destroy(portalList[0]);
                portalList.RemoveAt(0);
            }

            while (portalParentList.Count > 0)
            {
                Destroy(portalParentList[0]);
                portalParentList.RemoveAt(0);
            }
        }

        if (pivot != null)
        {
            Destroy(pivot);
        }
        pivot = new GameObject ();
        pivot.transform.position = new Vector3(0, 0, 0);
        pivot.transform.parent = transform;

        portalParentList = new List<GameObject>();
        portalList = new List<GameObject>();

        //spawn five portals
        for (int i = 0; i < 5; i++)
        {
            portalParentList.Add(Instantiate(portals, new Vector3(Mathf.Cos(i * 2 * Mathf.PI / 5 + Mathf.PI/2) * spawnRadius,
                0, Mathf.Sin(i * 2 * Mathf.PI / 5 + Mathf.PI / 2) * spawnRadius),
                portals.transform.rotation));

            //fixed the angles
            if (fixedSpawn)
            {
                int angle = 0;

                switch (i)
                {
                    case 1: angle = -72; break;
                    case 2: angle = 28; break;
                    case 3: angle = -28; break;
                    case 4: angle = 72; break;
                }
                portalParentList[i].transform.Rotate(0, angle, 0);
            }
            else
            {
                portalParentList[i].transform.Rotate(0, Random.Range(0, 360), 0);
            }

            portalParentList[i].transform.parent = pivot.transform;

            portalList.Add(portalParentList[i].GetComponent<PortalParent>().portal1);
            portalList.Add(portalParentList[i].GetComponent<PortalParent>().portal2);
        }

        //rotate the portals
        if (!fixedSpawn)
        {
            pivot.transform.Rotate(0, Random.Range(0, 360), 0);
        }
        else
        {
            portalList.Add(portalList[0]);
            portalList.RemoveAt(0);
        }

        //color and set pairs
        int color = 1;

        while (portalList.Count > 0)
        {
            int ran = Random.Range(1, portalList.Count);

            if (fixedSpawn)
            {
                ran = 1;
            }

            portalList[0].GetComponent<Portal>().pair = portalList[ran];
            portalList[ran].GetComponent<Portal>().pair = portalList[0];

            switch (color)
            {
                case 1:
                    portalList[0].GetComponent<Portal>().rend.material = blue1;
                    portalList[ran].GetComponent<Portal>().rend.material = blue1;
                    break;
                case 2:
                    portalList[0].GetComponent<Portal>().rend.material = red2;
                    portalList[ran].GetComponent<Portal>().rend.material = red2;
                    break;
                case 3:
                    portalList[0].GetComponent<Portal>().rend.material = yellow3;
                    portalList[ran].GetComponent<Portal>().rend.material = yellow3;
                    break;
                case 4:
                    portalList[0].GetComponent<Portal>().rend.material = cyan4;
                    portalList[ran].GetComponent<Portal>().rend.material = cyan4;
                    break;
                case 5:
                    portalList[0].GetComponent<Portal>().rend.material = green5;
                    portalList[ran].GetComponent<Portal>().rend.material = green5;
                    break;
            }

            portalList.Remove(portalList[ran]);
            portalList.Remove(portalList[0]);

            color++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
