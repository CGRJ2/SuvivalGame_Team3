using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CsvParser
{
    // 단일 섹션 Csv용 파싱 함수
    public static List<string[]> Parse(string text)
    {
        var result = new List<string[]>();

        if (string.IsNullOrWhiteSpace(text))
            return result;

        var lines = text.Split('\n');

        foreach (var line in lines)
        {
            // 필요하면 여기서 "타이틀,설명" 같은 인용부도 파싱하도록 확장
            var cols = line.Split(',');
            for (int i = 0; i < cols.Length; i++)
                cols[i] = cols[i].Trim(); // 앞뒤 공백 제거

            result.Add(cols);
        }

        return result;
    }

    // 한 Csv파일에 섹션이 여러 개인 경우 
    // 섹션 별로 Rows로 분리해 키로 저장하는 함수 (컬럼 수가 각기 달라도 대응)
    public static Dictionary<string, List<string[]>> ParseSections(string text)
    {
        var result = new Dictionary<string, List<string[]>>();

        if (string.IsNullOrWhiteSpace(text))
            return result;

        // 전체 행 데이터 (빈칸 포함, 줄 마다 데이터를 string[]로 저장)
        var rows = new List<string[]>();

        // 1. 줄 단위로 먼저 나누기
        var lineArray = text.Split('\n');

        foreach (var line in lineArray)
        {
            string trimmedLine = line.Trim();

            // 일단 완전 공백이면 바로 빈 줄 처리
            if (string.IsNullOrWhiteSpace(trimmedLine))
            {
                rows.Add(Array.Empty<string>());
                continue;
            }

            // 콤마로 나누기
            var cols = trimmedLine.Split(',');

            bool allEmpty = true;
            for (int i = 0; i < cols.Length; i++)
            {
                cols[i] = cols[i].Trim();

                // 하나라도 내용이 있으면 이 줄은 "데이터 줄"
                if (!string.IsNullOrWhiteSpace(cols[i]))
                    allEmpty = false;
            }

            if (allEmpty)
                rows.Add(Array.Empty<string>()); // 쉼표만 있는 줄 (",,,,") -> 빈 줄로 취급
            else
                rows.Add(cols);
        }

        // 2. rows를 섹션별로 나누기
        bool sectionStart = false;
        string currentSectionKey = null;
        List<string[]> currentSectionRaw = new();

        for (int i = 0; i < rows.Count; i++)
        {
            // 현재 줄이 빈칸이 아니라면
            if (rows[i].Length != 0)
            {
                // 그리고 섹션이 시작되지 않았다면
                if (!sectionStart)   // 1행은 스킵, 섹션 시작
                {
                    sectionStart = true;
                }
                // 섹션의 데이터가 시작되었다면
                else
                {
                    // 두번째 행에서 섹션 key 설정
                    if (string.IsNullOrWhiteSpace(currentSectionKey))
                    {
                        currentSectionKey = rows[i][0]; // 섹션의 두번째 행 1열이 섹션 이름
                    }
                    // 세번째 행부터 데이터 저장
                    else
                    {
                        currentSectionRaw.Add(rows[i]);
                    }
                }
            }
            // 빈칸 줄의 차례가 오면
            else
            {
                // 윗 줄까지 만들어두었던 섹션 저장 및 상태 초기화
                if (!string.IsNullOrWhiteSpace(currentSectionKey) && currentSectionRaw.Count > 0)
                    result.Add(currentSectionKey, currentSectionRaw);   // 마지막 섹션은 "4)"에서 처리

                sectionStart = false;
                currentSectionKey = null;
                currentSectionRaw = new();
            }
        }

        // 마지막 섹션 처리
        if (!string.IsNullOrWhiteSpace(currentSectionKey) && currentSectionRaw.Count > 0)
        {
            result.Add(currentSectionKey, currentSectionRaw);
        }

        return result;
    }
}

// 데이터 섹션 별 한 줄을 다루기 위한 클래스
public class CsvRow
{
    private readonly Dictionary<string, int> _headerIndex;
    private readonly string[] _values;

    public CsvRow(Dictionary<string, int> headerIndex, string[] values)
    {
        _headerIndex = headerIndex;
        _values = values;
    }

    public string this[string columnName] // 인덱서 사용
    {
        get
        {
            if (!_headerIndex.TryGetValue(columnName, out int index))
                throw new Exception($"CSV column not found: {columnName}");

            if (index < 0 || index >= _values.Length)
                return string.Empty;

            return _values[index];
        }
    }

    // 타입 별 변환 함수
    public int GetInt(string columnName, int defaultValue = 0)
    {
        var s = this[columnName];
        return int.TryParse(s, out int v) ? v : defaultValue;
    }

    public float GetFloat(string columnName, float defaultValue = 0f)
    {
        var s = this[columnName];
        return float.TryParse(s, out float v) ? v : defaultValue;
    }

    public bool GetBool(string columnName, bool defaultValue = false)
    {
        var s = this[columnName];
        if (bool.TryParse(s, out bool v)) return v;
        if (int.TryParse(s, out int i)) return i != 0;
        return defaultValue;
    }
}

public interface ICsvRecord
{
    // CsvRow를 받아서 자기 필드를 채우는 메서드
    void FromCsvRow(CsvRow row);

    // ID(문자열 키)
    string GetKey();
}

// 데이터 맵을 테이블로 전환하는 클래스
public class CsvTable<T> where T : ICsvRecord, new()
{
    private readonly List<T> _records = new List<T>();
    private readonly Dictionary<string, T> _recordByKey = new Dictionary<string, T>();

    public IReadOnlyList<T> Records => _records;

    public T FindByKey(string key)
    {
        _recordByKey.TryGetValue(key, out var value);
        return value;
    }

    // 섹션 rows를 그대로 넣는 함수
    public static CsvTable<T> FromRows(List<string[]> rows)
    {
        var table = new CsvTable<T>();

        if (rows == null || rows.Count == 0)
            return table;

        // 첫번째 줄(raw[0]) : 헤더
        var headerRow = rows[0];
        var headerIndex = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase); // 대소문자 구분 X
        for (int i = 0; i < headerRow.Length; i++)
        {
            var name = headerRow[i];
            if (!string.IsNullOrWhiteSpace(name))
                headerIndex[name] = i;  // 헤더 key 중복 예방
        }

        // raw[1]~ : 데이터
        for (int i = 1; i < rows.Count; i++)
        {
            var row = new CsvRow(headerIndex, rows[i]);

            var record = new T();
            record.FromCsvRow(row);

            var key = record.GetKey();
            if (!string.IsNullOrEmpty(key))
            {
                if (!table._recordByKey.ContainsKey(key))
                    table._recordByKey.Add(key, record);
                else
                    Debug.LogWarning($"Duplicate key in CSV ({typeof(T).Name}): {key}");
            }

            table._records.Add(record);
        }

        return table;
    }
}