using UnityEngine;
using System.Collections;

public class TP_Camera : MonoBehaviour 
{
    public static TP_Camera Instance;
    public Transform TargetLookAt;
    public float Distance = 5f;
    public float DistanceMin = 1f;
    public float DistanceMax = 10f;
    public float DistanceSmooth = 0.05f;
    public float DistanceResumeSmooth = 1f;
    public float X_MouseSensitivity = 10f;
    public float Y_MouseSensitivity = 10f;
    public float MouseWheelSensitivity = 5f;
    public float X_Smooth = 0.05f;
    public float Y_Smooth = 0.01f;
    public float Y_MinLimit = -40f;
    public float Y_MaxLimit = 80f;
    public float OcclusionDistanceStep = 0.5f;
    public int MaxOcclusionChecks = 10;

    private float mouseX = 0f;
    private float mouseY = 0f;
    private float velocityX = 0f;
    private float velocityY = 0f;
    private float velocityZ = 0f;
    private float velocityDistance = 0f;
    private float startDistance = 0f;
    private Vector3 desiredPosition = Vector3.zero;
    private Vector3 position = Vector3.zero;
    private float desiredDistance = 0f;
    private float distanceSmooth = 0f;
    private float preOccludedDistance = 0f;

	void Awake() 
    {
        Instance = this;
	}

    void Start()
    {
        Distance = Mathf.Clamp(Distance, DistanceMin, DistanceMax);
        startDistance = Distance;
        Reset();
    }
	
    void Update()
    {
        if (TargetLookAt == null)
            return;

        HandlePlayerInput();

        int count = 0;
        do 
        {
            CalculateDesiredPosition();
            count++;
        } while (CheckIfOccluded(count));

        UpdatePosition();
    }

    void HandlePlayerInput()
    {
        float deadZone = 0.01f;
        //if (Input.GetMouseButton(1))
        //{
            mouseX += Input.GetAxis("Mouse X") * X_MouseSensitivity;
            mouseY -= Input.GetAxis("Mouse Y") * Y_MouseSensitivity;
        //}

        mouseY = Helper.ClampAngle(mouseY, Y_MinLimit, Y_MaxLimit);

        if (Input.GetAxis("Mouse ScrollWheel") < -deadZone ||
            Input.GetAxis("Mouse ScrollWheel") > deadZone)
        {
            desiredDistance = Mathf.Clamp(Distance - Input.GetAxis("Mouse ScrollWheel") * MouseWheelSensitivity,
                                          DistanceMin, DistanceMax);
            preOccludedDistance = desiredDistance;
            distanceSmooth = DistanceSmooth;
        }
    }

    void CalculateDesiredPosition()
    {
        ResetDesiredDistance();
        Distance = Mathf.SmoothDamp(Distance, desiredDistance, ref velocityDistance, distanceSmooth);
        desiredPosition = CalculatePosition(mouseY, mouseX, Distance);
    }

    Vector3 CalculatePosition(float rotationX, float rotationY, float distance)
    {
        Vector3 direction = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
        return TargetLookAt.position + rotation * direction;
    }

    bool CheckIfOccluded(int count)
    {
        bool isOccluded = false;
        float nearestDistance = CheckCameraPoints(TargetLookAt.position, desiredPosition);
        if (nearestDistance != -1)
        {
            if (count < MaxOcclusionChecks)
            {
                isOccluded = true;
                Distance -= OcclusionDistanceStep;

                if (Distance < 0.25f)
                    Distance = 0.25f;
            }
            else
            {
                Distance = nearestDistance - Camera.main.nearClipPlane;
            }

            desiredDistance = Distance;
            distanceSmooth = DistanceResumeSmooth;
        }
        return isOccluded;
    }

    float CheckCameraPoints(Vector3 from, Vector3 to)
    {
        float nearestDistance = -1f;
        RaycastHit hitInfo;
        Helper.ClipPlanePoints clipPlanePoints = Helper.ClipPlaneAtNear(to);
        
        if (Physics.Linecast(from, clipPlanePoints.UpperLeft, out hitInfo) && hitInfo.collider.tag != "Player")
            nearestDistance = hitInfo.distance;

        if (Physics.Linecast(from, clipPlanePoints.LowerLeft, out hitInfo) && hitInfo.collider.tag != "Player")
        {
            if (hitInfo.distance < nearestDistance || nearestDistance == -1)
                nearestDistance = hitInfo.distance;
        }
        if (Physics.Linecast(from, clipPlanePoints.UpperRight, out hitInfo) && hitInfo.collider.tag != "Player")
        {
            if (hitInfo.distance < nearestDistance || nearestDistance == -1)
                nearestDistance = hitInfo.distance;
        }
        if (Physics.Linecast(from, clipPlanePoints.LowerRight, out hitInfo) && hitInfo.collider.tag != "Player")
        {
            if (hitInfo.distance < nearestDistance || nearestDistance == -1)
                nearestDistance = hitInfo.distance;
        }
        if (Physics.Linecast(from, to + transform.forward * -GetComponent<Camera>().nearClipPlane, out hitInfo) && hitInfo.collider.tag != "Player")
        {
            if (hitInfo.distance < nearestDistance || nearestDistance == -1)
                nearestDistance = hitInfo.distance;
        }

        return nearestDistance;
    }

    void ResetDesiredDistance()
    {
        if (desiredDistance < preOccludedDistance)
        {
            var pos = CalculatePosition(mouseY, mouseX, preOccludedDistance);
            var nearestDistance = CheckCameraPoints(TargetLookAt.position, pos);
            if (nearestDistance == -1 || nearestDistance > preOccludedDistance)
            {
                desiredDistance = preOccludedDistance;
            }
        }
    }

    void UpdatePosition()
    {
        float posX = Mathf.SmoothDamp(position.x, desiredPosition.x, ref velocityX, X_Smooth);
        float posY = Mathf.SmoothDamp(position.y, desiredPosition.y, ref velocityY, Y_Smooth);
        float posZ = Mathf.SmoothDamp(position.z, desiredPosition.z, ref velocityZ, X_Smooth);
        position = new Vector3(posX, posY, posZ);

        transform.position = position;

        transform.LookAt(TargetLookAt);
    }

    public void Reset()
    {
        mouseX = 0;
        mouseY = 10;
        Distance = startDistance;
        desiredDistance = Distance;
        preOccludedDistance = Distance;
    }
}
