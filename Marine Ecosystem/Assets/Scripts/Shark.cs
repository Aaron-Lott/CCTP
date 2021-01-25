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
            }
            else
            {
                anim.SetBool("isAttacking", false);
            }
        }
    }
}
