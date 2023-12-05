using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticCollider : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        NewFishMove newFishMove = collision.GetComponent<NewFishMove>();
        if (newFishMove)
        {
            newFishMove.MoveTo(transform.position);
            newFishMove.MakeSmall();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        NewFishMove newFishMove = collision.GetComponent<NewFishMove>();
        if (newFishMove)
        {
            newFishMove.MakeBig();
        }
    }
}
