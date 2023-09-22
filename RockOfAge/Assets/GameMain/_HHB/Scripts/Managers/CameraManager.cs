using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : GlobalSingleton<CameraManager>
{
    public void SetRockCamera(GameObject userRock, Vector3 startPoint)
    {
        Transform motherRock = userRock.transform;
        Transform childRock = motherRock.Find("RockObject");
        Vector3 cameraTransform = startPoint;
        childRock.transform.position = startPoint;
        GameObject rockCamera = Global_PSC.FindTopLevelGameObject("RockCamera");
        // lookFree 카메라 설정
        CinemachineFreeLook virtualRockCamera = rockCamera.GetComponent<CinemachineFreeLook>();
        virtualRockCamera.transform.position = cameraTransform;
        virtualRockCamera.Follow = childRock.transform;
        virtualRockCamera.LookAt = childRock.transform;
        //CinemachineComposer rockAim = virtualRockCamera.GetComponent<CinemachineComposer>();

        CinemachineOrbitalTransposer[] transposers = new CinemachineOrbitalTransposer[3];
        CinemachineComposer[] composers = new CinemachineComposer[3];
        for (int i = 0; i < 3; i++)
        {
            composers[i] = virtualRockCamera.GetRig(i).GetCinemachineComponent<CinemachineComposer>();
            transposers[i] = virtualRockCamera.GetRig(i).GetCinemachineComponent<CinemachineOrbitalTransposer>();


            if (i == 2)
            {
                composers[i].m_ScreenY = 0.49f;
            }

            transposers[i].m_ZDamping = 0.0f;
            transposers[i].m_YDamping = 0.0f;   
            composers[i].m_BiasY = 0.43f;
            composers[i].m_SoftZoneWidth = 0.2f;
            composers[i].m_SoftZoneHeight = 0.2f;
            composers[i].m_HorizontalDamping = 0f;
            composers[i].m_VerticalDamping = 0f;
        }


    }
}