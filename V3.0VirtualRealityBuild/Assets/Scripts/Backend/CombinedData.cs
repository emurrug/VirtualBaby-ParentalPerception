using UnityEngine;

//this script keeps all variables the same (unless changed) across the many game scenes
//this is helpful for making sure things like ID and COND stay constant
//DBConnect will reference this script to pull the most current variable


public static class CombinedData
{

    private static string timenow; //counts time in milliseconds since beginning of game
    private static string blockname = "NA"; //references AdvanceScenes script to get block info 
    private static string trial = "NA";
    private static string carspeed = "NA";
    private static string babylocation = "NA";
    private static string responsename = "NA"; //references player actions
    private static string eventname = "NA"; //references game actions

    public static string gaze = "NA"; //direction of sub's eye gaze (in coordinates on a screen)
    public static string gazeCategory = "NA"; //a categorical assignment of what object is in the center of subjects gaze (fovea generated as X)
    public static string RHandRot = "NA"; //xyz rotational vector or R Hand
    public static string LHandRot = "NA"; //xyz rotational vector of L Hand
    public static string RHandLoc = "NA"; //xyz transpotion vector of R Hand
    public static string LHandLoc = "NA"; //xyz transpotion vector of L Hand
    public static string headRot = "NA"; //xyz rotational vector of center eye
    public static string headLoc = "NA"; //xyz transposition vector of center eye

    public static string TIMENOW
    {
        get { return timenow; }
        set { timenow = value; }
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
    public static string GAZE
    {
        get { return gaze; }
        set { gaze = value; }
    }
    public static string GAZECATEGORY
    {
        get { return gazeCategory; }
        set { gazeCategory = value; }
    }
    public static string RHANDROT
    {
        get { return RHandRot; }
        set { RHandRot= value; }
    }
    public static string LHANDROT
    {
        get { return LHandRot; }
        set { LHandRot = value; }
    }
    public static string RHANDLOC
    {
        get { return RHandLoc; }
        set { RHandLoc = value; }
    }
    public static string LHANDLOC
    {
        get { return LHandLoc; }
        set { LHandLoc = value; }
    }
    public static string HEADROT
    {
        get { return headRot; }
        set { headRot = value; }
    }
    public static string HEADLOC
    {
        get { return headLoc; }
        set { headLoc = value; }
    }
}

