// Eco-Simulator
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
            switch (creature.mutCoeffType)
            {
                case MutationDeviationCoefficientType.exponentialDecay:
                    creature.mutationDeviationCoefficient *= creature.annealMutationFraction;
                    break;
                case MutationDeviationCoefficientType.sine:
                    creature.sineFunctStep += .1f;
                    creature.mutationDeviationCoefficient = (float)Math.Cos(creature.sineFunctStep);
                    break;
                case MutationDeviationCoefficientType.invPopsize:
                    double fraction = (double)creature.parentPopulation.size / creature.parentPopulation.maxSize;
                    creature.mutationDeviationCoefficient = (float) System.Math.Min(2.0, .01 * Math.Pow(1.0 / fraction, 8.0));
                    
                    break;
                default:
                    break;
            }
            //Debug.Log("coefficient: " + creature.mutationDeviationCoefficient);

            float deviation = creature.mutationStandardDeviation * creature.mutationDeviationCoefficient;
            // if new deviation goes below baseline, set deviation to baseline
            if(deviation < creature.baseMutationDeviation)
            {
                deviation = creature.baseMutationDeviation;
            }
            //Debug.Log("Deviation: " + deviation);

            childCreature.iD = creature.iD + "-" + creature.childIndex;
            creature.childIndex++;
            creature.actualMutationDeviation = deviation;
            childCreature.addVariationToWeights(deviation);
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