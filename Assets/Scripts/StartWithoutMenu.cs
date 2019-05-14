// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWithoutMenu : MonoBehaviour
{
    public GameObject managerObj;
    public GameObject cameraObj;
    public GameObject dropDown;
    public int demoIndex;

    // Start is called before the first frame update
    void Start()
    {
        managerObj.GetComponent<ThreadManager>().menuStart(demoIndex);
        managerObj.GetComponent<SimRunnerTest>().menuStart();
        cameraObj.GetComponent<CameraPositioner>().menuStart();
        dropDown.GetComponent<ResourceSelectorDD>().menuStart();

    }


}
