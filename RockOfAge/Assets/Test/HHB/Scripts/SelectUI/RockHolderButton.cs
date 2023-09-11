using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RockHolderButton : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerClickHandler
{
    #region ����
    // ���� �̹���
    private GameObject _removeImage;
    // RockButton���� ���� ID
    [HideInInspector]
    public int id;
    // �ڱ� �ڽ��� �̹���
    private Image _image;
    // Ŭ�� �� ����
    private Color _orignialColor;
    private Color _clickedColor;
    #endregion

    private void Awake()
    {
        PackAwake();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PackOnPointerExit();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        PackOnPointerEnter();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        PackOnPointerClick();
    }

    #region Packed
    //{ PackAwake()
    public void PackAwake()
    {
        _image = GetComponent<Image>();
        _orignialColor = _image.color;
        _clickedColor = new Color(100f / 255f, 100f / 255f, 100f / 255f);
        _removeImage = transform.Find("RemoveImage").gameObject;
    }
    //} PackAwake()

    //{ PackOnPointerExit()
    public void PackOnPointerExit()
    {
        _image.color = _orignialColor;
        _removeImage.SetActive(false);
    }
    //} PackOnPointerExit()

    //{ PackOnPointerEnter()
    public void PackOnPointerEnter()
    {
        _image.color = _clickedColor;
        if (ItemManager.itemManager.CheckItemList(id))
        {
            _removeImage.SetActive(true);
        }
    }
    //} PackOnPointerEnter()

    //{ PackOnPointerClick()
    public void PackOnPointerClick()
    {
        ItemManager.itemManager.rockSelected.Remove(id);
        ItemManager.itemManager.RockRePrintHolder();
        RockButton[] rockButtons = FindObjectsOfType<RockButton>();
        foreach (RockButton rockButton in rockButtons)
        {
            if (rockButton.id == id)
            {
                rockButton.BackToOriginalColor(id);
            }
        }
    }
    //} PackOnPointerClick()
    #endregion

}
