using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildColorHighLight : MonoBehaviour, IHitObjectHandler
{
    public Vector2 size;
    private GameObject allowPlane;        // �Ǽ� ���ɽ�
    private GameObject denyPlane;        // �Ǽ� �Ұ��ɽ�

    void Awake()
    {
        allowPlane = transform.Find("BuildAllow").gameObject;
        denyPlane = transform.Find("BuildDeny").gameObject;

        MeshRenderer allowRenderer =  denyPlane.GetComponent<MeshRenderer>();
        MeshRenderer denyRenderer = denyPlane.GetComponent<MeshRenderer>();

        // �Ұ����� plane�� �� ���� ���� ���� ť�� ����
        denyRenderer.material.renderQueue = allowRenderer.material.renderQueue + 1;
    }


    public void UpdateColorHighLightSize(Vector2 _size)
    {
        this.size = _size;
        allowPlane.transform.localScale = new Vector3(size.x, 1, size.y);
        denyPlane.transform.localScale = new Vector3(size.x, 1, size.y);
    }

    public void UpdateColorHighLightColor(bool canBuild)
    {
        if (canBuild)
        {
            allowPlane.SetActive(true);
            denyPlane.SetActive(false);
        }
        else
        {
            allowPlane.SetActive(false);
            denyPlane.SetActive(true); 
        }
    }

    public void Hit(int damage)
    {
        throw new System.NotImplementedException();
    }

    public void HitReaction()
    {
        throw new System.NotImplementedException();
    }
}
