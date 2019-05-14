// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DepositEditor : ActionEditorAbstract
{
    Deposit actionRef;

    public DepositEditor(Deposit inAction)
    {
        action = inAction;
        actionRef = inAction;
    }

    public void setNeighborIndex(int index)
    {
        actionRef.neighborIndex = index;
    }

    public void setDepositResource(string res)
    {
        actionRef.resourceToDeposit = res;
    }

    public void setAmtToDeposit(float amt)
    {
        actionRef.amountToDeposit = amt;
    }

}