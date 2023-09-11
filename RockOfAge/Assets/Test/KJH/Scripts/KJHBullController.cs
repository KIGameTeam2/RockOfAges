using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System;

public class KJHBullController : MonoBehaviour
{
    public float detectionRange = 5.0f;
    public float chargeSpeed = 10.0f;
    public LayerMask Rock;

    public float health = 100f; // ü�� ����
    public float attackPower = 10f; // ���ݷ� ����
    public float chargeCool = 5.0f;

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

    private void Update() // Update �޼��带 �����մϴ�.
    {
        if (!isCharging) // ���� ���� �ƴ� ���
        {
            lastChargeTime += Time.deltaTime;
            if (lastChargeTime >= chargeCool) // ��Ÿ���� ������ ���
            {
                // �ڱ��ڸ����� ���ư�.
                DetectRock(); // ���� Ž���մϴ�.
            }
        }
        else
        {
            ChargeTowardsRock(); // ���� ���� �����մϴ�.
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

        if (distanceToTarget > 1f) // ���� �����ϱ� ������ ����
        {
            bullRigidbody.velocity = direction * chargeSpeed;
        }
        else // ���� �������� �� ���� ���� �ʱ�ȭ �� �浹 ó��
        {
            chargeCount++;
            Rigidbody rockRigidbody = targetRock.GetComponent<Rigidbody>();
            if (rockRigidbody == null)
            {
                targetRock.gameObject.AddComponent<Rigidbody>();
                rockRigidbody = targetRock.GetComponent<Rigidbody>();
            }

            Vector3 forceDirection = (targetRock.position - transform.position).normalized;
            rockRigidbody.AddForce(forceDirection * attackPower, ForceMode.Impulse);
            lastChargeTime = Time.time; // ��Ÿ�� ����
            bullRigidbody.velocity = Vector3.zero; // Ȳ���� �ӵ��� 0���� ����
        }
    }

    void ResetCharge() // ���� ���¸� �ʱ�ȭ�ϴ� �޼��带 �����մϴ�.
    {
        isCharging = false;
    }
    void OnCollisionEnter(Collision collision) // �浹�� �߻����� ���� �޼��带 �����մϴ�.
    {
        RockBase rock = collision.gameObject.GetComponent<RockBase>();

        if (rock != null)
        {
            Rigidbody rockRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Rigidbody bullRigidbody = GetComponent<Rigidbody>();

            if (bullRigidbody != null)
            {
                bullRigidbody.velocity = Vector3.zero;
            }

            TakeDamage(1f);
            if (rockRigidbody != null)
            {
                rockRigidbody.velocity = bullRigidbody.velocity;
            }
        }
        lastChargeTime = 0f;
        ResetCharge();
    }

}