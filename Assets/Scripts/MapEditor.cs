using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class MapEditor
{
    /// <summary>
    /// Map to be edited
    /// </summary>
    public List<List<Land>> map;

    public MapEditor(List<List<Land>> _map)
    {
        map = _map;
    }

    /// <summary>
    /// Adds resource uniformly to all lands on map.
    /// </summary>
    /// <param name="level">Fraction of max possible amount of resource. Must be between 0 and 1.</param>
    public void addUniformResource(string resource, float level)
    {
        throw new System.NotImplementedException();
    }
}