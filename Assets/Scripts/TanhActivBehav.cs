// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Implements Tanh activation function.
/// </summary>
public class TanhActivBehav : ActivationBehavior
{
    public float activFunct(float input)
    {
        //Debug.Log("in tanh");
        return (float)Math.Tanh((double)input);
    }
}