using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldUI : MonoBehaviour
{
    [Header("Tilemap")]
    public Tilemap tilemap;

    [Header("ui")]
    public SpriteRenderer spriteRenderer;
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = tilemap.WorldToCell(mousePosition);
            spriteRenderer.transform.position = tilemap.GetCellCenterWorld(cellPosition) - new Vector3(0.5f, 0.5f, 0);
            Debug.Log("鼠标点击的单元格位置：" + cellPosition);
        }
    }
}
