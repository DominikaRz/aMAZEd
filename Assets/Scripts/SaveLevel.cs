using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLevel : MonoBehaviour
{
    public void Save(SaveData data)
    {
        //PlayerPrefs.SetFloat("key", value); SetInt SetString
        PlayerPrefs.SetInt("levelNumber", data.getLevel());

        PlayerPrefs.SetFloat("playerPositionX", data.getPositionX());
        PlayerPrefs.SetFloat("playerPositionY", -0.033f);
        PlayerPrefs.SetFloat("playerPositionZ", data.getPositionZ());

        PlayerPrefs.SetInt("playerHealth", data.playerHealth);
        PlayerPrefs.SetInt("numberOfCollectedKeyes", data.getKeyes());
    }

    public SaveData Load()
    {
        SaveData data = new SaveData();

        data.setLevel(PlayerPrefs.GetInt("levelNumber"));

        data.setPosition(
            PlayerPrefs.GetFloat("playerPositionX"),
            PlayerPrefs.GetFloat("playerPositionY"),
            PlayerPrefs.GetFloat("playerPositionZ"));

        data.playerHealth = PlayerPrefs.GetInt("playerHealth");
        data.setKeyes(PlayerPrefs.GetInt("numberOfCollectedKeyes"));
        
        return data;

    }

    public int LoadLvl()
    {
        return PlayerPrefs.GetInt("levelNumber");
    }
    public void SaveLvl(int level)
    {
        PlayerPrefs.SetInt("levelNumber", level);
    }

    //delets everything from save
    public void Delete()
    {
        PlayerPrefs.DeleteAll();
    }

}
