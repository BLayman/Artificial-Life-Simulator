// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatMenuBehav : MonoBehaviour
{
    CreatureEditor creatureCreator;
    EcosystemEditor ecoCreator;

    public GameObject uIParent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// load the creature editor menu using the ecosystem editor
    /// </summary>
    public void loadCreatMenu(EcosystemEditor ec)
    {
        ecoCreator = ec;
        creatureCreator = ec.creatureCreator;
        uIParent.SetActive(true);

        //TODO: load information into menu from prev created creature
    }

    /// <summary>
    /// save the creature creator's creature to the ecosystem
    /// </summary>
    public void saveSettings()
    {
        // adds creature to list of founders
        ecoCreator.addToFounders();
        // saves founders to ecosystem species list
        ecoCreator.saveFoundersToSpecies();
    }
}
