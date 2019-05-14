// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

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