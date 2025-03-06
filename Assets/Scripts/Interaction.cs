using System.Collections;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField]
    private float radius = 0f; // ��ʼֵ
    private float maxRadius = 8f; // Ŀ��ֵ
    private float duration = 2f; // ����ʱ��
    private Coroutine transitionCoroutine; // �洢��ǰ�Ľ���Э��

    void Update()
    {
        Shader.SetGlobalVector("_Position", transform.position);
        Shader.SetGlobalFloat("_Radius", radius);
    }

    public void ActivateUVEffect()
    {
        // **����Ѿ���ִ�л��˵� 0������ֹͣ**
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        // **��ʼ�� 0 �� 8 �Ľ���**
        transitionCoroutine = StartCoroutine(SmoothTransition(radius, maxRadius));
    }

    public void DeactivateUVEffect()
    {
        // **�������ִ��ǰ���� 8������ֹͣ**
        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        // **��ʼ�ӵ�ǰֵ�� 0 �Ľ���**
        transitionCoroutine = StartCoroutine(SmoothTransition(radius, 0));
    }

    private IEnumerator SmoothTransition(float start, float target)
    {
        float elapsedTime = 0f;
        float duration = 2f; // ����ʱ��
        while (elapsedTime < duration)
        {
            radius = Mathf.Lerp(start, target, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        radius = target; // ȷ������ֵ��ȷ
    }
}
