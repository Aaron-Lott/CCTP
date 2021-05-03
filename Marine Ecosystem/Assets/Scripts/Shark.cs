using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : Consumer
{
    protected override void Update()
    {
        base.Update();

        if(anim)
        {
            if (CurrentAction == CreatureAction.Foraging)
            {
                anim.SetBool("isAttacking", true);
                MoveSpeed = FastMoveSpeed;
            }
            else
            {
                anim.SetBool("isAttacking", false);
                MoveSpeed = Random.Range(BaseMoveSpeed * 0.8f, BaseMoveSpeed * 1.2f);
            }
        }
    }
}
