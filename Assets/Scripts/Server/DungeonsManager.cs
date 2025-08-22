using System.Collections.Generic;
using UnityEngine;

public class DungeonsManager : MonoBehaviour
{
	[SerializeField] private DungeonInstance dungeonPrefab;
	[SerializeField] private List<DungeonInstance> dungeons;

	public void CreateNewDungeon(Party party)
	{
		foreach (var dungeon in dungeons)
		{
			if (!dungeon.IsDungeonActive)
			{
				dungeon.StartDungeon(party);
				break;
			}
		}
	}




}

