using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderTriggerUIDelay : MonoBehaviour
{
    public Slider targetSlider; // Ŀ�� Slider
    public GameObject uiPanel; // UI ���
    public Text uiText; // UI ����
    public GameObject targetObject; // Ŀ������
    public GameObject firstHalo; // ��һ����Ȧ

    public float radioMinValue = 150.0f;
    public float radioMaxValue = 174.0f;
    public float uvMinValue = 75.0f;
    public float uvMaxValue = 100.0f;
    public float delayTime = 0.5f;

    private Coroutine checkCoroutine;
    private bool isUIVisible = false;
    private bool isObjectVisible = false;
    private bool isFirstHaloVisible = false;
    private bool hasTriggeredUVEffect = false; // ȷ�� UV ����ֻ����һ��
    private bool hasExitedUVRange = true; // ȷ���뿪 UV ��Χֻ����һ��

    private void Start()
    {
        if (targetSlider != null && uiPanel != null && targetObject != null && firstHalo != null)
        {
            uiPanel.SetActive(false);
            targetObject.SetActive(false);
            firstHalo.SetActive(false);
            targetSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }
        else
        {
            Debug.LogError("���� Inspector ��� Slider��UI ��塢Ŀ������ �� ��һ����Ȧ");
        }
    }

    private void OnSliderValueChanged(float value)
    {
        if (ElectromagneticMode.isRadioModeUnlocked && value >= radioMinValue && value <= radioMaxValue)
        {
            if (checkCoroutine == null)
            {
                checkCoroutine = StartCoroutine(WaitToShowUIAndObject("Radio connection successful"));
            }
        }
        else if (ElectromagneticMode.isUVModeUnlocked && value >= uvMinValue && value <= uvMaxValue)
        {
            if (checkCoroutine == null)
            {
                checkCoroutine = StartCoroutine(WaitToShowUIAndObject("UV connection successful"));
            }

            // **���� UV Ƶ�����䣬ֻ����һ��**
            if (!hasTriggeredUVEffect)
            {
                Interactor interactor = FindObjectOfType<Interactor>();
                if (interactor != null)
                {
                    interactor.ActivateUVEffect();
                    hasTriggeredUVEffect = true;
                    hasExitedUVRange = false; // �����뿪ʱ���� DeactivateUVEffect()
                }
            }
        }
        else
        {
            if (checkCoroutine != null)
            {
                StopCoroutine(checkCoroutine);
                checkCoroutine = null;
            }
            HideUIAndObject();

            // **�뿪 UV Ƶ�����䣬ֻ����һ��**
            if (!hasExitedUVRange)
            {
                Interactor interactor = FindObjectOfType<Interactor>();
                if (interactor != null)
                {
                    interactor.DeactivateUVEffect();
                    hasExitedUVRange = true;
                    hasTriggeredUVEffect = false; // �������´��� ActivateUVEffect()
                }
            }
        }
    }

    private IEnumerator WaitToShowUIAndObject(string message)
    {
        yield return new WaitForSeconds(delayTime);

        if (!isUIVisible && uiPanel != null && uiText != null)
        {
            uiPanel.SetActive(true);
            uiText.text = message;
            isUIVisible = true;
            Debug.Log($"UI ��ʾ: {message}");
        }

        if (!isObjectVisible && targetObject != null)
        {
            targetObject.SetActive(true);
            isObjectVisible = true;
            Debug.Log("Ŀ��������ʾ");
        }

        if (!isFirstHaloVisible && firstHalo != null)
        {
            firstHalo.SetActive(true);
            isFirstHaloVisible = true;

            HaloSequence haloSequence = firstHalo.GetComponentInParent<HaloSequence>();
            if (haloSequence != null)
            {
                haloSequence.ActivateFirstHalo();
            }

            Debug.Log("First halo activated");
        }
    }

    private void HideUIAndObject()
    {
        if (uiPanel != null && isUIVisible)
        {
            uiPanel.SetActive(false);
            isUIVisible = false;
            Debug.Log("UI ����");
        }

        if (targetObject != null && isObjectVisible)
        {
            targetObject.SetActive(false);
            isObjectVisible = false;
            Debug.Log("Ŀ����������");
        }

        if (firstHalo != null && isFirstHaloVisible)
        {
            firstHalo.SetActive(false);
            isFirstHaloVisible = false;
            Debug.Log("��һ����Ȧ����");
        }
    }

    private void OnDestroy()
    {
        if (targetSlider != null)
        {
            targetSlider.onValueChanged.RemoveListener(OnSliderValueChanged);
        }
    }
}
