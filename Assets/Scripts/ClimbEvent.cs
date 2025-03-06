using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.XR.CoreUtils;
using UnityEngine.InputSystem;

public class ClimbEvent : MonoBehaviour
{
    public Transform xrOrigin;
    public Transform leftHand;
    public Transform rightHand;
    public ActionBasedController leftController;
    public ActionBasedController rightController;
    public float climbSpeed = 3f;
    public float handResetSpeed = 2f;
    public float climbSmoothTime = 0.2f;

    private bool leftHandGrabbing = false;
    private bool rightHandGrabbing = false;
    private bool isClimbing = false;
    private bool isFreeMoving = false;
    private Vector3 leftHandGrabPosition;
    private Vector3 rightHandGrabPosition;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        if (xrOrigin == null)
        {
            xrOrigin = FindObjectOfType<XROrigin>().transform;
            Debug.Log($"[Start] xrOrigin 自动赋值: {xrOrigin?.name}");
        }

        if (leftController == null)
        {
            leftController = FindObjectOfType<ActionBasedController>();
            Debug.Log($"[Start] leftController 自动赋值: {leftController?.name}");
        }

        if (rightController == null)
        {
            rightController = FindObjectOfType<ActionBasedController>();
            Debug.Log($"[Start] rightController 自动赋值: {rightController?.name}");
        }

        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnGrab);
            grabInteractable.selectExited.AddListener(OnRelease);
        }
    }

    void Update()
    {
        bool leftGripPressed = leftController.selectAction.action?.ReadValue<float>() > 0.5f;
        bool rightGripPressed = rightController.selectAction.action?.ReadValue<float>() > 0.5f;

        if (isClimbing)
        {
            if (leftHandGrabbing)
                leftHand.position = leftHandGrabPosition;
            if (rightHandGrabbing)
                rightHand.position = rightHandGrabPosition;
        }

        if (isFreeMoving)
        {
            Vector2 moveInput = leftController.translateAnchorAction.action?.ReadValue<Vector2>() ?? Vector2.zero;
            Vector3 moveDirection = xrOrigin.forward * moveInput.y * Time.deltaTime * 2f;
            xrOrigin.position += moveDirection;
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        Transform handTransform = args.interactorObject.transform;
        Debug.Log($"[OnGrab] 抓取物体: {handTransform.name}");

        if (handTransform.CompareTag("Left Hand"))
        {
            leftHandGrabbing = true;
            leftHandGrabPosition = handTransform.position;
        }
        else if (handTransform.CompareTag("Right Hand"))
        {
            rightHandGrabbing = true;
            rightHandGrabPosition = handTransform.position;
        }

        if (leftHandGrabbing && rightHandGrabbing && !isClimbing)
        {
            StartCoroutine(Climb());
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        Transform handTransform = args.interactorObject.transform;
        Debug.Log($"[OnRelease] 松开物体: {handTransform.name}");

        if (handTransform.CompareTag("Left Hand"))
        {
            leftHandGrabbing = false;
        }
        else if (handTransform.CompareTag("Right Hand"))
        {
            rightHandGrabbing = false;
        }

        if (!leftHandGrabbing && !rightHandGrabbing)
        {
            isClimbing = false;
            isFreeMoving = true;
            StartCoroutine(SmoothResetHandPosition());
        }
    }

    private IEnumerator Climb()
    {
        isClimbing = true;
        isFreeMoving = false;
        Vector3 startPos = xrOrigin.position;
        Vector3 endPos = startPos + Vector3.up * climbSpeed * 3f; // **调整攀爬高度，变成 3 倍**
        float elapsedTime = 0f;

        Debug.Log($"[Climb] 开始攀爬: 从 {startPos} 到 {endPos}");

        while (elapsedTime < climbSmoothTime)
        {
            elapsedTime += Time.deltaTime;
            xrOrigin.position = Vector3.SmoothDamp(xrOrigin.position, endPos, ref velocity, climbSmoothTime);
            yield return null;
        }

        xrOrigin.position = endPos;
        isClimbing = false;
        isFreeMoving = true;
    }

    private IEnumerator SmoothResetHandPosition()
    {
        Debug.Log("[SmoothResetHandPosition] 开始手部复位");

        Vector3 leftStart = leftHand.position;
        Vector3 rightStart = rightHand.position;
        Vector3 leftEnd = leftController.transform.TransformPoint(Vector3.zero); // **修正手部目标位置**
        Vector3 rightEnd = rightController.transform.TransformPoint(Vector3.zero);

        float elapsedTime = 0f;
        float duration = 0.5f; // **手复位的时间**

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            leftHand.position = Vector3.Lerp(leftStart, leftEnd, t);
            rightHand.position = Vector3.Lerp(rightStart, rightEnd, t);
            yield return null;
        }

        // **确保最终位置完全对齐**
        leftHand.position = leftEnd;
        rightHand.position = rightEnd;

        Debug.Log("[SmoothResetHandPosition] 手部复位完成");
    }
}