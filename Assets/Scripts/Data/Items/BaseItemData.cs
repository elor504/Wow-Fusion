using System;
using UnityEngine;

[Serializable]
public class BaseItemData
{
	[SerializeField] private string itemName;
	[SerializeField] private string itemDescription;
	[SerializeField] private int itemAmount;

	public string ItemName => itemName;
	public string ItemDescription => itemDescription;
	public int ItemAmount => itemAmount;


	public BaseItemData(string itemName, string itemDescription, int itemAmount = 1)
	{
		this.itemName = itemName;
		this.itemDescription = itemDescription;
		this.itemAmount = itemAmount;
	}
}