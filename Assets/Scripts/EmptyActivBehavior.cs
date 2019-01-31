using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// No activation function (node simply uses linear combination).
/// </summary>
public class EmptyActivBehavior : ActivationBehavior
{
    public float activFunct(float input)
    {
        return input;
    }
}