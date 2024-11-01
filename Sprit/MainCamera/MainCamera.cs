using System.Drawing;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float scrollSpeed = 2f;

    void Update()
    {
        // 左右移动
        // 范围：-10~10

        float horizontalInput = Input.GetAxis("Horizontal");
        transform.position += new Vector3(horizontalInput * moveSpeed * Time.deltaTime, 0f, 0f);


        // 上下移动
        // 范围：-10~10
        float verticalInput = Input.GetAxis("Vertical");
        transform.position += new Vector3(0f, verticalInput * moveSpeed * Time.deltaTime, 0f);

        // 鼠标滚轮缩放
        // 缩放范围：0.5~20

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize -= scroll * scrollSpeed;
    }
}
