using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 地形生成脚本
/// 生成平坦地形、小路、水流、洞穴和建筑
/// </summary>
public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private TerrainConfig config;
    [SerializeField] private bool generateOnStart = true;
    
    private GameObject terrainObject;
    private List<GameObject> decorationObjects = new List<GameObject>();
    
    void Start()
    {
        if (generateOnStart)
        {
            GenerateMap();
        }
    }
    
    public void GenerateMap()
    {
        // 清除旧地形
        ClearMap();
        
        // 创建地形
        CreateBaseTerrain();
        
        // 添加路径
        CreatePaths();
        
        // 添加水流
        CreateWater();
        
        // 生成装饰物
        SpawnDecorations();
        
        Debug.Log("地图生成完成!");
    }
    
    void CreateBaseTerrain()
    {
        terrainObject = new GameObject("Terrain");
        terrainObject.transform.parent = transform;
        
        // 创建地形网格
        Mesh terrainMesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Color> colors = new List<Color>();
        
        // 生成顶点
        for (int z = 0; z <= config.mapLength; z++)
        {
            for (int x = 0; x <= config.mapWidth; x++)
            {
                float posX = x * config.terrainScale;
                float posZ = z * config.terrainScale;
                float posY = Mathf.PerlinNoise(x * 0.05f, z * 0.05f) * 0.5f; // 轻微起伏
                
                vertices.Add(new Vector3(posX, posY, posZ));
                colors.Add(config.grassColor);
            }
        }
        
        // 生成三角形
        for (int z = 0; z < config.mapLength; z++)
        {
            for (int x = 0; x < config.mapWidth; x++)
            {
                int vertexIndex = z * (config.mapWidth + 1) + x;
                
                // 第一个三角形
                triangles.Add(vertexIndex);
                triangles.Add(vertexIndex + config.mapWidth + 1);
                triangles.Add(vertexIndex + 1);
                
                // 第二个三角形
                triangles.Add(vertexIndex + 1);
                triangles.Add(vertexIndex + config.mapWidth + 1);
                triangles.Add(vertexIndex + config.mapWidth + 2);
            }
        }
        
        terrainMesh.vertices = vertices.ToArray();
        terrainMesh.triangles = triangles.ToArray();
        terrainMesh.colors = colors.ToArray();
        terrainMesh.RecalculateNormals();
        
        // 添加碰撞体和渲染器
        MeshCollider collider = terrainObject.AddComponent<MeshCollider>();
        collider.convex = false;
        
        MeshFilter meshFilter = terrainObject.AddComponent<MeshFilter>();
        meshFilter.mesh = terrainMesh;
        
        MeshRenderer renderer = terrainObject.AddComponent<MeshRenderer>();
        renderer.material = config.terrainMaterial != null ? config.terrainMaterial : new Material(Shader.Find("Standard"));
        
        Debug.Log("基础地形创建完成");
    }
    
    void CreatePaths()
    {
        GameObject pathsObject = new GameObject("Paths");
        pathsObject.transform.parent = terrainObject.transform;
        
        // 创建十字形路径
        CreatePathLine(pathsObject, new Vector3(0, 0.1f, config.mapLength * 0.5f * config.terrainScale), 
                      new Vector3(config.mapWidth * config.terrainScale, 0.1f, config.mapLength * 0.5f * config.terrainScale), 
                      config.pathWidth);
        
        CreatePathLine(pathsObject, new Vector3(config.mapWidth * 0.5f * config.terrainScale, 0.1f, 0), 
                      new Vector3(config.mapWidth * 0.5f * config.terrainScale, 0.1f, config.mapLength * config.terrainScale), 
                      config.pathWidth);
        
        Debug.Log("路径创建完成");
    }
    
    void CreatePathLine(GameObject parent, Vector3 start, Vector3 end, float width)
    {
        float length = Vector3.Distance(start, end);
        GameObject pathSegment = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pathSegment.name = "PathSegment";
        pathSegment.transform.parent = parent.transform;
        pathSegment.transform.position = (start + end) * 0.5f;
        pathSegment.transform.localScale = new Vector3(width, 0.05f, length);
        
        // 旋转到正确方向
        Vector3 direction = (end - start).normalized;
        pathSegment.transform.rotation = Quaternion.LookRotation(direction);
        
        // 设置路径颜色
        Material pathMat = new Material(Shader.Find("Standard"));
        pathMat.color = config.pathColor;
        pathSegment.GetComponent<MeshRenderer>().material = pathMat;
        
        // 移除碰撞体脚本
        DestroyImmediate(pathSegment.GetComponent<Collider>());
    }
    
    void CreateWater()
    {
        GameObject waterObject = new GameObject("Water");
        waterObject.transform.parent = transform;
        waterObject.transform.position = new Vector3(config.mapWidth * config.terrainScale * 0.5f, config.waterLevel, config.mapLength * config.terrainScale * 0.3f);
        
        // 创建水面
        GameObject waterPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        waterPlane.name = "WaterPlane";
        waterPlane.transform.parent = waterObject.transform;
        waterPlane.transform.localScale = new Vector3(8, 1, 5);
        waterPlane.transform.localPosition = Vector3.zero;
        
        Material waterMat = new Material(Shader.Find("Standard"));
        waterMat.color = config.waterColor;
        waterMat.SetFloat("_Glossiness", 0.8f);
        waterPlane.GetComponent<MeshRenderer>().material = waterMat;
        
        // 移除碰撞体
        DestroyImmediate(waterPlane.GetComponent<Collider>());
        
        Debug.Log("水体创建完成");
    }
    
    void SpawnDecorations()
    {
        // 生成树木
        for (int i = 0; i < config.treeCount; i++)
        {
            SpawnRandomObject(config.treePrefab, "Trees");
        }
        
        // 生成岩石
        for (int i = 0; i < config.rockCount; i++)
        {
            SpawnRandomObject(config.rockPrefab, "Rocks");
        }
        
        // 生成房屋
        for (int i = 0; i < config.houseCount; i++)
        {
            SpawnRandomObject(config.housePrefab, "Houses");
        }
        
        // 生成洞穴
        for (int i = 0; i < config.caveCount; i++)
        {
            SpawnRandomObject(config.cavePrefab, "Caves");
        }
        
        Debug.Log("装饰物生成完成");
    }
    
    void SpawnRandomObject(GameObject prefab, string parentName)
    {
        if (prefab == null) return;
        
        GameObject parent = GameObject.Find(parentName);
        if (parent == null)
        {
            parent = new GameObject(parentName);
            parent.transform.parent = transform;
        }
        
        Vector3 randomPos = new Vector3(
            Random.Range(5, config.mapWidth * config.terrainScale - 5),
            1f,
            Random.Range(5, config.mapLength * config.terrainScale - 5)
        );
        
        GameObject obj = Instantiate(prefab, randomPos, Quaternion.identity, parent.transform);
        obj.name = prefab.name;
    }
    
    void ClearMap()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        decorationObjects.Clear();
    }
}
