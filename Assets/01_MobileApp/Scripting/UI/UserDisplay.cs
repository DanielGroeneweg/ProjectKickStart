using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UserDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private RectTransform cardImage;
    public int id = 0;
    public void SetUserName(string name)
    {
        usernameText.text = name;
    }
    public float CardHeight()
    {
        return cardImage.sizeDelta.y;
    }
}