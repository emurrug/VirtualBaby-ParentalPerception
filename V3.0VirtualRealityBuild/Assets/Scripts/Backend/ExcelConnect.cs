using System.IO;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class ExcelConnect : MonoBehaviour
{

	public string timenow; //time in ms since beginning of game
	public string blockname; //references AdvanceScenes script to get block info 
	public string trial; // the number of car loops (including "hits" and "misses")
	public string carspeed; //the current speed of the car
	public string responsename; //references player actions
	public string eventname; //references game actions

    public string gaze; //direction of sub's eye gaze (in coordinates on a screen)
    public string RHandRot; //xyz rotational vector or R Hand
    public string LHandRot; //xyz rotational vector of L Hand
    public string RHandLoc; //xyz transpotion vector of R Hand
    public string LHandLoc; //xyz transpotion vector of L Hand
    public string headRot; //xyz rotational vector of center eye
    public string headLoc; //xyz transposition vector of center eye

    private List<string[]> rowData = new List<string[]>();
	string[] rowDataTemp = new string[13];


	void Start()
	{

		//create first row of titles and initializing variables
		rowDataTemp[0] = "timenow";
		rowDataTemp[1] = "blockname";
		rowDataTemp[2] = "trial";
		rowDataTemp[3] = "carspeed";
		rowDataTemp[4] = "responsename";
		rowDataTemp[5] = "eventname";
        rowDataTemp[6] = "gaze";
        rowDataTemp[7] = "RHandRot";
        rowDataTemp[8] = "LHandRot";
        rowDataTemp[9] = "RHandLoc";
        rowDataTemp[10] = "LHandLoc";
        rowDataTemp[11] = "headRot";
        rowDataTemp[12] = "headLoc";
        rowData.Add(rowDataTemp);
		Save();
	}

	public void Save()
	{
		//update row contents
		rowDataTemp = new string[13];
		rowDataTemp[0] = timenow;
		rowDataTemp[1] = blockname;
		rowDataTemp[2] = trial;
		rowDataTemp[3] = carspeed;
		rowDataTemp[4] = responsename;
		rowDataTemp[5] = eventname;
        rowDataTemp[6] = gaze;
        rowDataTemp[7] = RHandRot;
        rowDataTemp[8] = LHandRot;
        rowDataTemp[9] = RHandLoc;
        rowDataTemp[10] = LHandLoc;
        rowDataTemp[11] = headRot;
        rowDataTemp[12] = headLoc;
        rowData.Add(rowDataTemp);

		//update row and column index
		string[][] output = new string[rowData.Count][];
		for (int i = 0; i < output.Length; i++)
		{
			output[i] = rowData[i];
		}
		int length = output.GetLength(0);
		string delimiter = ",";
		StringBuilder sb = new StringBuilder();
		for (int index = 0; index < length; index++)
			sb.AppendLine(string.Join(delimiter, output[index]));
//EM: a temp file path while coding from home...
        StreamWriter outStream = System.IO.File.CreateText("C:/Users/Virtual Baby/Documents/VirtualBaby-ParentPerception/Data Analysis/V3.0VirtualReality/" + blockname + ".csv");
		outStream.WriteLine(sb);
		outStream.Close();
	}


	void Update()
	{
		timenow = CombinedData.TIMENOW;
		blockname = CombinedData.BLOCKNAME;
		trial = CombinedData.TRIAL;
		carspeed = CombinedData.CARSPEED;
		eventname = CombinedData.EVENTNAME;
		responsename = CombinedData.RESPONSENAME;
        gaze = CombinedData.GAZE;
        RHandRot = CombinedData.RHANDROT;
        LHandRot = CombinedData.LHANDROT;
        RHandLoc = CombinedData.RHANDLOC;
        LHandLoc = CombinedData.LHANDLOC;
        headRot = CombinedData.HEADROT;
        headLoc = CombinedData.HEADLOC;
	}

}