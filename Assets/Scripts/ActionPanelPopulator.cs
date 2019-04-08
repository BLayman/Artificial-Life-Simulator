using Priority_Queue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ActionPanelPopulator : MonoBehaviour
{
    public GameObject prefab;
    public GameObject canvas;
    [HideInInspector]
    public List<GameObject> instantiated = new List<GameObject>();



    public void setActions(SimplePriorityQueue<Action> actionQueue)
    {
        int x = 80;
        int y = -40;

        int i = 1;
        /*
        foreach (Action action in actionQueue)
        {
            GameObject created = GameObject.Instantiate(prefab);
            instantiated.Add(created);

            GameObject keyObj = created.transform.GetChild(0).gameObject;
            GameObject valObj = created.transform.GetChild(1).gameObject;
            keyObj.GetComponent<Text>().text = action.name;
            valObj.GetComponent<Text>().text = i.ToString();
            i++;

            created.transform.SetParent(canvas.transform, false);
            RectTransform transform = created.GetComponent<RectTransform>();
            transform.anchoredPosition = new Vector2(x, y);

            y += 40;
        }
        */
        Debug.Log("queue length: " + actionQueue.Count);
        Debug.Log("queue front: " + actionQueue.First.name);
    }


    private void OnDisable()
    {
        for (int i = 0; i < instantiated.Count; i++)
        {
            GameObject.Destroy(instantiated[i]);
        }
    }
}
