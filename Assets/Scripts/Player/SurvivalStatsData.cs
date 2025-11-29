using UnityEngine;

public partial class DataManager
{
    private const string _survivalStatCsvPath = "Data/SurvivalStats";

    public CsvTable<CapacityData> CapacityTable { get; private set; }
    public CsvTable<BatteryConsumeData> BatteryConsumeTable { get; private set; }
    public CsvTable<BatteryRecoverData> BatteryRecoverTable { get; private set; }
    public CsvTable<WillConsumeData> WillConsumeTable { get; private set; }
    public CsvTable<WillRecoverData> WillRecoverTable { get; private set; }

    private void InitSurvialStatDatas()
    {
        var text = Resources.Load<TextAsset>(_survivalStatCsvPath).text;
        var sections = CsvParser.ParseSections(text);

        CapacityTable = CsvTable<CapacityData>.FromRows(sections["Capacity"]);
        BatteryConsumeTable = CsvTable<BatteryConsumeData>.FromRows(sections["BatteryConsume"]);
        BatteryRecoverTable = CsvTable<BatteryRecoverData>.FromRows(sections["BatteryRecover"]);
        WillConsumeTable = CsvTable<WillConsumeData>.FromRows(sections["WillConsume"]);
        WillRecoverTable = CsvTable<WillRecoverData>.FromRows(sections["WillRecover"]);
    }
}

// 생존 수치 최대/최소값 데이터
public class CapacityData : ICsvRecord
{
    public string Id;
    public float Max;
    public float Min;
    public float ReduceAmount;


    public void FromCsvRow(CsvRow row)
    {
        Id = row["ID"];
        Max = row.GetFloat("Max");
        Min = row.GetFloat("Min");
        ReduceAmount = row.GetFloat("ReduceAmount");
    }

    public string GetKey() => Id;

}

// 배터리 소모/회복 요소별 소모/회복값 데이터
public class BatteryConsumeData : ICsvRecord
{
    public string Id;
    public float Amount;


    public void FromCsvRow(CsvRow row)
    {
        Id = row["ID"];
        Amount = row.GetFloat("Amount");
    }

    public string GetKey() => Id;
}
public class BatteryRecoverData : ICsvRecord
{
    public string Id;
    public float Amount;


    public void FromCsvRow(CsvRow row)
    {
        Id = row["ID"];
        Amount = row.GetFloat("Amount");
    }

    public string GetKey() => Id;
}

// 정신력 소모/회복 요소별 소모/회복값 데이터
public class WillConsumeData : ICsvRecord
{
    public string Id;
    public float Amount;


    public void FromCsvRow(CsvRow row)
    {
        Id = row["ID"];
        Amount = row.GetFloat("Amount");
    }

    public string GetKey() => Id;
}
public class WillRecoverData : ICsvRecord
{
    public string Id;
    public float Amount;


    public void FromCsvRow(CsvRow row)
    {
        Id = row["ID"];
        Amount = row.GetFloat("Amount");
    }

    public string GetKey() => Id;
}
