using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class ReproNetwork: Network
{
    /// <summary>
    /// Index of neighbor to reproduce with.
    /// </summary>
    public int reproduceWith;
}