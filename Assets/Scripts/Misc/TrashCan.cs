using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    public GameObject trashcanContent;
    [SerializeField] Sprite[] contentSprites;
    [field: SerializeField] public FmodEvent sound {  get; private set; }

    public Animator animator {  get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void UpdateVisual(GameManager.LevelState levelState)
    {
        switch (levelState)
        {
            case GameManager.LevelState.Soft:
                trashcanContent.GetComponent<SpriteRenderer>().sprite = contentSprites[0];
                break;
            case GameManager.LevelState.Medium:
                trashcanContent.GetComponent<SpriteRenderer>().sprite = contentSprites[1];
                break;
            case GameManager.LevelState.Hard:
                trashcanContent.GetComponent<SpriteRenderer>().sprite = contentSprites[2];
                break;
            case GameManager.LevelState.Finish:
                trashcanContent.GetComponent<SpriteRenderer>().sprite = contentSprites[3];
                break;
            default:
                trashcanContent.GetComponent<SpriteRenderer>().sprite = contentSprites[0];
                break;
        }
    }

    public void UpdateVisual(int spriteIndex)
    {
        if (spriteIndex <= contentSprites.Length && spriteIndex != 0)
        {
            trashcanContent.GetComponent<SpriteRenderer>().sprite = contentSprites[spriteIndex];
        }
    }

    public void PlaySound()
    {
        if(sound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySound(sound);
        }
    }
}
