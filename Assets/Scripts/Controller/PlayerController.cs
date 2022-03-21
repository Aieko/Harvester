using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    private Rigidbody RB;

    private Vector3 moveVector;
    private Animator animator;
    private GameManager gameManager;

    private bool isMoving;
    private bool isHarvesting;

    [Header("References")]
    [SerializeField] private GameObject scypheObject;
    [SerializeField] private Transform bag;
    [SerializeField] private VirtualJoystick joystick;
    [Header("Controller Settings")]
    [SerializeField] private float moveSpeed = 50;
    

    private float currentMoveSpeed;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        RB = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        scypheObject.SetActive(false);
        currentMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        CheckMovement();
        CheckAnimations();
    }

    private void CheckAnimations()
    {
        if(isMoving && !isHarvesting && RB.velocity.magnitude > 0)
        {
            animator.SetBool("Move", true);
            animator.SetBool("Idle", false);
        }
        else if(!isMoving && !isHarvesting)
        {
            animator.SetBool("Move", false);
            animator.SetBool("Idle", true);
        }
        else if(isHarvesting)
        {
            animator.SetBool("Move", false);
            animator.SetBool("Idle", false);
            animator.SetBool("Harvest", true);
        }
    }

    private void CheckMovement()
    {
        if (joystick.wasTouched)
        {
            moveVector = PoolInput();

            if (!isMoving) isMoving = true;
        }
        else
        {
            if (isMoving) isMoving = false;
        }
    }

    private void FixedUpdate()
    {
        if(isMoving && moveVector != Vector3.zero)
        {
            Move(moveVector);
        }
    }

    private void Move(Vector3 direction)
    {
        transform.forward = direction;

        RB.velocity = transform.forward * currentMoveSpeed * Time.deltaTime;       
    }

    private Vector3 PoolInput()
    {
        var direction = Vector3.zero;

        if(joystick.wasTouched)
        {
            direction.x = joystick.Horizontal();
            direction.z = joystick.Vertical();
        }
      
        if(direction.magnitude > 1)  direction.Normalize();

        return direction;

    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "WheatField")
        {
            isHarvesting = true;
            scypheObject.SetActive(true);
            currentMoveSpeed = moveSpeed/3;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "WheatField")
        {
            isHarvesting = false;
            scypheObject.SetActive(false);
            currentMoveSpeed = moveSpeed;
        }
    }
}
