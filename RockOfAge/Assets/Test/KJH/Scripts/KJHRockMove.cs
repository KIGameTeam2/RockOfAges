using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KJHRockMove : RockBase
{


    private float attackPower;

    void Start()
    {
        Init();
    }

    void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // ���ݷ��� ���� �ӵ��� ����ϰ� ����մϴ�.
        float currentSpeed = rb.velocity.magnitude;
        attackPower = attackPowerBase * (rockStatus.Health + currentSpeed);
    }
   
    override public void Attack(GameObject target)
    {
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