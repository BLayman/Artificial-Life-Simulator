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
        int x = 147;
        int y = -80;

        node = _node;
        Debug.Log(node.value);

        List<float> averages = node.getWeightAverages();
        List<float> sDs = node.getWeightSDs();
        Debug.Log(averages.Count);

        for (int i = 0; i < node.weights.Count; i++)
        {
            GameObject created = GameObject.Instantiate(textObj);
            Text txt = created.GetComponent<Text>();

            double rounded = System.Math.Round((double)node.weights[i], 2);

            // case for phenotype network nodes
            if (averages.Count == node.weights.Count)
            {
                double rounded2 = System.Math.Round((double)averages[i], 2);
                double rounded3 = System.Math.Round((double)sDs[i], 2);
                txt.text = rounded.ToString() + "          " + rounded2 + "           " + rounded3;
            }
            else
            {
                txt.text = rounded.ToString();
            }
            
            


            

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
