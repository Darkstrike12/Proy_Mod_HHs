using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition2D : MonoBehaviour
{
    [SerializeField] Camera SceneCamera;
    [SerializeField] GameObject MousePointerObject;
    [SerializeField] Grid grid;
    [SerializeField] SelectedTileIndicator TileIndicatorPrefab;

    GameObject MousePointer;
    SelectedTileIndicator TileIndicator;

    private void Awake()
    {
        //MousePointer = Instantiate(MousePointerObject, new Vector3(0f,0f,0f), Quaternion.identity);
        //MousePointer.transform.parent = transform;

        MousePointer = Instantiate(MousePointerObject, transform);
        TileIndicator = Instantiate(TileIndicatorPrefab, transform);
        TileIndicator.gameObject.SetActive(false);
    }

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

    public void SetSelectedTile()
    {
        TileIndicator.gameObject.SetActive(true);
        TileIndicator.transform.position = MousePointer.transform.position;
    }

    public Vector3 GetMousePointerPosition()
    {
        return MousePointer.transform.position;
    }
}
