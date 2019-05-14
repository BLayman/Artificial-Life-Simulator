// Eco-Simulator
// Copyright (c) 2019 Brett Layman
// This file is subject to the terms and conditions defined in 'LICENSE.txt', which is part of this source code repository.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureMakerExample : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        Texture2D texture = new Texture2D(10, 10, TextureFormat.ARGB32, false);
        Color[] colors = new Color[10 * 10];

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {

                colors[i * 10 + j] = new Color(i/10f,j/10f,1f);
            }
        }

        texture.filterMode = FilterMode.Point;
        texture.SetPixels(colors);

        // Apply all SetPixel calls
        texture.Apply();

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 1, 0, SpriteMeshType.FullRect);
        gameObject.gameObject.GetComponent<SpriteRenderer>().sharedMaterials[0].mainTexture = texture;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
