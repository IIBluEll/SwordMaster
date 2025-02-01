using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SwordCSVConverter_new : Editor
{
    [MenuItem("CSVTool/Generate Sword SO Data")]
    public static void GenerateSwordSo()
    {
        var csvFilePath = EditorUtility.OpenFilePanel(
            "Select CSV File",
            "Assets/03_Resources/CSV",
            "csv");

        if (string.IsNullOrEmpty(csvFilePath))
        {
            return;
        }

        var selectedFolder = EditorUtility.OpenFolderPanel(
            "Select Output Folder for SO",
            Application.dataPath,
            "");

        // if (string.IsNullOrEmpty(selectedFolder))
        // {
        //     return;
        // }

        var projectPath = Application.dataPath;

        if (!selectedFolder.StartsWith(projectPath))
        {
            return;
        }

        string relativeFolderPath = "Assets" + selectedFolder.Substring(projectPath.Length);

        Debug.Log($"SwordSO를 생성/업데이트할 폴더: {relativeFolderPath}");

        if (!File.Exists(csvFilePath))
        {
            Debug.LogError("CSV 파일을 찾을 수 없습니다: " + csvFilePath);
            return;
        }

        var lines = File.ReadAllLines(csvFilePath);
        if (lines.Length <= 1)
        {
            return;
        }

        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var tokens = line.Split(',');
            if (tokens.Length < 0)
            {
                continue;
            }

            #region DataParsing

            int level = int.Parse(tokens[0].Trim());
            string swordNameKR = tokens[2].Trim();
            string swordNameEN = tokens[3].Trim();

            int nextSwordLevel = int.Parse(tokens[4].Trim());
            int upgradeCost = int.Parse(tokens[5].Trim());

            float upgradeRate = float.Parse(tokens[6].Trim());
            float damage = float.Parse(tokens[7].Trim());
            float attackSpeed = float.Parse(tokens[8].Trim());

            string assetName = $"Sword_LV{level}_{swordNameEN}.asset";
            string assetPath = Path.Combine(relativeFolderPath, assetName);

            // 기존 에셋 이미 있는지 확인
            SwordSO existingSO = AssetDatabase.LoadAssetAtPath<SwordSO>(assetPath);

            if (existingSO != null)
            {
                // 덮어쓰기
                existingSO.swordData.swordLevel = level;
                existingSO.swordData.swordName_KR = swordNameKR;
                existingSO.swordData.swordName_EN = swordNameEN;
                existingSO.swordData.nextSwordLevel = nextSwordLevel;
                existingSO.swordData.upgradeCost = upgradeCost;
                existingSO.swordData.upgradeRate = upgradeRate;
                existingSO.swordData.damage = damage;
                existingSO.swordData.attackSpeed = attackSpeed;

                var loadedSprite = LoadSpriteFromAddress(swordNameEN + ".png");

                if (loadedSprite != null)
                {
                    existingSO.swordData.swordSprite = loadedSprite;
                }
                
                EditorUtility.SetDirty(existingSO);
                Debug.Log($"기존 SwordSO 갱신: {assetPath}");
            }
            else
            {
                // 새로 생성
                SwordSO newSwordSO = ScriptableObject.CreateInstance<SwordSO>();
                newSwordSO.hideFlags = HideFlags.None;
                var loadedSprite = LoadSpriteFromAddress(swordNameEN + ".png");

                newSwordSO.swordData = new SwordData()
                {
                    swordLevel = level,
                    swordName_KR = swordNameKR,
                    swordName_EN = swordNameEN,
                    nextSwordLevel = nextSwordLevel,
                    upgradeCost = upgradeCost,
                    upgradeRate = upgradeRate,
                    damage = damage,
                    attackSpeed = attackSpeed,
                    swordSprite = loadedSprite,
                };

                AssetDatabase.CreateAsset(newSwordSO, assetPath);
                Debug.Log($"새 SwordSO 생성: {assetPath}");
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    
    // Addressables에서 스프라이트 로드
    private static Sprite LoadSpriteFromAddress(string addressKey)
    {
        AsyncOperationHandle<Sprite> handleSprite = Addressables.LoadAssetAsync<Sprite>(addressKey);

        Sprite result = handleSprite.WaitForCompletion();

        return result;
    }
        #endregion
}
