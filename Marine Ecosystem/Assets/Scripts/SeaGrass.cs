using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaGrass : Producer
{
    protected override void Update()
    {
        base.Update();
    }

    protected override void Die(CauseOfDeath cause)
    {
        SeaGrassSpawner.Instance.SpawnGrass();
        base.Die(cause);
    }
}
