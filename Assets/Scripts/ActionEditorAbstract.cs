// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionEditorAbstract
{
    public Action action;

    public Action getAction()
    {
        return action;
    }

    public void setTimeCost(int cost)
    {
        action.timeCost = cost;
    }

    public void addResourceCost(string key, float value)
    {
        action.resourceCosts[key] = value;
    }

    public void setPriority(int priority)
    {
        action.priority = priority;
        
    }

    public void setName(string name)
    {
        action.name = name;
    }

}