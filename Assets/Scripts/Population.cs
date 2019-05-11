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
    public List<float> weightSDs = new List<float>(); 
    public float overallVariability;

    // public List<float> weightSDs = new List<float>();
    public List<List<float>> weightsByCreature = new List<List<float>>(); // stores every weight of every creature

    public Creature generateMember()
    {
        size++;
        //Debug.Log("founder species: " + founder.species);
        Creature c = Copier.getCreatureCopy(founder);
        //Debug.Log("copy species: " + c.species);
        return c;
    }

    public void calculateWeightStats()
    {
        
        updateWeightsByCreature();

        // NOTE: only works if all creatures in the population have the same number of weights
        // calculate averages
        List<float> averages = new List<float>();

        int count;
        
        // assuming any creatures were left
        if(weightsByCreature.Count > 0)
        {
            // for every weight
            for (int i = 0; i < weightsByCreature[0].Count; i++)
            {
                count = 0;
                float sum = 0;
                // for every creature
                for (int j = 0; j < weightsByCreature.Count; j++)
                {
                    // TODO: fix this: Probably relates to creature removal from list?
                    try
                    {
                        count++;
                        sum += weightsByCreature[j][i]; // add the weight for that creature to the sum for that weight
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        //Debug.Log("Out of range: " + j + " " + i);
                    }
                }
                averages.Add(sum / count);
            }

            // calculate variances and standard deviations
            List<float> variances = new List<float>();
            List<float> sDs = new List<float>();


            for (int i = 0; i < weightsByCreature[0].Count; i++)
            {
                count = 0;
                float sumSquaredDiff = 0;
                // for every creature
                for (int j = 0; j < weightsByCreature.Count; j++)
                {
                    // TODO: fix this
                    try
                    {
                        count++;
                        sumSquaredDiff += (float)Math.Pow(weightsByCreature[j][i] - averages[i], 2.0); // add the weight for that creature to the sum for that weight
                    }
                    catch (ArgumentOutOfRangeException e)
                    {
                        //Debug.Log("Out of range: " + j + " " + i);
                    }
                }
                variances.Add(sumSquaredDiff / count);
                sDs.Add((float)Math.Sqrt((double)sumSquaredDiff / count));
            }


            // set instance variables
            weightAverages = averages;
            weightSDs = sDs;

            // calculate measure of overall variability


            float varianceSum = 0;
            for (int i = 0; i < variances.Count; i++)
            {
                varianceSum += variances[i];
            }
            overallVariability = (float)Math.Sqrt((double)(varianceSum / variances.Count));
        }


        CreatureAveragesIO.saveAverages(weightAverages);
        
    }

    public void initKMeans()
    {
        KMeans.createRandomCenters(weightsByCreature);
        Debug.Log("initializing KMeans");
    }

    public void runKMeans()
    {
        KMeans.runKMeans(weightsByCreature);
        Debug.Log("updating clusters");
    }


    public void updateColors()
    {
        updateWeightsByCreature();
        Debug.Log("updating colors");
        KMeans.setColors(weightsByCreature, creatures);
    }


    public void updateWeightsByCreature()
    {
        weightsByCreature = new List<List<float>>(); // reset for new set of creatures
        // store weights of all creatures in 2D list
        // for the ith creature
        for (int i = 0; i < creatures.Count; i++)
        {
            int creatureWeightIndex = 0; // the index of the the weight out of all weights in a creature
            // add a list for every creature
            weightsByCreature.Add(new List<float>());

            // for every node
            for (int j = 0; j < creatures[i].networks.Count; j++)
            {
                foreach (string key in creatures[i].networks[j].Keys)
                {
                    // if it's not a phenotype network
                    if (!key.StartsWith("phenotypeNet"))
                    {
                        for (int k = 0; k < creatures[i].networks[j][key].net.Count; k++)
                        {
                            for (int l = 0; l < creatures[i].networks[j][key].net[k].Count; l++)
                            {
                                bool castWorked = true;
                                NonInputNode node = null;
                                // get node and convert it to Non-Input node if possible
                                try
                                {
                                    node = (NonInputNode)creatures[i].networks[j][key].net[k][l];
                                }
                                catch (InvalidCastException e)
                                {
                                    castWorked = false;
                                }
                                // if it is a non-input node
                                if (castWorked)
                                {
                                    // reset creature weights indicies in node
                                    node.creatureWeightsIndicies = new List<int>();

                                    // for every weight
                                    for (int m = 0; m < node.weights.Count; m++)
                                    {
                                        // add the weight to our weights by creature list
                                        weightsByCreature[i].Add(node.weights[m]); // store weights by creature and weight index
                                                                                   // add index of where weights are stored in population to node
                                        node.creatureWeightsIndicies.Add(creatureWeightIndex);
                                        creatureWeightIndex++;

                                    }
                                    //Debug.Log(node.creatureWeightsIndicies.Count);
                                }
                            }
                        }
                    }

                }
            }
        }
    }


    public Population shallowCopy()
    {
        return (Population)this.MemberwiseClone();
    }
}
