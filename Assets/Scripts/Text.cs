using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class JoystickUsageHint : MonoBehaviour
{
    public GameObject hintPanel; // UI Panel containing hints
    public Text hintText; // UI text component for hints
    public Image leftHintImage; // UI image for left joystick hint
    public Image rightHintImage; // UI image for right joystick hint
    public InputActionReference moveAction; // Left joystick (movement)
    public InputActionReference rotateAction; // Right joystick (camera control)

    private bool leftJoystickUsed = false;
    private bool rightJoystickUsed = false;

    private void Start()
    {
        if (hintText != null)
        {
            hintText.text = "Left Joystick: Move\nRight Joystick: Look"; // Initial hint text
        }

        // Listen to joystick input
        moveAction.action.performed += OnLeftJoystickUsed;
        rotateAction.action.performed += OnRightJoystickUsed;
    }

    private void OnLeftJoystickUsed(InputAction.CallbackContext context)
    {
        if (!leftJoystickUsed)
        {
            leftJoystickUsed = true;
            if (leftHintImage != null)
            {
                leftHintImage.gameObject.SetActive(false); // Hide left joystick hint image
            }
            UpdateHintUI();
        }
    }

    private void OnRightJoystickUsed(InputAction.CallbackContext context)
    {
        if (!rightJoystickUsed)
        {
            rightJoystickUsed = true;
            if (rightHintImage != null)
            {
                rightHintImage.gameObject.SetActive(false); // Hide right joystick hint image
            }
            UpdateHintUI();
        }
    }

    private void UpdateHintUI()
    {
        if (hintText == null || hintPanel == null) return;

        if (leftJoystickUsed && rightJoystickUsed)
        {
            hintPanel.SetActive(false); // Hide entire UI panel
            Debug.Log("Both joysticks used, hiding UI panel");
        }
        else if (leftJoystickUsed)
        {
            hintText.text = "Right Joystick: Look";
        }
        else if (rightJoystickUsed)
        {
            hintText.text = "Left Joystick: Move";
        }
    }

    private void OnDestroy()
    {
        // Remove listeners to prevent errors
        moveAction.action.performed -= OnLeftJoystickUsed;
        rotateAction.action.performed -= OnRightJoystickUsed;
    }
}
