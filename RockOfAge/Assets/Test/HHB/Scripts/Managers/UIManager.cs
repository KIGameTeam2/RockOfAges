using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

#region Enum StoneTimer 
// �������ӵ� ����,����,����
public enum StoneTimer
{
    Fast = 5, Normal = 7, Slow = 10
}
#endregion

// Select UI
public partial class UIManager : MonoBehaviour
{ 
    public static UIManager uiManager;

    #region ����
    // Card
    public Sprite[] rockSprites;
    public Sprite[] obstructionSprites;
    public Image cardImage;
    public Image cardSandImage;
    public Image cardClockImage;
    public TextMeshProUGUI cardNameTxt;
    public TextMeshProUGUI cardInfoTxt;
    public TextMeshProUGUI cardGoldTxt;
    // Holder
    public Sprite[] rockHolderSprites;
    // Selection To Defence
    public TextMeshProUGUI readyTxt;
    public Image readyImg;
    // selectionUI
    public GameObject userSelectUI;
    #endregion

    private void Awake()
    {
        uiManager = this;
        DontDestroyOnLoad(uiManager);
    }

    #region Functions
    //{ PrintCard
    public void PrintRockCard(int id_, string name_, string explain_, float time_)
    {
        float maxTime = 0.1f;
        // ���϶�
        if (id_ <= 10)
        {
            cardClockImage.gameObject.SetActive(true);
            cardSandImage.gameObject.SetActive(true);
            cardNameTxt.text = name_;
            cardInfoTxt.text = explain_;
            cardGoldTxt.text = ""; // ��� ����
            cardSandImage.fillAmount = (float)ConvertCoolTimeToEnum(time_) * maxTime;
            MatchIDToSprite(id_);
        }
    }
    //} PrintCard

    //{ PrintObstacleCard()
    public void PrintObstacleCard(int id_, string name_, string explain_, float gold_)
    {
        // ���ع��϶�
        if (id_ > 10)
        {
            cardNameTxt.text = name_;
            cardInfoTxt.text = explain_;
            cardGoldTxt.text = gold_.ToString();
            cardClockImage.gameObject.SetActive(false);
            cardSandImage.gameObject.SetActive(false);
            MatchIDToSprite(id_);
        }
    }
    //} PrintObstacleCard()

    //{ StoneTimer ConvertCoolTimeToEnum
    public StoneTimer ConvertCoolTimeToEnum(float time_)
    {
        float normalMaxTime = 65f;
        float normalMinTime = 55f;

        // ����
        if (time_ > normalMaxTime)
        {
            return StoneTimer.Fast;
        }
        // ����
        else if (time_ >= normalMinTime && time_ <= normalMaxTime)
        {
            return StoneTimer.Normal;
        }
        // ����
        else
        {
            return StoneTimer.Slow;
        }
    }
    //} StoneTimer ConvertCoolTimeToEnum

    //{ MatchIDToSprite()
    public void MatchIDToSprite(int id_)
    {
        if (id_ < 10)
        {
            int index = id_ - 1;
            cardImage.sprite = rockSprites[index];
        }
        if (id_ > 10)
        {
            int index = id_ - 11;
            cardImage.sprite = obstructionSprites[index];
        }
    }
    //} MatchIDToSprite()

    //{ MatchHolderIDSprite()
    public void MatchHolderIDSprite(Image image_, int id_)
    {
        if (id_ < 10)
        {
            int index = id_ - 1;
            image_.sprite = rockHolderSprites[index];
        }
        if (id_ >= 11)
        {
            int index = id_ - 11;
            image_.sprite = obstructionSprites[index];
        }
    }
    //} MatchHolderIDSprite()

    //{ PrintReadyText()
    public void PrintReadyText()
    {
        readyTxt.text = "�غ�!";
        readyImg.gameObject.SetActive(true);
    }
    //} PrintReadyText()

    //{ PrintReadyText()
    public void PrintNotReadyText()
    {
        readyTxt.text = "�غ���";
        readyImg.gameObject.SetActive(false);
    }
    //} PrintReadyText()


    //{ ShutDownUserSelectUI()
    public void ShutDownUserSelectUI()
    {
        userSelectUI.SetActive(false);
    }
    //} ShutDownUserSelectUI()
    #endregion
}
