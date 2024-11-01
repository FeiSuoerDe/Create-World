using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldTileMap : MonoBehaviour
{
    [Header("Tilemap")]
    public Tilemap tilemap;

    [Header("Map Size")]
    public Vector2Int maxMapSize;

    [Header("Seed")]
    public int seed;
    public bool useCustomSeed;

    [Header("Tiles")]
    public TileBase DeepTile;
    public TileBase WaterTile;
    public TileBase BeachTile;
    public TileBase SandTile;
    public TileBase GroundTile;
    public TileBase MountainPeakTile;
    public TileBase SnowTile;
    public TileBase RiverTile;

    [Header("Elevation Levels")]
    [Range(0f, 1f)]
    public float deepLevel;
    [Range(0f, 1f)]
    public float waterLevel;
    [Range(0f, 1f)]
    public float beachLevel;
    [Range(0f, 1f)]
    public float sandLevel;
    [Range(0f, 1f)]
    public float groundLevel;
    [Range(0f, 1f)]
    public float mountainLevel;
    [Range(0f, 1f)]
    public float snowLevel;
    [Range(0.1f, 1f)]
    public float offset;

    [Header("Rivers")]
    public int riverNumber;
    public int riverMaxLength;

    [Header("Noise Settings")]
    [Range(1, 8)]
    public int octaves = 1;
    [Range(1f, 4f)]
    public float frequencyMultiplier = 2f;
    [Range(0.1f, 1f)]
    public float amplitudeReducer = 0.5f;

    private float[,] mapData;
    private WorldTileMapData[,] worldTileMapDataArray;

    public void MakeMap()
    {
        mapData = new float[maxMapSize.x, maxMapSize.y];
        worldTileMapDataArray = new WorldTileMapData[maxMapSize.x, maxMapSize.y];
        GenerateMap();
        GenerateRivers();
    }

    public void GenerateMap()
    {
        ClearMap();
        InitializeRandomSeed();

        float randomOffset = UnityEngine.Random.Range(-1000, 1000);
        for (int x = 0; x < maxMapSize.x; x++)
        {
            for (int y = 0; y < maxMapSize.y; y++)
            {
                float noiseValue = GenerateNoise(x, y, randomOffset);
                mapData[x, y] = noiseValue;

                SetTileAtPosition(x, y, noiseValue);
            }
        }
    }

    private void GenerateRivers()
    {
        UnityEngine.Random.InitState(seed / 2);
        float randomOffset = UnityEngine.Random.Range(-1000, 1000);

        for (int x = 0; x < maxMapSize.x; x++)
        {
            for (int y = 0; y < maxMapSize.y; y++)
            {
                float noiseValue = GenerateNoise(x, y, randomOffset, 0.5f);
                if (noiseValue > 0.5f && noiseValue < 0.51f && mapData[x, y] > beachLevel)
                {
                    SetTileAtPosition(x, y, noiseValue, RiverTile, 7);
                }
            }
        }
    }

    private float GenerateNoise(int x, int y, float randomOffset, float scale = 1f)
    {
        // 生成噪声值并进行归一化处理
        // Generate noise value and normalize it
        float a = 0f;
        float amplitude = 1f;
        float frequency = 1f;
        for (int i = 0; i < octaves; i++)
        {
            a += Mathf.PerlinNoise((x * offset * scale + randomOffset) * frequency, (y * offset * scale + randomOffset) * frequency) * amplitude;
            frequency *= frequencyMultiplier;
            amplitude *= amplitudeReducer;
        }
        return a / (2f * (1f - Mathf.Pow(amplitudeReducer, octaves)));
    }

    private void SetTileAtPosition(int x, int y, float noiseValue, TileBase tile = null, int climateType = -1)
    {
        // 设置指定位置的瓷砖并记录地形数据
        // Set the tile at the specified position and record the terrain data
        if (tile == null)
        {
            tile = GetTileByNoiseValue(noiseValue);
            climateType = GetClimateTypeByNoiseValue(noiseValue);
        }

        tilemap.SetTile(new Vector3Int(x, y, 0), tile);

        int elevation = (int)((noiseValue - beachLevel) * 100);
        string name = "12"; // 这里可以根据实际需求进行修改
        WorldTileMapData worldTileMapData = new WorldTileMapData(name, new Vector2Int(x, y), elevation, climateType);
        worldTileMapDataArray[x, y] = worldTileMapData;
    }

    private TileBase GetTileByNoiseValue(float noiseValue)
    {
        // 根据噪声值获取对应的瓷砖
        // Get the corresponding tile based on the noise value
        if (noiseValue <= deepLevel) return DeepTile;
        if (noiseValue <= waterLevel) return WaterTile;
        if (noiseValue <= beachLevel) return BeachTile;
        if (noiseValue <= sandLevel) return SandTile;
        if (noiseValue <= groundLevel) return GroundTile;
        if (noiseValue <= mountainLevel) return MountainPeakTile;
        return SnowTile;
    }

    private int GetClimateTypeByNoiseValue(float noiseValue)
    {
        // 根据噪声值获取对应的气候类型
        // Get the corresponding climate type based on the noise value
        if (noiseValue <= deepLevel) return 0; // 深海 Deep Sea
        if (noiseValue <= waterLevel && noiseValue > deepLevel) return 1; // 海水 Water
        if (noiseValue <= beachLevel && noiseValue > waterLevel) return 2; // 浅滩 Beach
        if (noiseValue <= sandLevel && noiseValue > beachLevel) return 3; // 沙地 Sand
        if (noiseValue <= groundLevel && noiseValue > sandLevel) return 4; // 泥土 Ground
        if (noiseValue <= mountainLevel) return 5; // 高山 Mountain Peak
        return 6; // 雪 Snow
    }

    private void ClearMap()
    {
        // 清除地图上的所有瓷砖
        // Clear all tiles on the map
        tilemap.ClearAllTiles();
    }

    private void InitializeRandomSeed()
    {
        // 初始化随机种子
        // Initialize the random seed
        if (useCustomSeed)
        {
            seed = Time.time.GetHashCode();
        }
        UnityEngine.Random.InitState(seed);
    }
}