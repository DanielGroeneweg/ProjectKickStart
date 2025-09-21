using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UserDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private RectTransform cardImage;
    public int senderID = 0;
    public int receiverID = 0;
    public int userID = 0;
    public void SetUserName(string name)
    {
        usernameText.text = name;
    }
    public float CardHeight()
    {
        return cardImage.sizeDelta.y;
    }
    public void Accept()
    {
        FriendsTab.instance.Accept(senderID, receiverID);
    }
    public void Deny()
    {
        FriendsTab.instance.Deny(senderID, receiverID);
    }
    public void Cancel()
    {
        FriendsTab.instance.Cancel(senderID, receiverID);
    }
}