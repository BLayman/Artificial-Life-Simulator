// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System.Collections;
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A population stores information about a list of creatures, including it's founder, and initial variation.
/// </summary>
public class Population
{
    public List<Creature> creatures;
    public List<Creature> offspring = new List<Creature>();
    public Creature founder;
    public float weightStandardDev;
    public float abilityStandardDev;
    public float mutationStandardDev;
    public int size = 0;
    public int maxSize = 1000;
    public List<float> weightAverages = new List<float>();
    // public List<float> weightSDs = new List<float>();
    public List<List<float>> weightsByCreature = new List<List<float>>();
    bool initializedWeightDataStructures = false;

    public Creature generateMember()
    {
        size++;
        //Debug.Log("founder species: " + founder.species);
        Creature c = Copier.getCreatureCopy(founder);
        //Debug.Log("copy species: " + c.species);
        return c;
    }

    // TODO: Debug this function
    public void calculateWeightStats()
    {
        weightsByCreature = new List<List<float>>();

        // store weights of all creatures
        // for the ith creature
        for (int i = 0; i < creatures.Count; i++)
        {
            int creatureWeightIndex = 0; // the index of the the weight out of all weights in a creature

            weightsByCreature.Add(new List<float>());

            for (int j = 0; j < creatures[i].networks.Count; j++)
            {
                foreach (string key in creatures[i].networks[j].Keys)
                {
                    for (int k = 0; k < creatures[i].networks[j][key].net.Count; k++)
                    {
                        for (int l = 0; l < creatures[i].networks[j][key].net[k].Count; l++)
                        {
                            bool castWorked = true;
                            NonInputNode node = null;
                            try
                            {
                                node = (NonInputNode)creatures[i].networks[j][key].net[k][l];
                            }
                            catch (InvalidCastException e)
                            {
                                castWorked = false;
                            }
                            if (castWorked)
                            {
                                for (int m = 0; m < node.weights.Count; m++)
                                {

                                    weightsByCreature[i].Add(node.weights[m]); // store weights by creature and weight index


                                    if(m >= node.creatureWeightsIndicies.Count)
                                    {
                                        node.creatureWeightsIndicies.Add(creatureWeightIndex);
                                    }
                                    else
                                    {
                                        node.creatureWeightsIndicies[m] = creatureWeightIndex;
                                    }

                                    creatureWeightIndex++;

                                }
                            }
                        }
                    }
                }
            }
        }
        // NOTE: only works if all creatures in the population have the same number of weights
        // calculate averages
        List<float> sums = new List<float>(); // sums by weight
        // for every weight
        for (int i = 0; i < weightsByCreature[0].Count; i++)
        {
            sums.Add(0);
            // for every creature
            for (int j = 0; j < weightsByCreature.Count; j++)
            {
                try
                {
                    sums[i] += weightsByCreature[j][i];
                }
                catch(ArgumentOutOfRangeException e)
                {
                    Debug.Log("Out of range: " + j + " " + i);
                }
                
            }
        }

        // create averages
        List<float> averages = new List<float>();

        for (int i = 0; i < sums.Count; i++)
        {
            averages.Add(sums[i] / creatures.Count);
        }
        weightAverages = averages;

        initializedWeightDataStructures = true;
    }

    public Population shallowCopy()
    {
        return (Population)this.MemberwiseClone();
    }
}
