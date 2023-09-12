using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class BuildHighLight : MonoBehaviour
{
    Dictionary<string, GameObject> highLight;
    GameObject currHighLight = null;
    // Start is called before the first frame update

    void Awake()
    {
        highLight = new Dictionary<string, GameObject>();
        foreach(var child in GetComponentsInChildren<MeshRenderer>())
        {
            highLight.Add(child.name, child.gameObject);
            child.gameObject.SetActive(false);
        }

    }

    //��� highlight��Ȱ��ȭ
    private void HideAllHighLight()
    {
        foreach (var child in highLight)
        {
            child.Value.SetActive(false);
        }
    }


    //���̶���Ʈ ����(�������� �����Ѵ�.(BuildViewer))
    public void ChangeHighLight(Vector2 size)
    {
        string key = "Highlight_" + size.x + "X" + size.y;
        ChangeHighLight(key);
    }
    public void ChangeHighLight(HighLightSize size)
    {
        string key = size.ToString();
        ChangeHighLight(key);
    }
    public void ChangeHighLight(string highLightName)
    {
        HideCurrHighLight();
        if (!highLight.ContainsKey(highLightName))
        {
            Debug.LogWarning(highLightName);
            Debug.Assert(false, "����� �´� ũ���� highlight�� ����");
            return;
        }
        currHighLight = highLight[highLightName];
        ShowCurrHighLight();
    }



    public void ShowCurrHighLight()
    {
        if (currHighLight != null)
        {
            currHighLight.SetActive(true);
        }
    }
    public void HideCurrHighLight()
    {
        if (currHighLight != null)
        {
            currHighLight.SetActive(false);
        }
    }
}

public enum HighLightSize
{
    Highlight_1X1 = 1,
    Highlight_2X2 = 2,
    Highlight_3X3 = 3
}
