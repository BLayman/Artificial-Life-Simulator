using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActionCreatorAbstract
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

    public void addResourceCost(string key, int value)
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