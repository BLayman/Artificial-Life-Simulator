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
    System.Object ecoQueueLock = new System.Object();
    System.Object threadFinishedLock = new System.Object();
    private int bufferLength = 20;
    private bool threadFinished = false;
    private int childThreadSleepTime = 1;

    public delegate void anony(Ecosystem eco); // anony represents an anonymous function with no inputs or outputs. To be stored in the queue

    LinkedList<Ecosystem> ecoQueue;

    // Thread safe?
    [HideInInspector]
    public int steps;

    void Awake()
    {
        ecoQueue = new LinkedList<Ecosystem>(); // queue for callback functions
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
        StartThreadedFunction(() => { runSystem(simulationEco, steps, Thread.CurrentThread); });
    }

    // update will restart the threaded function when necessary
    private void Update()
    {
        Ecosystem lastEnqueued;
        bool checkFinished;
        lock (threadFinishedLock)
        {
            checkFinished = threadFinished;
        }
        if (checkFinished)
        {
            int queueLength;
            lock (ecoQueueLock)
            {
                queueLength = ecoQueue.Count;
                if(ecoQueue.Last != null)
                {
                    lastEnqueued = ecoQueue.Last.Value;
                }
                else
                {
                    lastEnqueued = unityEco;
                }
                
            }
            // if the last thread has finished and the queue length is getting short, run the simulation some more.
            if (queueLength < bufferLength)
            {
                // create a copy, and set simulationEco to set the copy, then use the copy for the simulation
                Ecosystem simulationEco = Utility.getEcosystemCopy(lastEnqueued);
                // this will reset checkFinished to false
                StartThreadedFunction(() => { runSystem(simulationEco, steps, Thread.CurrentThread); });
            }
        }

        
    }

    public bool updateEcoIfReady()
    {
        bool updateOccured = false;

        // if ecoQueue is not being modified by the child thread
        lock (ecoQueueLock)
        {
            // if the queue isn't empty, update ecosystem with what's on it
            if (ecoQueue.Count > 0)
            {
                updateOccured = true;
                // take off a function
                Ecosystem updatedEco = ecoQueue.First.Value;
                ecoQueue.RemoveFirst();

                Debug.Log("Length of Queue: " + ecoQueue.Count);

                // call applyEcoData(), which sets unityEco to reference the modified copy
                applyEcosystemData(updatedEco);

                
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


    void applyEcosystemData(Ecosystem eco)
    {
        Debug.Log("safely applying data created in thread.");
        // make unityEco reference modified copy of itself, NOTE: this won't change all references to the ecosystem, such as those used in the UI 
        unityEco = eco;
        Debug.Log("ecosystem age:" + unityEco.count);
    }


    // called from child thread to enqueue functions to run
    public void QueueMainThread(Ecosystem ecosys)
    {
        // lock ecoQueue before enqueueing
        lock (ecoQueueLock)
        {
            ecoQueue.AddLast(ecosys);
            //Debug.Log("adding to queue with length: " + ecoQueue.Count);
        }

    }

    // eco is address pointing copy of unityEco
    // don't use any variables besides what's sent into the function, or created in the function (to avoid threading issues)
    void runSystem(Ecosystem eco, int steps, Thread mainThread)
    {

        lock (threadFinishedLock)
        {
            threadFinished = false;
        }
        // change data in copy
        //eco.name = eco.name + "1";
        for (int i = 0; i < bufferLength; i++)
        {
            // terminate thread if main thread dies
            if (!mainThread.IsAlive)
            {
                Thread.CurrentThread.Abort();
            }
        
            eco.runSystem(steps); // run ecosystem

            QueueMainThread(eco); // queue main with ecosystem

            eco = Utility.getEcosystemCopy(eco); // make eco point to a new ecosystem object, so that each object in the queue is different

            Thread.Sleep(childThreadSleepTime); // give time for other thread to catch up
        }

        lock (threadFinishedLock)
        {
            threadFinished = true;
        }
        
    }

}

