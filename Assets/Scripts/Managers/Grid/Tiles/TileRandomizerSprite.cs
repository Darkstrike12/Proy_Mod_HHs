using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRandomizerSprite : MonoBehaviour
{
    [SerializeField] bool autoSelectTarget;
    [Header("SpriteList")]
    [SerializeField] List<Sprite> TopSprites;
    [SerializeField] List<Sprite> BaseSprites;

    [Header("Spite Target")]
    [SerializeField] Transform TileTop;
    [SerializeField] Transform TileBase;

    void Start()
    {
        if(autoSelectTarget)
        {
            TileTop = transform.Find("TileTop");
            TileBase = transform.Find("TileBase");
        }

        RandomizeSprite();
    }

    public void RandomizeSprite()
    {
        if(TileBase != null && BaseSprites.Count > 0)
        {
            SpriteRenderer BottomSpriteRenderer = TileBase.GetComponent<SpriteRenderer>();
            BottomSpriteRenderer.sprite = BaseSprites[Random.Range(0, BaseSprites.Count)];
        }
        if(TileTop != null && TopSprites.Count > 0)
        {
            SpriteRenderer TopSpriteRenderer = TileTop.GetComponent<SpriteRenderer>();
            TopSpriteRenderer.sprite = TopSprites[Random.Range(0, TopSprites.Count)];
        }
    }
}
