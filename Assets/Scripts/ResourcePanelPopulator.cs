using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourcePanelPopulator : MonoBehaviour
{
    public GameObject keyValPrefab;
    public GameObject canvas;
    public List<GameObject> instantiated = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setResources(Dictionary<string, CreatureResource> resources)
    {
        float x = 50;
        float y = -50;
        foreach (string key in resources.Keys)
        {
            GameObject created = GameObject.Instantiate(keyValPrefab);
            GameObject keyObj = created.transform.GetChild(0).gameObject;
            GameObject valObj = created.transform.GetChild(1).gameObject;
            Debug.Log(key + resources[key].currentLevel);
            keyObj.GetComponent<Text>().text = key;
            string currentLvl = (System.Math.Round(resources[key].currentLevel, 2)).ToString();
            valObj.GetComponent<Text>().text = currentLvl + " / " + resources[key].maxLevel.ToString();
            created.transform.SetParent(canvas.transform, false);
            RectTransform transform = created.GetComponent<RectTransform>();
            transform.anchoredPosition = new Vector2(x,y);
            y -= 25;
            instantiated.Add(created);
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < instantiated.Count; i++)
        {
            GameObject.Destroy(instantiated[i]);
        }
    }

}
