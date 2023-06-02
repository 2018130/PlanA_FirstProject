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
        //����� ��ǰ ġƮŰ
        if (Input.GetKeyDown(KeyCode.Y))
        {
            PlayYawnAnimation();
        }
    }


    //�����Ͽ콺 ���� �Լ� �Դϴ�.
    public void PlayYawnAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Yawn");
        }
    }

    //�����Ͽ콺 ���� �Լ� �Դϴ�.
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
