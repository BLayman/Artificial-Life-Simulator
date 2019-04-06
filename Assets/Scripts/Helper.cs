using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Helper
{
    public static string neighborIndexToWord(int i)
    {
        //Debug.Log("neighbor index " + i);
        string word = "";
        switch (i)
        {
            case 0:
                word = "center";
                break;
            case 1:
                word = "up";
                break;
            case 2:
                word = "down";
                break;
            case 3:
                word = "left";
                break;
            case 4:
                word = "right";
                break;
            default:
                break;
        }
        //Debug.Log(word);
        return word;
    }

}

