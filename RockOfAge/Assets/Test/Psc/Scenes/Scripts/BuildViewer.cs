
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
        meshFilter.sharedMesh = target.GetComponent<MeshFilter>().sharedMesh;
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
                float height = bounds.size.y;

                // ��� ���
                Debug.Log("Object Height: " + height);

                return height;
            }
        }

        return 0f;
    }
}
