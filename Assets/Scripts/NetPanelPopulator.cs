// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NetPanelPopulator : MonoBehaviour
{
    public GameObject netButtonPrefab;
    public GameObject canvas;
    public GameObject indNetPan;
    List<GameObject> instantiated = new List<GameObject>();
    List<Dictionary<string, Network>> nets;



    public void setNets(List<Dictionary<string, Network>> _nets)
    {
        nets = _nets;
        float x = 100;
        float y = -70;
        for (int i = 0; i < nets.Count; i++)
        {
            foreach (string key in nets[i].Keys)
            {
                GameObject created = GameObject.Instantiate(netButtonPrefab);
                GameObject netObj = created.transform.GetChild(0).gameObject;
                netObj.GetComponent<Text>().text = key;
                created.transform.SetParent(canvas.transform, false);
                RectTransform transform = created.GetComponent<RectTransform>();
                transform.anchoredPosition = new Vector2(x, y);
                y -= 30;
                instantiated.Add(created);
                Button btn = created.GetComponent<Button>();
                //Debug.Log("listener for " + key + " set with index " + i);
                int localI = i;
                btn.onClick.AddListener(() => setNetworkPanel(localI, key));
            }
            y = -70;
            x += 100;
        }
        
    }


    public void setNetworkPanel(int index, string key)
    {
        Debug.Log("setNetworkPanel called with " + key + " and " + index);
        indNetPan.SetActive(true);
        indNetPan.GetComponent<IndivNetPanelPop>().setNetwork(nets[index][key]);
    }

    private void OnDisable()
    {
        for (int i = 0; i < instantiated.Count; i++)
        {
            GameObject.Destroy(instantiated[i]);
        }
    }
}
