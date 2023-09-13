using System.Collections.Generic;
using UnityEngine;


public class ItemManager : MonoBehaviour
{
    public static ItemManager itemManager;
    #region ����
    public int[] userRockChoosed = new int[1];
    public List<int> rockSelected = new List<int>();
    public List<int> unitSelected = new List<int>();
    public int rockCount = 0;
    public int unitCount = 0;
    public int capacity = 8;
    #endregion

    private void Awake()
    {
        itemManager = this;
        DontDestroyOnLoad(itemManager);
    }

    #region Functions
    //{ CheckUserListCapacity 
    // �������� ������ �ʰ��ϴ��� Ȯ���ϴ� �Լ�
    public bool CheckUserListCapacity(int count_)
    {
        float userItemCount = rockCount * 2 + unitCount + count_;
        //Debug.Log(userItemCount);
        if (userItemCount > capacity)
        {
            return false;
        }
        else { return true; }
    }
    //} CheckUserListCapacity 
    

    //{ CheckItemList()
    // �������� ������ �ִ��� �����ϴ� �Լ�
    public bool CheckItemList(int id_)
    {
        // ������ ������ �ִٸ�
        if (unitSelected.Contains(id_) || rockSelected.Contains(id_))
        {
            return true;
        }
        else // ������ ���� �ʴٸ� 
        {
            return false;
        }
    }
    //} CheckItemList()

    //{ RockRePrintHolder()
    // (��)��� ������ ������ �����ġ�� ������
    public void RockRePrintHolder()
    {
        // ��µ� ���� 1�� �ۿ� ���� ��
        if (rockCount <= 1)
        {
            GameObject rockHolder = GameObject.Find("RockHolder");
            if (rockHolder != null)
            {
                rockCount--;
                Destroy(rockHolder);
            }
        }
        if (rockCount > 1)
        { 
            GameObject rocks = GameObject.Find("Rocks");
            if (rocks != null)
            {
                Transform[] rockHolders = rocks.GetComponentsInChildren<Transform>();

                foreach (Transform child in rockHolders)
                {
                    if (child.name == "RockHolder")
                    {
                        GameObject destroyObj = child.gameObject;
                        Destroy(destroyObj);
                        rockCount = 0;
                    }
                }
            }
            foreach (int id in rockSelected)
            {
                RockButton rockButton = FindObjectOfType<RockButton>();
                rockButton.InstantiateRockHolder(id);
            }
        }
    }
    //} RockRePrintHolder()

    //{ UnitRePrintHolder()
    // (����)��� ������ ������ �����ġ�� ������
    public void UnitRePrintHolder()
    {
        // ��µ� ���� 1�� �ۿ� ���� ��
        if (unitCount <= 1)
        {
            GameObject unitHolder = GameObject.Find("UnitHolder");
            if (unitHolder != null)
            {
                unitCount--;
                Destroy(unitHolder);
            }
        }
        if (unitCount > 1)
        {
            GameObject units = GameObject.Find("Units");
            if (units != null)
            {
                Transform[] rockHolders = units.GetComponentsInChildren<Transform>();

                foreach (Transform child in rockHolders)
                {
                    if (child.name == "UnitHolder")
                    {
                        GameObject destroyObj = child.gameObject;
                        Destroy(destroyObj);
                        unitCount = 0;
                    }
                }
            }
            foreach (int id in unitSelected)
            {
                UnitButton unitButton = FindObjectOfType<UnitButton>();
                unitButton.InstantiateUnitHolder(id);
            }
        }
    }
    //} UnitRePrintHolder()
    #endregion
}