using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public delegate void IntFunct(int x);
public delegate void StringFunct(string s);


// TODO: change from setter to validator
public class HelperSetter
{
    public static GameObject errorObj;

    public static bool validateIntegerString(string text)
    {
        bool valid = false;
        int intVal;
        if (text.Equals(""))
        {
            string errorText = "Empty input error.";
            Debug.LogError(errorText);
            errorObj.GetComponent<Text>().text = errorText;
            errorObj.SetActive(true);
        }
        else if (text.Contains("."))
        {
            string errorText = "Floating point given instead of integer.";
            Debug.LogError(errorText);
            errorObj.GetComponent<Text>().text = errorText;
            errorObj.SetActive(true);
        }
        else if (Int32.TryParse(text, out intVal))
        {
            valid = true;
        }
        else
        {
            string errorText = "Error parsing integer from string.";
            Debug.LogError(errorText);
            Debug.LogError(text);
            errorObj.GetComponent<Text>().text = errorText;
            errorObj.SetActive(true);
        }
        return valid;
    }


    // parses string to integer and passes it to a function, sends error message to error game object
    public static bool setIntegerFunct(GameObject go, IntFunct setInt, GameObject errorObj)
    {
        bool valid = false;
        string text = go.GetComponent<Text>().text;
        int intVal;
        if (text.Equals(""))
        {
            string errorText = "Empty input error.";
            Debug.LogError(errorText);
            errorObj.GetComponent<Text>().text = errorText;
            errorObj.SetActive(true);
        }
        else if (text.Contains("."))
        {
            string errorText = "Floating point given instead of integer.";
            Debug.LogError(errorText);
            errorObj.GetComponent<Text>().text = errorText;
            errorObj.SetActive(true);
        }
        else if (Int32.TryParse(text, out intVal))
        {
            setInt(intVal);
            valid = true;
        }
        else
        {
            string errorText = "Error parsing integer from string.";
            Debug.LogError(errorText);
            Debug.LogError(text);
            errorObj.GetComponent<Text>().text = errorText;
            errorObj.SetActive(true);
        }
        return valid;
    }

    // calls EcosystemEditor method to save ecosystem name
    public static bool setName(GameObject textBox, GameObject errorObj, StringFunct stringSetter)
    {
        bool valid = false;
        string nameText = textBox.GetComponent<Text>().text;
        if (nameText.Equals(""))
        {
            string errorText = "Empty input error.";
            Debug.LogError(errorText);
            errorObj.GetComponent<Text>().text = errorText;
            errorObj.SetActive(true);
        }
        else
        {
            stringSetter(nameText);
            valid = true;
        }
        return valid;
    }

}
