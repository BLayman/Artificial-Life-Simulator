using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class StatsPrinter : MonoBehaviour
{
    string fileName = ".\\OutputFiles\\output.csv";

    public void printFirstLine(Ecosystem eco)
    {
        StringBuilder csv = new StringBuilder();
        StringBuilder firstLine = new StringBuilder(eco.populations.Keys.Count * 20);
        firstLine.Append("Time Steps, ");
        foreach (string key in eco.populations.Keys)
        {
            firstLine.Append(key + " size, " + key + " variability, ");
        }
        firstLine.Remove(firstLine.Length - 3, 2);
        csv.AppendLine(firstLine.ToString());

        File.AppendAllText(fileName, csv.ToString());
    }

    public void printStats(Ecosystem eco)
    {
        string age = eco.age.ToString();

        StringBuilder csv = new StringBuilder();

        StringBuilder line = new StringBuilder(eco.populations.Keys.Count * 30);
        foreach (string key in eco.populations.Keys)
        {
            line.Append( age + ", " + eco.populations[key].size + ", " + eco.populations[key].overallVariability + ", ");
        }
        line.Remove(line.Length -3, 2);
        //Debug.Log(line);
        csv.AppendLine(line.ToString());
        File.AppendAllText(fileName, csv.ToString());
    }
}
