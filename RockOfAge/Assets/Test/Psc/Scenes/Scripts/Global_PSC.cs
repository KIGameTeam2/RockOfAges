using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Global_PSC
{
    public static void InitLocalTransformData(Transform _GameObject, Transform parent=null)
    {
        _GameObject.parent = parent.transform;
        _GameObject.localPosition = Vector3.zero;
        _GameObject.localRotation = Quaternion.identity;
        _GameObject.localScale = Vector3.one;
    }
    public static int FindLayerToName(string layerName)
    {

        int layerIndex = LayerMask.NameToLayer(layerName);

        if (layerIndex == -1)
        {
            Debug.LogWarning("���̾� " + layerName + "�� ã�� �� �����ϴ�.");
            return FindLayerToName("Default");
        }
        
        return 1 << layerIndex;
    }

    public static Vector3 GetWorldMousePositionFromMainCamera(float depth)
    {
        Vector3 mouseCurrPos = Input.mousePosition;
        mouseCurrPos.z = depth;
        return Camera.main.ScreenToWorldPoint(mouseCurrPos);

    }
    public static Vector3 GetWorldMousePositionFromMainCamera()
    {
        // ī�޶�
        Camera mainCamera = Camera.main;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, 1000, Global_PSC.FindLayerToName("Terrains")))
        {
            return raycastHit.point;
        }

        // ��ȯ�մϴ�.
        return Vector3.zero ;

    }
}

