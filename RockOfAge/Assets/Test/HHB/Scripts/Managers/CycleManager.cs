using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleManager : MonoBehaviour
{
    public bool selection = true;
    public bool attack = false;
    public bool defence = false;
    public bool isGameOver = false;

    private void Update()
    {
        UpdateSelectionCycle();
        UpdateCommonUICycle();
    }

    //{ UpdateSelectionCycle()
    // ���� ����Ŭ
    // ���ϳ� �̻� ���� & ���� enter -> defence
    public void UpdateSelectionCycle()
    {
        if (selection == true)
        {
            // �� �ϳ� �̻� ���ý� ������� ����
            if (CheckUserBall() == true)
            {
                UIManager.uiManager.PrintReadyText();
            }
            else { UIManager.uiManager.PrintNotReadyText(); }
            // ���ʹ�����
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                // ���� �� ���� ����Ŭ��
                if (CheckUserBall() == true)
                {
                    selection = false;
                    defence = true;
                    UIManager.uiManager.ShutDownUserSelectUI();
                }
                else { return; }
            }
        }
    }
    //} UpdateSelectionCycle()

    //{ CheckUserBall()
    // ���� �ϳ� �̻� ���õǾ����� �����ϴ� �Լ�
    public bool CheckUserBall()
    {
        if (ItemManager.itemManager.rockSelected.Count >= 1)
        {
            return true;
        }
        else { return false; }
    }
    //} CheckUserBall()

    //{ UpdateDefenceCycle()
    // ��������ñ��� ���� �ִ� UI
    public void UpdateCommonUICycle()
    {
        if (isGameOver == false && selection == false)
        { 
            UIManager.uiManager.TurnOnCommonUI();
            UIManager.uiManager.GetRotationKey();        
        }
    }
    //} UpdateDefenceCycle()


}
