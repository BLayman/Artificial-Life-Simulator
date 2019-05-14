// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcoMenuTester : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        EcoMenuBehav ecoMenu = GameObject.Find("EcosystemMenu").GetComponent<EcoMenuBehav>();
        Ecosystem eco = new Ecosystem();
        ecoMenu.loadEcoMenu(eco);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
