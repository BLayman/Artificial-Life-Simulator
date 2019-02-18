using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for running simulation.
/// </summary>
public class SimRunnerTest : MonoBehaviour
{
    public GameObject tilePrefab;
    public GameObject stepsText;
    List<List<GameObject>> tiles = new List<List<GameObject>>();
    float elapsedTime = 0.0f;
    float intervalTime = .25f; // updates every fraction of a second if possible
    ThreadManager threader;
    int intervalSteps = 1;
    bool paused = false;

    // Use this for initialization
    void Start()
    {
        // create threader
        threader = gameObject.GetComponent<ThreadManager>();
        // perform initial renderings
        startRender(threader.getEcosystem());
        updateRender(threader.getEcosystem()); // is this one necessary? costly?
        // set steps for each interval
        threader.setSteps(intervalSteps);
        // initiate simulation on child thread
        threader.StartEcoSim();

    }


    public void flipPaused()
    {
        if (paused)
        {
            paused = false;
        }
        else
        {
            paused = true;
        }
    }

    public void setSteps()
    {
        string text = stepsText.GetComponent<Text>().text;
        bool valid = HelperSetter.validateIntegerString(text);
        if (valid)
        {
            threader.setSteps(Int32.Parse(text));
        }
        
    }

    public void getValFromSlider(float value)
    {
        // TODO: don't hard code slider
        intervalTime = .5f - value;
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            elapsedTime += Time.deltaTime;
            // only update every intervalTime seconds
            if (elapsedTime > intervalTime)
            {
                // call function to update ecosystem if the child thread has added ecosystem objects to the queue
                bool updated = threader.updateEcoIfReady();
                // only re-render when necessary (the ecosystem has changed)
                if (updated)
                {
                    updateRender(threader.getEcosystem());
                }
                elapsedTime = 0.0f; // reset timer
            }
        }
        else // paused
        {
            // TODO: render user changes?
        }
        

    }


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

    // TODO : make Getter classes for ecosystem, creature, and other classes to retrieve information from them to use in the UI

}
