using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UnitHolderButton : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerClickHandler
{
    #region ����
    // ���� �̹���
    private GameObject _removeImage;
    // UnitButton���� ���� ID
    [HideInInspector]
    public int unitId;
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
    private void PackAwake()
    {
        _image = GetComponent<Image>();
        _orignialColor = _image.color;
        _clickedColor = new Color(100f / 255f, 100f / 255f, 100f / 255f);
        _removeImage = transform.Find("RemoveImage").gameObject;
    }
    //} PackAwake()

    //{ PackOnPointerExit()
    private void PackOnPointerExit()
    {
        _image.color = _orignialColor;
        _removeImage.SetActive(false);
    }
    //} PackOnPointerExit()

    //{ PackOnPointerEnter()
    private void PackOnPointerEnter()
    {
        _image.color = _clickedColor;
        if (ItemManager.itemManager.CheckItemList(unitId))
        {
            _removeImage.SetActive(true);
        }
    }
    //} PackOnPointerEnter()

    //{ PackOnPointerClick()
    private void PackOnPointerClick()
    {
        ItemManager.itemManager.unitSelected.Remove(unitId);
        ItemManager.itemManager.UnitRePrintHolder();
        UnitButton[] unitButtons = FindObjectsOfType<UnitButton>();
        foreach (UnitButton unitButton in unitButtons)
        {
            if (unitButton.id == unitId)
            {
                unitButton.BackToOriginalColor(unitId);
            }
        }
    }
    //} PackOnPointerClick()
    #endregion
}
