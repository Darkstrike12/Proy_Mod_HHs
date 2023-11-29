using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] Vector2 movementSpeed;
    [SerializeField] SpriteRenderer spriteRenderer;

    Vector2 offset;
    Material backgroundMaterial;

    void Start()
    {
        if(spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        backgroundMaterial = spriteRenderer.material;
    }

    void Update()
    {
        offset = movementSpeed * Time.deltaTime;
        backgroundMaterial.mainTextureOffset += offset;
    }
}
