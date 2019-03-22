using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationIcon : MonoBehaviour
{
    private Animator animator;
    private bool selected = false;

    public bool IsSelected()
    {
        return selected;
    }

    public void SetSelected(bool inSelected)
    {
        selected = inSelected;

        if (animator == null)
            animator = GetComponent<Animator>();

        animator.SetBool("Selected", selected);
    }



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        if(animator == null)
            animator = GetComponent<Animator>();
    }
}
