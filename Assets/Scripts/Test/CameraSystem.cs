using Cinemachine;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private bool useEdgeScrolling = false;
    [SerializeField] private bool useDragPan = false;

    private bool dragPanMoveActive;
    private Vector2 lastMousePosition;
    private float targetFieldOfView;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 InputDir = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) InputDir.z = +1f;
        if (Input.GetKey(KeyCode.S)) InputDir.z = -1f;
        if (Input.GetKey(KeyCode.A)) InputDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) InputDir.x = +1f;

        if (useEdgeScrolling)
        {
            int edgeScrollSize = 20;

            if (Input.mousePosition.x < edgeScrollSize) InputDir.x = -1f;
            if (Input.mousePosition.y < edgeScrollSize) InputDir.z = -1f;
            if (Input.mousePosition.x > Screen.width - edgeScrollSize) InputDir.x = +1f;
            if (Input.mousePosition.x > Screen.height - edgeScrollSize) InputDir.z = +1f;
        }

        if (Input.GetMouseButtonDown(0))
        {
            dragPanMoveActive = true;
            lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            dragPanMoveActive = false;
        }

        if (dragPanMoveActive)
        {
            Vector2 mouseMovementDelta = (Vector2)Input.mousePosition - lastMousePosition;

            float dragPanSpeed = 20f;
            InputDir.x = -mouseMovementDelta.x * dragPanSpeed * Time.deltaTime;
            InputDir.z = -mouseMovementDelta.y * dragPanSpeed * Time.deltaTime;

            lastMousePosition = Input.mousePosition;
        }



        Vector3 moveDir = transform.forward * InputDir.z + transform.right * InputDir.x;
        float moveSpeed = 50f;
        transform.position += moveDir * moveSpeed * Time.deltaTime;


        float rotateDir = 0f;
        if (Input.GetKey(KeyCode.Q)) rotateDir = +1f;
        if (Input.GetKey(KeyCode.E)) rotateDir = -1f;

        float rotateSpeed = 50f;
        transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);
    }

    private void HandleCameraMovement()
    {

    }

    private void HandleCameraMovementEdgeScrolling()
    {

    }

    private void HandleCameraMovementDragPan()
    {

    }

    private void HandleCameraRotation()
    {

    }

    private void HandleCameraZoom_FieldOfView()
    {

    }

    private void HandleCameraZoom_MoveForward()
    {

    }

    private void HandleCameraZoom_LowerY()
    {

    }
}
