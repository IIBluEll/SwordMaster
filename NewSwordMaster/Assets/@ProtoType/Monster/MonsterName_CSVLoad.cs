using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MonsterName_CSVLoad : MonoBehaviour
{
   public static List<string> LoadMonsterName(string filePath)
   {
      List<string> names = new List<string>();
      string[] lines = File.ReadAllLines(filePath);

      foreach (var line in lines)
      {
         if (!string.IsNullOrWhiteSpace(line))
         {
            names.Add(line.Trim());
         }
      }

      return names;
   }
}
