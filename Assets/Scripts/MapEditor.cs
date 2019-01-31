using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                ResourceStore resStore = resourceOptions["grass"].shallowCopy();
                // only store the fraction specified by level
                resStore.amountStored = (int)Math.Round(resStore.amountStored * level);
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
                level = ((float)j / (float)map[i].Count) * maxAmt;
                // get copy of resource to be added
                ResourceStore resStore = resourceOptions[resource].shallowCopy();
                // only store the fraction specified by level
                resStore.amountStored = (int)Math.Round(resStore.amountStored * level);
                // add resource to the properties of that land
                map[i][j].propertyDict.Add(resource, resStore);
            }
        }
    }

    public void generateMap(int length, int width)
    {
        for (int i = 0; i < length; i++)
        {
            List<Land> row = new List<Land>();
            // add each row
            map.Add(row);
            for (int j = 0; j < width; j++)
            {
                // add land to each column
                row.Add(new Land());
            }

        }
    }
}