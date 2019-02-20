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
        //Debug.Log("resource before: " + creature.storedResources["grass"].currentLevel);
        bool enoughResources = spendTimeAndResources(creature);
        if (!enoughResources) { return; }

        //Debug.Log("resource after: " + creature.storedResources["grass"].currentLevel);

        // TODO: other directions
        // TODO: what if creature moves to space where other creature is?
        // TODO: reduce redundancy in code
        switch (direction)
        {
            case moveDir.up:
                if(!creature.neighborLands[1].creatureIsOn() && creature.position[1] + 1 < creature.map[creature.position[0]].Count)
                {
                    creature.neighborLands[0].creatureOn = null;
                    creature.position[1] += 1;
                    creature.updateNeighbors();
                    creature.neighborLands[0].creatureOn = creature;
                }
                break;
            case moveDir.down:
                if (!creature.neighborLands[2].creatureIsOn() && creature.position[1] - 1 >= 0)
                {
                    creature.neighborLands[0].creatureOn = null;
                    creature.position[1] -= 1;
                    creature.updateNeighbors();
                    creature.neighborLands[0].creatureOn = creature;
                }
                break;
            case moveDir.left:
                if (!creature.neighborLands[3].creatureIsOn() && creature.position[0] - 1 >= 0)
                {
                    creature.neighborLands[0].creatureOn = null;
                    creature.position[0] -= 1;
                    creature.updateNeighbors();
                    creature.neighborLands[0].creatureOn = creature;
                }
                break;
            case moveDir.right:
                if (!creature.neighborLands[4].creatureIsOn() && creature.position[0] + 1 < creature.map.Count)
                {
                    creature.neighborLands[0].creatureOn = null;
                    creature.position[0] += 1;
                    creature.updateNeighbors();
                    creature.neighborLands[0].creatureOn = creature;
                }
                break;
            default:
                break;
        }
    }

    
}