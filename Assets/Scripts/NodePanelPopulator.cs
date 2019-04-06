using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodePanelPopulator : MonoBehaviour
{
    public GameObject textObj;
    public GameObject canvas;
    [HideInInspector]
    public List<GameObject> instantiated = new List<GameObject>();
    NonInputNode node;



    public void setNode(NonInputNode _node)
    {
        int x = 127;
        int y = -80;

        node = _node;
        Debug.Log(node.value);
        for (int i = 0; i < node.weights.Count; i++)
        {
            GameObject created = GameObject.Instantiate(textObj);
            Text txt = created.GetComponent<Text>();

            double rounded = System.Math.Round((double)node.weights[i], 2);
            txt.text = rounded.ToString();
            created.transform.SetParent(canvas.transform, false);
            RectTransform transform = created.GetComponent<RectTransform>();
            transform.anchoredPosition = new Vector2(x, y);
            y -= 20;
            if(i % 10 == 0 && i != 0)
            {
                y = -80;
                x += 70;
            }
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
