using UnityEngine;

/// <summary>
/// 洞穴管理脚本 - BUG修复版
/// 修复：检查灯光是否已存在，防止重复添加
/// </summary>
public class CaveManager : MonoBehaviour
{
    [SerializeField] private Material caveMaterial;
    [SerializeField] private bool enableLighting = true;
    [SerializeField] private float lightRange = 10f;
    [SerializeField] private float lightIntensity = 0.8f;
    
    private readonly Color lightColor = new Color(1f, 0.9f, 0.7f);
    
    void Start()
    {
        InitializeCaves();
    }
    
    void InitializeCaves()
    {
        foreach (Transform child in transform)
        {
            if (child.name.Contains("Cave"))
            {
                if (enableLighting)
                {
                    AddCaveLighting(child.gameObject);
                }
                
                ApplyCaveMaterial(child.gameObject);
            }
        }
    }
    
    void AddCaveLighting(GameObject cave)
    {
        // 修复：检查是否已有 Light 组件，防止重复添加
        Light existingLight = cave.GetComponent<Light>();
        if (existingLight != null)
        {
            return;  // 已存在，不再添加
        }
        
        Light light = cave.AddComponent<Light>();
        light.type = LightType.Point;
        light.range = lightRange;
        light.intensity = lightIntensity;
        light.color = lightColor;
    }
    
    void ApplyCaveMaterial(GameObject cave)
    {
        if (caveMaterial == null) return;
        
        MeshRenderer renderer = cave.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material = caveMaterial;
        }
    }
}
