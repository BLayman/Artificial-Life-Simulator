// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class KMeans
{
    public static List<List<float>> centers = new List<List<float>>();
    public static int numOfClusters = 10;
    public static List<Color> colors = new List<Color>();

    public static void createRandomCenters(List<List<float>> weightsByCreature)
    {
        System.Random rand = new System.Random();

        // initialize centers
        centers.Clear();

        for (int i = 0; i < numOfClusters; i++)
        {
            colors.Add(new Color((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble()));
        }
        // set clusters to random points

        // for each cluster
        for (int i = 0; i < numOfClusters; i++)
        {
            centers.Add(new List<float>());
            // for each weight in cluster
            for (int j = 0; j < weightsByCreature[0].Count; j++)
            {
                // produce number in range [-1, 1]
                float num = (float)(rand.NextDouble() * 2 - 1);
                centers[i].Add(num);
            }
        }


    }


    public static void runKMeans(List<List<float>> weightsByCreature, List<Creature> creatures)
    {

        // TODO: change this into a while loop with a convergence condition
        for (int i = 0; i < 5; i++)
        {

            // create list to store creatures by the cluster they belong to 
            List<List<List<float>>> creaturesByCluster = new List<List<List<float>>>();
            for (int n = 0; n < centers.Count; n++)
            {
                // Add a list of creatures for each cluster
                creaturesByCluster.Add(new List<List<float>>());
            }

            // 1. Place creatures into centers

            // for every creature
            for (int j = 0; j < weightsByCreature.Count; j++)
            {
                int closestCenter = -1;
                double shortestDist = double.PositiveInfinity;
                // calculate its distance to each center to find the closest center
                for (int k = 0; k < centers.Count; k++)
                {
                    double dist = calcDistance(weightsByCreature[j], centers[k]);
                    if (dist < shortestDist)
                    {
                        shortestDist = dist;
                        closestCenter = k;
                    }
                }
                // place creature in its cluster
                creaturesByCluster[closestCenter].Add(weightsByCreature[j]);
            }


            // 2. recalculate centers based on averages from creatures in each cluster

            centers.Clear();
            // for each cluster
            for (int p = 0; p < creaturesByCluster.Count; p++)
            {
                //Debug.Log("creatures in cluster " + p + ": " + creaturesByCluster[p].Count);
                // handle empty clusters
                if(creaturesByCluster[p].Count == 0)
                {
                    //creaturesByCluster.RemoveAt(p);
                    Debug.Log("empty cluster");
                    continue;
                    /*
                    createRandomCenters(weightsByCreature);
                    runKMeans(weightsByCreature);
                    return;
                    */
                }

                // initialize sums to zeros
                List<float> sums = new List<float>();
                // the number of sums equals the number of weights in a creature
                for (int l = 0; l < creaturesByCluster[p][0].Count; l++)
                {
                    sums.Add(0);
                }
                // for every creature in cluster i
                for (int j = 0; j < creaturesByCluster[p].Count; j++)
                {

                    // for every weight k in creature j
                    for (int k = 0; k < creaturesByCluster[p][0].Count; k++)
                    {
                        // sum weights by index
                        sums[k] += creaturesByCluster[p][j][k];
                    }
                }

                // divide sums by number of creatures
                for (int m = 0; m < sums.Count; m++)
                {
                    sums[m] /= creaturesByCluster[p].Count;
                }

                // now sums are averages (centers was cleared above)
                centers.Add(sums);
            }
        }
    }


    public static double calcDistance(List<float> creatureWeights, List<float> center)
    {
        if(creatureWeights.Count != center.Count)
        {
            Debug.Log("weight count mismatch");
            return -1;
        }
        else
        {

            double sum = 0;
            for (int i = 0; i < center.Count; i++)
            {
                sum += Math.Pow((center[i] - creatureWeights[i]), 2.0);
            }
            return Math.Sqrt(sum); 
        }
    }


    public static void setColors(List<List<float>> weightsByCreature, List<Creature> creatures)
    {

        // for every creature
        // for every creature
        for (int j = 0; j < weightsByCreature.Count; j++)
        {
            int closestCenter = -1;
            double shortestDist = double.PositiveInfinity;
            // calculate its distance to each center to find the closest center
            for (int k = 0; k < centers.Count; k++)
            {
                double dist = calcDistance(weightsByCreature[j], centers[k]);
                if (dist < shortestDist)
                {
                    shortestDist = dist;
                    closestCenter = k;
                }
            }
            // place creature in its cluster
            creatures[j].color = colors[closestCenter];
        }

    }

}
