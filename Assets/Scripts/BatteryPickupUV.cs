using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BatteryPickupUV : MonoBehaviour
{
    public GameObject objectToDestroy; // ��Ҫ���ٵ����壨�����ǽ��
    public GameObject batteryObject; // ��Ҫ��ʾ�����壨Tag Ϊ "Battery2"��
    public GameObject uiCanvas; // ��Ҫ������ UI Canvas
    public Text uiText; // UI �ı����
    public Image uiImage; // ���� UI ͼƬ
    public AudioClip pickupSound; // ��������Ч
    public InputActionReference hideUIAction; // �������� UI

    private AudioSource audioSource;
    private bool isPickedUp = false; // �Ƿ���ʰȡ
    private ToggleUIWithButton uiToggleScript; // UI �����ű�

    private void Start()
    {
        // UI Ĭ������
        if (uiCanvas != null)
        {
            uiCanvas.SetActive(false);
        }

        if (uiImage != null)
        {
            uiImage.gameObject.SetActive(false);
        }

        // ��ȡ�������Ƶ���
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ���������¼�
        hideUIAction.action.performed += ctx => HideUI();

        // ��ȡ UI �����ű�
        uiToggleScript = FindObjectOfType<ToggleUIWithButton>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // ��ң��֣����� Battery2
        if ((other.CompareTag("Left Hand") || other.CompareTag("Right Hand")) && !isPickedUp && CompareTag("Battery2"))
        {
            isPickedUp = true;
            Debug.Log("���ʰȡ�� Battery2");

            // ��ʾ UI �������ı�
            if (uiCanvas != null && uiText != null)
            {
                uiCanvas.SetActive(true);
                uiText.text = "This is a UV battery.";
                Debug.Log("UI ��ʾ�������ı���" + uiText.text);
            }

            // ����ָ�������壨�����ǽ��
            if (objectToDestroy != null)
            {
                Debug.Log("���٣�" + objectToDestroy.name);
                Destroy(objectToDestroy);
            }
        }

        // Battery2 ���� Detection
        if (other.CompareTag("Detection") && isPickedUp)
        {
            Debug.Log("Battery2 ������ Detection");

            // ������Ч
            if (pickupSound != null)
            {
                audioSource.PlayOneShot(pickupSound);
                Debug.Log("������Ч��" + pickupSound.name);
            }

            // **�л��� UV ģʽ**
            ElectromagneticMode modeManager = FindObjectOfType<ElectromagneticMode>();
            if (modeManager != null)
            {
                modeManager.UnlockUVMode();
            }

            // �������� Battery2
            Destroy(gameObject);
            Debug.Log("Battery2 ������");

            // ���� UI �ı�����ʾ UI Image
            if (uiCanvas != null && uiText != null)
            {
                uiText.text = "I need to tune the frequency to 750THz~30PHz.";
                uiImage.gameObject.SetActive(true);
                Debug.Log("UI �ı����£�" + uiText.text);
            }

            // ���� UI ����������ҿ��԰����л� UI
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
            Debug.Log("UI ������");
        }
    }

    private void OnDestroy()
    {
        // ȡ����������ֹ����
        hideUIAction.action.performed -= ctx => HideUI();
    }
}
