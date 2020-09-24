using UnityEngine;

public class LoadMasterDataFromJason
{
    public static T LoadFromJson<T>(string filePath)
    {
        return JsonUtility.FromJson<T>(JsonHelper.GetJsonFile("/", "filePath"));
    }
    public static ScenarioMasterData LoadScenarioMasterDataFromJson()
    {
        return JsonUtility.FromJson<ScenarioMasterData>(JsonHelper.GetJsonFile("/", "scenario.json"));
    }
}
