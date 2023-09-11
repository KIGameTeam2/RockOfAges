
using UnityEngine;

public class BuildViewer : MonoBehaviour
{
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void ChangeTarget(GameObject target)
    {
        MeshFilter _meshFilter = target.GetComponent<MeshFilter>();
        if (_meshFilter == null)
        {
            Debug.Assert(_meshFilter == null, "Ÿ�� �޽����� ����");
            return;
        }

        // sourceMeshFilter���� ���� Mesh�� �����ɴϴ�.
        Mesh sourceMesh = _meshFilter.sharedMesh;

        // ���ο� Mesh�� �����ϰ� ���� Mesh�� �����մϴ�.
        Mesh copyMesh = new Mesh();
        copyMesh.vertices = sourceMesh.vertices;
        copyMesh.triangles = sourceMesh.triangles;
        copyMesh.normals = sourceMesh.normals;
        copyMesh.uv = sourceMesh.uv;

        // ����� Mesh�� ���ο� MeshFilter�� �Ҵ��մϴ�.
        meshFilter.sharedMesh = copyMesh;
        HideViewer();
    }

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
