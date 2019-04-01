using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ExperimentRunner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartThreadedFunction(runExperiment);
    }

    // start a new thread that calls a function: runSystem(simulationEco, steps)
    public void StartThreadedFunction(ThreadStart someFunction)
    {
        Thread t = new Thread(someFunction);
        t.Start();
    }

    public void runExperiment()
    {
        EcoDemo1 demo = new EcoDemo1();
        demo.runExperiment(5, 500);
    }

}
