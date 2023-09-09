using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �̱��� Ŭ������ �����ϰ� ����ϵ��� �����ִ� Ŭ����, GlobalFunctionBase(MonoBehaviour) �� ��ӹ޽��ϴ�.
/// </summary>
/// <typeparam name="T">���׸� Ŭ����(T)�� �̱������� ���� Ŭ������ �ֽ��ϴ�.<para></para>
/// </typeparam>
public class GlobalSingleton<T> : GlobalFunctionBase where T : GlobalSingleton<T>
{
    // private -> T �� ���� Ŭ������ _instance �� �ܺο��� ������ �Ұ����ϴ�.
    // static -> _instance �� ��𿡼��� ���� ������ ���� Ŭ�����̴�. 
    // default -> _instance �� �ʱ�ȭ�Ѵ�.
    private static T _instance = default;

    // public -> _instance �� ������Ƽ (Instance) �� ���� �����ϰ� �����ش�.
    public static T Instance 
    {
        get
        {
            // { ���ǹ� : ���� �̱������� ����� T Ŭ������ _instance �� default �ų� null �� ���,
            if (GlobalSingleton<T>._instance == default || GlobalSingleton<T>._instance == null)
            {
                // ���ο� ���� ������Ʈ�� T Ŭ������ ������Ʈ�� �ٿ��� �����Ѵ�.
                GlobalSingleton<T>._instance = new GameObject().AddComponent<T>();
                // �� �̱��� ������Ʈ�� ���� �ٲ� �ı����� �ʵ��� �Ѵ�.
                DontDestroyOnLoad(_instance.gameObject);
            }
            // } ���ǹ� ����

            // _instance �� ������Ƽ (Instance) �� �����ϸ� _instance �� ��ȯ�Ѵ�.
            return _instance;
        }
    }
}
