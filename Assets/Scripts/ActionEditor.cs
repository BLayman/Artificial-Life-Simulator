using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ActionCreatorType { commActionCreator, moveActionCreator, consumeCreator, reproduceCreator }

public class ActionEditor
{
    ActionEditorAbstract actionCreator;
    ActionCreatorType actionType;
    CreatureEditor creatureCreator;

    // user picks layer to create node in, initializing a node creator
    public ActionEditor(CreatureEditor cc)
    {
        creatureCreator = cc;
    }


    // puser picks sensory input node as the type they would like to create
    public void setCreator(ActionCreatorType type)
    {
        actionType = type;

        switch (type)
        {
            case ActionCreatorType.commActionCreator:
                // now node will be modified by siNodeCreator
                actionCreator = new CommActionEditor(new CommAction());
                break;
            case ActionCreatorType.moveActionCreator:
                // now node will be modified by siNodeCreator
                actionCreator = new MoveActionEditor(new MoveAction());
                break;
            case ActionCreatorType.consumeCreator:
                actionCreator = new ConsumeFromLandEditor(new ConsumeFromLand());
                break;
            case ActionCreatorType.reproduceCreator:
                actionCreator = new ReproActionEditor(new ReproAction());
                break;
            default:
                break;
        }
        // now this NodeCreator will now be passed to a gameObject ( or referenced by a gameObject) 
        // that only displays node edits for it's type and only uses the variable for that kind of nodeCreator      
    }

    public ActionEditorAbstract getActionCreator()
    {
        switch (actionType)
        {
            case ActionCreatorType.commActionCreator:
                return (CommActionEditor)actionCreator;
            case ActionCreatorType.moveActionCreator:
                return (MoveActionEditor)actionCreator;
            case ActionCreatorType.consumeCreator:
                return (ConsumeFromLandEditor)actionCreator;
            case ActionCreatorType.reproduceCreator:
                return (ReproActionEditor)actionCreator;
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