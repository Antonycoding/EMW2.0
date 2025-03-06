using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class ToggleUIWithButton : MonoBehaviour
{
    public GameObject uiPanel; // ��Ҫ��ʾ/���ص� UI ���
    public InputActionReference toggleAction; // `B` ��ť����������
    public bool isUIActive = false;
    private bool canToggleUI = false; // **Ĭ�� UI ���ܱ���������**

    public static event Action<bool> OnUIStateChanged; // �¼�֪ͨ UI ״̬�仯

    private void Start()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false); // ��ʼ���� UI
        }

        // ���� B ��ť����
        toggleAction.action.performed += ctx => TryToggleUI();
    }

    private void OnDestroy()
    {
        // ȡ����������ֹ����
        toggleAction.action.performed -= ctx => TryToggleUI();
    }

    private void TryToggleUI()
    {
        if (!canToggleUI) return; // **ֻ�н�������ܴ��� UI**

        isUIActive = !isUIActive;
        if (uiPanel != null)
        {
            uiPanel.SetActive(isUIActive);
        }

        Debug.Log($"UI �л�����ǰ״̬: {isUIActive}");
        OnUIStateChanged?.Invoke(isUIActive);
    }

    // **�� `Battery1` ���� `Detection` ʱ������ UI ����**
    public void EnableUIToggle()
    {
        canToggleUI = true;
        Debug.Log("UI �����ѽ��������ڿ��԰������� UI �ˣ�");
    }
}
