// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// API for OutputNodes.
/// </summary>
public class OutputNodeEditor : NodeEditorInterface
{
    public OutputNode foNode;
    public int nodeLayer;

    public OutputNodeEditor(OutputNode foNode, int nodeLayer)
    {
        this.foNode = foNode;
        this.nodeLayer = nodeLayer;
    }

    public Node getNode()
    {
        return foNode;
    }

    // action key parameter comes from creature's actionPool keys
    public void setAction(Action a)
    {
        foNode.setAction(a);
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
            case ActivationBehaviorTypes.Tanh:
                foNode.setActivBehavior(new TanhActivBehav());
                break;
            default:
                Debug.LogError("Activation function type not found.");
                break;
        }
    }

}