using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalOutputNodeCreator : NodeCreatorInterface
{
    public FinalOutputNode foNode;
    public int nodeLayer;

    public FinalOutputNodeCreator(FinalOutputNode foNode, int nodeLayer)
    {
        this.foNode = foNode;
        this.nodeLayer = nodeLayer;
    }

    public Node getNode()
    {
        return foNode;
    }

    // action key parameter comes from creature's actionPool keys
    public void setAction(string actionKey)
    {
        foNode.setAction(actionKey);
    }


    public void setActivationFunction(ActivationBehaviorTypes activType)
    {
        switch (activType)
        {
            case ActivationBehaviorTypes.LogisticAB:
                foNode.setActivBehavior(new LogisticActivBehavior());
                break;
            case ActivationBehaviorTypes.EmptyAB:
                foNode.setActivBehavior(new EmptyActivBehavior());
                break;
            default:
                break;
        }
    }

}