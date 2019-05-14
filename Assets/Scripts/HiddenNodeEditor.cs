// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class HiddenNodeEditor : NodeEditorInterface
{

    public NonInputNode hiddenNode;
    public int nodeLayer;

    public HiddenNodeEditor(NonInputNode node, int nodeLayer)
    {
        this.hiddenNode = node;
        this.nodeLayer = nodeLayer;
    }

    public Node getNode()
    {
        return hiddenNode;
    }


    public void setActivationFunction(ActivationBehaviorTypes activType)
    {
        switch (activType)
        {
            case ActivationBehaviorTypes.LogisticAB:
                hiddenNode.setActivBehavior(new LogisticActivBehavior());
                break;
            case ActivationBehaviorTypes.EmptyAB:
                hiddenNode.setActivBehavior(new EmptyActivBehavior());
                break;
            case ActivationBehaviorTypes.LogWithNeg:
                hiddenNode.setActivBehavior(new LogisticWithNegActivBehav());
                break;
            case ActivationBehaviorTypes.Tanh:
                hiddenNode.setActivBehavior(new TanhActivBehav());
                break;
            default:
                Debug.LogError("Activation function type not found.");
                break;
        }
    }
}