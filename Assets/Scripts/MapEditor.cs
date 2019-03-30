// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// API for editing the map: a 2D array of Land spaces. Used to generate the map, and add resources.
/// </summary>
public class MapEditor
{
    /// <summary>
    /// Map to be edited
    /// </summary>
    public List<List<Land>> map;
    /// <summary>
    /// Resources to be distributed across map.
    /// </summary>
    private Dictionary<string, ResourceStore> resourceOptions;

    public MapEditor(List<List<Land>> _map, Dictionary<string, ResourceStore> _resOptions)
    {
        map = _map;
        resourceOptions = _resOptions;
    }

    /// <summary>
    /// Adds resource uniformly to all lands on map.
    /// </summary>
    /// <param name="level">Fraction of max possible amount of resource. Must be between 0 and 1.</param>
    public void addUniformResource(string resource, float level)
    {
        // for every row and column
        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j < map[i].Count; j++)
            {
                // get copy of resource to be added
                ResourceStore resStore = resourceOptions[resource].shallowCopy();
                // only store the fraction specified by level
                resStore.amountStored = resStore.maxAmount * level;
                // add resource to the properties of that land
                map[i][j].propertyDict.Add(resource, resStore);
            }
        }
    }

    public void addLERPXResource(string resource, float maxAmt)
    {
        float level = 0;
        // for every row and column
        for (int i = 0; i < map.Count; i++)
        {
            for (int j = 0; j < map[i].Count; j++)
            {
                level = ((float)j / (float)map[i].Count);
                // get copy of resource to be added
                ResourceStore resStore = resourceOptions[resource].shallowCopy();
                // only store the fraction specified by level
                resStore.amountStored = resStore.maxAmount * level;
                // add resource to the properties of that land
                map[i][j].propertyDict.Add(resource, resStore);
            }
        }
        //Debug.Log("grass stored at 10,10: " + map[10][10].propertyDict["grass"].amountStored);
    }

    public void generateMap(int length, int width)
    {
        for (int i = 0; i < length; i++)
        {
            List<Land> column = new List<Land>();
            // add each row
            map.Add(column);
            for (int j = 0; j < width; j++)
            {
                // add land to each column
                column.Add(new Land());
            }

        }
        //Debug.Log("finished generating map");
    }
}