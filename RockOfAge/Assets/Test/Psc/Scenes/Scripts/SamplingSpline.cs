using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using UnityEditor;

[ExecuteInEditMode()]
public class SamplingSpline : MonoBehaviour
{
    //spline�� ������ ������ �ִ� container
    [SerializeField]
    private SplineContainer m_splineContainer;

    //ground�� ���� ��(6��)
    [SerializeField]
    private float m_width = 3f;




    public void SampleSplineWidth(float time, out Vector3 rightPoint, out Vector3 leftPoint, int index=0)
    {
        //evaout�� ����� ������
        float3 position;
        float3 tangent;
        float3 upVector;

        //index : �� ������Ʈ�� �����ϴ� spline �ε���
        m_splineContainer.Evaluate(index, time, out position, out tangent, out upVector);

        Vector3 _position = position;
        Vector3 right = Vector3.Cross(tangent, upVector).normalized;

        rightPoint = _position + (right * m_width);
        leftPoint = _position + (-right * m_width);
    }

    public bool IsClosed()
    {
        return m_splineContainer.Spline.Closed;
    }
    public int GetSplineCount()
    {
        return m_splineContainer.Spline.Count;
    }
}
