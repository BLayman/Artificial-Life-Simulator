using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreatureSelector : MonoBehaviour
{
    float timePressed;
    float startTime;
    public GameObject dataRetriever;
    EcosystemGetter ecoGetter;
    public GameObject creatureUIPanel;
    public GameObject uIDataSetter;
    public GameObject resourcePanel;
    public GameObject netsPanel;
    CreaturePanelPopulator dataSetter;
    ResourcePanelPopulator resPop;
    NetPanelPopulator netsPop;
    Ecosystem eco;
    CreatureGetter cg;

    // Start is called before the first frame update
    void Start()
    {
        ecoGetter = dataRetriever.GetComponent<EcosystemGetter>();
        dataSetter = uIDataSetter.GetComponent<CreaturePanelPopulator>();
        resPop = resourcePanel.GetComponent<ResourcePanelPopulator>();
        netsPop = netsPanel.GetComponent<NetPanelPopulator>();
    }

    // Update is called once per frame
    void Update()
    {
        // if mouse button is being pressed, increment timePressed
        if (Input.GetMouseButton(0) && EventSystem.current.currentSelectedGameObject == null)
        {
            timePressed += Time.deltaTime;
        }

        // if mouse button was pressed this frame, reset timePressed
        if (Input.GetMouseButtonDown(0) && EventSystem.current.currentSelectedGameObject == null)
        {
            timePressed = 0;
        }
        // mouse button release
        if (Input.GetMouseButtonUp(0) && EventSystem.current.currentSelectedGameObject == null)
        {
            // if short enough time pressed, treat as mouse click, not drag
            if(timePressed < .2f)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                int x = (int)ray.origin.x;
                int y = (int)ray.origin.y;

                Debug.Log(x + ", " + y);
                retrieveCreatureData(x, y);
            }

            
        }
    }

    void retrieveCreatureData(int x, int y)
    {
        eco = ecoGetter.GetEcosystem();
        if (x > 0 && x < eco.map.Count && y > 0 && y < eco.map[0].Count)
        {
            if (eco.map[x][y].creatureIsOn())
            {
                Debug.Log("found creature");
                cg = new CreatureGetter(eco.map[x][y].creatureOn);
                string species = cg.getSpecies();
                string iD = cg.getId();
                string health = cg.getHealth();
                string maxHealth = cg.getMaxHealth();
                string mutationAmt = cg.getMutationAmt();
                creatureUIPanel.SetActive(true);
                dataSetter.setData(species, iD, health, mutationAmt, maxHealth);

            }
        }
    }

    public void setCreatureResources()
    {
        resourcePanel.SetActive(true);
        resPop.setResources(cg.getResources());
    }

    public void viewCreatureNeuralNets()
    {
        netsPanel.SetActive(true);
        List<Dictionary<string, Network>> nets = cg.getNets();
        netsPop.setNets(nets);
    }
}
