// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcosystemGetter : MonoBehaviour
{
    public GameObject simRunnerObj;
    ThreadManager threader;


    // Start is called before the first frame update
    void Awake()
    {
        threader = simRunnerObj.GetComponent<ThreadManager>();
    }

    public int getMapWidth()
    {
        return threader.getEcosystem().map.Count;
    }

    public int getMapHeight()
    {
        return threader.getEcosystem().map[0].Count;
    }

    public Dictionary<string, float> getPopVariabilities()
    {
        Dictionary<string, float> popVars = new Dictionary<string, float>();
        foreach (string key in threader.getEcosystem().populations.Keys)
        {
            popVars[key] = threader.getEcosystem().populations[key].overallVariability;
        }
        return popVars;
    }

    public int getAge()
    {
        return threader.getEcosystem().age;
    }

    public Ecosystem GetEcosystem()
    {
        Ecosystem eco = threader.getEcosystem();
        /*
        Debug.Log("******************                     **********************               ***********");
        for (int i = 0; i < eco.map.Count; i++)
        {
            for (int j = 0; j < eco.map[i].Count; j++)
            {

                if (eco.map[i][j].creatureIsOn())
                {
                    Debug.Log("*** queue length: " + eco.map[i][j].creatureOn.actionQueue.Count);
                }
            }
        }
        Debug.Log("******************                     **********************               ***********");
        */
        return threader.getEcosystem();
    }

}
