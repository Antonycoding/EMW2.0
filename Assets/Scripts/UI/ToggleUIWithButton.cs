using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class ToggleUIWithButton : MonoBehaviour
{
    public GameObject uiPanel; // 需要显示/隐藏的 UI 面板
    public InputActionReference toggleAction; // `B` 按钮的输入引用
    public bool isUIActive = false;
    private bool canToggleUI = false; // **默认 UI 不能被按键触发**

    public static event Action<bool> OnUIStateChanged; // 事件通知 UI 状态变化

    private void Start()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false); // 初始隐藏 UI
        }

        // 监听 B 按钮按下
        toggleAction.action.performed += ctx => TryToggleUI();
    }

    private void OnDestroy()
    {
        // 取消监听，防止报错
        toggleAction.action.performed -= ctx => TryToggleUI();
    }

    private void TryToggleUI()
    {
        if (!canToggleUI) return; // **只有解锁后才能触发 UI**

        isUIActive = !isUIActive;
        if (uiPanel != null)
        {
            uiPanel.SetActive(isUIActive);
        }

        Debug.Log($"UI 切换，当前状态: {isUIActive}");
        OnUIStateChanged?.Invoke(isUIActive);
    }

    // **当 `Battery1` 碰到 `Detection` 时，解锁 UI 交互**
    public void EnableUIToggle()
    {
        canToggleUI = true;
        Debug.Log("UI 交互已解锁，现在可以按键开关 UI 了！");
    }
}
