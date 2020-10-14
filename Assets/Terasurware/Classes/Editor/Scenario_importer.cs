using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;
using System.Xml.Serialization;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;

public class Scenario_importer : AssetPostprocessor {
	private static readonly string filePath = "Assets/Scenario.xlsx";
	private static readonly string exportPath = "Assets/Scenario.asset";
	private static readonly string[] sheetNames = { "Sheet1", };
	
	static void OnPostprocessAllAssets (string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		foreach (string asset in importedAssets) {
			if (!filePath.Equals (asset))
				continue;
				
			Scenario data = (Scenario)AssetDatabase.LoadAssetAtPath (exportPath, typeof(Scenario));
			if (data == null) {
				data = ScriptableObject.CreateInstance<Scenario> ();
				AssetDatabase.CreateAsset ((ScriptableObject)data, exportPath);
				data.hideFlags = HideFlags.NotEditable;
			}
			
			data.sheets.Clear ();
			using (FileStream stream = File.Open (filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
				IWorkbook book = null;
				if (Path.GetExtension (filePath) == ".xls") {
					book = new HSSFWorkbook(stream);
				} else {
					book = new XSSFWorkbook(stream);
				}
				
				foreach(string sheetName in sheetNames) {
					ISheet sheet = book.GetSheet(sheetName);
					if( sheet == null ) {
						Debug.LogError("[QuestData] sheet not found:" + sheetName);
						continue;
					}

					Scenario.Sheet s = new Scenario.Sheet ();
					s.name = sheetName;
				
					for (int i=1; i<= sheet.LastRowNum; i++) {
						IRow row = sheet.GetRow (i);
						ICell cell = null;
						
						Scenario.Param p = new Scenario.Param ();
						
					cell = row.GetCell(0); p.senarioNo = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(1); p.messageString = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(2); p.charaNoString = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(3); p.branchString = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(4); p.displayCharaString = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(5); p.backgroundImageNo = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(6); p.bgmNo = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(7); p.branchMessageString = (cell == null ? "" : cell.StringCellValue);
					cell = row.GetCell(8); p.autoScenarioNo = (int)(cell == null ? 0 : cell.NumericCellValue);
					cell = row.GetCell(9); p.endingNo = (int)(cell == null ? 0 : cell.NumericCellValue);
						s.list.Add (p);
					}
					data.sheets.Add(s);
				}
			}

			ScriptableObject obj = AssetDatabase.LoadAssetAtPath (exportPath, typeof(ScriptableObject)) as ScriptableObject;
			EditorUtility.SetDirty (obj);
		}
	}
}
