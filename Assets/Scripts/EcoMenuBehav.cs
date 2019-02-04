using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcoMenuBehav : MonoBehaviour
{
    public GameObject uIParent;
    public GameObject creatureMenuObj;
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

    }
}
