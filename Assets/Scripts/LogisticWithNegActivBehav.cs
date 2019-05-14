// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Logistic activation function, but mapped to [-1,1]
/// </summary>
public class LogisticWithNegActivBehav : ActivationBehavior
{
    public float activFunct(float input)
    {
        return (float)((2 * (1.0 / (1 + (Math.Exp(-1.0 * (input)))))) - 1);
    }
}