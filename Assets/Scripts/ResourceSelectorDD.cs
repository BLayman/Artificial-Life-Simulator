// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceSelectorDD : MonoBehaviour
{
    public GameObject dataRetriever;
    public GameObject simRunner;
    Ecosystem sys;
    Dropdown dropD;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void menuStart()
    {
        List<string> options = new List<string>();
        dropD = gameObject.GetComponent<Dropdown>();
        sys = dataRetriever.GetComponent<EcosystemGetter>().GetEcosystem();
        foreach (string res in sys.resourceOptions.Keys)
        {
            options.Add(res);
        }
        options.Add("Black");
        dropD.ClearOptions();
        dropD.AddOptions(options);

        dropD.onValueChanged.AddListener(delegate {
            valueChanged();
        });
    }

    void valueChanged()
    {
        simRunner.GetComponent<UIListener>().setVisibleResource(dropD.captionText.text);
    }


}
