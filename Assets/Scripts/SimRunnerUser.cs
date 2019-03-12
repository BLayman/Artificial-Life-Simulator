// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for running simulation.
/// </summary>
public class SimRunnerUser : MonoBehaviour
{
    Ecosystem eco;
    public GameObject tilePrefab;
    List<List<GameObject>> tiles = new List<List<GameObject>>();
    float elapsedTime = 0.0f;
    float intervalTime = .2f;
    bool run = false;
    int stepInterval = 1;


    // Update is called once per frame
    void Update()
    {
        if (run)
        {
            Debug.Log("sim runner user running");
            // only update every intervalTime seconds
            elapsedTime += Time.deltaTime;
            if (elapsedTime > intervalTime)
            {
                updateRender(eco);
                eco.runSystem(stepInterval);
                elapsedTime = 0.0f;
            }
        }
    }

    public void startSim(int interval)
    {
        stepInterval = interval;
        startRender(eco);
        updateRender(eco);
        run = true;
    }

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
