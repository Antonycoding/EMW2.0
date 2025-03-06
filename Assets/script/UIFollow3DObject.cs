using UnityEngine;
using UnityEngine.UI;

public class UIFollow3DObject : MonoBehaviour
{
    public Transform mainCam;
    public Transform target;
    public Transform worldSpaceCanvas;
    public Vector3 offset;


    private void Start()
    {
        mainCam = Camera.main.transform;
        transform.SetParent(worldSpaceCanvas);

    }
    void Update()
    {
        // 保持 UI 朝向摄像机
        transform.rotation = Quaternion.LookRotation(transform.position - mainCam.transform.position); // look at camera

        // 更新 UI 的位置
        transform.position = target.position + offset;

    }
}