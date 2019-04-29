// Artificial Life Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

class ThreadManager : MonoBehaviour
{
    DemoInterface ecoMan;
    Ecosystem unityEco;
    System.Object ecoQueueLock = new System.Object();
    System.Object threadFinishedLock = new System.Object();
    System.Object endThreadLock = new System.Object();
    System.Object visibleResourceLock = new System.Object();
    private int bufferLength = 5;
    private bool threadFinished = false;
    private int childThreadSleepTime = 0;
    bool finishChildThread = false;
    LinkedList<Ecosystem> ecoQueue;
    [HideInInspector]
    public string visibleResource;
    [HideInInspector]
    public GameObject statsPrinterObj;
    StatsPrinter statsPrinter;
    public int demoIndex = 1;
    

    // Thread safe?
    [HideInInspector]
    public int steps;

    // anony represents an anonymous function with no inputs or outputs. To be stored in the queue
    public delegate void anony(Ecosystem eco); 



    void Awake()
    {
        // There is a bug where every class is reset mid way through program.
        // I think it only occurs when there are too many debug statements on the child thread, but I'm not sure
        Debug.LogWarning("*********************            thread manager awake                     *************************");

        ecoQueue = new LinkedList<Ecosystem>(); // queue for callback functions

        // create ecosystem using EcoManager
        switch (demoIndex)
        {
            case 1:
                ecoMan = new EcoDemo1();
                break;
            case 2:
                ecoMan = new EcoDemo2();
                break;
            case 3:
                ecoMan = new EcoDemo3();
                break;
            default:
                Debug.LogError("That demo index does not exist.");
                break;
        }
        ecoMan.makeEco();
        // get newly created ecosystem and set unityEco to reference it
        unityEco = ecoMan.getEcosystem();
        visibleResource = unityEco.resourceOptions.Keys.First();

    }

    public void setVisibleResource(string resource)
    {
        lock (visibleResourceLock)
        {
            visibleResource = resource;
        }
    }

    public void setSteps(int _steps)
    {
        steps = _steps;
        // reset the queue
        lock (ecoQueueLock)
        {
            ecoQueue.Clear();
        }

        // signal child thread to stop looping, so new thread can start
        lock (endThreadLock)
        {
            finishChildThread = true;
        }

    }

    public Ecosystem getEcosystem()
    {
        
        return unityEco;
        

    }

    public void StartEcoSim()
    {
        statsPrinter = statsPrinterObj.GetComponent<StatsPrinter>();
        statsPrinter.printFirstLine(unityEco);

        // create a copy, and set simulationEco to set the copy, then use the copy for the simulation
        Ecosystem simulationEco = Copier.getEcosystemCopy(unityEco);
        int localSteps = steps; // just in case next line causes threading error
        StartThreadedFunction(() => { runSystem(simulationEco, localSteps, Thread.CurrentThread); });
    }


    public bool updateEcoIfReady()
    {
        

        bool updateOccured = false; // change to true if update occured
        Ecosystem lastEnqueued;
        bool checkFinished;

        // check if child thread has finished (only one child thread runs at a time)
        lock (threadFinishedLock)
        {
            checkFinished = threadFinished;
        }
        // if finished, we may want to restart the thread
        if (checkFinished)
        {
            int queueLength;
            // get length of queue to see if it's short enough to run the simulation thread again
            lock (ecoQueueLock)
            {
                queueLength = ecoQueue.Count;
            }
            // if the last thread has finished and the queue length is getting short, run the simulation some more.
            if (queueLength < bufferLength)
            {
                bool emptyQueue;
                // check if queue is empty
                lock (ecoQueueLock)
                {
                    emptyQueue = (ecoQueue.Last == null);
                }
                // if it's not empty, start simulation where it left off with last ecosystem in queue
                if (!emptyQueue)
                {
                    lastEnqueued = ecoQueue.Last.Value;
                }
                // otherwise unityEco will hold the most recent ecosystem
                else
                {
                    lastEnqueued = unityEco;
                }
                // lastEnqueued.populations[cow].creatures[0].printNetworks();
                // create a copy using the latest ecosystem, then use the copy for the simulation
                Ecosystem simulationEco = Copier.getEcosystemCopy(lastEnqueued);
                // this will start the child thread, reseting checkFinished to false
                int localSteps = steps; // just in case next line causes threading error
                StartThreadedFunction(() => { runSystem(simulationEco, localSteps, Thread.CurrentThread); });
            }   
        } // end checkFinished

        // if ecoQueue is not being modified by the child thread
        lock (ecoQueueLock)
        {
            // if the queue isn't empty, update ecosystem with what's on it
            if (ecoQueue.Count > 0)
            {
                updateOccured = true;
                // remove ecosystem from queue
                Ecosystem updatedEco = ecoQueue.First.Value;
                ecoQueue.RemoveFirst();
                
                // make unityEco reference modified copy of itself, NOTE: this won't change all references to the ecosystem, such as those used in the UI 
                unityEco = updatedEco;

                statsPrinter.printStats(unityEco);
                //Debug.Log("ecosystem age:" + unityEco.count);
            }
        }

        return updateOccured; // return whether or not the ecosystem was updated (if not we don't need to re-render it)
    }

    // start a new thread that calls a function: runSystem(simulationEco, steps)
    public void StartThreadedFunction(ThreadStart someFunction)
    {
        Thread t = new Thread(someFunction);
        t.Start();
    }


    // called on child thread to enqueue functions to run
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
    void runSystem(Ecosystem eco, int Localsteps, Thread mainThread)
    {
        

        string localVisRes;
        lock (threadFinishedLock)
        {
            threadFinished = false;
        }
        // change data in copy
        //eco.name = eco.name + "1";
        for (int i = 0; i < bufferLength; i++)
        {

            /*
            // terminate thread if main thread dies
            if (!mainThread.IsAlive)
            {
                Debug.Log("main thread dead, aborting simulation thread");
                Thread.CurrentThread.Abort();
            }
            */

            // get whether finishChildThread is true
            bool tempFinishChildThread;
            lock (endThreadLock)
            {
                tempFinishChildThread = finishChildThread;
            }

            // if it shouldn't be ended, run simulation and add state to queue
            if (!tempFinishChildThread)
            {
                

                eco.runSystem(Localsteps); // run ecosystem
                //float st = System.DateTime.Now.Second;
                //Debug.Log("starting threads");
                

                // update visible resource in case it was changed by the user
                lock (visibleResourceLock)
                {
                    localVisRes = visibleResource;
                }

                eco.updateTexture(localVisRes);
                /*
                eco.startTextureThreads();
                
                while (!eco.checkThreadsFinished())
                {
                    Thread.Sleep(20);
                }
                */

                //Debug.Log("threads done");
                //float et = System.DateTime.Now.Second;
                //Debug.Log(et - st);


                //Debug.Log("all threads finished");

                QueueMainThread(eco); // queue main with ecosystem

                eco = Copier.getEcosystemCopy(eco); // make eco point to a new ecosystem object, so that each object in the queue is different

                Thread.Sleep(childThreadSleepTime); // give time for other thread to catch up
            }
            // if it should be ended
            else
            {
                // reset variable, and break out of loop
                lock (endThreadLock)
                {
                    finishChildThread = false;
                }
                break;
            }
            
        }


        lock (threadFinishedLock)
        {
            threadFinished = true;
        }
        
    }

}

