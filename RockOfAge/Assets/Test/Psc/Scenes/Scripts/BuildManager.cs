using ExitGames.Client.Photon.StructWrapping;
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
    public GameObject gridTest;


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

    public static readonly Vector3 GRID_OFFSET = new Vector3(.5f, 0, .5f);



    private void Awake()
    {
        buildState = new BitArray(MAP_SIZE_X * MAP_SIZE_Z);
        viewer = GetComponentInChildren<BuildViewer>();
        viewer.HideViewer();

        InitTerrainData();
        tmp.transform.localScale = Vector3.zero;
    }


    //���� ���� ����Ŭ ���¿� ���� �����Ѵ�. 
    //Ŀ���� ��ġ�� ���� ���� grid ���� �������ϰ�
    //���� grid ��ġ�� terrain�� ���� ���,
    //�ش� ������ �Ǽ� ���������� �Ǵ��Ѵ�.
    void Update()
    {
        ChangeCurrGrid();

        //���� ���¿� ���� �ش� ��ũ��Ʈ�� ó������ ���Ѵ�.
        //DEFANCE ���, ���� ��ġ�� �׸��忡 ������ ����, ���� �Ǽ� ��������
        if (!IsDefance() || !IsTerrain() || !CanBuild())
        {
            viewer.HideViewer();
            return;
        }

        viewer.ShowViewer();

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("In");
            tmp.Build(currCursorGridIndex+GRID_OFFSET, Quaternion.Euler(0, (int)whereLookAt * ONCE_ROTATE_EULER_ANGLE, 0));
            buildState.Set((currCursorGridIndex.z + MAP_SIZE_Z / 2) * MAP_SIZE_Z + (currCursorGridIndex.x + MAP_SIZE_X / 2), false);
        }

    }

    //�� ����� map �Ǽ� ���� ���¸� init��
    //terrain �񱳽� tag�� team, �⺻ �Ǽ� �Ұ� Ÿ�ϵ��� ���ϴ� �κ� �߰��ؾ��ҰŰ���
    //���� ȸ���� �߰�
    void InitTerrainData()
    {
        buildState.SetAll(false);
        for (int z = -MAP_SIZE_Z / 2; z < MAP_SIZE_Z / 2; z++)
        {
            for (int x = -MAP_SIZE_X / 2; x < MAP_SIZE_X / 2; x++)
            {
                RaycastHit raycastHit;
                if (Physics.Raycast(new Vector3(x, MAP_SIZE_Y, z) + GRID_OFFSET, Vector3.down, out raycastHit, float.MaxValue, Global_PSC.FindLayerToName("Terrains")))
                {
                    GameObject gameObject = Instantiate(gridTest, raycastHit.point, Quaternion.identity);
                    buildState.Set((z + MAP_SIZE_Z / 2) * MAP_SIZE_Z + (x + MAP_SIZE_X / 2), true);
                    //Debug.LogError((i + MAP_SIZE_Z / 2) * MAP_SIZE_Z + (j + MAP_SIZE_X / 2)+"/"+buildState.Get((i + MAP_SIZE_Z / 2) * MAP_SIZE_Z + (j + MAP_SIZE_X / 2)));
                }
            }
        }
    }


    void ChangeCurrGrid()
    {
        //���콺 Ŀ���� �⺻������ (���ϴ� 0,0)�� �������� ���ȴ�.
        //�׷��� ������ ���콺 Ŀ���� �����µ� �̸� ���� ��ǥ�� ��ȯ�Ѵ�.

        Vector3 mouseWorldPos = Global_PSC.GetWorldMousePositionFromMainCamera(Camera.main.transform.position.y);

        Vector3Int _currCursorGridIndex = new Vector3Int(Mathf.FloorToInt(mouseWorldPos.x), 0, Mathf.FloorToInt(mouseWorldPos.z));
        if(currCursorGridIndex != _currCursorGridIndex)
        {
            currCursorGridIndex = _currCursorGridIndex;
            ChangeBuildPosition();
        }
    }

    void ChangeBuildTarget(TestObstacle target)
    {
        viewer.ChangeTarget(target.gameObject);
    }


    //grid ������ �ٲ𶧸��� �ҷ��´�.
    //target�� ��ġ�� �����Ѵ�.
    void ChangeBuildPosition()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(currCursorGridIndex+ GRID_OFFSET + Vector3.up * MAP_SIZE_Y, Vector3.down, out raycastHit, float.MaxValue, Global_PSC.FindLayerToName("Terrains")))
        {
            Vector3 newPosition = raycastHit.point + transform.up * (viewer.GetHeight() / 2);
            transform.position = newPosition;
        }
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


    bool IsDefance()
    {
        return true;
    }

    //���� �׸��� ��ġ�� �Ǽ� ������ ������ �����ϴ��� üũ
    bool IsTerrain()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(currCursorGridIndex + GRID_OFFSET+ Vector3.up * MAP_SIZE_Y, Vector3.down, out raycastHit, float.MaxValue, Global_PSC.FindLayerToName("Terrains")))
        {
            return true;
        }

        return false;
    }

    //�������� limit���¿� �ش� terrain�� �Ǽ� ���� ���¸� &&�Ѵ�.
    bool CanBuild()
    {
        return GetBuildEnable(currCursorGridIndex) && GetItemLimitState();
    }

    //���� �Ǽ��� �������� �ִ� �Ǽ� ������ ���� �Ǽ� ������ ���Ѵ�.
    //true : ���� �Ǽ� ������ �ִ� �Ǽ����� ����
    bool GetItemLimitState()
    {
        return true;
    }

    //���� grid��ġ�� �ֺ� ��ġ�� terrain�� ���¸� ���� ��
    bool GetBuildEnable(Vector3Int grid, Vector2 buildSize)
    {
        //for

        return buildState.Get((grid.z + MAP_SIZE_Z / 2) * MAP_SIZE_Z + (grid.x + MAP_SIZE_X / 2));
    }
    bool GetBuildEnable(Vector3Int grid)
    {
        return GetBuildEnable(grid, Vector2.one);
    }
}





public enum BuildRotateDirection
{
    UP = 0,
    RIGHT = 1,
    DOWN = 2,
    LEFT = 3
}
