using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    //!!!!!!!!!!!!!!!!!!!!!!!!!�׽�Ʈ �ڵ�
    //�ش� ������ item manager�� ����� ����??
    //���� �� ������
    public TestObstacle buildTarget;
    public GameObject gridTest;
    public Material red;


    //Ŀ���� ���� �׸�����ġ
    private Vector3Int currCursorGridIndex = Vector3Int.zero;
    //�ǹ��� �ٶ󺸴� ����
    private BuildRotateDirection whereLookAt;


    //���� ���
    //�������� ���� Viewer ������Ʈ
    private BuildViewer viewer;

    //�ش� ������ �Ǽ� ���� ���¸� ���� 
    private BitArray buildState;
    private Vector3 gridOffset;

    //�� ������ 
    public static readonly int MAP_SIZE_X = 256;
    public static readonly int MAP_SIZE_Z = 256;
    public static readonly int MAP_SIZE_Y = 50;
    public static readonly Vector3 FIXED_GRID_OFFSET = (Vector3.one - Vector3.up) * .5f;

    const int ONCE_ROTATE_EULER_ANGLE = 90;




    private void Awake()
    {
        instance = this;

        buildState = new BitArray(MAP_SIZE_X * MAP_SIZE_Z);
        viewer = GetComponentInChildren<BuildViewer>();
        viewer.HideViewer();

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
        viewer.UpdateMouseMove(CanBuild());

        if (CanBuild())
        {
            if (Input.GetMouseButtonDown(1))
            {
                TestObstacle build = buildTarget.Build(viewer.transform.position-Vector3.up*.7f, Quaternion.Euler(0, (int)whereLookAt * ONCE_ROTATE_EULER_ANGLE, 0));
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
    public void InitTerrainData()
    {
        buildState.SetAll(false);
        for (int z = -MAP_SIZE_Z / 2; z < MAP_SIZE_Z / 2; z++)
        {
            for (int x = -MAP_SIZE_X / 2; x < MAP_SIZE_X / 2; x++)
            {
                RaycastHit raycastHit;
                if (Physics.Raycast(new Vector3(x, MAP_SIZE_Y, z) + (Vector3.one - Vector3.up)*.5f, Vector3.down, out raycastHit, float.MaxValue, Global_PSC.FindLayerToName("Terrains")))
                {
                    if (raycastHit.collider.CompareTag("Block"))
                    {
                        continue;
                    }

                    buildState.Set((z + MAP_SIZE_Z / 2) * MAP_SIZE_Z + (x + MAP_SIZE_X / 2), true);

                    ///////////////////////////////////////////////////////////�׽�Ʈ �ڵ�
                    GameObject gameObject = Instantiate(gridTest, raycastHit.point+Vector3.up*0.02f, Quaternion.FromToRotation(-Vector3.forward, raycastHit.normal));
                    gameObject.name = gridTest.name + "_" + z + "_" + x;
                    gameObject.transform.localScale = Vector3.one*0.8f;
                    /////////////////////////////////////////////////////////////////////
                }
            }
        }
    }
    void SetBitArrays(Vector3 grid, Vector2Int buildSize)
    {
        for (int y = (int)(buildSize.y * .5f); y >-(buildSize.y * .5f); y--)
        {
            for (int x = (int)(buildSize.x * .5f); x >-(buildSize.x * .5f); x--)                                                                                                                                                                                       
            {
                Vector3 _grid = grid - Vector3Int.right *( x) - Vector3Int.forward * (y);
                buildState.Set((int)(_grid.z + MAP_SIZE_Z *.5f) * (int)MAP_SIZE_Z + (int)(_grid.x + MAP_SIZE_X * .5f), false);
                Debug.Log(grid);
                GameObject floor = GameObject.Find("GridTestCube_" + _grid.z + "_" + _grid.x);
                Debug.Log(floor.name);
                floor.GetComponent<MeshRenderer>().material = red;
            }
        }

    }


    void ChangeBuildTarget(TestObstacle target)
    {
        buildTarget = target;
        gridOffset = new Vector3((buildTarget.size.x) % 2 * .5f, 0, (buildTarget.size.y) % 2 * .5f);
        viewer.UpdateTargetChange(target);
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
                result = result && GetBuildEnable(grid - Vector3Int.right * (x) - Vector3Int.forward * (y));
            
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
