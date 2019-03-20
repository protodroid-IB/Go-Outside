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

        animator.SetBool("Selected", selected);
    }



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }
}
