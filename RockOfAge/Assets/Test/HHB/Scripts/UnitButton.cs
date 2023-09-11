using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    #region ����
    // �о�� ��ũ���ͺ� ����
    public ObstacleStatus obstacleStatus;
    #region ObstacleScriptable
    /// UnitScriptable ������
    /// id = Id
    /// name = obsName
    /// price = Price
    [HideInInspector]
    public int id;
    [HideInInspector]
    public string obsName;
    [HideInInspector]
    public float price;
    [HideInInspector]
    public string explain;
    #endregion ObstacleScriptable
    // ���� ���� ���
    public GameObject statUI;
    // 
    public GameObject holderButton;
    // ���� �̹���
    private GameObject _selectImage;
    // ���� �̹���
    private GameObject _removeImage;
    // �ڱ� �ڽ��� �̹���
    private Image _image;
    // Ŭ�� �� ����
    private Color _orignialColor;
    private Color _clickedColor;
    #endregion

    public void OnPointerClick(PointerEventData eventData)
    { 
    
    }

    public void OnPointerEnter(PointerEventData eventData)
    { 
    
    
    }

    public void OnPointerExit(PointerEventData eventData)
    { 
    
    }

    //{ PackAwake()
    private void PackAwake()
    {
        _image = GetComponent<Image>();
        _orignialColor = _image.color;
        _clickedColor = new Color(100f / 255f, 100f / 255f, 100f / 255f);
        _selectImage = transform.Find("SelectImage").gameObject;
        _removeImage = transform.Find("RemoveImage").gameObject;
        GetInfoFromScriptObj();
    }
    //} PackAwake()

    //{ PackOnPointerEnter()
    public void PackOnPointerEnter()
    {
        if (ItemManager.itemManager.CheckItemList(id))
        {
            _removeImage.SetActive(true);
        }
        else
        {
            _selectImage.SetActive(true);
        }
        UIManager.uiManager.PrintObstacleCard(id, explain, name, price);
    }
    //} PackOnPointerEnter()

    //{ GetInforFromScriptObj()
    // scriptable ����
    private void GetInfoFromScriptObj()
    {
        if (obstacleStatus != null)
        {
            id = obstacleStatus.Id;
            obsName = obstacleStatus.ObstacleName;
            price = obstacleStatus.Price;
        }
    }
    //} GetInforFromScriptObj()



}
