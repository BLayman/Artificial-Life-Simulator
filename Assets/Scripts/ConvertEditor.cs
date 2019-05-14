// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class ConvertEditor : ActionEditorAbstract
{
    Convert actionRef;

    public ConvertEditor(Convert inAction)
    {
        action = inAction;
        actionRef = inAction;
    }

    public void addStartResource(string res, float coefficient)
    {
        actionRef.startResources[res] = coefficient;
    }

    public void addEndResource(string res, float coefficient)
    {
        actionRef.endResources[res] = coefficient;
    }

    public void setAmtToProduce(float amt)
    {
        actionRef.amtToProduce = amt;
    }

}