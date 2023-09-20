using UnityEngine;

/// <summary>
/// Ŭ������ �̱������� ���� ��ȯ���ִ� Ŭ�����Դϴ�, GlobalFunctionBase(MonoBehaviour) ��ӹް� �ֽ��ϴ�.
/// <para></para>
/// ���� : �̱������� ���� Ŭ������ �� Ŭ������ ��ӽ�Ű�� �˴ϴ�.
/// <para></para>
/// ����) public class ItemManager : GlobalSingleton(ItemManager)
/// </summary>
/// <typeparam name="T">���׸� Ŭ����(T)�� �̱������� ���� Ŭ������ ���� �� �ֽ��ϴ�.<para></para>
/// </typeparam>
public class GlobalSingleton<T> : GlobalFunctionBase where T : GlobalSingleton<T>
{
    // private -> T �� ���� Ŭ������ _instance ��� �ʵ带 ������ ���� ��ȣ ������ private �Դϴ�.
    // static -> _instance �� static ���� ����� ���α׷� ������� �޸𸮿� �����ֽ��ϴ�. 
    // default -> _instance �� �ʱ�ȭ�մϴ�.
    private static T _instance = default;

    // public -> _instance �� ������Ƽ (Instance) �� �����Ͽ� �ܺο��� ������ �� �ֵ��� �մϴ�.
    public static T Instance
    {
        get
        {
            // { ���ǹ� : �̱������� ���� T �� _instance �� default �̰ų� null ���¶��,
            if (GlobalSingleton<T>._instance == default || GlobalSingleton<T>._instance == null)
            {
                // T �� ���� ������Ʈ�� ������Ʈ�� �ٿ��� �ν��Ͻ�ȭ �մϴ�.
                GlobalSingleton<T>._instance = new GameObject().AddComponent<T>();
                _instance.GetComponent<T>().enabled = true;

                // �̱������� ������Ʈȭ �� ���� ������Ʈ�� Scene �� �ٲ� �ı����� �ʽ��ϴ�.
                DontDestroyOnLoad(_instance.gameObject);
            }
            // } ���ǹ� ����

            // _instance �� ������Ƽ (Instance) �� _instance �� �ְ� ��ȯ�մϴ�.
            return _instance;
        }
    }

    public void CreateThisManager() 
    {
        /*Intentionally Emptied*/
    }
}
