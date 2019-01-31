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