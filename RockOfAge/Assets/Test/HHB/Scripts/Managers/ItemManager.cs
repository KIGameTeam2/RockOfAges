using System.Collections.Generic;
using UnityEngine;


public class ItemManager : MonoBehaviour
{
    public static ItemManager itemManager;
    public List<int> userSelectedItems = new List<int>();
    public int rockCount = 0;
    public int unitCount = 0;
    public int capacity = 8;


    private void Awake()
    {
        itemManager = this;
    }

    //{ CheckUserListCapacity 
    // �������� ������ �ʰ��ϴ��� Ȯ���ϴ� �Լ�
    public void CheckUserListCapacity()
    {

    }

    //{ CheckItemList()
    // �������� ������ �ִ��� �����ϴ� �Լ�
    public bool CheckItemList(int id_)
    {
        // ������ ������ �ִٸ�
        if (userSelectedItems.Contains(id_))
        {
            return true;
        }
        else // ������ ���� �ʴٸ� 
        {
            return false;
        }
    }
    //} CheckItemList()

    //{ RePrintHolder()
    // ��� ������ ������ �����ġ�� ������
    public void RePrintHolder()
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
            foreach (int id in userSelectedItems)
            {
                RockButton rockButton = FindObjectOfType<RockButton>();
                rockButton.InstantiateHolder(id);
            }
        }
    }
    //} RePrintHolder()
}