using UnityEngine;

/// <summary>
/// 环境管理基类 - 抽象出重复代码
/// </summary>
public abstract class EnvironmentManager : MonoBehaviour
{
    protected Material GetOrCreateMaterial(Material originalMaterial)
    {
        if (originalMaterial != null) return originalMaterial;
        return new Material(Shader.Find("Standard"));
    }
    
    protected void ApplyMaterialToObject(GameObject obj, Material material)
    {
        if (material == null) return;
        
        MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }
    }
}
