using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogisticWithNegActivBehav : ActivationBehavior
{
    public float activFunct(float input)
    {
        return (float)((2 *(1.0 / (1 + (Math.Exp(-1.0 * (input)))))) - 1);
    }
}