using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour, IHitObjectHandler
{
    private GatePhase currPhase = GatePhase.NORMAL;    
    private Transform gateCollider;

    [SerializeField]
    private List<GameObject> gateSkin;

    const int GATE_MAX_HP = 600;
    const float GATE_CRACK_CHANGE = .3f;
    //600 599~200 199~1 0
    //1 1~0.3 0.3~0 0

    [Range(0, GATE_MAX_HP)]
    [SerializeField]
    private float gateHP = GATE_MAX_HP;

    private void Awake()
    {
        gateCollider = transform.Find("DoorCollider");
        ChangePhase();
    }

    void ChangePhase()
    {
        GatePhase prePhase = currPhase;

        if (gateHP == GATE_MAX_HP)
        {
            currPhase = GatePhase.NORMAL;
        }
        else if (gateHP < GATE_MAX_HP && gateHP> GATE_MAX_HP*GATE_CRACK_CHANGE)
        {
            currPhase = GatePhase.CRACK;
        }
        else if (gateHP <= GATE_MAX_HP * GATE_CRACK_CHANGE && gateHP > 0)
        {
            currPhase = GatePhase.COLLAPSE;
        }
        else
        {
            currPhase = GatePhase.DESTROY;
            gateCollider.gameObject.SetActive(false);
        }

        if (prePhase != currPhase)
        {
            gateSkin[(int)prePhase].SetActive(false);
            gateSkin[(int)currPhase].SetActive(true);
        }

    }

    public void Hit(int damage)
    {
        //��Ÿ ���� ���� ���ҽ� ó��
        //���� : ī�޶�, ����Ŭó��?

        gateHP -= damage;
        HitReaction();
        ChangePhase();
    }

    public void HitReaction()
    {
        //�ִϸ��̼�, �ؽ�Ʈ, �Ҹ� �� ����Ŭ���� ������ ���׼��� ���⼭ ����
        //������?
    }
}


public enum GatePhase
{
    NORMAL = 0,
    CRACK = 1,
    COLLAPSE = 2,
    DESTROY = 3
}