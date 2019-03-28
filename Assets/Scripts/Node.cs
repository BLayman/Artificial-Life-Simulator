// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

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


    public Node clone()
    {
        return (Node)this.MemberwiseClone();
    }

}