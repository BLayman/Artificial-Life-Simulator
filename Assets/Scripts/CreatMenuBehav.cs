using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatMenuBehav : MonoBehaviour
{
    CreatureEditor creatureCreator;
    EcosystemEditor ecoCreator;

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
    public void loadCreatMenu(EcosystemEditor ec)
    {
        ecoCreator = ec;
        creatureCreator = ec.creatureCreator;
        //TODO: display creature menu game object
    }

    /// <summary>
    /// save the creature creator's creature to the ecosystem
    /// </summary>
    public void saveSettings()
    {
        // adds creature to list of founders
        ecoCreator.addToFounders();
        // saves founders to ecosystem species list
        ecoCreator.saveFoundersToSpecies();
    }
}
