using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Скорость")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float crouchSpeed = 1.5f;

    [Header("Прыжок и Гравитация")]
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Присед (Физика)")]
    public float normalHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchTransitionSpeed = 10f;

    [Tooltip("Создай пустой объект 'CameraRoot', положи в него FPS и TPS, и перетащи сюда")]
    public Transform cameraRoot;
    public float cameraStandHeight = 2f;
    public float cameraCrouchHeight = 1.5f;

    [Header("Проверка земли")]
    public Transform groundCheck;
    public float groundDistance = 0.5f;
    public LayerMask groundMask;

    private CharacterController controller;
    private Animator animator;

    private Vector3 velocity;
    private bool isGrounded;
    private bool isCrouching = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        isCrouching = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.C);

        float currentSpeed = walkSpeed;
        if (isCrouching) currentSpeed = crouchSpeed;
        else if (isRunning) currentSpeed = runSpeed;

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded && !isCrouching)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger("Jump");
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        AdjustCapsuleHeight();
        UpdateAnimator(move.magnitude, isRunning);
    }

    private void AdjustCapsuleHeight()
    {
        float targetHeight = isCrouching ? crouchHeight : normalHeight;
        float targetCameraY = isCrouching ? cameraCrouchHeight : cameraStandHeight;
        float targetCenterY = targetHeight / 2f;

        controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * crouchTransitionSpeed);
        controller.center = Vector3.Lerp(controller.center, new Vector3(0, targetCenterY, 0), Time.deltaTime * crouchTransitionSpeed);

        if (cameraRoot != null)
        {
            Vector3 newPos = cameraRoot.localPosition;
            newPos.y = Mathf.Lerp(newPos.y, targetCameraY, Time.deltaTime * crouchTransitionSpeed);
            cameraRoot.localPosition = newPos;
        }
    }
    private void UpdateAnimator(float moveMagnitude, bool isRunning)
    {
        float animSpeed = 0f;
        if (moveMagnitude > 0.1f)
        {
            animSpeed = isRunning ? 1f : 0.5f;
        }

        animator.SetFloat("Speed", animSpeed, 0.1f, Time.deltaTime);
        animator.SetBool("IsCrouching", isCrouching);
        animator.SetBool("IsGrounded", isGrounded);
    }
}