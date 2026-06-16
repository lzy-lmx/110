using UnityEngine;

/// <summary>
/// 相机控制脚本 - 45度俯视角视角 - 优化版
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float distance = 10f;
    [SerializeField] private float height = 8f;
    [SerializeField] private float angle = 45f;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private float lookAheadHeight = 1.5f;
    
    private Vector3 offset;
    private const float DIAGONAL_FACTOR = 0.707f; // sqrt(2)/2，用于45度角
    
    void Start()
    {
        if (playerTransform == null)
        {
            PlayerController playerController = FindObjectOfType<PlayerController>();
            if (playerController != null)
            {
                playerTransform = playerController.transform;
            }
            else
            {
                Debug.LogError("未找到玩家对象！");
                enabled = false;
                return;
            }
        }
        
        UpdateCameraOffset();
    }
    
    void LateUpdate()
    {
        if (playerTransform == null) return;
        
        Vector3 targetPosition = playerTransform.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(playerTransform.position + Vector3.up * lookAheadHeight);
    }
    
    void UpdateCameraOffset()
    {
        float angleRad = angle * Mathf.Deg2Rad;
        float horizontalDistance = distance * Mathf.Cos(angleRad);
        
        offset = new Vector3(-horizontalDistance * DIAGONAL_FACTOR, height, -horizontalDistance * DIAGONAL_FACTOR);
    }
    
    public void SetDistance(float newDistance)
    {
        distance = newDistance;
        UpdateCameraOffset();
    }
}
