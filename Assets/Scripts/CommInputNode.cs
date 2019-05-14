// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class CommInputNode : Node
{
    /// <summary>
    /// Stores reference to creature for getting access to neighbors
    /// </summary>
    public Creature creature;
    /// <summary>
    /// stores which property to access in CommSignal
    /// </summary>
    public string commProperty;
    /// <summary>
    /// Stores index of specific bit to convert to 0 or 1 from a particular property of the CommSignal.
    /// </summary>
    public int bitIndex;

    /// <summary>
    /// Will check if a creature is at this neighbor, and get it's comm output, or otherwise sets value to 0.
    /// </summary>
    public override void updateValue()
    {
        throw new NotImplementedException();
    }
}