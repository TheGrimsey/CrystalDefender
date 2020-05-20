﻿using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HeightSpriteSorter : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }


    // Update is called once per frame
    void Update()
    {
        _spriteRenderer.sortingOrder = (int)(-transform.position.y * 10f);
    }
}
