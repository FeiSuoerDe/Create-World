using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


public class WorldTileMap : MonoBehaviour
{
    // 地图的Tilemap组件
    [Header("Tilemap")]
    public Tilemap tilemap;

    // 地图的最大尺寸
    public Vector2Int maxMapSize;
    [Header("seed")]
    // 随机种子
    public int seed;

    // 是否使用自定义种子
    public Boolean RandomSeed;

    [Header("Tiles")]
    // 深海瓷砖
    public TileBase DeepTile;

    // 海瓷砖
    public TileBase WaterTile;

    // 浅滩瓷砖
    public TileBase BeachTile;
    // 沙地瓷砖
    public TileBase SandTile;
    // 泥土瓷砖
    public TileBase GroundTile;
    // 高山
    public TileBase MountainPeakTile;
    // 雪
    public TileBase SnowTile;
    // 河
    public TileBase RiverTile;

    [Header("tileVale")]
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
    [Range(0.1f, 0f)]
    public float offset;

    [Header("River")]
    public int RiverNumber;
    public int RiverMaxLong;
    [Header("Noise")]

    [Range(1, 8)]
    public int octaves = 1; // 噪声分形程度滑条
    [Range(1f, 4f)]
    public float frequencyMultiplier = 2f; // 频率倍增因子
    [Range(0.1f, 1f)]
    public float amplitudeReducer = 0.5f; // 振幅衰减因子

    private float[,] mapData;
    WorldTileMapData[,] worldTileMapDataArray;


    // 生成地图的方法
    public void MakeMap()
    {
        mapData = new float[maxMapSize.x, maxMapSize.y];
        worldTileMapDataArray = new WorldTileMapData[maxMapSize.x, maxMapSize.y];
        GenerateMap();
        GenerateRiver();

    }

    // 生成地图的具体实现
    public void GenerateMap()
    {
        ClearMap();
        // 如果没有指定种子，则使用当前时间作为种子
        if (RandomSeed)
        {
            seed = Time.time.GetHashCode();
        }
        UnityEngine.Random.InitState(seed);
        float randomoffset = UnityEngine.Random.Range(-1000, 1000);
        for (int X = 0; X < maxMapSize.x; X++)
        {
            for (int Y = 0; Y < maxMapSize.y; Y++)
            {
                float a = 0f;
                float amplitude = 1f;
                float frequency = 1f;
                for (int i = 0; i < octaves; i++)
                {
                    a += Mathf.PerlinNoise((X * offset + randomoffset) * frequency, (Y * offset + randomoffset) * frequency) * amplitude;
                    frequency *= frequencyMultiplier;
                    amplitude *= amplitudeReducer;
                }
                a /= 2f * (1f - Mathf.Pow(amplitudeReducer, octaves)); // 归一化
                mapData[X, Y] = a;

                int elevation;
                int climateType = -1;
                if (a <= beachLevel)
                {
                    elevation = 0;
                }
                else
                {
                    elevation = (int)((a - beachLevel) * 100);
                }
                string name = "12";



                if (a <= deepLevel)
                {
                    tilemap.SetTile(new Vector3Int(X, Y, 0), DeepTile);
                    climateType = 0;
                }
                else if (a > deepLevel && a <= waterLevel)
                {

                    tilemap.SetTile(new Vector3Int(X, Y, 0), WaterTile);
                    climateType = 1;
                }
                else if (a > waterLevel && a <= beachLevel)
                {
                    tilemap.SetTile(new Vector3Int(X, Y, 0), BeachTile);
                    climateType = 2;
                }
                else if (a > beachLevel && a <= sandLevel)
                {
                    tilemap.SetTile(new Vector3Int(X, Y, 0), SandTile);
                    climateType = 3;
                }
                else if (a > sandLevel && a <= groundLevel)
                {
                    tilemap.SetTile(new Vector3Int(X, Y, 0), GroundTile);
                    climateType = 4;
                }
                else if (a > groundLevel && a <= mountainLevel)
                {
                    tilemap.SetTile(new Vector3Int(X, Y, 0), MountainPeakTile);
                    climateType = 5;
                }
                else if (a > mountainLevel)
                {
                    tilemap.SetTile(new Vector3Int(X, Y, 0), SnowTile);
                    climateType = 6;
                }


                WorldTileMapData worldTileMapData = new WorldTileMapData(name, new Vector2Int(X, Y), elevation, climateType);
                worldTileMapDataArray[X, Y] = worldTileMapData;


            }
        }
    }


    // 生成河流
    private void GenerateRiver()
    {
        UnityEngine.Random.InitState(seed / 2);
        float randomoffset = UnityEngine.Random.Range(-1000, 1000);
        for (int X = 0; X < maxMapSize.x; X++)
        {
            for (int Y = 0; Y < maxMapSize.y; Y++)
            {
                float a = 0f;
                float amplitude = 1f;
                float frequency = 1f;
                for (int i = 0; i < octaves; i++)
                {
                    a += Mathf.PerlinNoise((X * offset / 2 + randomoffset) * frequency, (Y * offset / 2 + randomoffset) * frequency) * amplitude;
                    frequency *= frequencyMultiplier;
                    amplitude *= amplitudeReducer;
                }
                a /= 2f * (1f - Mathf.Pow(amplitudeReducer, octaves)); // 归一化
                if (a > 0.5f && a < 0.51f && mapData[X, Y] > beachLevel)
                {
                    tilemap.SetTile(new Vector3Int(X, Y, 0), BeachTile);
                    int elevation = (int)((a - beachLevel) * 100);
                    WorldTileMapData worldTileMapData = new WorldTileMapData(name, new Vector2Int(X, Y), elevation, 7);
                    worldTileMapDataArray[X, Y] = worldTileMapData;
                }
            }
        }
    }



    // 清除地图上的所有瓷砖
    public void ClearMap()
    {
        tilemap.ClearAllTiles();


    }
}
