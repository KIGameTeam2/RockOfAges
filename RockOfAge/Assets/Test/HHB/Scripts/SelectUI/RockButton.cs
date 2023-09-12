using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RockButton : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerClickHandler
{
    #region ����
    // �о�� ��ũ���ͺ� ����
    public RockStatus rockStatus;
    #region RockScriptable
    /// UnitScriptable ������
    /// id = Id
    /// name = StoneName
    /// hp = Health
    /// speed = Speed
    /// acc = Accleration
    /// dmg = Damage
    /// weight = Weight
    /// time = Cooldown
    /// explain = TempString
    [HideInInspector]
    public int id;
    [HideInInspector]
    public string stoneName;
    [HideInInspector]
    public float hp;
    [HideInInspector]
    public float speed;
    [HideInInspector]
    public float acc;
    [HideInInspector]
    public float dmg;
    [HideInInspector]
    public float weight;
    [HideInInspector]
    public float time;
    [HideInInspector]
    public string explain;
    #endregion RockScriptable
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
    // Info Max
    #region Info fillamount �ִ밪
    private float _maxHp = 3000f;
    private float _maxSpeed = 30f;
    private float _maxAcc = 30f;
    private float _maxDmg = 300f;
    private float _maxWeight = 200f;
    #endregion

    // Ŭ���� ���¸� ����ȳ����� �ϴ� bool
    [HideInInspector]
    public bool _isChecked { get; private set;}
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
        // ���ʸ��콺 Ŭ����
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            PackOnPointerClick();
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        PackOnPointerExit();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        PackOnPointerEnter();
    }

    #region Packed
    //{ PackAwake()
    private void PackAwake()
    {
        xPos = -240f;
        yPos = 350f;
        _isChecked = false;
        _image = GetComponent<Image>();
        _orignialColor = _image.color;
        _clickedColor = new Color(100f / 255f, 100f / 255f, 100f / 255f);
        _selectImage = transform.Find("SelectImage").gameObject;
        _removeImage = transform.Find("RemoveImage").gameObject;
        GetInfoFromScriptObj();
    }
    //} PackAwake()

    //{ PackOnPointerExit()
    public void PackOnPointerExit()
    {
        _removeImage.SetActive(false);
        _selectImage.SetActive(false);
        DestroyStatImg();
        DestroyPreviewHolder();
    }
    //} PackOnPointerExit()

    //{ PackOnPointerEnter()
    public void PackOnPointerEnter()
    {
        if (ItemManager.itemManager.CheckItemList(id))
        {
            _removeImage.SetActive(true);
        }
        else
        {
            if (ItemManager.itemManager.CheckUserListCapacity(2) == true)
            { 
                _selectImage.SetActive(true);
            }
        }
        UIManager.uiManager.PrintRockCard(id, name, explain, time);
        if (_isChecked == false)
        {
            if (ItemManager.itemManager.CheckUserListCapacity(2) == true)
            { 
                InstantiatePreviewHolder();
            }
            InstantiateStatImg(hp, speed, acc, dmg, weight);
        }
        
    }
    //} PackOnPointerEnter()

    //{ PackOnPointerClick()
    public void PackOnPointerClick()
    {
        // ������ ������ ���� �ʴٸ�
        if (ItemManager.itemManager.CheckItemList(id) == false)
        {
            if (ItemManager.itemManager.CheckUserListCapacity(2) == true)
            {
                // original -> dark
                _image.color = _clickedColor;
                ItemManager.itemManager.rockSelected.Add(id);
                _isChecked = true;
                InstantiateRockHolder(id);
            }
        }
        else
        // ������ �ִٸ�
        {
            // dark -> orignial
            _image.color = _orignialColor;
            ItemManager.itemManager.rockSelected.Remove(id);
            _isChecked = false;
            ItemManager.itemManager.RockRePrintHolder();
        }
    }
    //} PackOnPointerClick()
    #endregion

    #region Functions

    //{ GetInforFromScriptObj()
    // scriptable ����
    private void GetInfoFromScriptObj()
    {
        if (rockStatus != null)
        {
            id = rockStatus.Id;
            stoneName = rockStatus.StoneName;
            hp = rockStatus.Health;
            speed = rockStatus.Speed;
            acc = rockStatus.Acceleration;
            dmg = rockStatus.Damage;
            weight = rockStatus.Weight;
            time = rockStatus.Cooldown;
            explain = rockStatus.TempString;
        }
    }
    //} GetInforFromScriptObj()

    //{ InstantiateStatImg()
    // Info �̹��� ����
    public void InstantiateStatImg(float hp_, float speed_, float acc_, float dmg_, float weight_)
    {
        GameObject newStatUI = Instantiate(statUI, transform);
        newStatUI.name = "StatImg";
        RectTransform rectTransform = newStatUI.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(20, -70);

        // ���� �ؽ�Ʈ
        TextMeshProUGUI[] textElements = newStatUI.GetComponentsInChildren<TextMeshProUGUI>();
        // ���� �̹���
        Image[] imageElements = newStatUI.GetComponentsInChildren<Image>();

        // text ���
        foreach (TextMeshProUGUI texts in textElements)
        {
            if (texts.name == "HpStatTxt")
            { 
                texts.text = hp_.ToString();
            }
            if (texts.name == "VelStatTxt")
            {
                texts.text = speed_.ToString();
            }
            if (texts.name == "AccStatTxt")
            {
                texts.text = acc_.ToString();
            }
            if (texts.name == "DmgStatTxt")
            {
                texts.text = dmg_.ToString();
            }
            if (texts.name == "WeightStatTxt")
            {
                texts.text = weight_.ToString();
            }
        }

        // fillamount ���
        foreach (Image images in imageElements)
        {
            if (images.name == "HpStatFront")
            {
                images.fillAmount = hp_ / _maxHp;
            }
            if (images.name == "VelStatFront")
            {
                images.fillAmount = speed_ / _maxSpeed;
            }
            if (images.name == "AccStatFront")
            {
                images.fillAmount = acc_ / _maxAcc;
            }
            if (images.name == "DmgStatFront")
            {
                images.fillAmount = dmg_ / _maxDmg;
            }
            if (images.name == "WeightStatFront")
            {
                images.fillAmount = weight_ / _maxWeight;
            }
        }
    }
    //} InstantiateStatImg()

    //{ DestroyStatImg()
    // Info �̹��� ����
    public void DestroyStatImg()
    {
        Transform statUIInstanceTransform = transform.Find("StatImg");
        if (statUIInstanceTransform != null)
        {
            GameObject statUIInstance = statUIInstanceTransform.gameObject;
            Destroy(statUIInstance);
        }
    }
    //} DestroyStatImg()

    //{ InstantiatePreviewHolder()
    // Holder �̸����� - ������ �ν��Ͻ�ȭ
    public void InstantiatePreviewHolder()
    {
        Transform parentTransform = transform.parent;
        GameObject newHolderButton = Instantiate(holderButton, parentTransform);
        newHolderButton.name = "RockHolderPreview";
        Image image = newHolderButton.GetComponent<Image>();
        UIManager.uiManager.MatchHolderIDSprite(image, id);
        image.color = _clickedColor;
        RectTransform rectTransform = newHolderButton.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(xPos + (float)(163.5*ItemManager.itemManager.rockCount), yPos);
    }
    //} InstantiatePreviewHolder()

    //{ DestroyPreviewHolder()
    // Holder �̸����� Destroy
    public void DestroyPreviewHolder()
    {
        GameObject rockHolder = GameObject.Find("RockHolderPreview");
        if (rockHolder != null)
        {
            Destroy(rockHolder);
        }
    }
    //} DestroyPreviewHolder()
    
    //{ InstantiateHolder()
    // Holder ����
    // ���� ������ ItemManager�� ������
    public void InstantiateRockHolder(int id_)
    { 
        Transform parentTransform = transform.parent;
        GameObject newHolderButton = Instantiate(holderButton, parentTransform);
        newHolderButton.name = "RockHolder";
        Image image = newHolderButton.GetComponent<Image>();
        UIManager.uiManager.MatchHolderIDSprite(image, id_);
        RectTransform rectTransform = newHolderButton.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(xPos + (float)(163.5 * ItemManager.itemManager.rockCount), yPos);
        RockHolderButton myHolderButton = FindObjectOfType<RockHolderButton>();
        myHolderButton.id = id_;
        ItemManager.itemManager.rockCount++;
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