using UnityEngine;

/// <summary>
/// 建筑管理脚本 - 优化版
/// </summary>
public class BuildingManager : EnvironmentManager
{
    [SerializeField] private Material woodMaterial;
    [SerializeField] private Material stoneMaterial;
    
    void Start()
    {
        InitializeBuildings();
    }
    
    void InitializeBuildings()
    {
        foreach (Transform child in transform)
        {
            if (child.name.Contains("House"))
            {
                ApplyMaterialToObject(child.gameObject, GetOrCreateMaterial(woodMaterial));
            }
            else if (child.name.Contains("Cave"))
            {
                ApplyMaterialToObject(child.gameObject, GetOrCreateMaterial(stoneMaterial));
            }
        }
    }
}
