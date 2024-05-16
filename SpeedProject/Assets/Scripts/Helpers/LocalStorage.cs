using System;
using System.Collections;
using UnityEngine;

public class LocalStorage : MonoBehaviour 
{
    public enum StorageValues
    {
        Temp,
        PlayMode,
        CurrentProgress,
        HighestScore,
        Streak
    }





    static string GetStorageName(StorageValues value)
    {
       return value.ToString();
    }
    public static string GetData(StorageValues value)
    {
		try
		{
            string key = GetStorageName(value);
            string jsonStr = PlayerPrefs.GetString(key);
            return jsonStr;
        }
		catch (System.Exception ex)
		{
            Debug.Log("GET DATA ERROR: " + ex.Message);
            return "";
		}
    }

    public static void SaveData(StorageValues value, string jsonStr, bool save = true)
    {
        try
        {
            string key = GetStorageName(value);
            PlayerPrefs.SetString(key, jsonStr);
            if (save)
            {
                PlayerPrefs.Save();
            }
            return;
        }
        catch (System.Exception ex)
        {
            Debug.Log("SAVE DATA" + ex.Message);

            throw;
        }
    }

    public static bool CheckData(StorageValues value)
    {
        try
        {
            string key = GetStorageName(value);
            string check = PlayerPrefs.GetString(key);
            if (string.IsNullOrEmpty(check))
            {
                return false;
            }
            else
                return true;
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.StackTrace);
            Debug.Log(ex.Message);
            throw;
        }
    }

    public static void RemoveData(StorageValues value)
    {
        try
        {
            string key = GetStorageName(value);
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
            return;
        }
        catch (System.Exception ex)
        {
            Debug.Log("REMOVE DATA" + ex.Message);
            throw;
        }
    }

    public static void Clear()
    {
        try
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            return;
        }
        catch (System.Exception)
        {

            throw;
        }
    }

}
