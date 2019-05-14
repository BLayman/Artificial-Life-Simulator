// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class CreatureAveragesIO : MonoBehaviour
{

    const string folder = "WeightAverages";
    const string fileName = "averages.dat";
    public static string persistantPath;




    public static void saveAverages(List<float> weightAverages)
    {

        string dirPath = Path.Combine(persistantPath, folder);
        // create directory if it doesn't exist
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        string fullPath = Path.Combine(dirPath, fileName);

        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Open(fullPath, FileMode.OpenOrCreate))
        {
            binaryFormatter.Serialize(fileStream, weightAverages);
        }
    }


    public static List<float> loadAverages()
    {
        string dirPath = Path.Combine(Application.persistentDataPath, folder);
        string fullPath = Path.Combine(dirPath, fileName);

        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (FileStream fileStream = File.Open(fullPath, FileMode.Open))
        {
            return (List<float>) binaryFormatter.Deserialize(fileStream);
        }
    }

}
