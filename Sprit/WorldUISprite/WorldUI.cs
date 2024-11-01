using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldUI : MonoBehaviour
{
    [Header("Tilemap")]
    public Tilemap tilemap;

    [Header("UI")]
    public SpriteRenderer spriteRenderer;

    private Camera mainCamera;

    void Start()
    {
        // 获取 Tilemap 组件
        // Get the Tilemap component
        tilemap = GetComponent<Tilemap>();

        // 获取主摄像机并缓存
        // Get the main camera and cache it
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 获取鼠标在世界坐标中的位置
            // Get the mouse position in world coordinates
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // 将世界坐标转换为单元格坐标
            // Convert world coordinates to cell coordinates
            Vector3Int cellPosition = tilemap.WorldToCell(mousePosition);

            // 更新 UI 元素的位置
            // Update the position of the UI element
            spriteRenderer.transform.position = tilemap.GetCellCenterWorld(cellPosition) - new Vector3(0.5f, 0.5f, 0);
        }
    }
}