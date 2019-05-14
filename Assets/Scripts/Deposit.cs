// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Deposit : Action
{

    /// <summary>
    /// Index of neighboring land to access.
    /// </summary>
    public int neighborIndex;
    /// <summary>
    /// Name of resource to deposit.
    /// </summary>
    public string resourceToDeposit;

    public float amountToDeposit;

    public Deposit() { }

    public Deposit(int neighborIndex, string resourceToDeposit)
    {
        this.neighborIndex = neighborIndex;
        this.resourceToDeposit = resourceToDeposit;
    }

    /// <summary>
    /// Deposit resource on neighbor land.
    /// </summary>
    public override void perform(Creature creature, Ecosystem eco)
    {
        Land land = creature.neighborLands[neighborIndex];
        if (!land.isDummy)
        {
            CreatureResource creatRes = creature.storedResources[resourceToDeposit];

            float actualAmtDeposited;

            if(creatRes.currentLevel < amountToDeposit)
            {
                actualAmtDeposited = creatRes.currentLevel;
            }
            else
            {
                actualAmtDeposited = amountToDeposit;
            }
            creatRes.currentLevel -= actualAmtDeposited;
            land.depositResource(resourceToDeposit, actualAmtDeposited);
        }
    }

}