using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EcoStatsPopulator : MonoBehaviour
{
    public GameObject ecoGetterObj;
    EcosystemGetter ecoGetter;
    public GameObject canvas;
    public GameObject textPrefab;
    public GameObject keyValPrefab;
    [HideInInspector]
    public List<GameObject> instantiated = new List<GameObject>();


    public void populateFields()
    {
        ecoGetter = ecoGetterObj.GetComponent<EcosystemGetter>();
        int x = 158;
        float y = -14;

        // display age
        float age = ecoGetter.getAge();
        GameObject created = GameObject.Instantiate(textPrefab);
        Text txt = created.GetComponent<Text>();
        txt.text = age.ToString();

        created.transform.SetParent(canvas.transform, false);
        RectTransform transform = created.GetComponent<RectTransform>();
        transform.anchoredPosition = new Vector2(x, y);
        instantiated.Add(created);

        // display population variabilities
        Dictionary<string, float> popVars = ecoGetter.getPopVariabilities();
        x = 318;
        y = -4.7f;

        foreach (string key in popVars.Keys)
        {
            created = GameObject.Instantiate(keyValPrefab);
            
            GameObject keyObj = created.transform.GetChild(0).gameObject;
            GameObject valObj = created.transform.GetChild(1).gameObject;

            keyObj.GetComponent<Text>().text = key;
            valObj.GetComponent<Text>().text = popVars[key].ToString();

            created.transform.SetParent(canvas.transform, false);
            transform = created.GetComponent<RectTransform>();
            transform.anchoredPosition = new Vector2(x, y);

            instantiated.Add(created);

            x += 60;
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
