using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class KMeans
{
    public static List<List<float>> centers = new List<List<float>>();


    public static void createRandomCenters(List<List<float>> weightsByCreature)
    {
        System.Random rand = new System.Random();

        // initialize centers
        centers.Clear();

        // one center for each color: red, green, blue
        for (int i = 0; i < 3; i++)
        {
            centers.Add(new List<float>());
        }

        // set clusters to random points

        // for each cluster
        for (int i = 0; i < 3; i++)
        {
            // for each weight in cluster
            for (int j = 0; j < weightsByCreature[0].Count; j++)
            {
                // produce number in range [-1, 1]
                float num = (float)(rand.NextDouble() * 2 - 1);
                centers[i].Add(num);
            }
        }
    }


    public static void runKMeans(List<List<float>> weightsByCreature)
    {

        // TODO: change this into a while loop with a convergence condition
        for (int i = 0; i < 10; i++)
        {

            // create list to store creatures by the cluster they belong to 
            List<List<List<float>>> creaturesByCluster = new List<List<List<float>>>();
            for (int n = 0; n < 3; n++)
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
                // TODO: handle empty clusters
                if(creaturesByCluster[p].Count == 0)
                {
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
                        sums[k] += creaturesByCluster[p][j][k];
                    }
                }
                // TODO: handle case where cluster is empty
                if (creaturesByCluster[p].Count != 0)
                {
                    // divide sums by number of creatures
                    for (int m = 0; m < sums.Count; m++)
                    {
                        sums[m] /= creaturesByCluster[p].Count;
                    }
                }
                else
                {
                    Debug.Log("empty cluster");
                }
                // now sums are averages
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
        for (int j = 0; j < weightsByCreature.Count; j++)
        {
            float maxDist = 0;
            float[] distances = new float[3];
            // calculate its distance to each center to find the closest center
            for (int k = 0; k < centers.Count; k++)
            {
                double dist = calcDistance(weightsByCreature[j], centers[k]);
                //Debug.Log("distance to " + k + " is " + dist);
                distances[k] = (float) dist;
                if (dist > maxDist)
                {
                    maxDist = (float) dist;
                }
            }
            //Debug.Log("max dist: " + maxDist);
            // place creature in its cluster
            creatures[j].color = new Color(1 - (distances[0] / maxDist), 1 - (distances[1] / maxDist), 1 - (distances[2] / maxDist));
        }

    }

}
