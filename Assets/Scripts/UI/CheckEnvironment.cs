using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CheckEnvironment : MonoBehaviour
{
    public GameObject uiPanel; // 绑定 UI 对象
    public float displayTime = 3f; // 显示时间（秒）

    private Coroutine hideCoroutine;

    private void Start()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false); // 初始隐藏 UI
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Left Hand") || other.CompareTag("Right Hand"))
        {
            Debug.Log("检测到手：" + other.gameObject.name);
            ShowUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Left Hand") || other.CompareTag("Right Hand"))
        {
            Debug.Log("手已离开：" + other.gameObject.name);
            HideUI();
        }
    }

    private void ShowUI()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(true);
            Debug.Log("UI 显示");
        }

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        hideCoroutine = StartCoroutine(AutoHideUI());
    }

    private IEnumerator AutoHideUI()
    {
        yield return new WaitForSeconds(displayTime);
        HideUI();
    }

    private void HideUI()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
        }

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
            hideCoroutine = null;
        }
    }
}
