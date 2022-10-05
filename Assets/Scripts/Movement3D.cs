using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement3D : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpHeight;
    
    [Range(1.0f, 4.0f)]
    [SerializeField] float gravityScale;        // 중력 배율.
    
    [SerializeField] float groundRadius;
    [SerializeField] LayerMask groundMask;

    CharacterController controller;

    Vector3 velocity;
    float GRAVITY => -9.8f * gravityScale;
    bool isGrounded;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        // 지면 체크.
        CheckGround();
        Movement();
        Jump();
        Gravity();
    }
    void CheckGround()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundRadius, groundMask);
    }
    void Movement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 direction = (transform.right * x) + (transform.forward * z);
        controller.Move(direction * moveSpeed * Time.deltaTime);
    }
    void Jump()
    {
        if(isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            // 중력에 따른 높이 공식.
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * GRAVITY);
        }
    }
    void Gravity()
    {
        // 아래로 하강 중 땅에 도착하면...
        if (isGrounded && velocity.y < 0.0f)
            velocity.y = -2f;

        velocity.y += GRAVITY * Time.deltaTime;         // 중력 가속도.
        controller.Move(velocity * Time.deltaTime);     // 아래쪽으로 이동.
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, groundRadius);
    }
}
