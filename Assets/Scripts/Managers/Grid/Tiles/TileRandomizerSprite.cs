using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileRandomizerSprite : MonoBehaviour
{
    [SerializeField] List<Sprite> TopSprites;
    [SerializeField] List<Sprite> BaseSprites;

    [SerializeField] Transform TileTop;
    [SerializeField] Transform TileBase;

    // Start is called before the first frame update
    void Start()
    {
        TileTop = gameObject.transform.Find("TileTop");
        TileBase = transform.Find("TileBase");

        RandomizeSprite();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RandomizeSprite()
    {
        SpriteRenderer TopSpriteRenderer = TileTop.GetComponent<SpriteRenderer>();
        SpriteRenderer BottomSpriteRenderer = TileBase.GetComponent<SpriteRenderer>();

        TopSpriteRenderer.sprite = TopSprites[Random.Range(0, TopSprites.Count)];
        BottomSpriteRenderer.sprite = BaseSprites[Random.Range(0, BaseSprites.Count)];
    }
}
