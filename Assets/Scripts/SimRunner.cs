using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimRunner : MonoBehaviour {
    EcoManager ecoMan;
    public GameObject tilePrefab;
    List<GameObject> tiles = new List<GameObject>();

	// Use this for initialization
	void Start () {
        ecoMan = new EcoManager();
        ecoMan.makeEco();
        
	}
	
	// Update is called once per frame
	void Update () {
        ecoMan.runSystem(1);
        render(ecoMan.getEcosystem());
        clearOldMap();

        GameObject tile = GameObject.Instantiate(tilePrefab, new Vector3(1, 1, 0), Quaternion.identity);
        GameObject tile2 = GameObject.Instantiate(tilePrefab, new Vector3(0, 0, 0), Quaternion.identity);

        tiles.Add(tile);
        tiles.Add(tile2);

    }

    private void clearOldMap()
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            GameObject.Destroy(tiles[i]);
        }
        tiles.Clear();
    }

    private void render(Ecosystem sys)
    {

    }
}
