using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionCreatorType { commActionCreator }

public class ActionCreator
{
    public Action action;

    ActionCreatorInterface actionCreator;
    ActionCreatorType actionType;


    // user picks layer to create node in, initializing a node creator
    public ActionCreator()
    {
    }


    // puser picks sensory input node as the type they would like to create
    public void setCreator(ActionCreatorType type)
    {
        actionType = type;

        switch (type)
        {
            case ActionCreatorType.commActionCreator:
                // now node will be modified by siNodeCreator
                actionCreator = new CommActionCreator();
                break;
            default:
                break;
        }
        // now this NodeCreator will now be passed to a gameObject ( or referenced by a gameObject) 
        // that only displays node edits for it's type and only uses the variable for that kind of nodeCreator      
    }

    public ActionCreatorInterface getActionCreator()
    {
        switch (actionType)
        {
            case ActionCreatorType.commActionCreator:
                return (CommActionCreator)actionCreator;
            default:
                return null;
        }
    }

    public Action getCreatedAction()
    {
        return actionCreator.getAction();
    }

}