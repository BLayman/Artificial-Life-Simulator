using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class ReproAction : Action
{

    public override void perform(Creature creature, Ecosystem eco)
    {
        Population parentPop = eco.populations[creature.species];
        // don't perform if excedes population max size
        int popSize = parentPop.size;
        if (popSize >= parentPop.maxSize)
        {
            return;
        }

        int childLoc = -1;
        for (int i = 1; i < creature.neighborLands.Length; i++)
        {
            if (!creature.neighborLands[i].creatureIsOn() && !creature.neighborLands[i].isDummy)
            {
                childLoc = i;
                break;
            }
        }

        if (childLoc != -1)
        {
            Creature childCreature = Copier.getCreatureChild(creature);
            setPosition(creature.position, childLoc, childCreature.position);
            childCreature.updateNeighbors();
            childCreature.addVariationToWeights(parentPop.weightStandardDev);
            creature.neighborLands[childLoc].creatureOn = childCreature;
            parentPop.offspring.Add(childCreature);
            parentPop.size++;

        }
    }

    public void setPosition(int[] parentPos, int neighborIndex, int[] childPosition)
    {
        switch (neighborIndex)
        {
            case 1:
                childPosition[0] = parentPos[0];
                childPosition[1] = parentPos[1] + 1;
                break;
            case 2:
                childPosition[0] = parentPos[0];
                childPosition[1] = parentPos[1] - 1;
                break;
            case 3:
                childPosition[0] = parentPos[0] - 1;
                childPosition[1] = parentPos[1];
                break;
            case 4:
                childPosition[0] = parentPos[0] + 1;
                childPosition[1] = parentPos[1];
                break;
            default:
                break;
        }
    }

    public ReproAction shallowCopy()
    {
        return (ReproAction)this.MemberwiseClone();
    }
}