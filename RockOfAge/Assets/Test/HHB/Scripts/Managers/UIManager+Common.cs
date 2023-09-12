using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Common
public partial class UIManager: MonoBehaviour
{
    public GameObject commonUI;
    private bool _mButtonPressed = false;
    private float _pressedTime = 0f;

    public void TurnOnCommonUI()
    {
        if (commonUI.activeSelf == false)
        {
            commonUI.SetActive(true);
        }
        else { return; }
    }

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

    public void RotateMirror()
    {
        // ��밡 �� ������ ���� �� ī�޶� ������ �Բ�(����ó��)
        MirrorRotate mirrorRotate = FindObjectOfType<MirrorRotate>();
        mirrorRotate.RotateMirror();
    }
}
