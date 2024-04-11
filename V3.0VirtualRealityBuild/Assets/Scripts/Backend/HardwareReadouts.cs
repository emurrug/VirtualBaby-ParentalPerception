using System.IO;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

//In the old version of "ExcelConnect", real time data such as baby location was recorded.
//Now real-time updates will be recorded within this new excel readout (sampled 1fps)
//ExcelConnect will contain identifiers and triggered event logging
//While HardwareReadout will contain time and and stable sampling

public class HardwareReadouts : MonoBehaviour
{

    public string timenow; //time in ms since beginning of game
    public string blockname;
    public string babylocation; //whether the baby is on blanket, grass, or road
    public string gaze; //direction of sub's eye gaze (in coordinates on a screen)
    public string gazeCategory; //a categorical assignment of what object is in the center of subjects gaze (fovea generated as X)
    public string RHandRot; //xyz rotational vector or R Hand
    public string LHandRot; //xyz rotational vector of L Hand
    public string RHandLoc; //xyz transpotion vector of R Hand
    public string LHandLoc; //xyz transpotion vector of L Hand
    public string headRot; //xyz rotational vector of center eye
    public string headLoc; //xyz transposition vector of center eye

    public bool isRewinding;
    public float elapsedTime;

    private List<string[]> rowData = new List<string[]>();
    string[] rowDataTemp = new string[10];

    // Start is called before the first frame update
    void Start()
    {
        //create first row of titles and initializing variables
        //create first row of titles and initializing variables
        rowDataTemp[0] = "timenow";
        rowDataTemp[1] = "babylocation";
        rowDataTemp[2] = "gaze";
        rowDataTemp[3] = "gazeCategory";
        rowDataTemp[4] = "RHandRot";
        rowDataTemp[5] = "LHandRot";
        rowDataTemp[6] = "RHandLoc";
        rowDataTemp[7] = "LHandLoc";
        rowDataTemp[8] = "headRot";
        rowDataTemp[9] = "headLoc";
        rowData.Add(rowDataTemp);
        Save();
        isRewinding = false;
        StartCoroutine("SaveEvery10ms");
    }

	public void Save()
	{
		//update row contents
		rowDataTemp = new string[10];
        rowDataTemp[0] = timenow;
        rowDataTemp[1] = babylocation;
        rowDataTemp[2] = gaze;
        rowDataTemp[3] = gazeCategory;
        rowDataTemp[4] = RHandRot;
        rowDataTemp[5] = LHandRot;
        rowDataTemp[6] = RHandLoc;
        rowDataTemp[7] = LHandLoc;
        rowDataTemp[8] = headRot;
        rowDataTemp[9] = headLoc;
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

		StreamWriter outStream = System.IO.File.CreateText("C:/Users/Virtual Baby/Documents/VirtualBaby-ParentPerception/Data Analysis/V3.0VirtualReality/HarwareReadout_" + blockname + ".txt");
		outStream.WriteLine(sb);
		outStream.Close();
	}


	void Update()
	{
		timenow = CombinedData.TIMENOW;
        blockname = CombinedData.BLOCKNAME;
        babylocation = CombinedData.BABYLOCATION;
		gaze = CombinedData.GAZE;
		gazeCategory = CombinedData.GAZECATEGORY;
		RHandRot = CombinedData.RHANDROT;
		LHandRot = CombinedData.LHANDROT;
		RHandLoc = CombinedData.RHANDLOC;
		LHandLoc = CombinedData.LHANDLOC;
        headRot = CombinedData.HEADROT;
        headLoc= CombinedData.HEADLOC;

        if (isRewinding == false)
        {
            StartCoroutine("RewindCoroutine");
        }
    }

    IEnumerator SaveEvery10ms()
    {
        while (true)
        {
            yield return new WaitForSeconds(.05f);
            if (isRewinding = false)
                Save();
        }
    }
    IEnumerator RewindCoroutine()
    {
        isRewinding = true;
        Save();
        elapsedTime = 0f;
        while (elapsedTime < 0.05f)
        {
            elapsedTime += Time.deltaTime;
            yield return null; 
        }
        isRewinding = false;

    }
}
