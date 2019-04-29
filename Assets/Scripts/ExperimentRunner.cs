using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

public class ExperimentRunner : MonoBehaviour
{
    public string fileName = ".\\OutputFiles\\experimentOutput.csv";
    public int instances = 10;
    public int maxLength = 500;
    public bool clearFile = true;

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
        if (clearFile)
        {
        File.WriteAllText(fileName, string.Empty);
        }
        Debug.Log("Low variation system results: ");
        experimentRunSystem(instances, maxLength, false, .2f, .1f);

        Debug.Log("high variation system results: ");
        experimentRunSystem(instances, maxLength, false, .6f, .1f);

        Debug.Log("Experiment done");
    }

    public void experimentRunSystem(int instances, int maxLength, bool nonLinear, float popVar, float indVar)
    {
        EcoDemo1 ecoDemo = new EcoDemo1();

        
        StringBuilder line = new StringBuilder();
        // create x instances of each ecosystem
        float sum = 0;
        for (int i = 0; i < instances; i++)
        {
            // Create a 100 X 100 mp
            ecoDemo.createEcosystem(200);
            // add cat species
            ecoDemo.addSpecies("cow", ColorChoice.blue, indVar, nonLinear, 1, indVar, false, 10);
            // populate with low standard deviation from founder creature
            ecoDemo.populateSpecies("cow", popVar, 500, 1000);

            while (!ecoDemo.getEcosystem().allDead && ecoDemo.getEcosystem().age < maxLength)
            {
                ecoDemo.getEcosystem().runSystem(1);
                //Debug.Log("age: " + ecosystem.age);
            }
            sum += ecoDemo.getEcosystem().age;
            Debug.Log("final age: " + ecoDemo.getEcosystem().age);
            line.Append(ecoDemo.getEcosystem().age.ToString() + ", ");
        }
        line.Append("\n");
        
        File.AppendAllText(fileName, line.ToString());
        Debug.Log("");
        Debug.Log("average: " + sum / instances);
    }

}
