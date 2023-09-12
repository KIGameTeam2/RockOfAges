using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using System;

public class KJHBullController : MonoBehaviour
{
    public float detectionRange = 5.0f;
    public float chargeSpeed = 10.0f;
    public LayerMask Rock;

    public float health = 100f; // ü�� ����
    public float attackPower = default; // ���ݷ� ����
    public float chargeCool = 5.0f;
    public Transform headTransform; // ���Ȳ�� �Ӹ��� Transform�� �����մϴ�.

    private float lastChargeTime = 0f;
    private Rigidbody bullRigidbody;
    private bool isCharging = false;
    private Transform targetRock;
    private Animator animator;
    private int chargeCount = 0;

    private void Start() // Start �޼��带 �����մϴ�.
    {
        animator = GetComponent<Animator>(); // �ִϸ����� ������Ʈ�� �����ɴϴ�.
        bullRigidbody = GetComponent<Rigidbody>(); // ������ٵ� ������Ʈ�� �����ɴϴ�.

    }

    private void Update()
    {
        if (!isCharging)
        {
            lastChargeTime += Time.deltaTime;
            if (lastChargeTime >= chargeCool)
            {
                DetectRock();
            }
        }
        else
        {
            ChargeTowardsRock();
        }

        // �Ӹ��� ���� �ٶ󺸰� �մϴ�.
        if (targetRock != null)
        {
            transform.LookAt(new Vector3(targetRock.transform.position.x, transform.position.y, targetRock.transform.position.z));
        }
    }

    public void TakeDamage(float damage) // �������� �޴� �޼��带 �����մϴ�.
    {
        health -= damage; // ü���� ���ҽ�ŵ�ϴ�.
        if (health <= 0) // ü���� 0 ������ ���
        {
            Destroy(gameObject); // ���� ������Ʈ�� �����մϴ�.
        }
    }

    private void DetectRock() // ���� Ž���ϴ� �޼��带 �����մϴ�.
    {
        Collider[] rocks = Physics.OverlapSphere(transform.position, detectionRange, Rock);
        if (rocks.Length > 0 && rocks[0] != null && chargeCount < 1)
        {
            targetRock = rocks[0].transform;
            isCharging = true;
        }
    }

    private void ChargeTowardsRock() // ���� ���� �����ϴ� �޼��带 �����մϴ�.
    {
        Vector3 direction = (targetRock.position - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(targetRock.position, transform.position);
        bullRigidbody.velocity = direction * chargeSpeed;
    }



    void ResetCharge() // ���� ���¸� �ʱ�ȭ�ϴ� �޼��带 �����մϴ�.
    {
        lastChargeTime = 0f;
        chargeCount = 0;
        isCharging = false;
    }
    void OnCollisionEnter(Collision collision) // �浹�� �߻����� ���� �޼��带 �����մϴ�.
    {
        RockBase rock = collision.gameObject.GetComponent<RockBase>();

        if (rock != null && chargeCount < 1)
        {
            chargeCount++;
            Rigidbody rockRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Rigidbody bullRigidbody = GetComponent<Rigidbody>();

            if (bullRigidbody != null)
            {
                bullRigidbody.velocity = Vector3.zero;
            }

            TakeDamage(1f);
            if (rockRigidbody != null)
            {
                Vector3 forceDirection = (targetRock.position - transform.position).normalized;
                rockRigidbody.velocity = Vector3.zero;
                rockRigidbody.AddForce(forceDirection * attackPower, ForceMode.VelocityChange);
                rockRigidbody.AddForce(Vector3.up * 10f, ForceMode.VelocityChange);

            }
            ResetCharge();
        }

    }

}