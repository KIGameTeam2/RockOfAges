using UnityEngine;
using Cinemachine;


public class CameraMouse : MonoBehaviour
{
    // topViewCamera ������Ʈ
    public CinemachineVirtualCamera nowOnCamera;
    // �� Ŭ���� �̵��Ǵ� ī�޶�
    public CinemachineVirtualCamera nextOnCamera;
    // �� ī�޶�
    public CinemachineVirtualCamera rockCamera;
    // topViewCamera ������ ����
    private CinemachineTransposer transposer;
    // X,Z ���� �Է�
    private float xInput;
    private float zInput;
    // ������ ����
    private Vector3 moveDir;
    // ���� ������ ������
    private Vector3 targetPosition;
    // ī�޶� �̵� �ӵ�
    private float cameraSpeed = 100f;
    // smoothDamp 
    private float smoothTime = 0.02f;
    // ref velocity
    private Vector3 velocity = Vector3.zero;
    // ���콺 �̵� ��������
    private float edgeSize = 20f;


    private void Start()
    {
        transposer = nowOnCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void Update()
    {
        if (CycleManager.cycleManager.userState == (int)UserState.Defence)
        { 
            MoveCameraFromKeyBoard();
            RotateCameraTransition();    
            MoveCameraFromMouse();
        }
        ChangeCameraToRock();
    }

    //{ MoveCameraFromInput()
    public void MoveCameraFromKeyBoard()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        zInput = Input.GetAxisRaw("Vertical");

        if (transposer != null)
        {
            moveDir = new Vector3(xInput ,0f, zInput).normalized;
            if(moveDir != Vector3.zero)
            {
                Vector3 moveDistance = moveDir * cameraSpeed * Time.deltaTime;

                targetPosition = transform.position + moveDistance;

                // SmothDamp ������ġ/ ������ġ/ ���� ������ �ӵ�/ ���Žð�(�������� ���� ������) 
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

                // ���� ī�޶� ���� �̵�
                if (nextOnCamera != null)
                {
                    float nowX = transform.position.x;
                    float nowZ = transform.position.z;
                    float nextY = nextOnCamera.transform.position.y;
                    nextOnCamera.transform.position = new Vector3(nowX, nextY, nowZ);
                }

            }
        }
    }
    //} MoveCameraFromInput()

    //{ RotateCameraFromInput()
    public void RotateCameraTransition()
    {
        if (Input.GetMouseButtonDown(2) == true)
        {
            nowOnCamera.gameObject.SetActive(false);
            nextOnCamera.gameObject.SetActive(true);
        }
    }
    //} RotateCameraFromInput()

    //{ ChangeCameraToRock()
    public void ChangeCameraToRock()
    {
        //IsMine
        if (CycleManager.cycleManager.userState == (int)UserState.Attack)
        {
            Debug.Log("top -> rock");
            rockCamera.gameObject.SetActive(true);
            nowOnCamera.gameObject.SetActive(false);
            nextOnCamera.gameObject.SetActive(false);
        }
    }
    //} ChangeCameraToRock()

    //{ MoveCameraFromMouse()
    public void MoveCameraFromMouse()
    {
        if (Input.mousePosition.x > Screen.width - edgeSize)
        {
            EdgeMove(Vector3.right);
        }
        if (Input.mousePosition.x < edgeSize)
        {
            EdgeMove(Vector3.left);
        }
        if (Input.mousePosition.y < edgeSize)
        {
            EdgeMove(-Vector3.forward);
        }
        if (Input.mousePosition.y > Screen.height - edgeSize)
        {
            EdgeMove(-Vector3.back);
        }

    }
    //} MoveCameraFromMouse()


    //{ EdgeMove(Vector3 dir)
    private void EdgeMove(Vector3 dir)
    {
        Vector3 moveDistance = dir * cameraSpeed * Time.deltaTime;

        targetPosition = transform.position + moveDistance;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        // ���� ī�޶� ���� �̵�
        if (nextOnCamera != null)
        {
            float nowX = transform.position.x;
            float nowZ = transform.position.z;
            float nextY = nextOnCamera.transform.position.y;
            nextOnCamera.transform.position = new Vector3(nowX, nextY, nowZ);
        }
    }
    //} EdgeMove(Vector3 dir)
}
