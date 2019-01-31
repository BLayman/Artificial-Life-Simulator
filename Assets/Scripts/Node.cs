using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

/// <summary>
/// Abstract parent class for all Node types. A Node represents a node in a creatures neural network.
/// </summary>
public abstract class Node
{
    public float value;

    /// <summary>
    /// Updates node value
    /// </summary>
    public abstract void updateValue();

}