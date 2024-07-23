
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed; // �������� ������

    [SerializeField]
    private float _gravity; // ����������

    [SerializeField]
    private Joystick _joystick; // ��������

    [SerializeField]
    private bool _isMobileDevice = false; // ���� ���������� ����������

    private bool _isTeleporting = false; // ���� ������������

    private CharacterController _characterController; // ������ � ���������

    private LineRenderer lineRenderer; // ������ � �����

    private Vector3 _moveDirection; // ����������� ������
    private Vector3 _verticalSpeed; // ������������ ��������
    private Vector3 _teleportDestination; // ����� ���������� ������������

    // Start is called before the first frame update
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();

        // �������� ������� �������� �� ���������� ���������
        if (!_isMobileDevice)
        {
            _joystick.gameObject.SetActive(false); // ������ ��������� �� ��������
        }

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false; // ���������� ����� �� ������������
    }

    // Update is called once per frame
    private void Update()
    {
        float horizontalInput = 0.0f;
        float verticalInput = 0.0f;

        // �������� ������� �������� �� ���������� ���������
        // �������� ���� �� ������������ ��� ����������� ����������� ��������
        if (_isMobileDevice)
        {
            horizontalInput = _joystick.Horizontal;
            verticalInput = _joystick.Vertical;
        }

        else
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
        }

        _moveDirection = transform.right * horizontalInput + transform.forward * verticalInput;
    }

    private void FixedUpdate()
    {
        if (!_isTeleporting)
        {
            Move(_moveDirection);
            GravityApply(_characterController.isGrounded);
            
        }
    }

    // ������� ��� ������������
    private void Move(Vector3 direction)
    {
        _characterController.Move(direction * _moveSpeed * Time.fixedDeltaTime);
    }

    // ������� ��� ����������
    private void GravityApply(bool isGrounded)
    {
        if (isGrounded && _verticalSpeed.y < 0)
        {
            _verticalSpeed.y = -1f;
        }

        _verticalSpeed.y -= _gravity * Time.fixedDeltaTime;
        _characterController.Move(_verticalSpeed * Time.fixedDeltaTime);
    }

    // ������� ��� ����������� � ������� (����� ����� OnClick)
    public void TeleportToDestination(Transform destinationObject)
    {
        Teleport(destinationObject.position); // ������� ����� ���������� ������������
    }

    // ������� ��� �����������
    public void Teleport(Vector3 destination)
    {
        _isTeleporting = true;
        _teleportDestination = destination;

        StartCoroutine(TeleportRoutine());
    }

    // ������� ��� �����������
    private IEnumerator TeleportRoutine()
    {
        // ���������� ���������� ���������� �� ����� ������������
        _characterController.enabled = false;

        // ������������� ���������
        transform.position = _teleportDestination;

        // ��������� ���������� ���������� ����� ������������
        _characterController.enabled = true;

        _isTeleporting = false;

        yield return null;
    }

    // ������� ��� ��������� (����� ����� onClick)
    public void CalculatePath(Transform target)
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path); // ������������ ���� �� NavMesh

        lineRenderer.positionCount = path.corners.Length; // ������������� ���������� ����� �� �����
        lineRenderer.SetPositions(path.corners); // ������������� ������� ����� ����

        lineRenderer.enabled = true; // ���������� �����
    }
}