using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputNodeCreator : NodeCreatorInterface
{
    public OutputNode foNode;
    public int nodeLayer;

    public OutputNodeCreator(OutputNode foNode, int nodeLayer)
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
            case ActivationBehaviorTypes.LogWithNeg:
                foNode.setActivBehavior(new LogisticWithNegActivBehav());
                break;
            default:
                break;
        }
    }

}