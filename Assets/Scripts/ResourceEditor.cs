using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// API for setting resources that creature can store, and how the resource effects the creature.
/// </summary>
public class ResourceEditor
{
    /// <summary>
    /// Resource being created.
    /// </summary>
    public CreatureResource resource;

    public ResourceEditor(CreatureResource _resource)
    {
        resource = _resource;
    }

    public void setLevel(int resourceLevel)
    {
        resource.currentLevel = resourceLevel;
    }

    /// <summary>
    /// Set amount of health drained per healthUpdate when below deficiency threshold.
    /// </summary>
    /// <param name="drainAmt">Amount of health drained per healthUpdate when below deficiency threshold.</param>
    public void setDeficiencyHealthDrain(int drainAmt)
    {
        resource.deficiencyHealthDrain = drainAmt;
    }

    /// <summary>
    /// Set threshold at which deficiency occurs (health drain takes effect).
    /// </summary>
    /// <param name="deficiencyThreshold">Threshold at which deficiency occurs (health drain takes effect).</param>
    public void setDeficiencyThreshold(int deficiencyThreshold)
    {
        resource.deficiencyThreshold = deficiencyThreshold;
    }

    /// <summary>
    /// Set health gained when health gain threshold is met.
    /// </summary>
    /// <param name="healthGain">Health gained when health gain threshold is met.</param>
    public void setHealthGain(int healthGain)
    {
        resource.healthGain = healthGain;
    }

    /// <summary>
    /// Set threshold at which health is gained when it's surpassed.
    /// </summary>
    /// <param name="gainThreshold">Threshold at which health is gained when it's surpassed.</param>
    public void setHealthGainThreshold(int gainThreshold)
    {
        resource.healthGainThreshold = gainThreshold;
    }

    public void setMaxLevel(int maxLevel)
    {
        resource.maxLevel = maxLevel;
    }

    public void setBaseUsage(int baseUsage)
    {
        resource.baseUsage = baseUsage;
    }

    public void setName(string name)
    {
        resource.name = name;
    }
}