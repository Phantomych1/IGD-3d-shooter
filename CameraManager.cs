using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Чувствительность мыши")]
    public float sensitivity = 300f;

    [Header("Привязка объектов")]
    public Transform playerBody;

    [Header("Точки обзора")]
    public Transform fpsPoint;
    public Transform tpsPoint;

    private float xRotation = 0f;
    private bool isThirdPerson = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        playerBody.Rotate(Vector3.up * mouseX);

        if (Input.GetKeyDown(KeyCode.V))
        {
            isThirdPerson = !isThirdPerson;
        }
    }

    void LateUpdate()
    {
        if (isThirdPerson)
        {
            transform.position = tpsPoint.position;
            transform.rotation = Quaternion.Euler(0f, playerBody.eulerAngles.y, 0f);
        }
        else
        {
            transform.position = fpsPoint.position;
            transform.rotation = Quaternion.Euler(0f, playerBody.eulerAngles.y, 0f);
        }
    }
}