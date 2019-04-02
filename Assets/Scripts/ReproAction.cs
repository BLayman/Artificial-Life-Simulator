// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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
            float newDeviation = creature.mutationStandardDeviation * creature.annealMutationFraction;
            // if new deviation goes below baseline, set deviation to baseline
            if(newDeviation < creature.baseMutationDeviation)
            {
                creature.mutationStandardDeviation = creature.baseMutationDeviation;
            }
            // otherwise set mutation deviation to new deviation
            else
            {
                creature.mutationStandardDeviation = newDeviation;
            }
            childCreature.iD = creature.iD + "-" + creature.childIndex;
            creature.childIndex++;
            //Debug.Log(creature.mutationStandardDeviation);
            childCreature.addVariationToWeights(creature.mutationStandardDeviation);
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