using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 地形生成脚本 - 优化版
/// 生成平坦地形、小路、水流、洞穴和建筑
/// </summary>
public class TerrainGenerator : MonoBehaviour
{
    [SerializeField] private TerrainConfig config;
    [SerializeField] private bool generateOnStart = true;
    
    private GameObject terrainObject;
    private Dictionary<string, GameObject> decorationParents = new Dictionary<string, GameObject>();
    
    void Start()
    {
        if (generateOnStart)
        {
            GenerateMap();
        }
    }
    
    public void GenerateMap()
    {
        ClearMap();
        CreateBaseTerrain();
        CreatePaths();
        CreateWater();
        SpawnDecorations();
        Debug.Log("地图生成完成!");
    }
    
    void CreateBaseTerrain()
    {
        terrainObject = new GameObject("Terrain");
        terrainObject.transform.parent = transform;
        
        Mesh terrainMesh = new Mesh();
        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        var colors = new List<Color>();
        
        // 生成顶点
        for (int z = 0; z <= config.mapLength; z++)
        {
            for (int x = 0; x <= config.mapWidth; x++)
            {
                float posX = x * config.terrainScale;
                float posZ = z * config.terrainScale;
                float posY = Mathf.PerlinNoise(x * 0.05f, z * 0.05f) * 0.5f;
                
                vertices.Add(new Vector3(posX, posY, posZ));
                colors.Add(config.grassColor);
            }
        }
        
        // 生成三角形
        int mapWidthPlus1 = config.mapWidth + 1;
        for (int z = 0; z < config.mapLength; z++)
        {
            for (int x = 0; x < config.mapWidth; x++)
            {
                int vertexIndex = z * mapWidthPlus1 + x;
                
                // 第一个三角形
                triangles.Add(vertexIndex);
                triangles.Add(vertexIndex + mapWidthPlus1);
                triangles.Add(vertexIndex + 1);
                
                // 第二个三角形
                triangles.Add(vertexIndex + 1);
                triangles.Add(vertexIndex + mapWidthPlus1);
                triangles.Add(vertexIndex + mapWidthPlus1 + 1);
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
        renderer.material = config.terrainMaterial ?? new Material(Shader.Find("Standard"));
        
        Debug.Log("基础地形创建完成");
    }
    
    void CreatePaths()
    {
        GameObject pathsObject = new GameObject("Paths");
        pathsObject.transform.parent = terrainObject.transform;
        
        float mapCenterX = config.mapWidth * 0.5f * config.terrainScale;
        float mapCenterZ = config.mapLength * 0.5f * config.terrainScale;
        float mapEndX = config.mapWidth * config.terrainScale;
        float mapEndZ = config.mapLength * config.terrainScale;
        
        // 水平路径
        CreatePathLine(pathsObject, new Vector3(0, 0.1f, mapCenterZ), 
                      new Vector3(mapEndX, 0.1f, mapCenterZ), config.pathWidth);
        
        // 垂直路径
        CreatePathLine(pathsObject, new Vector3(mapCenterX, 0.1f, 0), 
                      new Vector3(mapCenterX, 0.1f, mapEndZ), config.pathWidth);
        
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
        
        Vector3 direction = (end - start).normalized;
        pathSegment.transform.rotation = Quaternion.LookRotation(direction);
        
        Material pathMat = new Material(Shader.Find("Standard"));
        pathMat.color = config.pathColor;
        pathSegment.GetComponent<MeshRenderer>().material = pathMat;
        
        DestroyImmediate(pathSegment.GetComponent<Collider>());
    }
    
    void CreateWater()
    {
        GameObject waterObject = new GameObject("Water");
        waterObject.transform.parent = transform;
        waterObject.transform.position = new Vector3(
            config.mapWidth * config.terrainScale * 0.5f, 
            config.waterLevel, 
            config.mapLength * config.terrainScale * 0.3f);
        
        GameObject waterPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        waterPlane.name = "WaterPlane";
        waterPlane.transform.parent = waterObject.transform;
        waterPlane.transform.localScale = new Vector3(8, 1, 5);
        waterPlane.transform.localPosition = Vector3.zero;
        
        Material waterMat = new Material(Shader.Find("Standard"));
        waterMat.color = config.waterColor;
        waterMat.SetFloat("_Glossiness", 0.8f);
        waterPlane.GetComponent<MeshRenderer>().material = waterMat;
        
        DestroyImmediate(waterPlane.GetComponent<Collider>());
        Debug.Log("水体创建完成");
    }
    
    void SpawnDecorations()
    {
        SpawnDecorationType(config.treePrefab, "Trees", config.treeCount);
        SpawnDecorationType(config.rockPrefab, "Rocks", config.rockCount);
        SpawnDecorationType(config.housePrefab, "Houses", config.houseCount);
        SpawnDecorationType(config.cavePrefab, "Caves", config.caveCount);
        Debug.Log("装饰物生成完成");
    }
    
    void SpawnDecorationType(GameObject prefab, string parentName, int count)
    {
        if (prefab == null) return;
        
        GameObject parent = GetOrCreateParent(parentName);
        
        for (int i = 0; i < count; i++)
        {
            SpawnRandomObject(prefab, parent);
        }
    }
    
    GameObject GetOrCreateParent(string parentName)
    {
        if (decorationParents.TryGetValue(parentName, out GameObject parent))
        {
            return parent;
        }
        
        parent = new GameObject(parentName);
        parent.transform.parent = transform;
        decorationParents[parentName] = parent;
        return parent;
    }
    
    void SpawnRandomObject(GameObject prefab, GameObject parent)
    {
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
        decorationParents.Clear();
    }
}
