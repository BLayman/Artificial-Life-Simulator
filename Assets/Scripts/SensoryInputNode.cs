using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class SensoryInputNode : Node
{
    /// <summary>
    /// specifies which neighbor Land (0 - 8) this node gets it's input from: 0 for currentPos
    /// </summary>
    public int neighborLandIndex;
    /// <summary>
    /// string designating resource to be found in neighbor dictionary
    /// </summary>
    public string sensedResource;
    /// <summary>
    /// stores a reference to the creature it belongs to (for getting neighbors)
    /// </summary>
    private Creature creature;

    /// <summary>
    /// Uses neighborIndex and key with propertyDict to get an updated value
    /// </summary>
    public float senseValFromNeighbor()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// calls senseValFromNeighbor to updateValue
    /// </summary>
    public override void updateValue()
    {
        throw new System.NotImplementedException();
    }
}