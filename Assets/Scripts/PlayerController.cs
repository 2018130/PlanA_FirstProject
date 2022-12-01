using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector3 destinationPos = Vector3.zero;
    [SerializeField]
    float speed = 10.0f;

    // Update is called once per frame
    void Update()
    {

        //마우스가 클릭된 위치로 플레이어의 위치를 이동시키는 코드 입니다.
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            destinationPos = Camera.main.ScreenToWorldPoint(mousePos);
            destinationPos.z = 0;
        }

        if(Vector3.Distance(destinationPos, transform.position) >= 0.1f)
        {
            Vector3 dirVector = (destinationPos - transform.position).normalized;
            GetComponent<Rigidbody2D>().MovePosition(transform.position + dirVector * Time.deltaTime * speed);
        }
    }
}
