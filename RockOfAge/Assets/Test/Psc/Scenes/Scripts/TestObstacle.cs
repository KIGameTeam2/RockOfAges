

using UnityEngine;

public class TestObstacle : MonoBehaviour
{
    public Vector2Int size = default;
    private MeshFilter meshFilter;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    public TestObstacle Build(Vector3 position, Quaternion rotate)
    {
        TestObstacle obstacle = Instantiate(this, position+Vector3.up* GetHeight(), rotate);
        obstacle.transform.localScale = Vector3.one * .1f;

        return obstacle;
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
}
