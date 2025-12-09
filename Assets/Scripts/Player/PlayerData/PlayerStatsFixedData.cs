using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public partial class DataManager
{
    private const string _playerFixedDataCsvPath = "Data/PlayerFixedData";

    public CsvTable<PlayerStatsFixedData> PlayerStatsTable { get; private set; }
    public CsvTable<BodyStatsFixedData> BodyStatsTable { get; private set; }

    private void InitPlayerFixedDatas()
    {
        var text = Resources.Load<TextAsset>(_playerFixedDataCsvPath).text;
        var sections = CsvParser.ParseSections(text);

        PlayerStatsTable = CsvTable<PlayerStatsFixedData>.FromRows(sections["PlayerStats"]);
        BodyStatsTable = CsvTable<BodyStatsFixedData>.FromRows(sections["BodyStats"]);
    }
}


public class PlayerStatsFixedData : ICsvRecord
{
    public string Id;
    public float InitValue;

    public void FromCsvRow(CsvRow row)
    {
        Id = row["ID"];
        InitValue = row.GetFloat("InitValue");
    }

    public string GetKey() => Id;
}

public class BodyStatsFixedData : ICsvRecord
{
    public string Id;
    public float Max;
    public float Min;
    public float ReduceAmount; //부위 파괴 시 감소되는 최대 내구도 량


    public void FromCsvRow(CsvRow row)
    {
        Id = row["ID"];
        Max = row.GetFloat("Max");
        Min = row.GetFloat("Min");
        ReduceAmount = row.GetFloat("ReduceAmount");
    }
    public string GetKey() => Id;
}


