/*
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int mazeWidth;
    public int mazeHeight;
    public List<CellData> cells;
    public PlayerData playerData;
    // Add other elements as needed
}

[System.Serializable]
public class CellData
{
    // Example properties
    public int x, y;
    public bool isWall;
    // Add other cell properties as needed
}

[System.Serializable]
public class PlayerData
{
    // Example properties
    public float positionX, positionY, positionZ;
    // Add other player properties as needed
}

// Add other data classes for zombies, keys, etc., similar to PlayerData
*/


using UnityEngine;
using System.Collections.Generic;

public class SaveData
{
    public int levelNumber;

    public float playerPositionX, playerPositionY, playerPositionZ;

    public int playerHealth;
    public int numberOfCollectedKeyes;

    public int GetLevelNumber() 
    {
        return this.levelNumber; // Assuming 'levelNumber' is the property storing the level number.
    }


   //getters
    public int getLevel(){
        return this.levelNumber;
    }

    public float getPositionX(){
        return this.playerPositionX;
    }
    
    public float getPositionY(){
        return this.playerPositionY;
    }
    public float getPositionZ(){
        return this.playerPositionZ;
    }
    
    public int getHealth(){
        return this.playerHealth;
    }
    public int getKeyes(){
        return this.numberOfCollectedKeyes;
    }

   //setters
    public void setLevel(int level){
        this.levelNumber = level;
    }

    public void setPosition(float x, float y, float z){
        this.playerPositionX = x;
        this.playerPositionY = y;
        this.playerPositionZ = z;
    }

    public void setKeyes(int keyes){
        this.numberOfCollectedKeyes = keyes;
    }
    /**/
    public void setHealth(int health){
        this.playerHealth = health;
    }


}
