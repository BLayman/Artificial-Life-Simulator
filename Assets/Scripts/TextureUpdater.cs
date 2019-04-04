using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class TextureUpdater
{

    // TODO run on GPU
    public static void updateTexture(List<List<Land>> map, Color[] colors, string visibleResource)
    {
        Color resourceShade = new Color(1, 1, 1);
        Color creatureColor = Color.blue;
        //float st = System.DateTime.Now.Millisecond;
        for (int x = 0; x < map.Count; x++)
        {
            for (int y = 0; y < map[x].Count; y++)
            {
                if (map[x][y].creatureIsOn())
                {
                    colors[y * map.Count + x] = map[x][y].creatureOn.color;
                }
                else
                {
                    float proportionStored = map[x][y].propertyDict[visibleResource].getProportionStored();
                    resourceShade.r = proportionStored;
                    resourceShade.g = proportionStored;
                    resourceShade.b = proportionStored;


                    colors[y * map.Count + x] = resourceShade;
                }
            }
        }
    }

}