using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] 
public class CharactersList
{
    [SerializeField] private List<PlayerCharacter> playerCharacters = new List<PlayerCharacter>();
    public List<PlayerCharacter> GetPlayerCharacters => playerCharacters;

    public void AddCharacterToList(PlayerCharacter character)
    {
        playerCharacters.Add(character);
	}
    public void RemoveCharacterFromList(PlayerCharacter character)
    {
        playerCharacters.Remove(character);
    }

    public void AddCharactersToList(List<PlayerCharacter> characters)
    {
        playerCharacters.AddRange(characters);
    }
    public void RemoveCharacters(List<PlayerCharacter> characters)
    {
		foreach (var character in characters)
		{
            playerCharacters.Remove(character);
        }
	}

    public bool TryGetCharacterByName(string characterName,out PlayerCharacter requestedCharacter)
    {
        requestedCharacter = null;

        foreach (var character in playerCharacters)
		{
            if(character.CharacterName == characterName)
            {
                requestedCharacter = character;
                return true;
			}
		}

        return false;
	}
    public bool TryGetCharacterByPlayerRef(PlayerRef characterRef, out PlayerCharacter requestedCharacter)
    {
        requestedCharacter = null;

        foreach (var character in playerCharacters)
        {
            if (character.NetworkObject.InputAuthority == characterRef)
            {
                requestedCharacter = character;
                return true;
            }
        }

        return false;
    }
}