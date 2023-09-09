using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    //!!!!!!!!!!!!!!!!!!!!!!!!!�׽�Ʈ �ڵ�
    //�ش� ������ item manager�� ����� ����??
    //���� �� ������
    public TestObstacle tmp;


    //Ŀ���� ���� �׸�����ġ
    public Vector3Int currCursorGridIndex = Vector3Int.zero;
    //�ǹ��� �ٶ󺸴� ����
    BuildRotateDirection whereLookAt;
    public static readonly int ONCE_ROTATE_EULER_ANGLE = 90;


    //���� ���
    //�������� ���� Viewer ������Ʈ
    BuildViewer viewer;

    //�ش� ������ �Ǽ� ���� ���¸� ���� 
    BitArray buildState;
    //�� ������ 
    public static readonly int MAP_SIZE_X = 256;
    public static readonly int MAP_SIZE_Z = 256;
    public static readonly int MAP_SIZE_Y = 50;




    private void Awake()
    {
        buildState = new BitArray(MAP_SIZE_X * MAP_SIZE_Z);
        viewer = GetComponentInChildren<BuildViewer>();
        viewer.transform.localScale = Vector3.zero;

        InitTerrainData();
    }


    //���� ���� ����Ŭ ���¿� ���� �����Ѵ�. 
    //Ŀ���� ��ġ�� ���� ���� grid ���� �������ϰ�
    //���� grid ��ġ�� terrain�� ���� ���,
    //�ش� ������ �Ǽ� ���������� �Ǵ��Ѵ�.
    void Update()
    {
        //���� ���¿� ���� �ش� ��ũ��Ʈ�� ó������ ���Ѵ�.
        //DEFANCE ���, ���� ��ġ�� �׸��忡 ������ ����, ���� �Ǽ� ��������
        if (!IsDefance() || !IsTerrain() || !CanBuild())
        {
            return;
        }
        /*
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit raycastHit;
                if (Physics.Raycast(ray, out raycastHit, float.MaxValue, LayerMask.NameToLayer("Terrains")))
                {
                    copyObject.enabled = true;
                    SetCurrBuildPosition(Input.mousePosition, out copyObject);
                }
        */


        if (Input.GetMouseButtonDown(1))
        {
        }

    }


    //�� ����� map �Ǽ� ���� ���¸� init��
    //terrain �񱳽� tag�� team, �⺻ �Ǽ� �Ұ� Ÿ�ϵ��� ���ϴ� �κ� �߰��ؾ��ҰŰ���
    //���� ȸ���� �߰�

    void InitTerrainData()
    {
        buildState.SetAll(false);
        for (int i = -MAP_SIZE_Z / 2; i < MAP_SIZE_Z / 2; i++)
        {

            for (int j = -MAP_SIZE_X / 2; j < MAP_SIZE_X / 2; j++)
            {
                Debug.Log(i + ", " + j);

                RaycastHit raycastHit;
                if (Physics.Raycast(new Vector3(i, MAP_SIZE_Y, j), Vector3.down, out raycastHit, float.MaxValue, LayerMask.NameToLayer("Terrains")))
                {
                    Debug.Log("Hit");
                    buildState.Set((i + MAP_SIZE_Z / 2) * MAP_SIZE_Z + (j + MAP_SIZE_X / 2), true);

                }
            }
        }
    }

    void ChangeBuilding(TestObstacle target)
    {
        viewer.ChangeTarget(target.gameObject);
    }


    //Ư��Ű ������ ���� ȸ�� ��Ų��.
    //�� ������ �����ȴ�.
    void ChangeBuildRotation()
    {
        if (whereLookAt != BuildRotateDirection.LEFT)
        {
            whereLookAt = whereLookAt + 1;
        }
        else
        {
            whereLookAt = 0;
        }
        viewer.transform.localEulerAngles = Vector3.up * ONCE_ROTATE_EULER_ANGLE * (int)whereLookAt;
    }

    void ChangeBuildPosition()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(currCursorGridIndex + Vector3.up * MAP_SIZE_Y, Vector3.down, out raycastHit, float.MaxValue, LayerMask.NameToLayer("Terrains")))
        {
            Vector3 newPosition = raycastHit.point + transform.up * (viewer.GetHeight() / 2);
            transform.position = newPosition;
        }
    }

    void ChangeCurrGrid()
    {
        Vector3 cursorPos = Input.mousePosition;
        currCursorGridIndex = new Vector3Int(Mathf.RoundToInt(cursorPos.x), 0, Mathf.RoundToInt(cursorPos.y));
        Debug.Log(currCursorGridIndex.x + " / " + currCursorGridIndex.y);
    }


    bool IsDefance()
    {
        return false;
    }

    //���� �׸��� ��ġ�� �Ǽ� ������ ������ �����ϴ��� üũ
    bool IsTerrain()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(currCursorGridIndex + Vector3.up * MAP_SIZE_Y, Vector3.down, out raycastHit, float.MaxValue, LayerMask.NameToLayer("Terrains")))
        {
            return true;
        }

        return false;
    }

    //�������� limit���¿� �ش� terrain�� �Ǽ� ���� ���¸� &&�Ѵ�.
    bool CanBuild()
    {
        return GetBuildEnable() && GetItemLimitState();
    }

    //���� �Ǽ��� �������� �ִ� �Ǽ� ������ ���� �Ǽ� ������ ���Ѵ�.
    //true : ���� �Ǽ� ������ �ִ� �Ǽ����� ����
    bool GetItemLimitState()
    {
        return true;
    }

    //���� grid��ġ�� �ֺ� ��ġ�� terrain�� ���¸� ���� ��
    bool GetBuildEnable(Vector2 buildSize)
    {
        return false;
    }
    bool GetBuildEnable()
    {
        return GetBuildEnable(Vector2.one);
    }
}





public enum BuildRotateDirection
{
    UP = 0,
    RIGHT = 1,
    DOWN = 2,
    LEFT = 3
}
