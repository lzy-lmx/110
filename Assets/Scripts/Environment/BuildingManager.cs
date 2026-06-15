using UnityEngine;

/// <summary>
/// 建筑管理脚本
/// </summary>
public class BuildingManager : MonoBehaviour
{
    [SerializeField] private Material woodMaterial;
    [SerializeField] private Material stoneMaterial;
    
    void Start()
    {
        // 初始化建筑
        InitializeBuildings();
    }
    
    void InitializeBuildings()
    {
        // 为所有建筑分配材质
        foreach (Transform child in transform)
        {
            if (child.name.Contains("House"))
            {
                ApplyBuildingMaterial(child.gameObject, woodMaterial);
            }
            else if (child.name.Contains("Cave"))
            {
                ApplyBuildingMaterial(child.gameObject, stoneMaterial);
            }
        }
    }
    
    void ApplyBuildingMaterial(GameObject building, Material material)
    {
        MeshRenderer renderer = building.GetComponent<MeshRenderer>();
        if (renderer != null && material != null)
        {
            renderer.material = material;
        }
    }
}
