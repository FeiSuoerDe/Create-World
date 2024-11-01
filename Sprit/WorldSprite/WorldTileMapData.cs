using System;
using UnityEngine;
public class WorldTileMapData

{
    // namme
    public string name { get; set; }

    // pos
    public Vector2Int pos { get; set; }
    //海拔
    public int elevation { get; set; }
    // 气候类型
    public int climateType { get; set; }

    public WorldTileMapData(string name, Vector2Int pos, int elevation, int climateType)
    {
        this.name = name;
        this.pos = pos;
        this.elevation = elevation;
        this.climateType = climateType;
    }
    public override string ToString()
    {
        return $"{name} {pos} {elevation} {climateType}";
    }

}