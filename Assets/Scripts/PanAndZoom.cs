using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.Serialization;

public class PanAndZoom : MonoBehaviour
{
    private CinemachineInputProvider _inputProvider;
    private CinemachineVirtualCamera _virtualCamera;
    private Transform _cameraTransform;
    
    public float panSpeed = 18f;
    public float zoomSpeed = 3f;
    private float _zoomInMax = 40f;
    private float _zoomOutMax = 90f;
    private void Awake()
    {
        _inputProvider = GetComponent<CinemachineInputProvider>();
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        _cameraTransform = _virtualCamera.VirtualCameraGameObject.transform;
    }

    public Vector2 PanDirection(float x, float y)
    {
        Vector2 direction = Vector2.zero;
        if (y >= Screen.height * .95f)
        {
            direction.y += 1;
        }
        else if (y <= Screen.height * 0.05f)
        {
            direction.y -= 1;
        }

        if (x >= Screen.width * 0.95f)
        {
            direction.x += 1;
        }
        else if (x <= Screen.width * 0.05f)
        {
            direction.x -= 1;
        }

        return direction;
    }

    private void ZoomScreen(float increment)
    {
        float fov = _virtualCamera.m_Lens.FieldOfView;
        float target = Mathf.Clamp(fov + increment, _zoomInMax, _zoomOutMax);

        _virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(fov, target, zoomSpeed * Time.deltaTime);
    }

    private void PanScreen(float x, float y)
    {
        Vector3 direction = PanDirection(x, y);
        var position = _cameraTransform.position;
        position = Vector3.Lerp(position, position + direction * panSpeed, Time.deltaTime);
        
        _cameraTransform.position = position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = _inputProvider.GetAxisValue(0);
        float y = _inputProvider.GetAxisValue(1);
        float z = _inputProvider.GetAxisValue(2);

        if (x != 0 || y != 0 )
        {
            PanScreen(x,y);
        }

        if (z != 0)
        {
            ZoomScreen(z);
        }
    }
}
