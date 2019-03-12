// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LRMenuBehav : MonoBehaviour
{
    public LandResourceEditor lrEditor;
    EcosystemEditor ecoEditor;

    public GameObject uIParent;
    public GameObject errorObj;

    public GameObject amtStoredText;
    public GameObject maxAmtText;
    public GameObject renewAmtText;
    public GameObject amtConsText;
    public GameObject resNameText;
    public GameObject propExtractText;

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
    public void loadLandResMenu(EcosystemEditor ee)
    {
        ecoEditor = ee;

        uIParent.SetActive(true);

        //TODO: load information into menu from prev created resource
    }


    /// <summary>
    /// saves all information that the user entered into the menu
    /// </summary>
    public void saveSettings()
    {
        string name = resNameText.GetComponent<Text>().text;
        ecoEditor.addResource(name);
        lrEditor = ecoEditor.lre;

        // TODO : covert HelperSetter to ValidationHelper

        string amtstoredString = amtStoredText.GetComponent<Text>().text;
        if (HelperValidator.validateFloatString(amtstoredString))
        {
            lrEditor.setAmountOfResource(float.Parse(amtstoredString));
        }

        string maxAmtString = maxAmtText.GetComponent<Text>().text;
        if (HelperValidator.validateFloatString(maxAmtString))
        {
            lrEditor.setMaxAmt(float.Parse(maxAmtString));
        }

        string renAmtString = renewAmtText.GetComponent<Text>().text;
        if (HelperValidator.validateFloatString(renAmtString))
        {
            lrEditor.setRenewalAmt(float.Parse(renAmtString));
        }

        string amtConsString = amtConsText.GetComponent<Text>().text;
        if (HelperValidator.validateFloatString(amtConsString))
        {
            lrEditor.setAmtConsumedPerTime(float.Parse(amtConsString));
        }


        // TODO: validate floats ( < 1)
        float prop;
        float.TryParse(propExtractText.GetComponent<Text>().text, out prop);
        lrEditor.setProportionExtracted(prop);

        ecoEditor.saveResource(); // saves to tentative resources
    }


}
