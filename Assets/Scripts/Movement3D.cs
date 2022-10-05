using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement3D : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpHeight;
    
    [Range(1.0f, 4.0f)]
    [SerializeField] float gravityScale;        // �߷� ����.
    
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
        // ���� üũ.
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
            // �߷¿� ���� ���� ����.
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * GRAVITY);
        }
    }
    void Gravity()
    {
        // �Ʒ��� �ϰ� �� ���� �����ϸ�...
        if (isGrounded && velocity.y < 0.0f)
            velocity.y = -2f;

        velocity.y += GRAVITY * Time.deltaTime;         // �߷� ���ӵ�.
        controller.Move(velocity * Time.deltaTime);     // �Ʒ������� �̵�.
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, groundRadius);
    }
}
