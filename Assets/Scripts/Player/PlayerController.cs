using UnityEngine;

/// <summary>
/// 玩家控制脚本
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private float groundDrag = 5f;
    
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    private float groundYVelocity = -2f;
    
    void Start()
    {
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
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
        
        // 获取相机方向
        Camera mainCamera = Camera.main;
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;
        
        // 将输入方向相对于相机调整
        moveDirection = (cameraForward * vertical + cameraRight * horizontal).normalized;
        
        // 如果有输入，旋转角色面向移动方向
        if (moveDirection.magnitude > 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    void ApplyGravity()
    {
        // 简单的重力模拟
        if (characterController.isGrounded)
        {
            velocity.y = groundYVelocity;
        }
        else
        {
            velocity.y -= 9.81f * Time.deltaTime;
        }
    }
    
    void Move()
    {
        Vector3 moveVelocity = moveDirection * moveSpeed;
        moveVelocity.y = velocity.y;
        
        characterController.Move(moveVelocity * Time.deltaTime);
    }
}
