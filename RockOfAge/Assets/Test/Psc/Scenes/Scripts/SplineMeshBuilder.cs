using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class SplineMeshBuilder : MonoBehaviour
{
    List<Vector3> rightPoint;
    List<Vector3> leftPoint;

    [SerializeField]
    int resolution = 100;
    [SerializeField]
    float height = 4f;

    GameObject upTerrains;
    GameObject leftTerrains;
    GameObject rightTerrains;

    //spline�� ������ ������ �ִ� container
    private SplineContainer[] splineContainer;

    public const int TEAM_COUNT = 1;

    //ground�� ���� ��(6��)
    [SerializeField]
    private float m_width = 3f;

    private void Awake()
    {
        //�� ���ڸ�ŭ �迭 ũ�� ����
        splineContainer = new SplineContainer[TEAM_COUNT];

        //�� ���ڸ�ŭ for��
        //�ش� for���� ���� ��ܿ������� �˻��� splineContainer�� �����´�
        for(int teamIndex = 0; teamIndex < TEAM_COUNT; teamIndex++)
        {
            splineContainer[teamIndex] = GameObject.Find("Team"+(teamIndex + 1)).transform.Find("BaseTerrains").GetComponent<SplineContainer>();
        }
    }

    void Start()
    {
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
        for (int splinceIndex = 0; splinceIndex < splineContainer[teamIndex].Splines.Count; splinceIndex++)
        {
            GameObject parent = new GameObject();
            parent.name = "SplineParts_" + splinceIndex;
            parent.transform.parent = splineContainer[teamIndex].transform;
            Build(splineContainer[teamIndex].Splines[splinceIndex], parent);
        }

    }

    void GetVerts(Spline spline)
    {
        rightPoint = new List<Vector3>();
        leftPoint = new List<Vector3>();
        int splineCount = spline.Count;
        if (resolution < splineCount)
        {
            splineCount = resolution;
        }

        float step = (1 / (float)resolution) * splineCount;
        for (int i = 0; i < resolution; i++)
        {
            float t = step * i;
            SampleSplineWidth(spline, t, out Vector3 vv1, out Vector3 vv2);
            rightPoint.Add(vv1);
            leftPoint.Add(vv2);
        }
    }

    void Build(Spline spline, GameObject parent)
    {
        GetVerts(spline);

        GameObject up = ConcreateGameObject(null);
        up.transform.parent = parent.transform;
        BuildUp(up, spline.Closed);

        GameObject right = ConcreateGameObject(null);
        right.transform.parent = parent.transform;
        BuildSide(right, rightPoint, spline.Closed);

        GameObject left = ConcreateGameObject(null);
        left.transform.parent = parent.transform;
        leftPoint.Reverse();
        BuildSide(left, leftPoint, spline.Closed);
    }

    GameObject ConcreateGameObject(Material material)
    {
        GameObject result = new GameObject();
        result.AddComponent<MeshFilter>();
        result.AddComponent<MeshRenderer>().material = material;
        result.AddComponent<MeshCollider>();

        return result;
    }

    void BuildUp(GameObject gameObject, bool isClosed)
    {
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();

        Mesh mesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();

        int offset = 0;

        int length = leftPoint.Count;

        for(int i = 1; i <=length; i++)
        {
            Vector3 p1 = rightPoint[i - 1];
            Vector3 p2 = leftPoint[i - 1];
            Vector3 p3;
            Vector3 p4;

            if (i == length)
            {
                if (!isClosed)
                {
                    break;
                }
                p3 = rightPoint[0];
                p4 = leftPoint[0];
            }
            else
            {
                p3 = rightPoint[i];
                p4 = leftPoint[i];
            }

            offset = 4 * (i - 1);

            int t1 = offset + 0;
            int t2 = offset + 2;
            int t3 = offset + 3;


            int t4 = offset + 3;
            int t5 = offset + 1;
            int t6 = offset + 0;

            verts.AddRange(new List<Vector3> { p1, p2, p3, p4 });
            tris.AddRange(new List<int> { t1,t2,t3,t4,t5,t6 });

        }


        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;

    }

        
    void BuildSide(GameObject gameObject, List<Vector3> point, bool isClosed)
    {
        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();

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

    public void SampleSplineWidth(Spline spline, float time, out Vector3 rightPoint, out Vector3 leftPoint)
    {
        //evaout�� ����� ������
        float3 position;
        float3 tangent;
        float3 upVector;

        //time�� spline�� ��ġ�� �����´�.
        spline.Evaluate(time, out position, out tangent, out upVector);

        Vector3 _position = position;
        Vector3 right = Vector3.Cross(tangent, upVector).normalized;

        rightPoint = _position + (right * m_width);
        leftPoint = _position + (-right * m_width);
    }

}
