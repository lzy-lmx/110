using UnityEngine;

/// <summary>
/// 玩家控制脚本 - 优化版
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private CharacterController characterController;
    
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    private const float GROUND_Y_VELOCITY = -2f;
    private const float GRAVITY = 9.81f;
    
    private Camera mainCamera;
    
    void Start()
    {
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }
        
        // 缓存相机引用，避免每帧都调用 Camera.main
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("未找到主摄像机！");
        }
    }
    
    void Update()
    {
        HandleInput();
        ApplyGravity();
        Move();
    }
    
    void HandleInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        if (mainCamera == null) return;
        
        // 获取相机方向
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;
        
        // 清除垂直方向，确保沿着地面移动
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();
        
        // 计算移动方向
        moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;
        
        // 旋转角色面向移动方向
        if (moveDirection.magnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    void ApplyGravity()
    {
        if (characterController.isGrounded)
        {
            velocity.y = GROUND_Y_VELOCITY;
        }
        else
        {
            velocity.y -= GRAVITY * Time.deltaTime;
        }
    }
    
    void Move()
    {
        Vector3 moveVelocity = moveDirection * moveSpeed;
        moveVelocity.y = velocity.y;
        
        characterController.Move(moveVelocity * Time.deltaTime);
    }
}
