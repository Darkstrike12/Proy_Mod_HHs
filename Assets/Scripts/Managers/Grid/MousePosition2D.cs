using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition2D : MonoBehaviour
{
    [SerializeField] Camera SceneCamera;
    [SerializeField] GameObject MousePointer;
    [SerializeField] Grid grid;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPosition = SceneCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0f;
        Vector3Int GridPosition = grid.WorldToCell(mouseWorldPosition);
        //MousePointer.transform.position = mouseWorldPosition;
        MousePointer.transform.position = grid.CellToWorld(GridPosition) + (grid.cellSize/2);
    }
}
