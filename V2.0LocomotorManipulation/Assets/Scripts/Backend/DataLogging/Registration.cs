
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class Registration : MonoBehaviour
{
    public InputField IdField; //subject ID
    public InputField ConditionField; //subject condition
    public Button SubmitButton; //submits ID and condition

    public string ID;
    public string CONDITION;
    public string BLOCKNAME;
    public string EVENTNAME;

    public ExcelConnect excelconnect;
    public AdvanceScenes advancescenes;

    void Start()
    {
        Button submit = SubmitButton.GetComponent<Button>();
        submit.onClick.AddListener(InsertRow);

        advancescenes.CurrentBlock();
        BLOCKNAME = advancescenes.block;
        EVENTNAME = "Start New Block";
        StartCoroutine("Insert");
    }

    void Update()
    {
        ID = IdField.text;
        CONDITION = ConditionField.text;

        CombinedData.ID = ID;
        CombinedData.CONDITION = CONDITION;
        CombinedData.BLOCKNAME = BLOCKNAME;
        CombinedData.EVENTNAME = EVENTNAME;
    }

    public void InsertRow()
    {
        StartCoroutine("Insert");
    }
    IEnumerator Insert()
    {
        yield return new WaitForFixedUpdate(); //added redunandance for peace of mind
        excelconnect.Save();
    }
}
