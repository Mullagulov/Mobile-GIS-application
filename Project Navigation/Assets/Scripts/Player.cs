
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed; // Скорость ходьбы

    [SerializeField]
    private float _gravity; // Гравитация

    [SerializeField]
    private Joystick _joystick; // Джойстик

    [SerializeField]
    private bool _isMobileDevice = false; // Флаг мобильного устройства

    private bool _isTeleporting = false; // Флаг телепортации

    private CharacterController _characterController; // Доступ к персонажу

    private LineRenderer lineRenderer; // Доступ к линии

    private Vector3 _moveDirection; // Направление ходьбы
    private Vector3 _verticalSpeed; // Вертикальная скорость
    private Vector3 _teleportDestination; // Место назначения телепортации

    // Start is called before the first frame update
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();

        // Проверка условия является ли устройство мобильным
        if (!_isMobileDevice)
        {
            _joystick.gameObject.SetActive(false); // Скрыть джойстики на десктопе
        }

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false; // Изначально линия не отображается
    }

    // Update is called once per frame
    private void Update()
    {
        float horizontalInput = 0.0f;
        float verticalInput = 0.0f;

        // Проверка условия является ли устройство мобильным
        // Получаем ввод от пользователя для определения направления движения
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

    // Функция для передвижения
    private void Move(Vector3 direction)
    {
        _characterController.Move(direction * _moveSpeed * Time.fixedDeltaTime);
    }

    // Функция для гравитации
    private void GravityApply(bool isGrounded)
    {
        if (isGrounded && _verticalSpeed.y < 0)
        {
            _verticalSpeed.y = -1f;
        }

        _verticalSpeed.y -= _gravity * Time.fixedDeltaTime;
        _characterController.Move(_verticalSpeed * Time.fixedDeltaTime);
    }

    // Функция для локализации к объекту (Вызов через OnClick)
    public void TeleportToDestination(Transform destinationObject)
    {
        Teleport(destinationObject.position); // Укажите место назначения телепортации
    }

    // Функция для локализации
    public void Teleport(Vector3 destination)
    {
        _isTeleporting = true;
        _teleportDestination = destination;

        StartCoroutine(TeleportRoutine());
    }

    // Функция для локализации
    private IEnumerator TeleportRoutine()
    {
        // Отключение управление персонажем на время телепортации
        _characterController.enabled = false;

        // Телепортируем персонажа
        transform.position = _teleportDestination;

        // Включение управление персонажем после телепортации
        _characterController.enabled = true;

        _isTeleporting = false;

        yield return null;
    }

    // Функция для навигации (Вызов через onClick)
    public void CalculatePath(Transform target)
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path); // Рассчитываем путь по NavMesh

        lineRenderer.positionCount = path.corners.Length; // Устанавливаем количество точек на линии
        lineRenderer.SetPositions(path.corners); // Устанавливаем позиции точек пути

        lineRenderer.enabled = true; // Показываем линию
    }
}