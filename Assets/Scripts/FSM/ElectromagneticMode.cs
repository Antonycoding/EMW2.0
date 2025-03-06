using UnityEngine;
using UnityEngine.UI;

public class ElectromagneticMode : MonoBehaviour
{
    public static bool isUVModeUnlocked = false;  // UV 模式是否解锁
    public static bool isXRayModeUnlocked = false; // X 射线模式是否解锁
    public static bool isRadioModeUnlocked = true; // 无线电模式默认可用

    public GameObject frequencyUI; // 频率图 UI 面板
    public Text modeText; // 显示当前模式的 UI 文本
    public Image modeIcon; // 模式图标
    public Sprite uvIcon, xrayIcon, radioIcon; // 代表不同模式的图标

    //private void Start()
    //{
    //    UpdateUI(); // 确保 UI 初始状态
    //}

    public void UnlockUVMode()
    {
        isUVModeUnlocked = true;
        Debug.Log("UV 模式已解锁！");
    }

    public void UnlockXRayMode()
    {
        isXRayModeUnlocked = true;
        Debug.Log("X-ray 模式已解锁！");
    }

    public void UnlockRadioMode()
    {
        isRadioModeUnlocked = true;
        Debug.Log("无线电模式已解锁！");
    }


}
