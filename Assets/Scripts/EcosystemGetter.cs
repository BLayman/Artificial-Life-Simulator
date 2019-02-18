using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcosystemGetter : MonoBehaviour
{
    public GameObject simRunnerObj;
    ThreadManager threader;
    // Start is called before the first frame update
    void Awake()
    {
        threader = simRunnerObj.GetComponent<ThreadManager>();
    }

    public int getMapWidth()
    {
        return threader.getEcosystem().map.Count;
    }

    public int getMapHeight()
    {
        return threader.getEcosystem().map[0].Count;
    }

}
