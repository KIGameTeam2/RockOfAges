using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class SplineMeshBuilder : MonoBehaviour
{
    //build�ÿ� ����� material
    public List<Material> materials = default;

    //spline�� ���ؼ� ���� side�� ����
    List<Vector3> rightPoint = default;
    List<Vector3> leftPoint = default;

    //spline�� ������ ������ �ִ� container
    //�ڵ����� �ҷ���
    private SplineContainer[] splineContainer = default;

    //������ spline ���� 
    [SerializeField]
    private int resolution = 100;
    //������ ����
    [SerializeField]
    private float height = 4f;
    //ground�� ���� ��(���� ũ��� m_width*2)
    [SerializeField]
    private float width = 3f;

    //�÷����� ���� �� ����
    //�ϴ� �׽�Ʈ������ 1���� ����
    public const int TEAM_COUNT = 2;


    private void Awake()
    {
        //�� ���ڸ�ŭ �迭 ũ�� ����
        splineContainer = new SplineContainer[TEAM_COUNT];

        //�� ���ڸ�ŭ for��
        //�ش� for���� ���� ��ܿ������� �˻��� splineContainer�� �����´�
        for(int teamIndex = 0; teamIndex < TEAM_COUNT; teamIndex++)
        {
            splineContainer[teamIndex] = GameObject.Find("Team"+(teamIndex + 1)).transform.Find("BaseTerrains").GetComponentInChildren<SplineContainer>();
        }
    }

    void Start()
    {
        //���۽� ���� ����.
        //�Ƹ� ���Ŀ��� �ش� Ŭ���� ��ü�� editor�������� ����Ǽ� �� object�� ����/������ ����Ұ�
        for(int teamIndex = 0; teamIndex < TEAM_COUNT; teamIndex++)
        {
            Generate(teamIndex);
        }
    }

    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!�׽�Ʈ �ڵ�
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Generate(0);
        }
    }
    /// //////////////////////////////////////////////

    void Generate(int teamIndex)
    {
        //spline���� ��������ŭ �����Ѵ�.
        for (int splinceIndex = 0; splinceIndex < splineContainer[teamIndex].Splines.Count; splinceIndex++)
        {
            //������������ ���� �θ�Ŭ���� ����
            GameObject partsParent = new GameObject();

            //������ �ʱ�ȭ
            partsParent.name = "SplineParts_" + splinceIndex;
            Global_PSC.InitLocalTransformData(partsParent.transform, splineContainer[teamIndex].transform.parent);

            //����
            Build(splineContainer[teamIndex].Splines[splinceIndex], partsParent);
        }

    }


    //������ �ܰ踦 ������.
    //1. ���� ���ϱ�
    //2. �ش� �޽��� ������ Ŭ���� ����
    //3. �޽� �����
    void Build(Spline spline, GameObject parent)
    {
        //1. ���� ���ϱ�
        GetVerts(spline);

        //2. �ش� �޽��� ������ Ŭ���� ����
        GameObject up = ConcreateGameObject(materials[0], parent);
        //3. �޽������
        BuildUp(up, spline.Closed);

        GameObject right = ConcreateGameObject(materials[1], parent);
        BuildSide(right, rightPoint, spline.Closed);

        GameObject left = ConcreateGameObject(materials[1], parent);
        //culling�� ���ؼ� ���� ����
        leftPoint.Reverse();
        BuildSide(left, leftPoint, spline.Closed);

        //�ε� �Ϸ��
        BuildManager.instance.InitTerrainData();
    }

    //spline�� �����͸� ������� ���� ����
    void GetVerts(Spline spline)
    {
        // ������/���� ������ ������ list ���� ����
        rightPoint = new List<Vector3>();
        leftPoint = new List<Vector3>();

        // ������ ������ ������ ���Ѵ�.
        int splineCount = spline.Count;
        if (resolution < splineCount)
        {
            splineCount = resolution;
        }
        float step = (splineCount / (float)resolution);

        //�� ���ݸ����� ���� ����
        for (int i = 0; i < resolution; i++)
        {
            float t = step * i;

            //�ش� spline�� ������(t)�� ���� ������ �����´�.
            SampleSplineWidth(spline, t, out Vector3 tmpRightPoint, out Vector3 tmpLeftPoint);

            //������ ������ ����
            rightPoint.Add(tmpRightPoint);
            leftPoint.Add(tmpLeftPoint);
        }
    }

    //�ش� spline�� ������(time)�� ���� ������ �����´�.
    public void SampleSplineWidth(Spline spline, float time, out Vector3 rightPoint, out Vector3 leftPoint)
    {
        //Evaluate�� ����� ������
        float3 position;
        float3 forward;
        float3 up;

        //������(time)�� ���� spline�� ��ġ�� �����´�.
        spline.Evaluate(time, out position, out forward, out up);

        //float3->vector3
        Vector3 positionVector = position;

        //������(up)�� �չ���(forward)�� ����(cross,������)�� ���Ѵ�. 
        Vector3 right = Vector3.Cross(forward, up).normalized;

        //���� spline�� ��ġ���� ���������� width��ŭ ������ point�� ����ؼ� ����.
        rightPoint = positionVector + (right * width);
        leftPoint = positionVector + (-right * width);
    }

    //�޽��� ������ object ����
    GameObject ConcreateGameObject(Material material, GameObject parent)
    {
        //mesh�� ���õ� component �ٿ��ֱ�
        GameObject result = new GameObject();
        result.AddComponent<MeshFilter>();
        result.AddComponent<MeshRenderer>().material = material;
        result.AddComponent<MeshCollider>();

        //�ش� object�� local���� �ʱ�ȭ
        Global_PSC.InitLocalTransformData(result.transform, parent.transform);
        result.layer = LayerMask.NameToLayer("Terrains");

        return result;
    }


    //���� mesh����(��)
    //right�� left�� point ������ ��� ����Ѵ�.
    void BuildUp(GameObject terrain, bool isClosed)
    {
        MeshFilter meshFilter = terrain.GetComponent<MeshFilter>();
        MeshCollider meshCollider = terrain.GetComponent<MeshCollider>();

        Mesh mesh = new Mesh();
     
        //mesh�� ��������� vertic�� triangle�� ������ ������ ���� ����
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();

        //triangle�� ������ ǥ������ offset��
        int offset = 0;

        //left�� right ���߿� ���� ���� �������.
        //�������� ���� ���� ����
        int length = leftPoint.Count;

        //��� ����
        for(int i = 1; i <=length; i++)
        {
            //���� point�� ���� point�� �����´�.
            Vector3 p1 = rightPoint[i - 1];
            Vector3 p2 = leftPoint[i - 1];
            Vector3 p3;
            Vector3 p4;

            //���� ������ point�� ���
            if (i == length)
            {
                //loop�� �ƴ϶�� ������.
                if (!isClosed)
                {
                    break;
                }
                //�ƴ϶�� ù point�� �����´�.
                p3 = rightPoint[0];
                p4 = leftPoint[0];
            }
            else
            { 
                p3 = rightPoint[i];
                p4 = leftPoint[i];
            }

            //�⺻������ �ش� mesh�� ����̸�, ���������� �����ȴ�.(���� ������ ������ �ʿ䰡����.)
            //���� ��ǥ�� �ߺ� ����ϱ� ������ triangle�� �����Ҷ� ���� ��ǥ�� ��������ʰ� offset��ŭ �̵��ؼ� �����ص��ȴ�. 

            offset = 4 * (i - 1);

            //������� ������ ������ ����.
            //left  right
            //1     0
            //3/5   2/4
            //7/9   6/8
            //11    10

            //�ð�������� ����
            int t1 = offset + 0;
            int t2 = offset + 2;
            int t3 = offset + 3;

            //�ð�������� ����
            int t4 = offset + 3;
            int t5 = offset + 1;
            int t6 = offset + 0;

            //list��� �߰��Ѵ�.
            verts.AddRange(new List<Vector3> { p1, p2, p3, p4 });
            tris.AddRange(new List<int> { t1,t2,t3,t4,t5,t6 });

        }

        //mesh�� �ִ´�.
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();

        //bound�� normal �籸��
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        //mesh �ִ´�.
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

    }


    //���� mesh����(��)
    //right�� left�� point ������ �ϳ��� ����Ѵ�.(height��ŭ ����������.)
    //��ü���� ������ ���� �����Ҷ��� ������ ����.
    void BuildSide(GameObject terrain, List<Vector3> point, bool isClosed)
    {
        MeshFilter meshFilter = terrain.GetComponent<MeshFilter>();
        MeshCollider meshCollider = terrain.GetComponent<MeshCollider>();

        Mesh mesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();

        int offset = 0;

        int length = point.Count;

        for (int i = 1; i <= length; i++)
        {
            Vector3 p1 = point[i-1]-Vector3.up* height;
            Vector3 p2 = point[i-1];
            Vector3 p3;
            Vector3 p4;

            if (i == length)
            {
                if (!isClosed)
                {
                    break;
                }
                p3 = point[0] - Vector3.up * height;
                p4 = point[0];
            }
            else
            {
                p3 = point[i] - Vector3.up * height;
                p4 = point[i];
            }

            offset = 4 * (i - 1);

            int t1 = offset + 0;
            int t2 = offset + 2;
            int t3 = offset + 3;


            int t4 = offset + 3;
            int t5 = offset + 1;
            int t6 = offset + 0;

            verts.AddRange(new List<Vector3> { p1, p2, p3, p4 });
            tris.AddRange(new List<int> { t1, t2, t3, t4, t5, t6 });

        }
        

        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

    }

}
