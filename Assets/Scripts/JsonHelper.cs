using UnityEngine;
using System;
using System.IO;
using System.Text;

public class JsonHelper
{
    public static String GetJsonFile(String filePath, String fileName)
    {
        Debug.Log("GetJson");
        string fileText = "";

        FileInfo fi = new FileInfo(Application.streamingAssetsPath + filePath + fileName);
        Debug.Log(fi);
        try
        {
            using(StreamReader sr = new StreamReader(fi.OpenRead(), Encoding.UTF8))
            {
                fileText = sr.ReadToEnd();
            }
        }
        catch (Exception e)
        {
            fileText += e + "¥n";
        }
        Debug.Log(fileText);
        return fileText;
    }

}
