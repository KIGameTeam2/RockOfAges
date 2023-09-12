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
    [HideInInspector]
    public bool _isChecked { get; private set; }
    // holderButton ���� x,y��ǥ
    [HideInInspector]
    public float xPos;
    [HideInInspector]
    public float yPos;
    #endregion

    private void Awake()
    {
        PackAwake();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PackOnPointerClick();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PackOnPointerEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PackOnPointerExit();
    }

    #region Packed
    //{ PackAwake()
    private void PackAwake()
    {
        xPos = 298f;
        yPos = 405f;
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
            if (ItemManager.itemManager.CheckUserListCapacity(1) == true)
            {
                _selectImage.SetActive(true);
            }
        }
        UIManager.uiManager.PrintObstacleCard(id, name, explain, price);
        if (_isChecked == false)
        {
            if (ItemManager.itemManager.CheckUserListCapacity(1) == true)
            { 
                InstantiatePreviewHolder();
            }
        }
    }
    //} PackOnPointerEnter()

    //{ PackOnPointerExit()
    public void PackOnPointerExit()
    {
        _removeImage.SetActive(false);
        _selectImage.SetActive(false);
        DestroyPreviewHolder();
    }
    //} PackOnPointerExit()

    //{ PackOnPointerClick()
    public void PackOnPointerClick()
    {
        // ������ ������ ���� �ʴٸ�
        if (ItemManager.itemManager.CheckItemList(id) == false)
        {
            if (ItemManager.itemManager.CheckUserListCapacity(1) == true)
            {
                // original -> dark
                _image.color = _clickedColor;
                ItemManager.itemManager.unitSelected.Add(id);
                _isChecked = true;
                InstantiateUnitHolder(id);
            }
        }
        else
        // ������ �ִٸ�
        {
            // dark -> orignial
            _image.color = _orignialColor;
            ItemManager.itemManager.unitSelected.Remove(id);
            _isChecked = false;
            ItemManager.itemManager.UnitRePrintHolder();
        }
    }
    //} PackOnPointerClick()
    #endregion

    #region Functions
    //{ GetInforFromScriptObj()
    // scriptable ����
    private void GetInfoFromScriptObj()
    {
        if (obstacleStatus != null)
        {
            id = obstacleStatus.Id;
            obsName = obstacleStatus.ObstacleName;
            price = obstacleStatus.Price;
            explain = obstacleStatus.TempString;
        }
    }
    //} GetInforFromScriptObj()

    //{ InstantiatePreviewHolder()
    // Holder �̸����� - ������ �ν��Ͻ�ȭ
    public void InstantiatePreviewHolder()
    {
        Transform parentTransform = transform.parent;
        GameObject newHolderButton = Instantiate(holderButton, parentTransform);
        newHolderButton.name = "UnitHolderPreview";
        Image image = newHolderButton.GetComponent<Image>();
        UIManager.uiManager.MatchHolderIDSprite(image, id);
        image.color = _clickedColor;
        RectTransform rectTransform = newHolderButton.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(xPos - (float)(84 * ItemManager.itemManager.unitCount), yPos);
    }
    //} InstantiatePreviewHolder()

    //{ DestroyPreviewHolder()
    // Holder �̸����� Destroy
    public void DestroyPreviewHolder()
    {
        GameObject unitHolder = GameObject.Find("UnitHolderPreview");
        if (unitHolder != null)
        {
            Destroy(unitHolder);
        }
    }
    //} DestroyPreviewHolder()

    //{ InstantiateHolder()
    // Holder ����
    // ���� ������ ItemManager�� ������
    public void InstantiateUnitHolder(int id_)
    {
        Transform parentTransform = transform.parent;
        GameObject newHolderButton = Instantiate(holderButton, parentTransform);
        newHolderButton.name = "UnitHolder";
        Image image = newHolderButton.GetComponent<Image>();
        UIManager.uiManager.MatchHolderIDSprite(image, id_);
        RectTransform rectTransform = newHolderButton.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(xPos - (float)(84 * ItemManager.itemManager.unitCount), yPos);
        UnitHolderButton myHolderButton = FindObjectOfType<UnitHolderButton>();
        myHolderButton.unitId = id_;
        ItemManager.itemManager.unitCount++;
    }
    //} InstantiateHolder()

    //{ BackToOriginalColor()
    // Holder�� Ŭ���� ������� ���� �ǵ����� �Լ�
    public void BackToOriginalColor(int id_)
    {
        // ������ �������� ������ ���� �ʴٸ�
        if (ItemManager.itemManager.CheckItemList(id_) == false)
        {
            _image.color = _orignialColor;
        }
    }
    //} BackToOriginalColor()

    #endregion
}
