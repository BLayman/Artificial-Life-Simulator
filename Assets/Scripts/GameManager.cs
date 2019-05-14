// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    Dictionary<string, Ecosystem> ecoDict;
    SimRunnerUser simRunnerUser;

    public void Start()
    {
        simRunnerUser = gameObject.GetComponent<SimRunnerUser>();
    }

    public void addEcosystem(Ecosystem eco)
    {
        ecoDict[eco.name] = eco;
    }

    public void startUserSimulation(int simSteps)
    {
        simRunnerUser.startSim(simSteps);
    }

}
