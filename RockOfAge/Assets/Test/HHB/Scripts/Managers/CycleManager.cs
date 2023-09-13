using PlayFab.GroupsModels;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public enum UserState
{ 
    UnitSelect = 0, RockSelect = 1, Defence = 2, Attack = 3, Ending = 4 
}

public class CycleManager : MonoBehaviour
{
    public static CycleManager cycleManager;

    #region ����
    public int userState;
    // ���ݿ��� ���� ���õ� bool
    public bool attackRockSelected = false;
    // �� ���� �ð��� �� ������� �����ϴ� bool
    public bool isRockCreated = false;
    ////! ���� team1 team2 ü��
    //public float team1Hp = 1000f;
    //public float team2Hp = 1000f;
    ////! ���� player gold
    //public int gold = 1000; 
    #endregion

    public void Awake()
    {
        cycleManager = this;
        userState = (int)UserState.UnitSelect;
    }

    private void Update()
    {
        GameCycle();
    }

    #region GameCycle
    //{ GameCycle()
    public void GameCycle()
    {
        UpdateSelectionCycle();
        UpdateCommonUICycle();
        // �� ���� �Ǻ�

        //UpdateDefenceCycle();

        //UpdateGameEndCycle();

    }
    //} GameCycle()
    #endregion


    #region SelectCycle
    //{ UpdateSelectionCycle()
    // ���� ����Ŭ
    // ���ϳ� �̻� ���� & ���� enter -> defence
    public void UpdateSelectionCycle()
    {
        if (userState == (int)UserState.UnitSelect)
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
                    UIManager.uiManager.PrintRockSelectUI();
                    ItemManager.itemManager.userRockChoosed[0] = 0;
                    UIManager.uiManager.InstantiateRockImgForAttack();
                    UIManager.uiManager.InstantiateUnitImgForDenfence();
                    UIManager.uiManager.ShutDownUserSelectUI();
                    userState = (int)UserState.RockSelect;
                    UIManager.uiManager.TurnOnCommonUI();
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

    #endregion

    #region CommonUICycle
    //{ UpdateDefenceCycle()
    // ��������ñ��� ���� �ִ� UI
    public void UpdateCommonUICycle()
    {
        // ���� ���ôܰ�� ���� �ܰ谡 �ƴ϶�� �׻� ���
        if (userState != (int)UserState.UnitSelect || userState != (int)UserState.Ending)
        { 
              UIManager.uiManager.GetRotationKey();
        }
    }
    //} UpdateDefenceCycle()
    #endregion

    #region AttackCycle
    //{ ChangeCycleAttackToDefence()
    // ���� ����Ŭ���� ��� ����Ŭ�� ��ȯ�ϴ� �Լ�
    public void ChangeCycleAttackToDefence()
    {
        if (userState == (int)UserState.Attack)
        {
            userState = (int)UserState.Defence;
        }
        else { Debug.Log("GAMELOGIC ERROR"); }
    }
    //} ChangeCycleAttackToDefence()
    #endregion


    #region DefenceCycle
    //{ UpdateDefenceCycle()
    public void UpdateDefenceCycle()
    {
        // �� ������ �Ǿ��� ��
        if (userState == (int)UserState.Defence)
        {
            // ��ư �ν��Ͻ�

            // ���ð� ��ŭ �ð� ������
            //StartCoroutine(WaitForRock());
            // ��ȯ�ð��ʰ��� C ������
            if (isRockCreated == true && Input.GetKey(KeyCode.C))
            {
                // �� ��ȯ�ϰ� ī�޶� �Ѱ� ���ְ�
                userState = (int)UserState.Attack;
            }
        }
    }
    //} UpdateDefenceCycle()

    IEnumerator WaitForRock(float time_)
    { 
    
        yield return new WaitForSeconds(time_);
        isRockCreated = true;
        //GameObject myRock = Instantiate();
    }


    #endregion


    #region GameEndCycle
    //{ UpdateGameEndCycle()
    public void UpdateGameEndCycle()
    {
        if (userState == (int)UserState.Ending)
        { 
        
        }
    }
    //} UpdateGameEndCycle()
    #endregion
}
