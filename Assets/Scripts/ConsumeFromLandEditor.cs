using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ConsumeFromLandEditor : ActionEditorAbstract
{
    ConsumeFromLand actionRef;

    public ConsumeFromLandEditor(ConsumeFromLand inAction)
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