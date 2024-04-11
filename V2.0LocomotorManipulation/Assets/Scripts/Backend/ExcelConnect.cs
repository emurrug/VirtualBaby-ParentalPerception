using System.IO;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


public class ExcelConnect : MonoBehaviour
{

	public string timenow; //time in ms since beginning of game
	public string id;//subject ID
	public string subcondition; //subject condition (currently no script exists for alternate conditions)
	public string blockname; //references AdvanceScenes script to get block info 
	public string trial; // the number of car loops (including "hits" and "misses")
	public string carspeed; //the current speed of the car
	public string babylocation; //whether the baby is on blanket, grass, or road
	public string responsename; //references player actions
	public string eventname; //references game actions
	public string mobility; //baby randomizer 
	public string clothes; //color of baby's clothes

	private List<string[]> rowData = new List<string[]>();
	string[] rowDataTemp = new string[11];


	void Start()
	{

		//create first row of titles and initializing variables
		rowDataTemp[0] = "timenow";
		rowDataTemp[1] = "id";
		rowDataTemp[2] = "subcondition";
		rowDataTemp[3] = "blockname";
		rowDataTemp[4] = "trial";
		rowDataTemp[5] = "carspeed";
		rowDataTemp[6] = "babylocation";
		rowDataTemp[7] = "responsename";
		rowDataTemp[8] = "eventname";
		rowDataTemp[9] = "mobility";
		rowDataTemp[10] = "clothes";
		rowData.Add(rowDataTemp);
		Save();
	}

	public void Save()
	{
		//update row contents
		rowDataTemp = new string[11];
		rowDataTemp[0] = timenow;
		rowDataTemp[1] = id;
		rowDataTemp[2] = subcondition;
		rowDataTemp[3] = blockname;
		rowDataTemp[4] = trial;
		rowDataTemp[5] = carspeed;
		rowDataTemp[6] = babylocation;
		rowDataTemp[7] = responsename;
		rowDataTemp[8] = eventname;
		rowDataTemp[9] = mobility;
		rowDataTemp[10] = clothes;
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

		StreamWriter outStream = System.IO.File.CreateText("C:/Users/zztop/Documents/Data/Unityoutput/subject" +  id + "." + blockname + mobility +  ".csv");
		outStream.WriteLine(sb);
		outStream.Close();
	}


	void Update()
	{
		timenow = CombinedData.TIMENOW;
		id = CombinedData.ID;
		subcondition = CombinedData.CONDITION;
		blockname = CombinedData.BLOCKNAME;
		trial = CombinedData.TRIAL;
		carspeed = CombinedData.CARSPEED;
		babylocation = CombinedData.BABYLOCATION;
		eventname = CombinedData.EVENTNAME;
		responsename = CombinedData.RESPONSENAME;
		mobility = CombinedData.MOBILITY;
		clothes = CombinedData.CLOTHES;
	}

}