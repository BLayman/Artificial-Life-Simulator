using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

public enum ActivationBehaviorTypes { LogisticAB, EmptyAB, LogWithNeg }

public class NonInputNode : Node
{
    public List<Node> prevNodes = new List<Node>();
    /// <summary>
    /// Holds an object that carries out an activation function.
    /// </summary>
    protected ActivationBehavior activBehavior;
    /// <summary>
    /// List of weights to be multiplied by previous node values.
    /// </summary>
    public List<float> weights = new List<float>();

    public Network parentNet;
    public int layer;

    public NonInputNode() { }

    public NonInputNode(Network parentNet, int layer)
    {
        this.parentNet = parentNet;
        // set to logistic activation by default
        activBehavior = new LogisticActivBehavior();
        this.layer = layer;
        resetAllPreviousNodes();
    }

    public override void updateValue()
    {
        float combination = linearCombinePrevVals();
        value = performActivBehavior(combination);
    }

    public void appendPrevNode(Node newNode)
    {
        prevNodes.Add(newNode);
        weights.Add(generateNewRandomWeight());
    }

    public void removePrevNode(int index)
    {
        prevNodes.RemoveAt(index);
        weights.RemoveAt(index);
    }

    private float linearCombinePrevVals()
    {
        float sum = 0;
        if(prevNodes.Count != weights.Count)
        {
            Debug.LogError("Error: Previous node count not equal to weights count.");
        }
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
    /// Sets prevNodes to include all nodes in previous layer.
    /// </summary>
    public void resetAllPreviousNodes()
    {
        prevNodes.Clear();
        weights.Clear();
        for (int i = 0; i < parentNet.net[layer - 1].Count; i++)
        {
            prevNodes.Add(parentNet.net[layer - 1][i]);
            weights.Add(generateNewRandomWeight());
        }
    }

    public void assignPrevNodes()
    {
        prevNodes.Clear();
        for (int i = 0; i < parentNet.net[layer - 1].Count; i++)
        {
            prevNodes.Add(parentNet.net[layer - 1][i]);
        }
    }

    public float generateNewRandomWeight()
    {
        System.Random rand = new System.Random();

        return (float)((rand.NextDouble() * 2.0) - 1);
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
}