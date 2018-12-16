using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionCreatorType { commActionCreator, moveActionCreator, consumeCreator }

public class ActionCreator
{
    ActionCreatorAbstract actionCreator;
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
                actionCreator = new CommActionCreator(new CommAction());
                break;
            case ActionCreatorType.moveActionCreator:
                // now node will be modified by siNodeCreator
                actionCreator = new MoveActionCreator(new MoveAction());
                break;
            case ActionCreatorType.consumeCreator:
                actionCreator = new ConsumeFromLandCreator(new ConsumeFromLand());
                break;
            default:
                break;
        }
        // now this NodeCreator will now be passed to a gameObject ( or referenced by a gameObject) 
        // that only displays node edits for it's type and only uses the variable for that kind of nodeCreator      
    }

    public ActionCreatorAbstract getActionCreator()
    {
        switch (actionType)
        {
            case ActionCreatorType.commActionCreator:
                return (CommActionCreator)actionCreator;
            case ActionCreatorType.moveActionCreator:
                return (MoveActionCreator)actionCreator;
            case ActionCreatorType.consumeCreator:
                return (ConsumeFromLandCreator)actionCreator;
            default:
                Debug.LogError("Invalid action creator type");
                return null;
        }
    }

    public Action getCreatedAction()
    {
        return actionCreator.getAction();
    }

}