using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class Land
{
    /// <summary>
    /// Stores resource values and other attributes of the land
    /// </summary>
    public System.Collections.Generic.Dictionary<string, ResourceStore> propertyDict = new Dictionary<string, ResourceStore>();
    /// <summary>
    /// Stores whatever creature is currently on this land.
    /// </summary>
    public Creature creatureOn;

    /// <summary>
    /// returns true if creatureOn is not null
    /// </summary>
    public bool creatureIsOn()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Try to consume resource of given name.
    /// </summary>
    /// <param name="resourceKey">Resource name.</param>
    /// <param name="timeDedicated">Comes from action timeCost variable.</param>
    public float attemptResourceConsumption(string resourceKey, int timeDedicated, int creatureAbility)
    {
        throw new System.NotImplementedException();
    }


}