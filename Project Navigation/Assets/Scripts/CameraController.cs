using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float _sensitivity; // Чувствительность мыши

    [SerializeField]
    private float _sensitivityJoystick; // Чувствительность джойстика

    [SerializeField]
    private float _maxYAngle; // Максимальный угол наклона по оси Y
    
    private float rotationX = 0.0f;

    [SerializeField]
    private Joystick _joystick;

    [SerializeField]
    private bool _isMobileDevice = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!_isMobileDevice)
        {
            _joystick.gameObject.SetActive(false); // Скрыть джойстики на десктопе
        }
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = 0.0f;
        float mouseY = 0.0f;

        if (_isMobileDevice)
        {
            float joyX = _joystick.Horizontal;
            float joyY = _joystick.Vertical;

            if (Mathf.Abs(joyX) > 0.7f)
            {
                mouseX = joyX * _sensitivityJoystick;
            }

            if (Mathf.Abs(joyY) > 0.7f)
            {
                mouseY = joyY * _sensitivityJoystick;
            }
        }
        else
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
        }

        transform.parent.Rotate(Vector3.up * mouseX * _sensitivity);

        rotationX -= mouseY * _sensitivity;
        rotationX = Mathf.Clamp(rotationX, -_maxYAngle, _maxYAngle);
        transform.localRotation = Quaternion.Euler(rotationX, 0.0f, 0.0f);
    } 
}
