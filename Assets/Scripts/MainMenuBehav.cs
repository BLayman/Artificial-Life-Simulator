// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBehav : MonoBehaviour
{
    public GameObject uIParent;
    public GameObject ecoMenu;
    EcoMenuBehav ecoMenuBehav;
    GameManager gm;
    public int simSteps;

    // Start is called before the first frame update
    void Start()
    {
        // NOT CURRENTLY IN USE
        //gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        //ecoMenuBehav = ecoMenu.GetComponent<EcoMenuBehav>();
    }

    /// <summary>
    /// provides the ecosystem menu with a new ecosystem to edit 
    /// </summary>
    public void createNewEcosystem()
    {
        uIParent.SetActive(false);
        Ecosystem eco = new Ecosystem();
        ecoMenuBehav.loadEcoMenu(eco);

    }

    /// <summary>
    /// saves last ecosystem edited by ecosystem menu to the ecosystem dictionary 
    /// </summary>
    public void saveCurrentEcosystem()
    {
        gm.addEcosystem(ecoMenuBehav.ecoEditor.ecosystem);
    }

    public void startSimulation()
    {
        gm.startUserSimulation(simSteps);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
