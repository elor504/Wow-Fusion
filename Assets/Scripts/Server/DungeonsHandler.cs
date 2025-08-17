using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.Unicode;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class DungeonsHandler : MonoBehaviour
{   
    [SerializeField] private DungeonManager dungeonPrefab;
    [SerializeField] private List<DungeonManager> dungeons;

    public void CreateNewDungeon(List<PlayerCharacter> characters)
    {
        foreach (var dungeon in dungeons)
        {
            if (!dungeon.IsDungeonActive)
            {
                dungeon.StartDungeon(characters);
            }
        }
}



}

