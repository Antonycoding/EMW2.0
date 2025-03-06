using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class JoystickToSlider : MonoBehaviour
{
    public Slider frequencySlider;
    public InputActionReference moveAction; // 左摇杆控制移动
    public InputActionReference rotateAction; // 右摇杆控制旋转
    public InputActionReference joystickInputAction; // 额外绑定摇杆输入
    public float sliderSpeed = 2.0f;

    private bool isUIActive = false;
    private ActionBasedController[] xrControllers;

    private void OnEnable()
    {
        ToggleUIWithButton.OnUIStateChanged += UpdateUIState;
    }

    private void OnDisable()
    {
        ToggleUIWithButton.OnUIStateChanged -= UpdateUIState;
    }

    private void Start()
    {
        xrControllers = FindObjectsOfType<ActionBasedController>(); // 获取所有 XR 控制器
    }

    private void UpdateUIState(bool active)
    {
        isUIActive = active;
        Debug.Log($"UpdateUIState() 触发，isUIActive: {isUIActive}");

        if (isUIActive)
        {
            DisableMovement(); // 禁用玩家移动
        }
        else
        {
            EnableMovement(); // 恢复玩家移动
        }
    }

    private void Update()
    {
        if (!isUIActive) return; // 如果 UI 关闭，直接跳出 Update

        float joystickX = joystickInputAction.action.ReadValue<Vector2>().x; // 直接从摇杆输入读取
        if (Mathf.Abs(joystickX) > 0.1f) // 避免微小输入
        {
            frequencySlider.value += joystickX * sliderSpeed * Time.deltaTime;
            //Debug.Log($"Slider 当前值: {frequencySlider.value}");
        }
    }

    private void DisableMovement()
    {
        if (moveAction.action != null) moveAction.action.Disable();
        if (rotateAction.action != null) rotateAction.action.Disable();

        Debug.Log("🚫 禁用玩家移动和旋转");
    }

    private void EnableMovement()
    {
        if (moveAction.action != null) moveAction.action.Enable();
        if (rotateAction.action != null) rotateAction.action.Enable();

        Debug.Log("✅ 恢复玩家移动和旋转");
    }
}
