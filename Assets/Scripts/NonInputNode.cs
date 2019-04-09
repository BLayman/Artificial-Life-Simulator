// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

public enum ActivationBehaviorTypes { LogisticAB, EmptyAB, LogWithNeg, Tanh }

/// <summary>
/// Encompasses hidden nodes (which use this class directly) and output nodes (which use the OutputNode class).
/// </summary>
public class NonInputNode : Node
{
    System.Random rand;
    public List<int> tabooPrevNodeIndicies = new List<int>();

    public List<Node> prevNodes = new List<Node>();
    /// <summary>
    /// Holds an object that carries out an activation function.
    /// </summary>
    protected ActivationBehavior activBehavior;
    /// <summary>
    /// List of weights to be multiplied by previous node values.
    /// </summary>
    public List<float> weights = new List<float>();

    public List<int> creatureWeightsIndicies = new List<int>();


    public List<float> extraWeights = new List<float>(); 

    public Network parentNet;
    public Creature parentCreature;
    public int layer;

    public NonInputNode()
    {
        rand = new System.Random(Guid.NewGuid().GetHashCode());
        activBehavior = new LogisticActivBehavior();
    }

    public NonInputNode(Network parentNet, Creature parentCreature, int layer)
    {
        rand = new System.Random(Guid.NewGuid().GetHashCode());
        this.parentNet = parentNet;
        this.parentCreature = parentCreature;
        // set to logistic activation by default
        activBehavior = new LogisticActivBehavior();
        this.layer = layer;
        resetAllPreviousNodes();
        generatePhenotypeWeights();
    }

    public override void updateValue()
    {
        if(parentCreature == null)
        {
            Debug.Log("null parent creature");
        }
        float combination = linearCombinePrevVals();
        //Debug.Log("combination = " + combination);
        value = performActivBehavior(combination);
    }

    public void appendPrevNodeNewWeight(Node newNode)
    {
        prevNodes.Add(newNode);
        weights.Add(generateNewRandomWeight());
    }

    public void appendPrevNode(Node newNode)
    {
        prevNodes.Add(newNode);
    }

    // add weights for each of 4 possible neighbors that could contribute values to the previous layer
    public void generatePhenotypeWeights()
    {
        for (int i = 0; i < 4; i++)
        {
            extraWeights.Add(generateNewRandomWeight());
        }
    }

    public void removePrevNode(int index)
    {
        prevNodes.RemoveAt(index);
        weights.RemoveAt(index);
        tabooPrevNodeIndicies.Add(index);
    }

    private float linearCombinePrevVals()
    {
        float sum = 0;

        // add extra weights to be used for neighbor phenotypes
        if (prevNodes.Count > weights.Count)
        {
            weights.AddRange(extraWeights);
        }

        // should never have more nodes than weights
        /*
        if (prevNodes.Count < weights.Count)
        {
            Debug.LogError("Error: Previous node count greater than weights count. " +
                "Weight count: " + weights.Count + ", node count: " + prevNodes.Count);
        }
        */
        for (int i = 0; i < prevNodes.Count; i++)
        {
            sum += prevNodes[i].value * weights[i];
        }
        return sum;
    }

    /// <param name="index">Index of previous node.</param>
    /// <param name="value">Weight coming from previous node.</param>
    public void setPrevNodeWeight(int index, float weight)
    {
        weights[index] = weight;
    }

    /// <summary>
    /// Calls activation function of activBehavior.
    /// </summary>
    public float performActivBehavior(float input)
    {
        return activBehavior.activFunct(input);
    }

    /// <summary>
    /// Sets a new activation behavior, thus altering the activation function.
    /// </summary>
    public void setActivBehavior(ActivationBehavior behavior)
    {
        activBehavior = behavior;
    }

    /// <summary>
    /// Sets prevNodes to include all nodes in previous layer. Resets nodes and weights
    /// </summary>
    public void resetAllPreviousNodes()
    {
        prevNodes.Clear();
        weights.Clear();
        for (int i = 0; i < parentNet.net[layer - 1].Count; i++)
        {
            //Debug.Log("assigning " + parentNet.net[layer - 1][i].value);
            if (!tabooPrevNodeIndicies.Contains(i))
            {
                prevNodes.Add(parentNet.net[layer - 1][i]);
                weights.Add(generateNewRandomWeight());
            }
        }
    }

    // resets nodes but not weights
    public void assignPrevNodes()
    {
        prevNodes.Clear();
        //Debug.Log("layer: " + layer);
        //Debug.Log("count: " + parentNet.net.Count);
        for (int i = 0; i < parentNet.net[layer - 1].Count; i++)
        {
            if (!tabooPrevNodeIndicies.Contains(i))
            {
                prevNodes.Add(parentNet.net[layer - 1][i]);
            }
        }
    }


    public float generateNewRandomWeight()
    {
        return (float)((rand.NextDouble() * 2.0) - 1);
    }

    public void resetRand()
    {
        rand = new System.Random(System.DateTime.Now.Millisecond);
    }

    public object clone()
    {
        return this.MemberwiseClone();
    }

    public void printInputsAndWeights()
    {
        Debug.Log("weights:");
        foreach (float weight in weights)
        {
            Debug.Log("w = " + weight);
        }
        Debug.Log("previous values:");
        foreach (Node node in prevNodes)
        {
            Debug.Log("v = " + node.value);
        }
    }

    public List<float> getWeightAverages()
    {

        List<float> weightAverages = new List<float>();
        
        for (int i = 0; i < creatureWeightsIndicies.Count; i++)
        {
            Debug.Log("index " + creatureWeightsIndicies[i]);
            Debug.Log("val " + parentCreature.parentPopulation.weightAverages[creatureWeightsIndicies[i]]);
            //weightAverages.Add(parentCreature.parentPopulation.weightAverages[creatureWeightsIndicies[i]]);
        }
        return weightAverages;
    }
}