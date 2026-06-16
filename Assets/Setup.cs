using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

/// <summary>
/// 一键设置脚本 - 创建整个游戏场景 - BUG修复版
/// 修复：使用 transform 而不是 GetComponent<Transform>()
/// </summary>
public class Setup : MonoBehaviour
{
#if UNITY_EDITOR
    [UnityEditor.MenuItem("Game/Setup Scene")]
    public static void SetupScene()
    {
        Debug.Log("开始设置游戏场景...");
        
        CreateMainCamera();
        CreatePlayer();
        CreateTerrain();
        SetupLighting();
        
        Debug.Log("场景设置完成!");
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
    
    static void CreateMainCamera()
    {
        if (Camera.main != null) return;
        
        GameObject cameraObj = new GameObject("Main Camera");
        Camera camera = cameraObj.AddComponent<Camera>();
        camera.tag = "MainCamera";
        cameraObj.AddComponent<CameraController>();
        
        Debug.Log("主摄像机创建完成");
    }
    
    static void CreatePlayer()
    {
        if (GameObject.Find("Player") != null) return;
        
        GameObject player = new GameObject("Player");
        
        // 添加子对象作为可视化
        GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        visual.name = "Visual";
        visual.transform.parent = player.transform;
        visual.transform.localPosition = Vector3.zero;
        Object.DestroyImmediate(visual.GetComponent<Collider>());
        
        // 设置颜色
        Material playerMat = CreateColoredMaterial(new Color(0.2f, 0.5f, 0.9f));
        visual.GetComponent<MeshRenderer>().material = playerMat;
        
        // 添加组件
        CharacterController controller = player.AddComponent<CharacterController>();
        controller.height = 2f;
        controller.radius = 0.5f;
        
        player.AddComponent<PlayerController>();
        player.transform.position = new Vector3(50, 1, 50);
        
        Debug.Log("玩家创建完成");
    }
    
    static void CreateTerrain()
    {
        if (GameObject.Find("MapGenerator") != null) return;
        
        GameObject mapGen = new GameObject("MapGenerator");
        TerrainGenerator generator = mapGen.AddComponent<TerrainGenerator>();
        generator.GenerateMap();
        
        Debug.Log("地形生成完成");
    }
    
    static void SetupLighting()
    {
        Light[] lights = Object.FindObjectsOfType<Light>();
        foreach (Light light in lights)
        {
            if (light.name == "Directional Light")
            {
                ConfigureDirectionalLight(light);
                return;
            }
        }
        
        // 创建新的光源
        GameObject lightObj = new GameObject("Directional Light");
        Light light2 = lightObj.AddComponent<Light>();
        light2.type = LightType.Directional;
        ConfigureDirectionalLight(light2);
        
        Debug.Log("光照设置完成");
    }
    
    static void ConfigureDirectionalLight(Light light)
    {
        light.intensity = 1.2f;
        light.color = new Color(1f, 0.95f, 0.8f);
        // 修复：直接使用 transform 而不是 GetComponent<Transform>()
        light.transform.rotation = Quaternion.Euler(50, -30, 0);
    }
    
    static Material CreateColoredMaterial(Color color)
    {
        Material mat = new Material(Shader.Find("Standard"));
        mat.color = color;
        return mat;
    }
#endif
}
