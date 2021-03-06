﻿// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// A network that converts incomming comm signals into action recommendations. A seperate comm network is needed for every neightbor that sends a comm signal.
/// </summary>
public class CommNetwork : Network
{
    /// <summary>
    /// Index of neighbor from which communication was recieved
    /// </summary>
    public int communicationFrom;


    public CommNetwork getShallowCopy()
    {
        return (CommNetwork)this.MemberwiseClone();
    }
}