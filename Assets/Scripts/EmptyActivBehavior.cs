using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class EmptyActivBehavior : ActivationBehavior
{
    public float activFunct(float input)
    {
        throw new NotImplementedException();
    }
}