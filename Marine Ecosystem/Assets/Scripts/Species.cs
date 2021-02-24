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
    TableCoral
}

public static class SpeciesPopulation 
{
    //public static List<Fish> ClownfishPopulation = new List<Fish>();
    public static List<Fish> ParrotfishPopulation = new List<Fish>();
    public static Dictionary<Species, int> EntityPopulations = new Dictionary<Species, int>();
}


