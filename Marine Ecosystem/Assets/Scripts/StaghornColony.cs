using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaghornColony : Producer
{
    public enum ColonySize { SMALL = 4, MEDIUM = 6, LARGE = 8 } // actual size is x sqrd.
    public ColonySize colonySize;

    public GameObject staghornCoral;

    private float minScale = 0.75f;
    private float maxScale = 1.25f;

    private int minRotation = 0;
    private int maxRotation = 360;

    private float maxSpacing = 1.5f;

    protected override void Init()
    {
        for(int x = 0; x < (int)colonySize; x++)
        {
            for (int z = 0; z < (int)colonySize; z++)
            {
                float spacing = Random.Range(1.0f, maxSpacing);
                GameObject coral = Instantiate(staghornCoral, 
                    new Vector3(Random.Range(-(int)colonySize * spacing, (int)colonySize * spacing), transform.position.y, Random.Range(-(int)colonySize * spacing, (int)colonySize * spacing)), 
                    Quaternion.Euler(Quaternion.identity.x, Random.Range(minRotation, maxRotation), Quaternion.identity.z));

                coral.transform.localScale = transform.localScale * Random.Range(minScale, maxScale);

                coral.transform.parent = transform;
            }
        }

        AddCollider();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void AddCollider()
    {
        float width = ((int)colonySize * 2) * ((1.0f + maxSpacing) / 2);
        float height = 3.0f;

        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size = new Vector3(width, height, width);
        collider.center = new Vector3(0, height / 2, 0);

    }
}
