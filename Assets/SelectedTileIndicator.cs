using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedTileIndicator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Base_Weapon weapon))
        {
            weapon.WeaponHit(transform.position);
            gameObject.SetActive(false);
        }
    }
}
