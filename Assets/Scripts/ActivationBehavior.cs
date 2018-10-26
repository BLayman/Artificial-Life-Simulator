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