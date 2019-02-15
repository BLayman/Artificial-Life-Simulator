using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for running simulation.
/// </summary>
public class SimRunnerTest : MonoBehaviour
{
    public GameObject tilePrefab;
    List<List<GameObject>> tiles = new List<List<GameObject>>();
    float elapsedTime = 0.0f;
    float intervalTime = .5f; // updates every fraction of a second if possible
    ThreadManager threader;
    int intervalSteps = 1;

    // Use this for initialization
    void Start()
    {
        threader = gameObject.GetComponent<ThreadManager>();
        startRender(threader.getEcosystem());
        updateRender(threader.getEcosystem());
        threader.steps = intervalSteps;
        threader.StartEcoSim();

    }

    // Update is called once per frame
    void Update()
    {
        // only update every intervalTime seconds
        elapsedTime += Time.deltaTime;
        if (elapsedTime > intervalTime)
        {
            bool updated = threader.updateEcoIfReady();
            if (updated)
            {
                updateRender(threader.getEcosystem());
            }
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

    // first render: called from Start function
    private void startRender(Ecosystem sys)
    {
        //Debug.Log("in render");
        for (int x = 0; x < sys.map.Count; x++)
        {
            tiles.Add(new List<GameObject>());
            for (int y = 0; y < sys.map[x].Count; y++)
            {
                GameObject tile = GameObject.Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                float proportionStored = (float)sys.map[x][y].propertyDict["grass"].getProportionStored();
                //Debug.Log(proportionStored);
                tile.GetComponent<SpriteRenderer>().color = new Color(proportionStored, proportionStored, proportionStored);
                tiles[x].Add(tile);
            }
        }
    }

    // called from update loop
    private void updateRender(Ecosystem sys)
    {
        //Debug.Log("in render");
        GameObject tile;
        ResourceStore store;
        Color updatedColor = new Color(1, 1, 1);
        float proportionStored;
        List<List<Land>> map = sys.map;

        for (int x = 0; x < sys.map.Count; x++)
        {
            for (int y = 0; y < sys.map[x].Count; y++)
            {
                tile = tiles[x][y];
                if (map[x][y].creatureIsOn())
                {
                    updatedColor = Color.blue;
                    tile.GetComponent<SpriteRenderer>().color = updatedColor;
                }
                else
                {
                    store = map[x][y].propertyDict["grass"];
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
