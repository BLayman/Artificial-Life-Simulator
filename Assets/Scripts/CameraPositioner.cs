﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositioner : MonoBehaviour
{
    public GameObject getterObj;
    EcosystemGetter ecoGetter;

    // Start is called before the first frame update
    void Start()
    {
        ecoGetter = getterObj.GetComponent<EcosystemGetter>();
        int w = ecoGetter.getMapWidth();
        int h = ecoGetter.getMapHeight();
        gameObject.transform.position = new Vector3(w/2f,h/2f, -1);
        //gameObject.transform.localScale = new Vector3(h, h, h);
        gameObject.GetComponent<Camera>().orthographicSize = h/2;
    }


}