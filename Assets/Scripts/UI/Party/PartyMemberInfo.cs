using UnityEngine;
using TMPro;
public class PartyMemberInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI memberNameText;
    [SerializeField] private TextMeshProUGUI memberClassText;
    [SerializeField] private GameObject isLeaderImage;


    public void UpdateMemberInfo(string name, string className, bool isLeader)
    {
        memberNameText.text = name;
        memberClassText.text = className;
        isLeaderImage.gameObject.SetActive(isLeader);
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
