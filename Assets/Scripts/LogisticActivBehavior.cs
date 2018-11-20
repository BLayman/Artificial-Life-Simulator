using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class LogisticActivBehavior : ActivationBehavior
{
    // TODO: implement logistic activation function
    public float activFunct(float input)
    {
        return (float) (1.0 / (1 + (Math.Exp(-1.0 * (input)))));
    }
}