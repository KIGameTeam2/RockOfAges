using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Common
public partial class UIManager: MonoBehaviour
{
    #region ����
    public GameObject commonUI;
    // MŰ �����ð��� bool ����
    private bool _mButtonPressed = false;
    private float _pressedTime = 0f;
    // �÷��̾� �̸�
    public TextMeshProUGUI player1Txt;
    public TextMeshProUGUI player2Txt;
    public TextMeshProUGUI player3Txt;
    public TextMeshProUGUI player4Txt;
    // �÷��̾� ü��
    public Image team1HpImg;
    public Image team2HpImg;
    // �÷��̾� �̹���
    public Image player1Img;
    public Image player2Img;
    public Image player3Img;
    public Image player4Img;
    // �÷��̾� ü��
    public TextMeshProUGUI playerGold;
    #endregion


    #region Functions
    //! ������ �������� �ٲٱ�
    //{ TurnOnCommonUI()
    // commonUI �Ѵ� �Լ�
    public void TurnOnCommonUI()
    {
        commonUI.transform.localScale = Vector3.one;
    }
    //} TurnOnCommonUI()

    //{ GetRotationKey()
    // M 1�� ������ ī�޶� ����
    public void GetRotationKey()
    {
        if (Input.GetKey(KeyCode.M) == true)
        {
            if (_mButtonPressed == false)
            {
                _mButtonPressed = true;
                _pressedTime = Time.time;
            }
        }
        else
        {
            _mButtonPressed = false;
        }

        if (_mButtonPressed && (Time.time - _pressedTime) >= 1f)
        {
            _mButtonPressed = false;
            _pressedTime = 0f;
            RotateMirror();
        }
    }
    //} GetRotationKey()

    //! ����
    //{ RotateMirror()
    // �̷� ī�޶� �ҷ����� �Լ�
    public void RotateMirror()
    {
        // ��밡 �� ������ ���� �� ī�޶� ������ �Բ�(����ó��)
        MirrorRotate mirrorRotate = FindObjectOfType<MirrorRotate>();
        mirrorRotate.RotateMirror();
    }
    //} RotateMirror()

    // ! ����
    //{ PrintPlayerText()
    // �÷��̾� �̸� ����ϴ� �Լ�
    public void PrintPlayerText(string player1_, string player2_, string player3_, string player4_)
    { 
        player1Txt.text = player1_;
        player2Txt.text = player2_;
        player3Txt.text = player3_;
        player4Txt.text = player4_;
    }
    //} PrintPlayerText()

    //! ����
    // �÷��̾� �̹��� ����ϴ� �Լ�
    //{ PrintPlayerImg()
    public void PrintPlayerImg(Image playerImg1_, Image playerImg2_, Image playerImg3_, Image playerImg4_)
    { 
        
    }
    //} PrintPlayerImg()

    //! ����
    //{ PrintTeamHP()
    // ��1 �� ��2�� ü���� ����ϴ� �Լ�
    public void PrintTeamHP()
    {
        //team1HpImg.fillAmount = CycleManager.cycleManager.   /teamMaxHp;
        //team2HpImg.fillAmount = CycleManager.cycleManager.   /teamMaxHp;
    }
    //} PrintTeamHP()

    //! ����
    //{ PrintMyGold()
    public void PrintMyGold()
    {
        //playerGold.text = 
    }
    //} PrintMyGold()
    #endregion
}
