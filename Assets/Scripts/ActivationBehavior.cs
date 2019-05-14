// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Interface for objects that implement an activation function.
/// </summary>
public interface ActivationBehavior
{
    // must have an activation function, even one that doesn't do anything
    float activFunct(float input);
}