using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldUI : MonoBehaviour
{
    [Header("Tilemap")]
    public Tilemap tilemap;

    [Header("UI")]
    public SpriteRenderer spriteRenderer;

    private Camera mainCamera;
    private WorldTileMap worldTileMap;

    void Start()
    {
        // 获取主摄像机并缓存
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
        }

        // 获取 WorldTileMap 组件
        worldTileMap = GetComponent<WorldTileMap>();
        if (worldTileMap == null)
        {
            Debug.LogError("WorldTileMap component not found!");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (mainCamera == null || tilemap == null || spriteRenderer == null || worldTileMap == null)
            {
                return;
            }

            // 获取鼠标在世界坐标中的位置
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            // 将世界坐标转换为单元格坐标
            Vector3Int cellPosition = tilemap.WorldToCell(mousePosition);

            // 更新 UI 元素的位置
            spriteRenderer.transform.position = tilemap.GetCellCenterWorld(cellPosition);

            WorldTileMapData[,] selectedWorldTile = new WorldTileMapData[3, 3];

            Vector2 selPos = new Vector2(cellPosition.x - 1, cellPosition.y - 1);
            for (int x = 0; x < selectedWorldTile.GetLength(0); x++)
            {
                for (int y = 0; y < selectedWorldTile.GetLength(1); y++)
                {
                    int arrayX = (int)selPos.x + x;
                    int arrayY = (int)selPos.y + y;

                    // 检查数组边界
                    if (arrayX >= 0 && arrayX < worldTileMap.worldTileMapDataArray.GetLength(0) &&
                        arrayY >= 0 && arrayY < worldTileMap.worldTileMapDataArray.GetLength(1))
                    {
                        selectedWorldTile[x, y] = worldTileMap.worldTileMapDataArray[arrayX, arrayY];
                    }
                    else
                    {
                        Debug.LogWarning($"Array index out of bounds: ({arrayX}, {arrayY})");
                    }
                }
            }
        }
    }
}