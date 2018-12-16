using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class LogisticActivBehavior : ActivationBehavior
{
    // TODO: implement logistic activation function
    public float activFunct(float input)
    {
        float output = (float)(1.0 / (1 + (System.Math.Exp(-1.0 * (input)))));
        return output;
    }
}