using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LRMenuBehav : MonoBehaviour
{
    LandResourceEditor lrEditor;
    EcosystemEditor ecoEditor;

    public GameObject uIParent;

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
        lrEditor = ee.lre;
        uIParent.SetActive(true);

        //TODO: load information into menu from prev created resource
    }

    /// <summary>
    /// save the lrc's LandResource to the ecosystem
    /// </summary>
    public void saveSettings()
    {
        ecoEditor.saveResource(); // saves to tentative resources
    }
}
