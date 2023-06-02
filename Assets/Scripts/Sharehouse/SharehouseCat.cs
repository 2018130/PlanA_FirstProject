using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharehouseCat : MonoBehaviour
{
    int openedPanelCount = 0;

    Animator animator;
    Rigidbody2D rigidBody2d;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidBody2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //고양이 하품 치트키
        if (Input.GetKeyDown(KeyCode.Y))
        {
            PlayYawnAnimation();
        }
    }


    //쉐어하우스 전용 함수 입니다.
    public void PlayYawnAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Yawn");
        }
    }

    //쉐어하우스 전용 함수 입니다.
    public void PlayWalkingAnimation(bool state)
    {
        if (animator != null)
        {
            animator.SetBool("IsWalking", state);
        }
    }

    public void FlipX(int x)
    {
        float scaleX = Mathf.Abs(transform.localScale.x) * x;
        transform.localScale = new Vector3(scaleX,
            transform.localScale.y,
            transform.localScale.z);
    }
    public int GetLookAtDirectionX()
    {
        if(transform.localScale.x < 0)
        {
            return -1;
        }
        return 1;
    }

    public void AnyPaenlOpened()
    {
        openedPanelCount++;
    }

    public void AnyPaenlClosed()
    {
        openedPanelCount--;
    }

    public int GetOpenedPanelCount()
    {
        return openedPanelCount;
    }
}
