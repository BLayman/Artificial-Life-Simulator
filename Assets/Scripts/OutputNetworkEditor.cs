using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class OutputNetworkEditor: NetworkEditor
{
    public OutputNetworkEditor(OutputNetwork net, CreatureEditor parentCreatureCreator) : base(net, parentCreatureCreator)
    {
    }

    /// <summary>
    /// assign output action to network
    /// </summary>
    public void setOutputAction(Action a)
    {
        OutputNetwork outNet = (OutputNetwork)network;
        outNet.outputAction = a;
    }

}