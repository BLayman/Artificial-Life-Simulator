using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ConsumeFromLandCreator : ActionCreatorAbstract
{
    ConsumeFromLand actionRef;

    public ConsumeFromLandCreator(ConsumeFromLand inAction)
    {
        action = inAction;
        actionRef = inAction;
    }

    public void setNeighborIndex(int index)
    {
        actionRef.neighborIndex = index;
    }

    public void setResourceToConsume(string res)
    {
        actionRef.resourceToConsume = res;
    }
}