using UnityEngine;
using UnityEditor;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections.Generic;
using System.IO;
public class SwordCSVImporter : Editor
{
    // csv 경로 
    private static readonly string CSV_FILE_PATH = "Assets/03_Resources/CSV/SwordDataCSV.csv";
    // scritable Object 경로 
    private static readonly string SWORD_DATA_LIST_PATH = "Assets/01_Scripts/Sword ScriptableObject/SwordDataList.asset";

    [MenuItem("Tools/Import Sword CSV")]
    public static void ImportSwordCSV()
    {
        if (!File.Exists(CSV_FILE_PATH))
        {
            Debug.LogError(("CSV 파일을 찾을 수 없음"));
            return;
        }

        SwordSO swordDataList = AssetDatabase.LoadAssetAtPath<SwordSO>(SWORD_DATA_LIST_PATH);
        {
            if (swordDataList == null)
            {
                Debug.LogError("SwordDataList Scriptable Object를 찾을 수 없습니다.");
                return;
            }
        }
        
        // CSV 파싱
        List<(int Level, string Sword_Name_KR, string Sword_Name_EN, int Next_Sword_Level, int Upgrade_Cost, float
            Upgrade_Rate, float Damage, float Attack_Speed)> csvData = ParaseCSV(CSV_FILE_PATH);

        if (csvData == null || csvData.Count == 0)
        {
            Debug.LogError("CSV 내용이 없거나 파싱 실패");
            return;
        }

        foreach (var entry in csvData)
        {
            SwordData existingData = swordDataList.SwordDatas.Find(s => s.swordLevel == entry.Level);

            if (existingData == null)
            {
                existingData = new SwordData();
                swordDataList.SwordDatas.Add(existingData);
            }

            existingData.swordLevel = entry.Level;
            existingData.swordName_KR = entry.Sword_Name_KR;
            existingData.swordName_EN = entry.Sword_Name_EN;
            existingData.nextSwordLevel = entry.Next_Sword_Level;
            existingData.upgradeCost = entry.Upgrade_Cost;
            existingData.upgradeRate = entry.Upgrade_Rate;
            existingData.damage = entry.Damage;
            existingData.attackSpeed = entry.Attack_Speed;

            Sprite loadedSprite = LoadSpriteFromAddress(existingData.swordName_EN + ".png");

            if (loadedSprite != null)
            {
                existingData.swordSprite = loadedSprite;
                Debug.Log($"검 스프라이트 로드 완료 : {existingData.swordName_KR}");
            }
            else
            {
                Debug.LogError($"검 스프라이트 로드 실패 : {existingData.swordName_EN}");
            }
        }

        EditorUtility.SetDirty(swordDataList);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    // CSV 파싱
    private static List<(int, string, string, int, int, float, float, float)> ParaseCSV(string filePath)
    {
        var resultList = new List<(int, string, string, int, int, float, float, float)>();
        var lines = File.ReadAllLines(filePath);

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            
            if(string.IsNullOrWhiteSpace(line))
                continue;

            Debug.Log(line);
            var tokens = line.Split(',');
            
            if(tokens.Length < 9)
            {
                continue;
            }

            int Level = int.Parse(tokens[0].Trim());
            string Sword_Name_KR = tokens[2].Trim();
            string Sword_Name_EN = tokens[3].Trim();
            int Next_Sword_Level = int.Parse(tokens[4].Trim());
            int Upgrade_Cost = int.Parse(tokens[5].Trim());

            float Upgrade_Rate = float.Parse(tokens[6].Trim());
            float Damage = float.Parse(tokens[7].Trim());
            float Attack_Speed = float.Parse(tokens[8].Trim());

            resultList.Add((Level, Sword_Name_KR, Sword_Name_EN, Next_Sword_Level, Upgrade_Cost, Upgrade_Rate, Damage,
                Attack_Speed));
        }

        return resultList;
    }

    // Addressables에서 스프라이트 로드
    private static Sprite LoadSpriteFromAddress(string addressKey)
    {
        AsyncOperationHandle<Sprite> handle = Addressables.LoadAssetAsync<Sprite>(addressKey);

        Sprite result = handle.WaitForCompletion();

        return result;
    }
}
