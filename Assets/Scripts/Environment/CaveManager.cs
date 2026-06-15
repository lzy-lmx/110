using UnityEngine;

/// <summary>
/// 洞穴管理脚本
/// </summary>
public class CaveManager : MonoBehaviour
{
    [SerializeField] private Material caveMaterial;
    [SerializeField] private bool enableLighting = true;
    
    void Start()
    {
        InitializeCaves();
    }
    
    void InitializeCaves()
    {
        // 为洞穴添加灯光
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
        // 添加光源到洞穴
        Light light = cave.AddComponent<Light>();
        light.type = LightType.Point;
        light.range = 10f;
        light.intensity = 0.8f;
        light.color = new Color(1f, 0.9f, 0.7f); // 温暖的光
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
