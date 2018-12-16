using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum moveDir { up, down, left, right}

public class MoveAction : Action
{
    public moveDir direction;

    public MoveAction() { }

    public MoveAction(moveDir dir)
    {
        direction = dir;
    }

    public override void perform(Creature creature)
    {
        Debug.Log("performing move action");
        creature.remainingTurnTime -= timeCost;

        // TODO: finish 
        switch (direction)
        {
            case moveDir.up:
                if(!creature.neighborLands[1].creatureIsOn() && creature.position[1] + 1 <= creature.map[creature.position[0]].Count)
                {
                    creature.neighborLands[0].creatureOn = null;
                    creature.position[1] += 1;
                    creature.updateNeighbors();
                    creature.neighborLands[0].creatureOn = creature;
                }
                
                
                break;
            case moveDir.down:
                break;
            case moveDir.left:
                break;
            case moveDir.right:
                break;
            default:
                break;
        }
    }

    
}