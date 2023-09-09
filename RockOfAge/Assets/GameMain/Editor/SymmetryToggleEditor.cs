using UnityEditor;
using UnityEngine;

public class SymmetryToggleEditor : EditorWindow
{
    private bool isSymmetryEnabled = false;
    private bool symmetryX = true;
    private bool symmetryY = true;
    private bool symmetryZ = true;
    private bool rotateX = true;
    private bool rotateY = true;

    [MenuItem("Tools/Toggle Symmetry")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<SymmetryToggleEditor>("Symmetry Toggle");
    }

    private void OnGUI()
    {
        GUILayout.Label("Symmetry Toggle", EditorStyles.boldLabel);

        isSymmetryEnabled = EditorGUILayout.Toggle("Enable Symmetry", isSymmetryEnabled);
        symmetryX = EditorGUILayout.Toggle("Symmetry X", symmetryX);
        symmetryY = EditorGUILayout.Toggle("Symmetry Y", symmetryY);
        symmetryZ = EditorGUILayout.Toggle("Symmetry Z", symmetryZ);
        rotateX = EditorGUILayout.Toggle("Rotate X", rotateX);
        rotateY = EditorGUILayout.Toggle("Rotate Y", rotateY);

        if (GUILayout.Button("Apply Symmetry"))
        {
            ApplySymmetry();
        }
    }

    private void ApplySymmetry()
    {
        if (Selection.gameObjects != null && Selection.gameObjects.Length > 0)
        {
            foreach (GameObject selectedObject in Selection.gameObjects)
            {
                Vector3 position = selectedObject.transform.position;
                Vector3 scale = selectedObject.transform.localScale;

                if (isSymmetryEnabled)
                {
                    // ��Ī ��ġ ���
                    Vector3 symmetryPosition = position;
                    if (symmetryX)
                    {
                        symmetryPosition.x *= -1; // X ���� �������� ��Ī
                    }
                    if (symmetryY)
                    {
                        symmetryPosition.y *= -1; // Y ���� �������� ��Ī
                    }
                    if (symmetryZ)
                    {
                        symmetryPosition.z *= -1; // Z ���� �������� ��Ī
                    }

                    // ������Ʈ ���� �� ��Ī ��ġ�� ��ġ
                    GameObject newObject = Instantiate(selectedObject, symmetryPosition, selectedObject.transform.rotation);
                    newObject.transform.localScale = scale;

                    // ���� ȸ���� ����
                    Vector3 localRotation = newObject.transform.localEulerAngles;
                    if (rotateX)
                    {
                        localRotation.x = (localRotation.x > 180) ? localRotation.x - 180 : localRotation.x + 180;
                    }
                    if (rotateY)
                    {
                        localRotation.y = (localRotation.y > 180) ? localRotation.y - 180 : localRotation.y + 180;
                    }
                    newObject.transform.localEulerAngles = localRotation;

                    // ���� ������ ������Ʈ�� ����
                    Selection.activeGameObject = newObject;
                }
            }
        }
    }
}