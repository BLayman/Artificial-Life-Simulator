using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

class ThreadManager : MonoBehaviour
{
    EcoManager ecoMan;
    Ecosystem unityEco;
    System.Object funcToRunLock = new System.Object();

    public delegate void anony(); // anony represents an anonymous function with no inputs or outputs. To be stored in the queue

    Queue<anony> functToRun;

    // Thread safe?
    [HideInInspector]
    public int steps = 1;

    void Awake()
    {
        functToRun = new Queue<anony>(); // queue for callback functions
        // create ecosystem using EcoManager
        ecoMan = new EcoManager();
        ecoMan.makeEco();

        // get newly created ecosystem and set unityEco to reference it
        unityEco = ecoMan.getEcosystem();

        // TODO: finish getEcosystemCopy
        // Get a copy of the ecosystem and have simulationEco reference the copy
        //Ecosystem simulationEco = Utility.getEcosystemCopy(unityEco);

        
        //Debug.Log("calling threaded function");
        // create new thread to execute runSystem with the simulationEco copy, for a particular number of steps
        //StartThreadedFunction(() => { runSystem(simulationEco, steps); });
        //Debug.Log("start done");
        //Debug.Log(unityEco.name);
    }

    public Ecosystem getEcosystem()
    {
        return unityEco;
    }

    /*
    void Update()
    {
        updateEcoIfReady();   
    }
    */

    public void StartEcoSim()
    {
        // create a copy, and set simulationEco to set the copy, then use the copy for the simulation
        Ecosystem simulationEco = Utility.getEcosystemCopy(unityEco);
        StartThreadedFunction(() => { runSystem(simulationEco, steps); });
    }

    public bool updateEcoIfReady()
    {

        bool updateOccured = false;
        // if functToRun is not being modified by the child thread
        lock (funcToRunLock)
        {

            /** run one function from the queue **/
            if (functToRun.Count > 0)
            {
                updateOccured = true;
                // take off a function
                anony funct = functToRun.Dequeue();

                Debug.Log(functToRun.Count);

                // run the function: calls applyEcoData(), which sets unityEco to reference the modified copy
                funct();

                //Debug.Log(unityEco.name);

                // TODO: finish getEcosystemCopy

                // start the process over again
                // create a copy, and set simulationEco to set the copy, then use the copy for the simulation
                Ecosystem simulationEco = Utility.getEcosystemCopy(unityEco);
                StartThreadedFunction(() => { runSystem(simulationEco, steps); });
            }
        }

        return updateOccured;
    }

    // start a new thread that calls a function: runSystem(simulationEco, steps)
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

