using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class Ability
{
    /// <summary>
    /// Name of resource to consume, or species to attack or defend against.
    /// </summary>
    public string target;
    /// <summary>
    /// Strength of ability.
    /// </summary>
    public int level;
    /// <summary>
    /// NOT IN USE. List of BoostRequirements that could give this ability a boost if the resource needs are met.
    /// </summary>
    public List<BoostRequirement> boostOptions;
}