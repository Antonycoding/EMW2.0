using UnityEngine;

public class DynamicSoundGuide : MonoBehaviour
{
    public Transform xrOrigin; // 绑定 XR Origin (XR Rig)
    public Transform leftHand; // 左手的 Transform
    public Transform rightHand; // 右手的 Transform
    public AudioSource audioSource; // 绑定音频
    public float maxVolume = 1.0f; // 最大音量
    public float minVolume = 0.1f; // 最小音量
    public float maxDistance = 20f; // 声音最大传播距离
    public bool useLeftHand = true; // 选择用左手或右手作为朝向参考

    private Transform referenceHand;

    private void Start()
    {
        // 选择哪个手作为参考
        referenceHand = useLeftHand ? leftHand : rightHand;

        if (xrOrigin == null)
        {
            Debug.LogError("请在 Inspector 绑定 XR Origin (XR Rig)");
        }
        if (audioSource == null)
        {
            Debug.LogError("请在 Inspector 绑定 AudioSource");
        }
    }

    private void Update()
    {
        if (xrOrigin == null || referenceHand == null || audioSource == null) return;

        // 计算玩家（XR Origin）与声音的距离
        float distance = Vector3.Distance(xrOrigin.position, transform.position);

        // 计算音量（根据距离衰减）
        float volume = Mathf.Lerp(minVolume, maxVolume, 1 - (distance / maxDistance));
        audioSource.volume = Mathf.Clamp(volume, minVolume, maxVolume);

        // 计算手朝向声音的角度
        Vector3 directionToSound = (transform.position - referenceHand.position).normalized;
        float angle = Vector3.Angle(referenceHand.forward, directionToSound);

        // 如果玩家手朝向声音，音量增加
        if (angle < 30f)
        {
            audioSource.volume *= 1.2f;
        }

        // 如果玩家手背对声音，音量降低
        if (angle > 150f)
        {
            audioSource.volume *= 0.5f;
        }
    }
}
