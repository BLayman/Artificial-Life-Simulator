using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class Node
{
    public float value;

    /// <summary>
    /// Updates node value
    /// </summary>
    public abstract void updateValue();
}