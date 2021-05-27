using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//using LitJson;
using System;
using UnityEngine.EventSystems;

public class CameraControl : MonoBehaviour
{
    //目标物体
    public Transform target;
    //目标物体状态
    public bool isShowTarget;

    [Header("镜头滑动条")]
    public Slider camSlider;

    //目标偏移
    public Vector3 targetOffset;
    //距离
    public float distance = 205f;
    //初始距离
    public float initDistance = 205;

    //最大距离
    public float maxDistance = 300f;
    //最小距离
    public float minDistance = 0.6f;
    //x轴速度
    public float xSpeed = 80.0f;
    //y轴速度
    public float ySpeed = 50.0f;

    //Y最小角度
    public float yMinAngleLimit = 20;
    //Y最大角度
    public float yMaxAngleLimit = 90;
    //缩放比
    public float zoomRate = 10f;
    public float panSpeed = 100f;
    public float panMinSpeed = 10f;
    public float zoomDampening = 3f;

    public float dragYMinPos = 0f;
    public float dragYMaxPos = 50f;

    private float xDeg = 0.0f;
    private float yDeg = 0.0f;

    private float currentDistance;
    private float desiredDistance;

    //当前旋转和预期旋转
    private Quaternion currentRotation;
    private Quaternion desiredRotation;
    private Quaternion rotation;
    private Vector3 position;

    //自动旋转偏移
    public float autoRotateOffset = 0.9f;

    int autoRotateDirection = -1;//-1为左， 1为右

    public Texture tex_left_button;
    public Texture tex_scroll;

    //实例
    public static CameraControl instance;


    Vector3 cameraOriginPosition;
    Quaternion cameraOrigionQuaternion;
    Vector3 cameraTargetOriginPosition;
    Quaternion cameraTargetOriginQuaternion;

    void Start()
    {
        instance = this;
    }



    void OnEnable() { Init(); }

    void OnDisable()
    {
        //////if (target != null)
        //////{
        //////    DestroyImmediate(target.gameObject);
        //////}
    }



    public void Init()
    {

        cameraOriginPosition = transform.position;
        //Debug.Log("cameraOriginPosition1:" + cameraOriginPosition);
        cameraOrigionQuaternion = transform.rotation;



        if (!target)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.name = "Cam Target";
            go.GetComponent<SphereCollider>().enabled = false;

            go.GetComponent<MeshRenderer>().enabled = isShowTarget;

            //go.transform.position = transform.position + (transform.forward * distance);
            //go.transform.position = transform.position + (transform.forward * initDistance);
            go.transform.position = transform.position + (transform.forward * distance);
            target = go.transform;
            target.gameObject.AddComponent<CameraTargetControl>();
            //Global.cameraTarget = target.gameObject;

            cameraTargetOriginPosition = target.transform.position;
            cameraTargetOriginQuaternion = target.transform.rotation;
        }

        distance = Vector3.Distance(transform.position, target.position);
        currentDistance = distance;
        desiredDistance = distance;

        position = transform.position;
        rotation = transform.rotation;
        currentRotation = transform.rotation;
        desiredRotation = transform.rotation;

        //xDeg = Vector3.Angle(Vector3.right, transform.right);
        //yDeg = Vector3.Angle(Vector3.up, transform.up);
        xDeg = transform.eulerAngles.y;
        yDeg = transform.eulerAngles.x;

        //////transform.position = cameraOriginPosition;
        //////transform.rotation = cameraOrigionQuaternion;
        //Debug.LogError("xDeg:" + xDeg);
        //Debug.LogError("yDeg:" + yDeg);
    }


    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        //////if (EventSystem.current.IsPointerOverGameObject())
        //////{
        //////    return;
        //////}

        distance = Vector3.Distance(transform.position, target.position);

        //////zoomRate = 0.1f * distance;
        //////panSpeed = 1f * distance;
        if (panSpeed < panMinSpeed)
        {
            panSpeed = panMinSpeed;
        }
        /*if (Input.GetMouseButton(1))//右键 放大缩小//Input.GetMouseButton(2) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftControl)
        {
            isAutoRotate = false;
            //desiredDistance -= Input.GetAxis("Mouse Y") * Time.deltaTime * zoomRate * 0.125f * Mathf.Abs(desiredDistance);
            //////desiredDistance += Input.GetAxis("Mouse Y") * Time.deltaTime * zoomRate * 0.125f * Mathf.Abs(desiredDistance);
            desiredDistance += Input.GetAxis("Mouse Y") * Time.deltaTime * zoomRate * 0.125f * 100f;

            camera_show.texture = tex_scroll;

        }*/
        if (Input.GetMouseButton(0))//左键 上下左右旋转 Input.GetMouseButton(2) && Input.GetKey(KeyCode.LeftAlt)
        {
            //Debug.Log("xDeg:" + Input.GetAxis("Mouse X"));

            xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            yDeg = ClampAngle(yDeg, yMinAngleLimit, yMaxAngleLimit);

            //Debug.Log("xDeg:" + xDeg + ", yDeg:" + yDeg);
            desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);
            currentRotation = transform.rotation;

            rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * zoomDampening);
            transform.rotation = rotation;
        }
        else if (Input.GetMouseButton(2))//中键 上下左右移动
        {
            return;
            target.rotation = transform.rotation;
            target.Translate(Vector3.right * -Input.GetAxis("Mouse X") * panSpeed * Time.deltaTime);
            if (target.transform.position.y + (-Input.GetAxis("Mouse Y") * panSpeed * Time.deltaTime) > dragYMinPos && target.transform.position.y + (-Input.GetAxis("Mouse Y") * panSpeed * Time.deltaTime) < dragYMaxPos)
            {
                target.Translate(transform.up * -Input.GetAxis("Mouse Y") * panSpeed * Time.deltaTime, Space.World);
            }
            //////target.Translate(transform.up * -Input.GetAxis("Mouse Y") * panSpeed * Time.deltaTime, Space.World);

        }
        else
        {
            xDeg = transform.eulerAngles.y;
            yDeg = transform.eulerAngles.x;
        }


        //////desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * Mathf.Abs(desiredDistance);
        desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * 100f;
        desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
        currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * zoomDampening);
        //Debug.Log("currentDistance:" + currentDistance);
        //Debug.Log("eulerAngles:" + rotation.eulerAngles);

        if (camSlider != null)
        {
            camSlider.value -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * zoomRate * 100f;
            desiredDistance = camSlider.value;
        }


        position = target.position - (rotation * Vector3.forward * currentDistance + targetOffset);
        transform.position = position;


        //Debug.LogError("dis:" + Vector3.Distance(target.transform.position, this.gameObject.transform.position));
    }

    /// <summary>
    /// 角度限制
    /// </summary>
    /// <param name="angle"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
