using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    //!!!!!!!!!!!!!!!!!!!!!!!!!�׽�Ʈ �ڵ�
    //�ش� ������ item manager�� ����� ����??
    //���� �� ������
    public TestObstacle buildTarget;
    public GameObject gridTest;


    //Ŀ���� ���� �׸�����ġ
    public Vector3Int currCursorGridIndex = Vector3Int.zero;
    //�ǹ��� �ٶ󺸴� ����
    BuildRotateDirection whereLookAt;


    //���� ���
    //�������� ���� Viewer ������Ʈ
    BuildViewer viewer;

    //�ش� ������ �Ǽ� ���� ���¸� ���� 
    BitArray buildState;
    public Vector3 gridOffset = new Vector3(.5f, 0, .5f);

    //�� ������ 
    public static readonly int MAP_SIZE_X = 256;
    public static readonly int MAP_SIZE_Z = 256;
    public static readonly int MAP_SIZE_Y = 50;
    public static readonly int ONCE_ROTATE_EULER_ANGLE = 90;




    private void Awake()
    {
        buildState = new BitArray(MAP_SIZE_X * MAP_SIZE_Z);
        viewer = GetComponentInChildren<BuildViewer>();
        viewer.HideViewer();

        InitTerrainData();
        buildTarget.transform.localScale = Vector3.zero;
    }


    //���� ���� ����Ŭ ���¿� ���� �����Ѵ�. 
    //Ŀ���� ��ġ�� ���� ���� grid ���� �������ϰ�
    //���� grid ��ġ�� terrain�� ���� ���,
    //�ش� ������ �Ǽ� ���������� �Ǵ��Ѵ�.
    void Update()
    {
        ChangeBuildTarget(buildTarget);
        if (buildTarget == null)
        {
            return;
        }

        ChangeCurrGrid();
        ChangeBuildPosition();

        //���� ���¿� ���� �ش� ��ũ��Ʈ�� ó������ ���Ѵ�.
        //DEFANCE ���, ���� ��ġ�� �׸��忡 ������ ����, ���� �Ǽ� ��������
        if (!IsDefance() || !IsTerrain())
        {
            viewer.HideViewer();
            return;
        }

        viewer.ShowViewer();
        if (!CanBuild())
        {
            //���̶���Ʈ �� ����
        }
        else
        {
            if (Input.GetMouseButtonDown(1))
            {
                TestObstacle build = buildTarget.Build(currCursorGridIndex + gridOffset, Quaternion.Euler(0, (int)whereLookAt * ONCE_ROTATE_EULER_ANGLE, 0));
                build.name = buildTarget.name + "_" + currCursorGridIndex.z + "_" + currCursorGridIndex.x;
                SetBitArrays(currCursorGridIndex, buildTarget.size);
            }

        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeBuildRotation(-1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeBuildRotation(1);
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
                if (Physics.Raycast(new Vector3(x, MAP_SIZE_Y, z) + gridOffset, Vector3.down, out raycastHit, float.MaxValue, Global_PSC.FindLayerToName("Terrains")))
                {
                    GameObject gameObject = Instantiate(gridTest, raycastHit.point, Quaternion.identity);
                    gameObject.name = gridTest.name + "_" + z + "_" + x;
                    buildState.Set((z + MAP_SIZE_Z / 2) * MAP_SIZE_Z + (x + MAP_SIZE_X / 2), true);
                }
            }
        }
    }

    void SetBitArrays(Vector3 grid, Vector2Int buildSize)
    {
        for (int y = (int)(buildSize.y * .5f); y > -buildSize.y * .5f; y--)
        {
            for (int x = (int)(buildSize.x * .5f); x > -buildSize.x * .5f; x--)
            {
                Vector3 _grid = grid + Vector3Int.right * x + Vector3Int.forward * y;
                buildState.Set((int)((_grid.z + MAP_SIZE_Z *.5f) * MAP_SIZE_Z + _grid.x + MAP_SIZE_X * .5f), false) ;
            }
        }

    }


    void ChangeBuildTarget(TestObstacle target)
    {
        buildTarget = target;
        viewer.ChangeTarget(buildTarget.gameObject);
        gridOffset = new Vector3((buildTarget.size.x+1) % 2 * .5f, 0, (buildTarget.size.y+1) % 2 * .5f);
        //gridOffset = new Vector3((int)(buildTarget.size.x + 1) % 2 * .5f, 0, (int)(buildTarget.size.y + 1) % 2 * .5f);

    }

    bool ChangeCurrGrid()
    {
        //���콺 Ŀ���� �⺻������ (���ϴ� 0,0)�� �������� ���ȴ�.
        //�׷��� ������ ���콺 Ŀ���� �����µ� �̸� ���� ��ǥ�� ��ȯ�Ѵ�.

        Vector3 mouseWorldPos = Global_PSC.GetWorldMousePositionFromMainCamera();

        Vector3Int _currCursorGridIndex;

        if(gridOffset.x == .5f)
        {

            _currCursorGridIndex = new Vector3Int(Mathf.FloorToInt(mouseWorldPos.x), 0, Mathf.FloorToInt(mouseWorldPos.z));
        }
        else
        {
            _currCursorGridIndex = new Vector3Int(Mathf.RoundToInt(mouseWorldPos.x), 0, Mathf.RoundToInt(mouseWorldPos.z));
        }


        if (currCursorGridIndex != _currCursorGridIndex)
        {
            currCursorGridIndex = _currCursorGridIndex;
            return true;
        }
        return false;
    }



    //grid ������ �ٲ𶧸��� �ҷ��´�.
    //target�� ��ġ�� �����Ѵ�.
    void ChangeBuildPosition()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(currCursorGridIndex+ gridOffset + Vector3.up * MAP_SIZE_Y, Vector3.down, out raycastHit, float.MaxValue, Global_PSC.FindLayerToName("Terrains")))
        {
            Vector3 newPosition = raycastHit.point + viewer.transform.up * (viewer.GetHeight() / 2);
            viewer.transform.position = newPosition;
        }
    }

    //Ư��Ű ������ ���� ȸ�� ��Ų��.
    //�� ������ �����ȴ�.
    void ChangeBuildRotation(int diff)
    {
        if(whereLookAt != BuildRotateDirection.LEFT && diff==1)
        {
            whereLookAt = BuildRotateDirection.UP;
        }
        else if (whereLookAt != BuildRotateDirection.UP && diff == -1)
        {
            whereLookAt = BuildRotateDirection.LEFT;
        }
        else
        {
            whereLookAt = whereLookAt + diff;
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
        if (Physics.Raycast(currCursorGridIndex + gridOffset+ Vector3.up * MAP_SIZE_Y, Vector3.down, out raycastHit, float.MaxValue, Global_PSC.FindLayerToName("Terrains")))
        {
            return true;
        }

        return false;
    }

    //�������� limit���¿� �ش� terrain�� �Ǽ� ���� ���¸� &&�Ѵ�.
    bool CanBuild()
    {
        return GetBuildEnable(currCursorGridIndex, buildTarget.size) && GetItemLimitState();
    }

    //���� �Ǽ��� �������� �ִ� �Ǽ� ������ ���� �Ǽ� ������ ���Ѵ�.
    //true : ���� �Ǽ� ������ �ִ� �Ǽ����� ����
    bool GetItemLimitState()
    {
        return true;
    }

    //���� grid��ġ�� �ֺ� ��ġ�� terrain�� ���¸� ���� ��
    bool GetBuildEnable(Vector3 grid, Vector2Int buildSize)
    {
        bool result = true;

        for (int y = (int)(buildSize.y * .5f); y > -buildSize.y * .5f; y--) 
        {
            for (int x = (int)(buildSize.x * .5f); x > -buildSize.x * .5f; x--)
            {
                result = result && GetBuildEnable(grid + Vector3Int.right*x + Vector3Int.forward*y);
            
            }
        }

        return result;
    }
    bool GetBuildEnable(Vector3 grid)
    {
         return buildState.Get((int)((grid.z + MAP_SIZE_Z / 2) * MAP_SIZE_Z + (grid.x + MAP_SIZE_X / 2)));
    }
}





public enum BuildRotateDirection
{
    UP = 0,
    RIGHT = 1,
    DOWN = 2,
    LEFT = 3
}
