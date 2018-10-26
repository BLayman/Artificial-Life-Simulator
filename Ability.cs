using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Ability
{
    /// <summary>
    /// Name of resource to consume, or species to attack or defend against.
    /// </summary>
    public sting target;
    /// <summary>
    /// Strength of ability.
    /// </summary>
    public int level;
    private Tuple resourcesForBoost;
}