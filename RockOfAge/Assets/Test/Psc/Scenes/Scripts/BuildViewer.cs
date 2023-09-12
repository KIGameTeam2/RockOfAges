
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildViewer : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    private BuildHighLight highLight;
    private BuildColorHighLight colorHighLight;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        highLight = GetComponentInChildren<BuildHighLight>();
        colorHighLight = GetComponentInChildren<BuildColorHighLight>();
    }

    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!���� target�� class�� ObstacleBase�� �����Ұ�
    void ChangeTarget(GameObject target)
    {
        MeshFilter _meshFilter = target.GetComponent<MeshFilter>();
        if (_meshFilter == null)
        {
            Debug.Assert(_meshFilter == null, "Ÿ�� �޽����� ����");
            return;
        }

        // sourceMeshFilter���� ���� Mesh�� ����
        Mesh sourceMesh = _meshFilter.sharedMesh;

        // ���ο� Mesh�� �����ϰ� ���� Mesh�� ����
        Mesh copyMesh = new Mesh();
        copyMesh.vertices = sourceMesh.vertices;
        copyMesh.triangles = sourceMesh.triangles;
        copyMesh.normals = sourceMesh.normals;
        copyMesh.uv = sourceMesh.uv;

        // ����� Mesh�� ���ο� MeshFilter�� �Ҵ�
        meshFilter.sharedMesh = copyMesh;

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!���� ��Ȳ�� �°� �����Ұ�.
        TestObstacle _target = target.GetComponent<TestObstacle>();
        highLight.ChangeHighLight(_target.size);
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        HideViewer();
    }


    public void UpdateMouseMove(bool canBuild)
    {
        colorHighLight.UpdateColorHighLightColor(canBuild);
    }

    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!���� obstacleBase�� �ٲܰ�
    public void UpdateTargetChange(TestObstacle target)
    {
        colorHighLight.UpdateColorHighLightSize(target.size);
        ChangeTarget(target.gameObject);
    }
    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!


    public float GetHeight()
    {
        if (meshFilter != null)
        {
            // �޽� ������ ��������
            Mesh mesh = meshFilter.sharedMesh;

            if (mesh != null)
            {
                // �޽��� ��� ���� (Bounds) ��������
                Bounds bounds = mesh.bounds;

                // ���� ���
                float height = bounds.size.y * .1f;

                // ��� ���
                //Debug.Log("Object Height: " + height);

                return height;
            }
        }

        return 0f;
    }

    public void HideViewer()
    {
        transform.localScale = Vector3.zero;
    }
    public void HideViewer(bool enable)
    {
        if (enable)
        {
            transform.localScale = Vector3.zero;
        }
        else
        {
            transform.localScale = Vector3.one * .1f;
        }
    }


    public void ShowViewer()
    {
        transform.localScale = Vector3.one * .1f;
    }
    public void ShowViewer(bool enable)
    {
        if (enable)
        {
            transform.localScale = Vector3.one * .1f;
        }
        else
        {
            transform.localScale = Vector3.zero;
        }
    }

}
