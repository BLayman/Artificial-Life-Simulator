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