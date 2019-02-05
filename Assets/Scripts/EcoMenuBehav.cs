using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void IntFunct(int x);

public class EcoMenuBehav : MonoBehaviour
{
    public GameObject uIParent;
    public GameObject errorObj;
    public GameObject creatureMenuObj;

    public GameObject ecoNameTextBox;
    public GameObject abilityPtsPerCreatTextBox;
    public GameObject commBitsText;
    public GameObject distinctPhenoText;
    public GameObject timeUnitsText;

    CreatMenuBehav creatMenuBehav;
    public EcosystemEditor ecoCreator;

    // Start is called before the first frame update
    void Start()
    {
        creatMenuBehav = creatureMenuObj.GetComponent<CreatMenuBehav>();
        // TODO: set other child menus
    }

    /// <summary>
    /// creates an ecosystem creator from the provided ecosystem
    /// </summary>
    public void loadEcoMenu(Ecosystem eco)
    {
        ecoCreator = new EcosystemEditor(eco);
        uIParent.SetActive(true);
        // TODO: properly populate menu from eco data
    }

    /// <summary>
    /// load the creature editor menu and pass it the current ecosystem editor
    /// </summary>
    public void createCreature()
    {
        creatMenuBehav.loadCreatMenu(ecoCreator);

    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public void saveSettings()
    {
        bool a = setEcoName();
        IntFunct setAPPC = new IntFunct(ecoCreator.setAbilityPointsPerCreature);
        bool b = setIntegerFunct(abilityPtsPerCreatTextBox, setAPPC);
        IntFunct setCommBits = new IntFunct(ecoCreator.setCommBits);
        bool c = setIntegerFunct(commBitsText, setCommBits);
        IntFunct setDistinctPheno = new IntFunct(ecoCreator.setDistinctPhenotypeNum);
        bool d = setIntegerFunct(distinctPhenoText, setDistinctPheno);
        IntFunct setTUPT = new IntFunct(ecoCreator.setTimeUnitsPerTurn);
        bool e = setIntegerFunct(timeUnitsText, setTUPT);

    }

    public bool setEcoName()
    {
        bool valid = false;
        string nameText = ecoNameTextBox.GetComponent<Text>().text;
        if (nameText.Equals(""))
        {
            string errorText = "Empty input error.";
            Debug.LogError(errorText);
            errorObj.GetComponent<Text>().text = errorText;
            errorObj.SetActive(true);
        }
        else
        {
            ecoCreator.setName(nameText);
            valid = true;
        }
        return valid;
    }

    public bool setIntegerFunct(GameObject go, IntFunct setInt)
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




}
