using UnityEngine;

/// <summary>
/// 地形配置类
/// </summary>
[System.Serializable]
public class TerrainConfig
{
    [Header("地形基础设置")]
    public int mapWidth = 100;
    public int mapLength = 100;
    public float terrainScale = 1f;
    public Material terrainMaterial;
    
    [Header("地形细节")]
    public float grassHeight = 0.5f;
    public float pathWidth = 2f;
    public float waterLevel = 0.2f;
    
    [Header("装饰物")]
    public GameObject treePrefab;
    public GameObject rockPrefab;
    public GameObject housePrefab;
    public GameObject cavePrefab;
    
    [Header("生成参数")]
    public int treeCount = 20;
    public int rockCount = 15;
    public int houseCount = 5;
    public int caveCount = 3;
    
    [Header("色彩设置")]
    public Color grassColor = new Color(0.2f, 0.8f, 0.3f);
    public Color pathColor = new Color(0.7f, 0.6f, 0.4f);
    public Color waterColor = new Color(0.2f, 0.6f, 0.9f);
}
