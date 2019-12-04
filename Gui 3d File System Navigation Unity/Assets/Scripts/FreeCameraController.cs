using UnityEngine;

/**
 * FreeCameraController
 */
public class FreeCameraController : MonoBehaviour
{
    // model
    public Transform model;
    // rotatespeed
    public float rotateSpeed = 32f;
    public float rotateLerp = 8;
    // movespeed
    public float moveSpeed = 1f;
    public float moveLerp = 10f;
    // camera zoom speed
    public float zoomSpeed = 10f;
    public float zoomLerp = 4f;

    // position 
    private Vector3 position, targetPosition;
    // rotation 
    private Quaternion rotation, targetRotation;
    // distance
    private float distance, targetDistance;
    // default distance
    private const float default_distance = 5f;
    // rotation range of y
    private const float min_angle_y = -89f;
    private const float max_angle_y = 89f;


    // Use this for initialization
    void Start()
    {

        
        targetRotation = Quaternion.identity;
        // 初始位置是模型
        targetPosition = model.position;
        // 初始镜头拉伸
        targetDistance = default_distance;
    }

    // Update is called once per frame
    void Update()
    {
        float dx = Input.GetAxis("Mouse X");
        float dy = Input.GetAxis("Mouse Y");

        if (Mathf.Abs(dx) > 5f || Mathf.Abs(dy) > 5f)
        {
            return;
        }

        float d_target_distance = targetDistance;
        if (d_target_distance < 2f)
        {
            d_target_distance = 2f;
        }

        // left-click move
        if (Input.GetMouseButton(0))
        {
            dx *= moveSpeed * d_target_distance / default_distance;
            dy *= moveSpeed * d_target_distance / default_distance;
            targetPosition -= transform.up * dy + transform.right * dx;
        }

        // right-click rotate
        if (Input.GetMouseButton(1))
        {
            dx *= rotateSpeed;
            dy *= rotateSpeed;
            if (Mathf.Abs(dx) > 0 || Mathf.Abs(dy) > 0)
            {
                
                Vector3 angles = transform.rotation.eulerAngles;
                
                angles.x = Mathf.Repeat(angles.x + 180f, 360f) - 180f;
                angles.y += dx;
                angles.x -= dy;
                angles.x = ClampAngle(angles.x, min_angle_y, max_angle_y);
                
                targetRotation.eulerAngles = new Vector3(angles.x, angles.y, 0);


                Vector3 temp_position =
                        Vector3.Lerp(targetPosition, model.position, Time.deltaTime * moveLerp);
                targetPosition = Vector3.Lerp(targetPosition, temp_position, Time.deltaTime * moveLerp);
            }
        }

        // move up
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            targetPosition -= transform.up * d_target_distance / (2f * default_distance);
        }

        // move down
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            targetPosition += transform.up * d_target_distance / (2f * default_distance);
        }

        // move left
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            targetPosition += transform.right * d_target_distance / (2f * default_distance);
        }

        // move right
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            targetPosition -= transform.right * d_target_distance / (2f * default_distance);
        }

        
        targetDistance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
    }

    
    float ClampAngle(float angle, float min, float max)
    {
        
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }

    private void FixedUpdate()
    {
        rotation = Quaternion.Slerp(rotation, targetRotation, Time.deltaTime * rotateLerp);
        position = Vector3.Lerp(position, targetPosition, Time.deltaTime * moveLerp);
        distance = Mathf.Lerp(distance, targetDistance, Time.deltaTime * zoomLerp);
        
        transform.rotation = rotation;
        
        transform.position = position - rotation * new Vector3(0, 0, distance);
    }
}