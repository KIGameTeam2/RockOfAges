using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;

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
    #endregion

    private void Awake()
    {
        uiManager = this;
    }

    #region Functions
    //{ PrintCard
    public void PrintRockCard(int id_, string name_, string explain_, float time_)
    {
        float maxTime = 0.1f;
        // ���϶�
        if (id_ <= 10)
        {
            cardNameTxt.text = name_;
            cardInfoTxt.text = explain_;
            cardGoldTxt.text = ""; // ��� ����
            cardSandImage.fillAmount = (float)ConvertCoolTimeToEnum(time_) * maxTime;
            MatchIDToSprite(id_);
        }
    }
    //} PrintCard

    //{ PrintObstacleCard()
    // TODO
    public void PrintObstacleCard(int id_, string name_, string explain_, float gold_)
    { 
    
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
            int index = id_ - 10;
            cardImage.sprite = obstructionSprites[index];
        }
    }
    //} MatchIDToSprite()

    public void MatchHolderIDSprite(Image image_, int id_)
    {
        if (id_ < 10)
        {
            int index = id_ - 1;
            image_.sprite = rockHolderSprites[index];
        }
        if (id_ > 10)
        {
            int index = id_ - 10;
            image_.sprite = obstructionSprites[index];
        }
    }


    #endregion
}
