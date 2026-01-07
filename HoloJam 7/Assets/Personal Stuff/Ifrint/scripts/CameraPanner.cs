using System.Numerics;
using UnityEngine;

public class CameraPanner : MonoBehaviour
{

    public float panSpeed = 10f;
    public float borderLeft = 830;
    public float borderRight = 840;
    public float borderTop = 10;
    public float borderBottom = -10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UnityEngine.Vector2 panPosition = new UnityEngine.Vector2(x:Input.GetAxis("Horizontal"), y:Input.GetAxis("Vertical"));

        UnityEngine.Vector3 newPosition = transform.localPosition;

        newPosition.x += panPosition.x * panSpeed * Time.deltaTime;
        newPosition.z += panPosition.y * panSpeed * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, borderLeft, borderRight);
        newPosition.z = Mathf.Clamp(newPosition.z, borderBottom, borderTop);

        transform.localPosition = newPosition;
    }
}
