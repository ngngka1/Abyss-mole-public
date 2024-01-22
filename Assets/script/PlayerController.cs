using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    public string Name;

    public float spriteWidth;

    public float spriteHeight;

    private bool isSprinting;

    public bool isMoving;

    public bool disableMovement = false;

    public Vector3 facingDirection;

    private Vector2 input;

    private Animator animator;
    private Light2D lightGlobal;
    public float lightIntensity;
    public float lightOuterRadius;

    public LayerMask solidObjectsLayer;
    public LayerMask interactablesLayer;

    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        animator = GetComponent<Animator>();
        lightGlobal = GetComponent<Light2D>();
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteWidth = spriteRenderer.size.x;
        spriteHeight = spriteRenderer.size.y;
        lightGlobal.intensity = lightIntensity;
        lightGlobal.pointLightOuterRadius = lightOuterRadius;
    }

    // Update is called once per frame
    public void HandleUpdate()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal") / 2;
            input.y = Input.GetAxisRaw("Vertical") / 2;

            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero) // this means: if sth is in input (arrow key input)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;
                
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    isSprinting = true;
                }

                if (IsWalkable(targetPos))
                    StartCoroutine(Move(targetPos));
            }
        }

        animator.SetBool("isMoving", isMoving);

        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            if (disableMovement)
            {
                isSprinting = false;
                isMoving = false;
                yield break;
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Mathf.Pow(2, System.Convert.ToInt32(isSprinting)) * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isSprinting = false;
        isMoving = false;
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer | interactablesLayer) != null)
        {
            return false;
        }
        return true;
    }

    public IEnumerator Teleport(Vector3 targetPos) // room swap for rooms in one scene
    {
        while (isMoving)
        {
            yield return new WaitForEndOfFrame();
        }
        transform.position = targetPos;
    }

    void Interact()
    {
        facingDirection = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDirection;

        var collider = Physics2D.OverlapCircle(interactPos, 0.2f, interactablesLayer);
        if (collider)
        {
            collider.GetComponent<Interactable>()?.Interact(); // get the collided component, if it's interactable, call its interact function
        }
    }
}
