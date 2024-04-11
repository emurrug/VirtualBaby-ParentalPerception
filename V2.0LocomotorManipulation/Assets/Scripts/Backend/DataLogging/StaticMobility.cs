using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticMobility 
{
    //list to select the baby's animation at random
    public static string mobility;
    public static List<string> mobility1 = new List<string>() { "walker", "crawler", "precrawler" };
    public static List<string> mobility2 = new List<string>() { };

    //list to create a texture list
    public static Texture clothes;
    public static List<Texture> clothes1 = new List<Texture>() {};
    public static List<Texture> clothes2 = new List<Texture>() {};


    public static string MOBILITY
    {
        get { return mobility; }
        set { mobility = value; }
    }
    public static List<string> MOBILITY1
    {
        get { return mobility1; }
        set { mobility1 = value; }
    }
    public static List<string> MOBILITY2
    {
        get { return mobility2; }
        set { mobility2 = value; }
    }

    public static Texture CLOTHES
    {
        get { return clothes; }
        set { clothes = value; }
    }
    public static List<Texture> CLOTHES1
    {
        get { return clothes1; }
        set { clothes1= value; }
    }
    public static List<Texture> CLOTHES2
    {
        get { return clothes2; }
        set { clothes2 = value; }
    }
}
