using System.Collections;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField]
    private float radius = 0f; // 初始值
    private float maxRadius = 8f; // 目标值
    private float duration = 2f; // 过渡时间
    private Coroutine transitionCoroutine; // 存储当前的渐变协程

    void Update()
    {
        Shader.SetGlobalVector("_Position", transform.position);
        Shader.SetGlobalFloat("_Radius", radius);
    }

    public void ActivateUVEffect()
    {
        // **如果已经在执行回退到 0，就先停止**
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        // **开始从 0 到 8 的渐变**
        transitionCoroutine = StartCoroutine(SmoothTransition(radius, maxRadius));
    }

    public void DeactivateUVEffect()
    {
        // **如果正在执行前进到 8，就先停止**
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        // **开始从当前值到 0 的渐变**
        transitionCoroutine = StartCoroutine(SmoothTransition(radius, 0));
    }

    private IEnumerator SmoothTransition(float start, float target)
    {
        float elapsedTime = 0f;
        float duration = 2f; // 渐变时间
        while (elapsedTime < duration)
        {
            radius = Mathf.Lerp(start, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        radius = target; // 确保最终值正确
    }
}
