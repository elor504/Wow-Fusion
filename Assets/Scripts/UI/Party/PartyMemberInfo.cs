using TMPro;
using UnityEngine;
public class PartyMemberInfo : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI memberNameText;
	[SerializeField] private TextMeshProUGUI memberClassText;
	[SerializeField] private GameObject isLeaderImage;


	public void UpdateMemberInfo(string name, string className, bool isLeader)
	{
		memberNameText.text = name;
		memberClassText.text = className;
		if (isLeaderImage)
			isLeaderImage.SetActive(isLeader);
	}

	public void ShowInfo()
	{
		gameObject.SetActive(true);
	}
	public void HideInfo()
	{
		gameObject.SetActive(false);
	}
}
