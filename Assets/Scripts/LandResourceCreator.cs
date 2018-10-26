using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class LandResourceCreator
{
    public ResourceStore resourceStore;

    public LandResourceCreator(ResourceStore _resourceStore)
    {
        resourceStore = _resourceStore;
    }

    public void setAmtConsumedPerTime(int amountConsumed)
    {
        throw new System.NotImplementedException();
    }

    public void setRenewalAmt(int amountRenewed)
    {
        throw new System.NotImplementedException();
    }

    public void setAmountOfResource(int amount)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Sets fraction of resource consumed per time unit that the creature actually recieves if creature does not have an ability for this.
    /// </summary>
    /// <param name="proportion">Must be between 0 and 1.</param>
    public void setProportionExtracted(float proportion)
    {
        throw new System.NotImplementedException();
    }
}