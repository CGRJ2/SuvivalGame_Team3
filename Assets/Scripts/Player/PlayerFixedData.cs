using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFixedData : ICsvRecord
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

public partial class DataManager
{
    private const string _playerFixedDataCsvPath = "Data/PlayerFixedData";

    public CsvTable<PlayerFixedData> PlayerFixedDataTable { get; private set; }


    private void InitPlayerFixedDatas()
    {
        var text = Resources.Load<TextAsset>(_playerFixedDataCsvPath).text;
        var sections = CsvParser.ParseSections(text);

        PlayerFixedDataTable = CsvTable<PlayerFixedData>.FromRows(sections["PlayerStat"]);
    }
}
