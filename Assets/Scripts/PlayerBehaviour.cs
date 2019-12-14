using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour, IGroundLayerInteraction
{
    [Header("Player Attributes")]
    public float speed;

    [Header("Camera Settings")]
    public GameObject mainCamera;
    public float smoothing;
    public Vector3 offset;

    [Header("Unity settings")]
    public LayerMask ground;
 
    private SmoothFollow _cameraMovement;
    private Rigidbody _rb;
    private Vector3 _motionDirection = Vector3.forward;

    [SerializeField]
    private enum Direction { Forward, Right };

    [SerializeField]
    private Direction _currentDirection = Direction.Forward;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _cameraMovement = mainCamera.GetComponent<SmoothFollow>();
    }

    void Update()
    {
        CheckGroundLayer(transform, ground);

        if(Input.touchCount > 0 && GameManager.instance.gameStarted)
        {
           Touch touch = Input.GetTouch(0);
           if (touch.phase == TouchPhase.Began)
           {
              ChangeDirection();
           }
        }
    }

    private void FixedUpdate()
    {
        ApplyMovement(_motionDirection);

        _cameraMovement.Set(transform, offset, smoothing);
    }

    private void ApplyMovement(Vector3 direction)
    {
        _rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
    }

    public void ChangeDirection()
    {
        switch (_currentDirection)
        {
            case Direction.Forward:
                _currentDirection = Direction.Right;
                _motionDirection = Vector3.right;
                break;

            case Direction.Right:
                _currentDirection = Direction.Forward;
                _motionDirection = Vector3.forward;
                break;
        }
    }

    public void CheckGroundLayer(Transform current, LayerMask ground)
    {

        Collider[] colliders = Physics.OverlapSphere(current.position, current.localScale.x /2, ground);

        bool isGrounded = colliders.Length != 0 ? true : false;

        if (!isGrounded)
        {
            GameManager.instance.gameIsOver = true;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x / 2);
    }
}
