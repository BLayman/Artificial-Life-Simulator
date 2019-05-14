// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// A specific kind of network that decided whether a creature should reproduce.
/// </summary>
public class ReproNetwork : Network
{
    /// <summary>
    /// Index of neighbor to reproduce with.
    /// </summary>
    public int reproduceWith;
}