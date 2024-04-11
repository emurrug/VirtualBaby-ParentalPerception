using UnityEngine;

//this script keeps all variables the same (unless changed) across the many game scenes
//this is helpful for making sure things like ID and COND stay constant
//DBConnect will reference this script to pull the most current variable


public static class CombinedData
{

    private static string timenow; //counts time in milliseconds since beginning of game
    private static string id = "NA";//subject ID
    private static string subcondition = "NA"; //subject condition (currently no script exists for alternate conditions)
    private static string blockname = "NA"; //references AdvanceScenes script to get block info 
    private static string trial = "NA";
    private static string carspeed = "NA";
    private static string babylocation = "NA";
    private static string responsename = "NA"; //references player actions
    private static string eventname = "NA"; //references game actions
    private static string mobility = "NA"; //baby randomizer 
    private static Texture clothesTexture; //color of baby's clothes
    private static string clothes = "NA";

    public static string TIMENOW
    {
        get { return timenow; }
        set { timenow = value; }
    }
    public static string ID
    {
        get {return id;}
        set {id = value;}
    }
    public static string CONDITION
    {
        get { return subcondition; }
        set { subcondition = value; }
    }
    public static string BLOCKNAME
    {
        get { return blockname; }
        set { blockname = value; }
    }
    public static string TRIAL
    {
        get { return trial; }
        set { trial = value; }
    }
    public static string CARSPEED
    {
        get { return carspeed; }
        set { carspeed = value; }
    }
    public static string BABYLOCATION
    {
        get { return babylocation; }
        set { babylocation = value; }
    }
    public static string RESPONSENAME
    {
        get { return responsename; }
        set { responsename = value; }
    }
    public static string EVENTNAME
    {
        get { return eventname; }
        set { eventname = value; }
    }
    public static string MOBILITY
    {
        get { return mobility; }
        set { mobility = value; }
    }
    public static Texture CLOTHESTEXTURE
    {
        get { return clothesTexture; }
        set { clothesTexture = value; }
    }
    public static string CLOTHES
    {
        get { return clothes; }
        set { clothes = value; }
    }
}

