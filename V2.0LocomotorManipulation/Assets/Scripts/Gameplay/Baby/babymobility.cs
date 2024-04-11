using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class babymobility : MonoBehaviour
{
    public GameObject babymodel;
    Renderer renderer;
    
    public Texture white;
    public Texture green;
    public Texture yellow;
    public Texture naked;

    public Texture clothes;
    public List<Texture> clothes1;
    public List<Texture> clothes2;
    public string mobility;
    public List<string> mobility1;
    public List<string> mobility2;

    private void Start()
    {
        mobility1 = StaticMobility.MOBILITY1;
        mobility2 = StaticMobility.MOBILITY2;
        clothes1 = StaticMobility.CLOTHES1;
        clothes2 = StaticMobility.CLOTHES2;

        mobility = StaticMobility.MOBILITY;
        clothes = StaticMobility.CLOTHES;        
        
        //sets the baby's clothes
        renderer = GetComponent<Renderer>();
        renderer.material.SetTexture("_MainTex", clothes);
    }

    public void AddClothingOptions()
    {
        //add to the static list the options we want to see
        //this should be called only in the first narrative 2 dialogue
        //so the functin command with go in the script "Narrative2" linked to a dialogue advance number
        StaticMobility.clothes1.Add(green);
        StaticMobility.clothes1.Add(yellow);
        StaticMobility.clothes1.Add(white);
        StaticMobility.clothes1.Add(naked);
    }

    public void GiveMeANewBaby() 
    {
        //keeps the lists accessible in a static class so they don't change everytime the scene resets
        //(note: this shouldn't be kept as an Update method
        //since this will create conflicts with "GetRidOfOption()"
        mobility1 = StaticMobility.MOBILITY1;
        mobility2 = StaticMobility.MOBILITY2;
        clothes1 = StaticMobility.CLOTHES1;
        clothes2 = StaticMobility.CLOTHES2;

        //gets called when the dialogue advance number at the first of each new tutorial (0, 6, 11)
        mobility = mobility1[Random.Range(0, (mobility1.Count))];
        clothes = clothes1[Random.Range(0, (clothes1.Count))];

        StaticMobility.CLOTHES = clothes;
        StaticMobility.MOBILITY = mobility;

        //sets the baby's clothes
        renderer = GetComponent<Renderer>();
        renderer.material.SetTexture("_MainTex", clothes);

        GetRidOfOption();
    }

    public void GetRidOfOption() 
    {
        //gets called at the end of each baby scene
        //makes sure to remove the index on the static list and not 
        //temp list being made here
        int mobilitylistindex = mobility1.IndexOf(mobility);
        string tempm = mobility1[mobilitylistindex];
            StaticMobility.mobility2.Add(tempm);
            StaticMobility.mobility1.RemoveAt(mobilitylistindex);

        int clotheslistindex = clothes1.IndexOf(clothes);
        Texture tempc = clothes1[clotheslistindex];
        StaticMobility.clothes2.Add(tempc);
        StaticMobility.clothes1.RemoveAt(clotheslistindex);
    }
}
