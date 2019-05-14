// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class IndivNetPanelPop : MonoBehaviour
{
    public GameObject nodePrefab;
    public GameObject canvas;
    public GameObject nodePanel;
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
                Button btn = created.transform.GetChild(3).gameObject.GetComponent<Button>();
                //btn.name = j.ToString();

                string type = layers[i][j].GetType().Name;
                //Debug.Log("type " + type);
                bool castWorked = true;
                NonInputNode nodeArg = null;
                try
                {
                    nodeArg = (NonInputNode)layers[i][j];
                }
                catch (InvalidCastException e)
                {
                    castWorked = false;
                }
                if (castWorked)
                {
                    btn.onClick.AddListener(() => { createNodePanel(nodeArg); });

                }


                string titleText = "";
                switch (type)
                {
                    case "SensoryInputNode":
                        SensoryInputNode siNode = (SensoryInputNode) layers[i][j];
                        titleText = "sense " + siNode.sensedResource + " " + Helper.neighborIndexToWord(siNode.neighborLandIndex);
                        break;
                    case "OutputNode":
                        OutputNode outNode = (OutputNode)layers[i][j];
                        titleText = outNode.action.name;
                        break;
                    case "InternalResourceInputNode":
                        InternalResourceInputNode internNode = (InternalResourceInputNode)layers[i][j];
                        titleText = internNode.sensedResource + " stored";
                        break;
                    case "InnerInputNode":
                        InnerInputNode innerNode = (InnerInputNode)layers[i][j];
                        titleText = innerNode.linkedNetName;
                        break;
                    case "PhenotypeInputNode":
                        PhenotypeInputNode phenoNode = (PhenotypeInputNode)layers[i][j];
                        titleText = "Pheno: " + phenoNode.neighborIndex;
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

    public void createNodePanel(NonInputNode node)
    {
        nodePanel.SetActive(true);
        NodePanelPopulator panelPop = nodePanel.GetComponent<NodePanelPopulator>();
        panelPop.setNode(node);
    }


    private void OnDisable()
    {
        for (int i = 0; i < instantiated.Count; i++)
        {
            GameObject.Destroy(instantiated[i]);
        }
    }
}
