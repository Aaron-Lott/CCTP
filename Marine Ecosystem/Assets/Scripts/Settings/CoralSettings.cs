using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Coral Settings")]
public class CoralSettings : ProducerSettings
{
    [Tooltip("Does the coral glow when sea temperatures rise?")]
    public bool doesFluoresce;

    [Tooltip("The colors the coral glows when sea temperatures rise.")]
    public Color[] fluorescentColors; //#003af0 BLUE //#fefe06 YELLOW //#9007f9 PURPLE

    public Color FluoresceBlue()
    {
        return new Color(0f, 0.227f, 0.941f);
    }

    public Color FluoresceYellow()
    {
        return new Color(0.996f, 0.996f, 0.024f);
    }

    public Color FluorescePurple()
    {
        return new Color(0.565f, 0.027f, 0.976f);
    }

    public Color DeadColor()
    {
        return new Color(0.8f, 0.8f, 0.8f);
    }
}
