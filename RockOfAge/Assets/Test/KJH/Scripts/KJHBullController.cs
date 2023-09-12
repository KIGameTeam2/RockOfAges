using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using System;

public class KJHBullController : MonoBehaviour, IHitObjectHandler
{
    public float detectionRange = default;
    public float chargeSpeed = 10.0f;
    public LayerMask Rock;

    public float health = 100f; // ü�� ����
    public float attackPower = default; // ���ݷ� ����
    public float chargeCool = 5.0f;
    public float walkSpeed = 3.0f;

    private Vector3 lastRockPosition; // ���� ������ ��ġ�� ������ ���� �߰�
    private bool hasCharged = false; // ���Ȳ�Ұ� �����ߴ��� ���θ� ��Ÿ���� ���� �߰�
    private float lastChargeTime = 0f;
    private Rigidbody bullRigidbody;
    private bool isCharging = false;
    private Transform targetRock;
    private Animator animator;
    private int chargeCount = 0;
    private Vector3 initialBullPosition;
    private bool isReturning = false;

    private void Start() // Start �޼��带 �����մϴ�.
    {
        animator = GetComponent<Animator>(); // �ִϸ����� ������Ʈ�� �����ɴϴ�.
        bullRigidbody = GetComponent<Rigidbody>(); // ������ٵ� ������Ʈ�� �����ɴϴ�.
        initialBullPosition = transform.position;
    }


    //private void Update()
    //{
    //    if (!isCharging)
    //    {
    //        lastChargeTime += Time.deltaTime;
    //        if (lastChargeTime >= chargeCool)
    //        {
    //            DetectRock();
    //            if (isCharging)
    //            {
    //                lastChargeTime = 0f;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        ChargeTowardsRock();

    //        float distanceToLastRockPosition = Vector3.Distance(transform.position, lastRockPosition);

    //        // ���� ���� �� ���� ������ ��ǥ�� �����ߴ��� Ȯ���մϴ�.
    //        if (distanceToLastRockPosition <= 10.0f)
    //        {
    //            Debug.LogFormat("????");
    //            ResetCharge();
    //        }
    //    }
    //    // �Ӹ��� ���� �ٶ󺸰� �մϴ�.
    //    if (targetRock != null)
    //    {
    //        if (isCharging)
    //        {
    //            // ���� ���� ���� ���� ������ �ٶ󺾴ϴ�.
    //            Vector3 chargeDirection = (lastRockPosition - transform.position).normalized;
    //            chargeDirection.y = 0;
    //            transform.rotation = Quaternion.LookRotation(chargeDirection);
    //        }
    //        else
    //        {
    //            // ���� ���� �ƴ� ���� ���� �ٶ󺾴ϴ�.
    //            transform.LookAt(new Vector3(targetRock.transform.position.x, transform.position.y, targetRock.transform.position.z));
    //        }
    //    }
    //}
    private void Update()
    {
        animator.SetBool("isCharging", isCharging);
        animator.SetBool("isReturning", isReturning);

        if (!isCharging && !isReturning)
        {
            lastChargeTime += Time.deltaTime;
            if (lastChargeTime >= chargeCool)
            {
                DetectRock();
                if (isCharging)
                {
                    Debug.LogFormat("�����߳�?");

                    lastChargeTime = 0f;
                }
            }
        }
        else if (isCharging)
        {
            ChargeTowardsRock();
       
            float distanceToLastRockPosition = Vector3.Distance(transform.position, lastRockPosition);

            if (distanceToLastRockPosition <= 10.0f)
            {
                isCharging = false;
                isReturning = true;
            }
        }
        else if (isReturning)
        {
            ReturnToInitialPositionBackwards();

            float distanceToInitialPosition = Vector3.Distance(transform.position, initialBullPosition);

            if (distanceToInitialPosition <= 1.0f)
            {
                ResetCharge();
                isReturning = false;
            }
        }
        // �Ӹ��� ���� �ٶ󺸰� �մϴ�.
        if (targetRock != null && !isCharging && !isReturning)
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
    //private void DetectRock() // ���� Ž���ϴ� �޼��带 �����մϴ�.
    //{
    //    Collider[] rocks = Physics.OverlapSphere(transform.position, detectionRange, Rock);
    //    if (rocks.Length > 0 && rocks[0] != null)
    //    {
    //        targetRock = rocks[0].transform;
    //        lastRockPosition = targetRock.position; // ���� ���� ��ġ�� ����
    //        isCharging = true; // ���� ���¸� true�� �����մϴ�.
    //    }
    //}

    private void DetectRock()
    {
        float coneAngle = 45f; // ������ ������ �����մϴ�. �ʿ信 ���� �����ϼ���.
        float halfConeAngle = coneAngle * 0.5f;

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, detectionRange, transform.forward, detectionRange, Rock);
        float minAngle = float.MaxValue;
        Transform closestRock = null;

        foreach (RaycastHit hit in hits)
        {
            Vector3 hitDirection = (hit.transform.position - transform.position).normalized;
            float angleToTarget = Vector3.Angle(transform.forward, hitDirection);

            if (angleToTarget <= halfConeAngle && angleToTarget < minAngle)
            {
                minAngle = angleToTarget;
                closestRock = hit.transform;
            }
        }

        if (closestRock != null)
        {
            targetRock = closestRock;
            lastRockPosition = targetRock.position;
            isCharging = true;
        }
    }
    private void ChargeTowardsRock() // ���� ���� �����ϴ� �޼��带 �����մϴ�.
    {
        Vector3 direction = (lastRockPosition - transform.position).normalized;
        direction.y = 0; // y�� ���� 0���� �����մϴ�.r
        float distanceToTarget = Vector3.Distance(lastRockPosition, transform.position);
        bullRigidbody.velocity = direction * chargeSpeed;
        // Ȳ�Ұ� ���� �ٶ󺸰� �մϴ�.
        transform.rotation = Quaternion.LookRotation(direction);
    }

    void ResetCharge()
    {
        targetRock = null;
        lastChargeTime = 0f;
        chargeCount = 0;
        isCharging = false;
    }

   
    void OnCollisionEnter(Collision collision)
    {
        RockBase rock = collision.gameObject.GetComponent<RockBase>();

        if (rock != null && chargeCount < 1)
        {
       Debug.LogFormat("�浹�߳�?");

            hasCharged = true;
            chargeCount++;
            isCharging = false;
            isReturning = true;

            Rigidbody rockRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Rigidbody bullRigidbody = GetComponent<Rigidbody>();

            if (bullRigidbody != null)
            {
                bullRigidbody.velocity = Vector3.zero;
            }

            if (rockRigidbody != null && targetRock != null)
            {
                Vector3 forceDirection = (targetRock.position - transform.position).normalized;
                rockRigidbody.velocity = Vector3.zero;
                rockRigidbody.AddForce(forceDirection * attackPower, ForceMode.VelocityChange);
                rockRigidbody.AddForce(Vector3.up * 15f, ForceMode.VelocityChange);
            }
            IHitObjectHandler hitObj = collision.gameObject.GetComponent<IHitObjectHandler>();
            if (hitObj != null)
            {
                hitObj.Hit((int)attackPower);
            }
            ResetCharge();
        }
    }

    private void ReturnToInitialPositionBackwards()
    {
        Vector3 direction = (initialBullPosition - transform.position).normalized;
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(-direction);
        transform.position += direction * walkSpeed * Time.deltaTime;

        float distanceToInitialPosition = Vector3.Distance(transform.position, initialBullPosition);
        if (distanceToInitialPosition <= 1.0f)
        {
            isReturning = false;
        }
    }

    public void Hit(int damage)
    {
        throw new NotImplementedException();
    }

    public void HitReaction()
    {
        throw new NotImplementedException();
    }
}