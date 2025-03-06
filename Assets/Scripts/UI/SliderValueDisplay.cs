using UnityEngine;
using UnityEngine.UI;

public class SliderValueDisplay : MonoBehaviour
{
    public Slider targetSlider; // 需要显示数值的 Slider
    public Text valueText; // 显示数值的 UI Text

    private void Start()
    {
        if (targetSlider != null && valueText != null)
        {
            UpdateSliderValue(targetSlider.value); // 初始化时更新 Text
            targetSlider.onValueChanged.AddListener(UpdateSliderValue); // 监听 Slider 变化
        }
        else
        {
            Debug.LogError("请在 Inspector 里绑定 Slider 和 Text 组件！");
        }
    }

    private void UpdateSliderValue(float value)
    {
        if (valueText != null)
        {
            valueText.text = $"Frequency: {value:F2} Hz"; // 显示两位小数
        }
    }

    private void OnDestroy()
    {
        if (targetSlider != null)
        {
            targetSlider.onValueChanged.RemoveListener(UpdateSliderValue);
        }
    }
}
