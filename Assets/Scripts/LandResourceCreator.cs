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
        resourceStore.amountConsumedPerTimeUnit = amountConsumed;
    }

    public void setRenewalAmt(int amountRenewed)
    {
        resourceStore.renewalAmt = amountRenewed;
    }

    public void setAmountOfResource(int amount)
    {
        resourceStore.amountStored = amount;
    }

    /// <summary>
    /// Sets fraction of resource consumed per time unit that the creature actually recieves if creature does not have an ability for this.
    /// </summary>
    /// <param name="proportion">Must be between 0 and 1.</param>
    public void setProportionExtracted(float proportion)
    {
        resourceStore.proportionExtracted = proportion;
    }

    /// <summary>
    /// Sets maximum amount of the resource that can be stored.
    /// </summary>
    public void setMaxAmt(int maxAmount)
    {
        resourceStore.maxAmount = maxAmount;
    }
}