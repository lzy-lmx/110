using UnityEngine;

/// <summary>
/// 洞穴管理脚本 - 优化版
/// </summary>
public class CaveManager : EnvironmentManager
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
                
                ApplyMaterialToObject(child.gameObject, GetOrCreateMaterial(caveMaterial));
            }
        }
    }
    
    void AddCaveLighting(GameObject cave)
    {
        Light light = cave.AddComponent<Light>();
        light.type = LightType.Point;
        light.range = lightRange;
        light.intensity = lightIntensity;
        light.color = lightColor;
    }
}
