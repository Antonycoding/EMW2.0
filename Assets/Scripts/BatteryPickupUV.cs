using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BatteryPickupUV : MonoBehaviour
{
    public GameObject objectToDestroy; // 需要销毁的物体（如空气墙）
    public GameObject batteryObject; // 需要显示的物体（Tag 为 "Battery2"）
    public GameObject uiCanvas; // 需要弹出的 UI Canvas
    public Text uiText; // UI 文本组件
    public Image uiImage; // 额外 UI 图片
    public AudioClip pickupSound; // 触发的音效
    public InputActionReference hideUIAction; // 按键隐藏 UI

    private AudioSource audioSource;
    private bool isPickedUp = false; // 是否已拾取
    private ToggleUIWithButton uiToggleScript; // UI 交互脚本

    private void Start()
    {
        // UI 默认隐藏
        if (uiCanvas != null)
        {
            uiCanvas.SetActive(false);
        }

        if (uiImage != null)
        {
            uiImage.gameObject.SetActive(false);
        }

        // 获取或添加音频组件
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // 监听按键事件
        hideUIAction.action.performed += ctx => HideUI();

        // 获取 UI 交互脚本
        uiToggleScript = FindObjectOfType<ToggleUIWithButton>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 玩家（手）碰到 Battery2
        if ((other.CompareTag("Left Hand") || other.CompareTag("Right Hand")) && !isPickedUp && CompareTag("Battery2"))
        {
            isPickedUp = true;
            Debug.Log("玩家拾取了 Battery2");

            // 显示 UI 并更新文本
            if (uiCanvas != null && uiText != null)
            {
                uiCanvas.SetActive(true);
                uiText.text = "This is a UV battery.";
                Debug.Log("UI 显示并更新文本：" + uiText.text);
            }

            // 销毁指定的物体（如空气墙）
            if (objectToDestroy != null)
            {
                Debug.Log("销毁：" + objectToDestroy.name);
                Destroy(objectToDestroy);
            }
        }

        // Battery2 碰到 Detection
        if (other.CompareTag("Detection") && isPickedUp)
        {
            Debug.Log("Battery2 碰到了 Detection");

            // 播放音效
            if (pickupSound != null)
            {
                audioSource.PlayOneShot(pickupSound);
                Debug.Log("播放音效：" + pickupSound.name);
            }

            // **切换到 UV 模式**
            ElectromagneticMode modeManager = FindObjectOfType<ElectromagneticMode>();
            if (modeManager != null)
            {
                modeManager.UnlockUVMode();
            }

            // 立即销毁 Battery2
            Destroy(gameObject);
            Debug.Log("Battery2 已销毁");

            // 更新 UI 文本并显示 UI Image
            if (uiCanvas != null && uiText != null)
            {
                uiText.text = "I need to tune the frequency to 750THz~30PHz.";
                uiImage.gameObject.SetActive(true);
                Debug.Log("UI 文本更新：" + uiText.text);
            }

            // 启用 UI 交互，让玩家可以按键切换 UI
            if (uiToggleScript != null)
            {
                uiToggleScript.EnableUIToggle();
            }
        }
    }

    private void HideUI()
    {
        if (uiCanvas != null)
        {
            uiCanvas.SetActive(false);
            Debug.Log("UI 已隐藏");
        }
    }

    private void OnDestroy()
    {
        // 取消监听，防止报错
        hideUIAction.action.performed -= ctx => HideUI();
    }
}
