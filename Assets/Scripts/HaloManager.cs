using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HaloSequence : MonoBehaviour
{
    public List<GameObject> halos; // 存储光圈的列表
    public GameObject uiPanel; // UI 面板
    public UnityEngine.UI.Text uiText; // UI 文本组件
    public float messageDuration = 3f; // UI 显示时长

    private int currentIndex = -1; // 默认不亮起任何光圈
    private readonly string[] haloMessages =
    {
        "Finally, I got a radio signal! I can use it to navigate.",
        "Which way is right? Let's take a closer look.",
        "The halo is getting brighter, the signal stronger. I'm getting closer!",
        "High ground can reflect signals, causing 'dead zones'. I have to find my way now."
    };

    private void Start()
    {
        // 初始化时所有光圈都隐藏
        foreach (GameObject halo in halos)
        {
            halo.SetActive(false);
        }
        if (uiPanel != null) uiPanel.SetActive(false);
    }

    /// <summary>
    /// 由 Slider 控制激活第一个光圈
    /// </summary>
    public void ActivateFirstHalo()
    {
        if (halos.Count > 0 && currentIndex == -1)
        {
            currentIndex = 0;
            halos[currentIndex].SetActive(true);
            Debug.Log("First halo activated by slider.");

            // **强制触发 Collider 检测**
            Collider haloCollider = halos[currentIndex].GetComponent<Collider>();
            if (haloCollider != null)
            {
                haloCollider.enabled = false; // 先禁用
                haloCollider.enabled = true;  // 再启用，确保物理引擎更新
            }
        }
    }

    private void Update()
    {
        if (currentIndex >= 0 && currentIndex < halos.Count)
        {
            Collider[] colliders = Physics.OverlapSphere(halos[currentIndex].transform.position, 1f);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Left Hand") || collider.CompareTag("Right Hand"))
                {
                    Debug.Log("Halo detected hand: " + collider.gameObject.name);
                    AdvanceHaloSequence();
                    break;
                }
            }
        }
    }

    private void AdvanceHaloSequence()
    {
        if (currentIndex >= 0 && currentIndex < halos.Count)
        {
            Debug.Log("Halo " + currentIndex + " triggered.");
            halos[currentIndex].SetActive(false);
            ShowMessage(currentIndex);

            currentIndex++;
            if (currentIndex < halos.Count)
            {
                halos[currentIndex].SetActive(true);
                Debug.Log("Next halo activated: " + currentIndex);
            }
            else
            {
                Debug.Log("All halos completed.");
            }
        }
    }

    private void ShowMessage(int index)
    {
        if (uiPanel != null && uiText != null && index < haloMessages.Length)
        {
            uiPanel.SetActive(true);
            uiText.text = haloMessages[index];
            Debug.Log("UI Message: " + haloMessages[index]);
            StartCoroutine(HideMessageAfterDelay());
        }
    }

    private IEnumerator HideMessageAfterDelay()
    {
        yield return new WaitForSeconds(messageDuration);
        HideMessage();
    }

    private void HideMessage()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
        }
    }
}
