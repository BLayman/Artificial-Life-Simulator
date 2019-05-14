// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AttackEditor: ActionEditorAbstract
{
    Attack actionRef;

    public AttackEditor(Attack inAction)
    {
        action = inAction;
        actionRef = inAction;
    }

    public void setVictimSpecies(string species)
    {
        actionRef.victimSpecies = species;
    }

    public void setBackfireHealthLost(float healthLost)
    {
        actionRef.backfireHealthLost = healthLost;
    }

    public void setResourceFracTaken(float fraction)
    {
        actionRef.baseResourceFractionTaken = fraction;
    }
}