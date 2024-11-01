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
            spriteRenderer.transform.position = tilemap.GetCellCenterWorld(cellPosition);


            WorldTileMapData[,] SelectedWorldTile = new WorldTileMapData[3, 3];


            Vector2 SelPos = new Vector2(cellPosition.x - 1, cellPosition.y - 1);
            for (int x = 0; x < SelectedWorldTile.GetLength(0); x++)
            {
                for (int y = 0; y < SelectedWorldTile.GetLength(1); y++)
                {
                    WorldTileMap worldTileMap = GetComponent<WorldTileMap>();
                    SelectedWorldTile[x, y] = worldTileMap.worldTileMapDataArray[(int)SelPos.x + x, (int)SelPos.y + y];

                }
            }
            for (int x = 0; x < SelectedWorldTile.GetLength(0); x++)
            {
                for (int y = 0; y < SelectedWorldTile.GetLength(1); y++)
                {
                    Debug.Log(SelectedWorldTile[x, y].ToString());
                }
            }


        }
    }
}