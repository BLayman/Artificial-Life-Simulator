using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

class ThreadExample : MonoBehaviour
{
    EcoManager ecoMan;
    Ecosystem unityEco;
    System.Object funcToRunLock = new System.Object();

    public delegate void anony();

    Queue<anony> functToRun;

    // Thread safe?
    int steps = 1000000;

    void Start()
    {
        ecoMan = new EcoManager();
        ecoMan.makeEco();

        unityEco = ecoMan.getEcosystem();
        unityEco.name = "Bob";
        // TODO: finish getEcosystemCopy
        Ecosystem simulationEco = Utility.getEcosystemCopy(unityEco);
        functToRun = new Queue<anony>();
        
        Debug.Log("calling threaded function");

        StartThreadedFunction(() => { runSystem(simulationEco, steps); });
        //Debug.Log("start done");
        //Debug.Log(unityEco.name);
    }

    Ecosystem getEcosystem()
    {
        return unityEco;
    }

    void Update()
    {

        lock (funcToRunLock)
        {
            while (functToRun.Count > 0)
            {
                anony funct = functToRun.Dequeue();
                // eventually this line should print Bob, because seperate thread should be modifying seperate Ecosystem object
                //Debug.Log(unityEco.name);
                // calls applyEcoData();
                funct();
                //Debug.Log(unityEco.name);

                // TODO: finish getEcosystemCopy
                Ecosystem simulationEco = Utility.getEcosystemCopy(unityEco);

                StartThreadedFunction(() => { runSystem(simulationEco, steps); });
            }
        }
        
    }


    public void StartThreadedFunction(ThreadStart someFunction)
    {
        Thread t = new Thread(someFunction);
        t.Start();
    }

    // called from child thread to enqueue functions to run
    public void QueueMainThread(anony someFunction)
    {
        // functToRun is instance variable, also modified by main thread, so need to lock
        lock (funcToRunLock)
        {
            functToRun.Enqueue(someFunction);
        }
        
    }

    // eco is address pointing copy of unityEco
    // don't use any variables besides what's sent into the function, or created in the function (to avoid threading issues)
    void runSystem(Ecosystem eco, int steps)
    {
        // do slow thing
        //Thread.Sleep(2000);
        // change data in copy
        //eco.name = eco.name + "1";

        eco.runSystem(steps);

        // this function will be called from main thread
        // it gives unityEco the address of eco, so that it points at the new and updated ecosystem
        anony applyEcoData = () =>
        {
            Debug.Log("safely applying data created in thread."); 
            // make unityEco reference modified copy of itself, NOTE: this won't change all references to the ecosystem, such as those used in the UI 
            unityEco = eco;
            Debug.Log("ecosystem age:" + unityEco.count);
        };

        QueueMainThread(applyEcoData);
    }

}

