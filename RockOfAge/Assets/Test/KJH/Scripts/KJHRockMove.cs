using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KJHRockMove : MonoBehaviour
{
    #region [[SerializeField]
    [SerializeField]
    private float forceAmount = 1f;
    [SerializeField]
    private float jumpForce = 3000f;
    [SerializeField]
    private float health = 100f;
    [SerializeField]
    private float attackPowerBase = 1f;
    #endregion

    private float attackPower;
    private Rigidbody rb;
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
        {
            Vector3 cameraForward = mainCamera.transform.forward;
            Vector3 cameraRight = mainCamera.transform.right;

            Vector3 forceDirection = (cameraForward * verticalInput + cameraRight * horizontalInput);
            forceDirection.y = 0;

            rb.AddForce(forceDirection * forceAmount, ForceMode.Acceleration);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // ���ݷ��� ���� �ӵ��� ����ϰ� ����մϴ�.
        float currentSpeed = rb.velocity.magnitude;
        attackPower = attackPowerBase * (health + currentSpeed);
    }
    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    public void Attack(GameObject target)
    {
        // Ÿ�ٿ��� ���ݷ¸�ŭ�� �������� �����ϴ�.
        // Ÿ���� ������ ó�� �޼��带 ȣ���ϰ� attackPower ���� �����մϴ�.
        target.GetComponent<Target>().TakeDamage(attackPower);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            Attack(collision.gameObject);
        }
    }
}