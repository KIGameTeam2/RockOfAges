using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockColliderEvent : MonoBehaviour
{
    RockBase parent;

    readonly float SHAKE_TIME = .25f;
    float power = 0;
    float powerLimitMax = 0;

    const float COLLISION_LIMIT_LOW = .5f;

    private void Awake()
    {
        parent = GetComponentInParent<RockBase>();
        powerLimitMax = parent.rockStatus.Damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        power = rigidbody.velocity.magnitude;


        if (parent.IsMove(COLLISION_LIMIT_LOW))
        {
            AttackObstacle(collision);
            AttackWall(collision);
            AttackGate(collision);
        }
    }

    private void AttackObstacle(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            StartCoroutine(parent.CameraShakeRoutine(SHAKE_TIME, power, 3));
            parent.Attack(collision);
        }
    }

    private void AttackWall(Collision collision)
    {
        if (parent.IsMove(6) && collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            StartCoroutine(parent.CameraShakeRoutine(SHAKE_TIME, power, 3));
            parent.Hit(50);
        }
    }

    private void AttackGate(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Castle"))
        {
            //추가적인 mode변경 coroutine 실행하면 됨
            StartCoroutine(parent.CameraShakeRoutine(SHAKE_TIME, power, 3));
            parent.Attack(collision);
        }
    }
}
