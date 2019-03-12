// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class CommAction : Action
{
    /// <summary>
    /// CommSignal to be attached to target creature.
    /// </summary>
    private CommSignal signal;
    /// <summary>
    /// Index of neighbor comm signal is being sent to.
    /// </summary>
    private int targetNeighborIndex;

    /// <summary>
    /// Adds signal to creatures output comm signals. (to be iterated over and passed to neighbors).
    /// </summary>
    public override void perform(Creature creature, Ecosystem eco)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Assigns properties of signal
    /// </summary>
    public void assignSignalProps()
    {
        throw new System.NotImplementedException();
    }
}