using UnityEngine;
using UnityEditor;

/// <summary>
/// 一键设置脚本 - 创建整个游戏场景
/// </summary>
public class Setup : MonoBehaviour
{
    [MenuItem("Game/Setup Scene")]
    public static void SetupScene()
    {
        Debug.Log("开始设置游戏场景...");
        
        // 创建主摄像机
        CreateMainCamera();
        
        // 创建玩家
        CreatePlayer();
        
        // 创建地形
        CreateTerrain();
        
        // 设置光照
        SetupLighting();
        
        Debug.Log("场景设置完成!");
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
        Destroy(visual.GetComponent<Collider>());
        
        // 设置颜色
        Material playerMat = new Material(Shader.Find("Standard"));
        playerMat.color = new Color(0.2f, 0.5f, 0.9f);
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
        
        // 自动生成地图
        generator.GenerateMap();
        
        Debug.Log("地形生成完成");
    }
    
    static void SetupLighting()
    {
        // 清除默认光
        Light[] lights = Object.FindObjectsOfType<Light>();
        foreach (Light light in lights)
        {
            if (light.name == "Directional Light")
            {
                light.intensity = 1.2f;
                light.color = new Color(1f, 0.95f, 0.8f);
                light.transform.rotation = Quaternion.Euler(50, -30, 0);
                return;
            }
        }
        
        // 如果没有找到，创建新的光源
        GameObject lightObj = new GameObject("Directional Light");
        Light light2 = lightObj.AddComponent<Light>();
        light2.type = LightType.Directional;
        light2.intensity = 1.2f;
        light2.color = new Color(1f, 0.95f, 0.8f);
        lightObj.transform.rotation = Quaternion.Euler(50, -30, 0);
        
        Debug.Log("光照设置完成");
    }
}
