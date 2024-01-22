using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    [SerializeField] Dialog dialog;
    public void Interact()
    {
        animator.SetFloat("facedX", PlayerController.Instance.GetComponent<PlayerController>().facingDirection.x);
        animator.SetFloat("facedY", PlayerController.Instance.GetComponent<PlayerController>().facingDirection.y);
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
    }
}
