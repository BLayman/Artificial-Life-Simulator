using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Resources stored by a creature.
/// </summary>
public class CreatureResource
{
    /// <summary>
    /// Threshold below which resource causes health damage.
    /// </summary>
    private int deficiencyThreshold;
    /// <summary>
    /// Amount of health drain from deficiency in one time step.
    /// </summary>
    private int deficiencyHealthDrain;
    /// <summary>
    /// Amount of the resource currently stored.
    /// </summary>
    private int currentLevel;
    /// <summary>
    /// Amount of health gained when resource is adequate.
    /// </summary>
    private int healthGain;
    /// <summary>
    /// Threshold above which health is gained.
    /// </summary>
    private int healthGainThreshold;

    /// <summary>
    /// Update creature health according to resource level.
    /// </summary>
    public void healthUpdate(Creature creature)
    {
        throw new System.NotImplementedException();
    }
}