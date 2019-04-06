using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class IndivNetPanelPop : MonoBehaviour
{
    public GameObject nodePrefab;
    public GameObject canvas;
    public List<GameObject> instantiated = new List<GameObject>();
    Network net;



    public void setNetwork(Network _net)
    {
        int x = 130;
        int y = -50;

        net = _net;
        List<List<Node>> layers = net.net;
        for (int i = 0; i < layers.Count; i++)
        {
            for (int j = 0; j < layers[i].Count; j++)
            {
                GameObject created = Instantiate(nodePrefab);

                Text title = created.transform.GetChild(1).gameObject.GetComponent<Text>();
                Text value = created.transform.GetChild(2).gameObject.GetComponent<Text>();
                
                string type = layers[i][j].GetType().Name;
                //Debug.Log("type " + type);
                string titleText = "";
                switch (type)
                {
                    case "SensoryInputNode":
                        SensoryInputNode siNode = (SensoryInputNode) layers[i][j];
                        titleText = "sense " + siNode.sensedResource + " " + Helper.neighborIndexToWord(siNode.neighborLandIndex);
                        //Debug.Log("land index " + siNode.neighborLandIndex);
                        break;
                    case "BiasNode":
                        titleText = "Bias Node";
                        break;
                    default:
                        titleText = "";
                        break;
                }
                double rounded = System.Math.Round((double)layers[i][j].value, 2);
                value.text = rounded.ToString();
                title.text = titleText;

                created.transform.SetParent(canvas.transform, false);
                RectTransform transform = created.GetComponent<RectTransform>();
                transform.anchoredPosition = new Vector2(x, y);
                y -= 40;
                if(j % 8 == 0 && j != 0)
                {
                    x += 70;
                    y = -50;
                }
                instantiated.Add(created);
            }
            x += 120;
            y = -50;
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
