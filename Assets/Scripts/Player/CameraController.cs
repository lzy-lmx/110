using UnityEngine;

/// <summary>
/// 相机控制脚本 - 45度俯视角视角
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float distance = 10f;
    [SerializeField] private float height = 8f;
    [SerializeField] private float angle = 45f;
    [SerializeField] private float smoothSpeed = 5f;
    
    private Vector3 offset;
    
    void Start()
    {
        if (playerTransform == null)
        {
            playerTransform = FindObjectOfType<PlayerController>().transform;
        }
        
        // 计算相机偏移位置
        UpdateCameraOffset();
    }
    
    void LateUpdate()
    {
        if (playerTransform == null) return;
        
        // 目标位置
        Vector3 targetPosition = playerTransform.position + offset;
        
        // 平滑移动相机
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        
        // 相机始终看向玩家
        transform.LookAt(playerTransform.position + Vector3.up * 1.5f);
    }
    
    void UpdateCameraOffset()
    {
        // 根据角度和距离计算偏移
        float angleRad = angle * Mathf.Deg2Rad;
        float horizontalDistance = distance * Mathf.Cos(angleRad);
        float verticalDistance = distance * Mathf.Sin(angleRad);
        
        offset = new Vector3(-horizontalDistance * 0.707f, height, -horizontalDistance * 0.707f);
    }
    
    public void SetDistance(float newDistance)
    {
        distance = newDistance;
        UpdateCameraOffset();
    }
}
