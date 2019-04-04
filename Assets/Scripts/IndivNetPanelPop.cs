using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndivNetPanelPop : MonoBehaviour
{
    public GameObject prefab;
    public GameObject canvas;
    public List<GameObject> instantiated = new List<GameObject>();
    Network net;



    public void setNetwork(Network _net)
    {
        net = _net;
        

    }


    private void OnDisable()
    {
        for (int i = 0; i < instantiated.Count; i++)
        {
            GameObject.Destroy(instantiated[i]);
        }
    }
}
