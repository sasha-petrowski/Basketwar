using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Layers
{
    public readonly static int Default = 1 << 0;
    public readonly static int Everything = ~0;

    public readonly static int TransparentFX = 1 << 1;
    public readonly static int IgnoreRaycast = 1 << 2;
    public readonly static int Player = 1 << 3;
    public readonly static int Water = 1 << 4;
    public readonly static int UI = 1 << 5;
    public readonly static int Terrain = 1 << 6;
    public readonly static int TerrainOnly = 1 << 7;
}

public static class LayersUtility
{

}
