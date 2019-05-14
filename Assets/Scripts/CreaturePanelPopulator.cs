// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CreaturePanelPopulator : MonoBehaviour
{

    public GameObject speciesObj;
    public GameObject iDObj;
    public GameObject healthObj;
    public GameObject mutationAmtObj;

    Text speciesText;
    Text iDText;
    Text healthText;
    Text mutationAmtText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        speciesText = speciesObj.GetComponent<Text>();
        iDText = iDObj.GetComponent<Text>();
        healthText = healthObj.GetComponent<Text>();
        mutationAmtText = mutationAmtObj.GetComponent<Text>();
    }

    public void setData(string species, string iD, string health, string mutationAmt, string maxHealth)
    {
        speciesText.text = species;
        iDText.text = iD;
        healthText.text = health + " / " + maxHealth;
        mutationAmtText.text = mutationAmt;

    }
}
