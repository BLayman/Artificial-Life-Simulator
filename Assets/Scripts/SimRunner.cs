using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimRunner : MonoBehaviour {
    EcoManager ecoMan;
    public GameObject tilePrefab;
    List<List<GameObject>> tiles = new List<List<GameObject>>();
    float elapsedTime = 0.0f;
    float intervalTime = 1.0f;

	// Use this for initialization
	void Start () {
        ecoMan = new EcoManager();
        ecoMan.makeEco();
        startRender(ecoMan.getEcosystem());
        updateRender(ecoMan.getEcosystem());
    }
	
	// Update is called once per frame
	void Update () {
        // only update every intervalTime seconds
        elapsedTime += Time.deltaTime;
        if (elapsedTime > intervalTime)
        {
            updateRender(ecoMan.getEcosystem());
            ecoMan.runSystem(5);
            elapsedTime = 0.0f;
        }

        /*
        ecoMan.runSystem(1);
        render(ecoMan.getEcosystem());
        clearOldMap();

        GameObject tile = GameObject.Instantiate(tilePrefab, new Vector3(1, 1, 0), Quaternion.identity);
        GameObject tile2 = GameObject.Instantiate(tilePrefab, new Vector3(0, 0, 0), Quaternion.identity);

        tiles.Add(tile);
        tiles.Add(tile2);
        */
    }

    /*
    private void clearOldMap()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            GameObject.Destroy(tiles[i]);
        }
        tiles.Clear();
    }
    */

    private void startRender(Ecosystem sys)
    {
        //Debug.Log("in render");
        for (int i = 0; i < sys.map.Count; i++)
        {
            tiles.Add(new List<GameObject>());
            for (int j = 0; j < sys.map[i].Count; j++)
            {
                GameObject tile = GameObject.Instantiate(tilePrefab, new Vector3(j, i, 0), Quaternion.identity);
                float proportionStored = (float)sys.map[i][j].propertyDict["grass"].getProportionStored();
                //Debug.Log(proportionStored);
                tile.GetComponent<SpriteRenderer>().color = new Color(proportionStored, proportionStored, proportionStored);
                tiles[i].Add(tile);
            }
        }
    }

    private void updateRender(Ecosystem sys)
    {
        //Debug.Log("in render");
        GameObject tile;
        ResourceStore store;
        Color updatedColor = new Color(1,1,1);
        float proportionStored;
        List<List<Land>> map = sys.map;

        for (int i = 0; i < sys.map.Count; i++)
        {
            for (int j = 0; j < sys.map[i].Count; j++)
            {
                tile = tiles[i][j];
                if (map[i][j].creatureIsOn())
                {
                    updatedColor = Color.blue;
                    tile.GetComponent<SpriteRenderer>().color = updatedColor;
                }
                else
                {
                    store = map[i][j].propertyDict["grass"];
                    proportionStored = (float)store.amountStored / (float)store.maxAmount;
                    //Debug.Log(proportionStored);
                    updatedColor.r = proportionStored;
                    updatedColor.g = proportionStored;
                    updatedColor.b = proportionStored;
                    tile.GetComponent<SpriteRenderer>().color = updatedColor;
                }
                
            }
        }
    }
}
