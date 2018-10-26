using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ResourceCreator
{
    /// <summary>
    /// Resource being created.
    /// </summary>
    public CreatureResource resource;

    public ResourceCreator(CreatureResource _resource)
    {
        resource = _resource;
    }

    public void setLevel(int resourceLevel)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Set amount of health drained per healthUpdate when below deficiency threshold.
    /// </summary>
    /// <param name="drainAmt">Amount of health drained per healthUpdate when below deficiency threshold.</param>
    public void setDeficiencyHealthDrain(int drainAmt)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Set threshold at which deficiency occurs (health drain takes effect).
    /// </summary>
    /// <param name="deficiencyThreshold">Threshold at which deficiency occurs (health drain takes effect).</param>
    public void setDeficiencyThreshold(int deficiencyThreshold)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Set health gained when health gain threshold is met.
    /// </summary>
    /// <param name="healthGain">Health gained when health gain threshold is met.</param>
    public void setHealthGain(int healthGain)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Set threshold at which health is gained when it's surpassed.
    /// </summary>
    /// <param name="gainThreshold">Threshold at which health is gained when it's surpassed.</param>
    public void setHealthGainThreshold(int gainThreshold)
    {
        throw new System.NotImplementedException();
    }
}