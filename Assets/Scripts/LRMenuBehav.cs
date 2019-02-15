using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LRMenuBehav : MonoBehaviour
{
    public LandResourceEditor lrEditor;
    EcosystemEditor ecoEditor;

    public GameObject uIParent;
    public GameObject errorObj;

    public GameObject amtStoredText;
    public GameObject maxAmtText;
    public GameObject renewAmtText;
    public GameObject amtConsText;
    public GameObject resNameText;
    public GameObject propExtractText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// load the creature editor menu using the ecosystem editor
    /// </summary>
    public void loadLandResMenu(EcosystemEditor ee)
    {
        ecoEditor = ee;

        uIParent.SetActive(true);

        //TODO: load information into menu from prev created resource
    }


    /// <summary>
    /// saves all information that the user entered into the menu
    /// </summary>
    public void saveSettings()
    {
        string name = resNameText.GetComponent<Text>().text;
        ecoEditor.addResource(name);
        lrEditor = ecoEditor.lre;

        // TODO : covert HelperSetter to ValidationHelper
        IntFunct setAmtStored = new IntFunct(lrEditor.setAmountOfResource);
        bool b = HelperSetter.setIntegerFunct(amtStoredText, setAmtStored, errorObj);
        IntFunct setMaxAmt = new IntFunct(lrEditor.setMaxAmt);
        bool c = HelperSetter.setIntegerFunct(maxAmtText, setMaxAmt, errorObj);
        IntFunct setRenewAmt = new IntFunct(lrEditor.setRenewalAmt);
        bool d = HelperSetter.setIntegerFunct(renewAmtText, setRenewAmt, errorObj);
        IntFunct setAmtConsumed = new IntFunct(lrEditor.setAmtConsumedPerTime);
        bool e = HelperSetter.setIntegerFunct(amtConsText, setAmtConsumed, errorObj);

        // TODO: validate floats ( < 1)
        float prop;
        float.TryParse(propExtractText.GetComponent<Text>().text, out prop);
        lrEditor.setProportionExtracted(prop);

        ecoEditor.saveResource(); // saves to tentative resources
    }


}
