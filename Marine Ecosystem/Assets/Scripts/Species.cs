using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Species
{
    Clownfish,
    MoorishIdol,
    YellowTang,
    StaghornCoral,
    YellowTubeSponge,
    BlacktipReefShark,
    Parrotfish,
    SeaGrass,
    TableCoral,
    Damselfish,
    Butterflyfish,
    WhitetipReefShark
}

public static class SpeciesPopulation 
{
    public static Dictionary<Species, int> EntityPopulations = new Dictionary<Species, int>();
}


