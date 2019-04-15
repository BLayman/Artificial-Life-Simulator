// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class EcoMenuBehav : MonoBehaviour
{
    public GameObject uIParent;
    public GameObject errorObj;

    public GameObject creatureMenuObj;
    CreatMenuBehav creatMenuBehav;

    public GameObject lrMenuObj;
    LRMenuBehav lrMenuBehav;

    public GameObject ecoNameTextBox;
    public GameObject abilityPtsPerCreatTextBox;
    public GameObject commBitsText;
    public GameObject distinctPhenoText;
    public GameObject timeUnitsText;

    public EcosystemEditor ecoEditor;

    // Start is called before the first frame update
    void Start()
    {
        creatMenuBehav = creatureMenuObj.GetComponent<CreatMenuBehav>();
        lrMenuBehav = lrMenuObj.GetComponent<LRMenuBehav>();
        // TODO: set other child menus
    }

    /// <summary>
    /// creates an ecosystem creator from the provided ecosystem
    /// </summary>
    public void loadEcoMenu(Ecosystem eco)
    {
        ecoEditor = new EcosystemEditor(eco);
        uIParent.SetActive(true);
        // TODO: properly populate menu from eco data
    }

    /// <summary>
    /// load the creature editor menu and pass it the current ecosystem editor
    /// </summary>
    public void addCreature()
    {
        creatMenuBehav.loadCreatMenu(ecoEditor);
        uIParent.SetActive(false);
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// saves all information that the user entered into the menu
    /// </summary>
    public void saveSettings()
    {

        // TODO : covert HelperSetter to ValidationHelper
        StringFunct setEcoName = new StringFunct(ecoEditor.setName);
        bool a = HelperValidator.setName(ecoNameTextBox,errorObj,setEcoName);
        IntFunct setAPPC = new IntFunct(ecoEditor.setAbilityPointsPerCreature);
        bool b = HelperValidator.setIntegerFunct(abilityPtsPerCreatTextBox, setAPPC, errorObj);
        IntFunct setCommBits = new IntFunct(ecoEditor.setCommBits);
        bool c = HelperValidator.setIntegerFunct(commBitsText, setCommBits, errorObj);
        IntFunct setDistinctPheno = new IntFunct(ecoEditor.setDistinctPhenotypeNum);
        bool d = HelperValidator.setIntegerFunct(distinctPhenoText, setDistinctPheno, errorObj);

        // saves tentative resource options to Ecosystem object
        ecoEditor.saveResourceOptions();

        // TODO : don't forget to call EcosystemEditor save methods


        /** call methods to save ecoEditor data to actual Ecosystem object **/

        // saves tentative resource options to Ecosystem object
        ecoEditor.saveResourceOptions();

        ecoEditor.saveFoundersToSpecies();

        ecoEditor.addCurrentPopulationToEcosystem();

        ecoEditor.saveMap();
    }

    public void addResource()
    {
        // TODO: change from creature menu to resource menu
        lrMenuBehav.loadLandResMenu(ecoEditor);
        uIParent.SetActive(false);
    }

}
