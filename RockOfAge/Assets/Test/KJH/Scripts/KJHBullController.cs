using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using System;

public class KJHBullController : MonoBehaviour, IHitObjectHandler
{
    public float detectionRange = default;
    public float chargeSpeed = default;
    public float health = 100f; // ü�� ����
    public float attackPower = default; // ���ݷ� ����
    public float chargeCool = default;
    public float walkSpeed = default;
    public LayerMask Rock;

    bool isDying = false;
    private int chargeCount = 0;
    private int rockCollisionCount = 0;// ���� �浹�� Ƚ���� �����ϴ� ���� �߰�
    private float lastChargeTime = 0f;
    private Vector3 lastRockPosition; // ���� ������ ��ġ�� ������ ���� �߰�
    private Vector3 initialBullPosition;
    private bool hasCharged = false; // ���Ȳ�Ұ� �����ߴ��� ���θ� ��Ÿ���� ���� �߰�
    private bool isCharging = false;
    private bool isReturning = false;
    private Rigidbody bullRigidbody;
    private Transform targetRock;
    private Animator animator;

    private void Start() // Start �޼��带 �����մϴ�.
    {
        animator = GetComponent<Animator>(); // �ִϸ����� ������Ʈ�� �����ɴϴ�.
        bullRigidbody = GetComponent<Rigidbody>(); // ������ٵ� ������Ʈ�� �����ɴϴ�.
        initialBullPosition = transform.position;
    }

    private void Update()
    {
        if(isDying == true)
        {
            return;
        }
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
        direction.y = 0; // y�� ���� 0���� �����մϴ�.
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
        // ���Ȳ�Ұ� ���� �浹���� �� �浹 Ƚ���� ������Ű��, �浹 Ƚ���� 2 �̻��̸� �״� �ڵ� �߰�
        if (collision.gameObject.layer == LayerMask.NameToLayer("Rock"))
        {
            rockCollisionCount++;
            if (rockCollisionCount >= 2)
            {
                isDying = true;
                StartCoroutine(Die()); // Ȳ�Ұ� ���� �� Die �ڷ�ƾ�� �����մϴ�.
            }
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
    private IEnumerator Die()
    {
        animator.SetBool("isDying", true); // �ִϸ������� "isDying" �Ķ���͸� true�� �����մϴ�.

        // ������ ����
        Rigidbody bullRigidbody = GetComponent<Rigidbody>();
        if(bullRigidbody != null)
        {
            bullRigidbody.velocity = Vector3.zero;
        }
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // �ִϸ��̼� Ŭ���� ���̸�ŭ ����մϴ�.
                                                                                         // Ȳ���� �������� �����մϴ�.
        if (bullRigidbody != null)
        {
            bullRigidbody.isKinematic = true; // Ȳ���� Rigidbody�� isKinematic ���·� �����Ͽ� �������� �����մϴ�.
        }
        // �ִϸ��̼��� ���� ���·� 3�� ���
        animator.speed = 0f;
        yield return new WaitForSeconds(3f);
        Destroy(gameObject); // �ִϸ��̼��� ���� �Ŀ� ���� ������Ʈ�� �ı��մϴ�.
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


