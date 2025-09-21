using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;
using static UnityEngine.Rendering.DebugUI;
public class FriendsTab : MonoBehaviour
{
    [SerializeField] private UserDisplay userFramePrefab;
    [SerializeField] private RectTransform friendList;
    [SerializeField] private RectTransform requestOutgoingList;
    [SerializeField] private RectTransform requestIncomingList;
    [SerializeField] private Inventory inventory;

    private List<UserDisplay> _friends = new List<UserDisplay>();
    private List<UserDisplay> _requestsIncoming = new List<UserDisplay>();
    private List<UserDisplay> _requestsOutgoing = new List<UserDisplay>();

    private static string url = "https://api.statusloop.nl/";

    private string targetUser;

    public static FriendsTab instance;
    #region Public methods
    public void UpdateFriendsTab(int? userID = null)
    {
        if (userID == null) StartCoroutine(UpdateTab());
        else StartCoroutine(UpdateTab((int)userID));
    }
    public void SetTargetUsername(string username)
    {
        targetUser = username;
    }
    public void SendRequest()
    {
        StartCoroutine(SendFriendRequest());
    }
    public void Accept(int senderID, int receiverID)
    {
        StartCoroutine(AcceptRequest(senderID, receiverID));
    }
    public void Deny(int senderID, int receiverID)
    {
        StartCoroutine(DenyRequest(senderID, receiverID));
    }
    public void Cancel(int senderID, int receiverID)
    {
        StartCoroutine(CancelRequest(senderID, receiverID));
    }
    #endregion

    #region internal methods
    private void OnEnable()
    {
        UpdateFriendsTab();
    }
    private IEnumerator UpdateTab(int? userID = null)
    {
        List<User> userList = new List<User>();
        UnityWebRequest request = UnityWebRequest.Get($"{url}users/");
        yield return request.SendWebRequest();

        string rawJson = request.downloadHandler.text;

        // Manually wrap the array in an object
        string wrappedJson = "{ \"users\": " + rawJson + " }";

        UserListWrapper wrapper = JsonUtility.FromJson<UserListWrapper>(wrappedJson);

        if (wrapper != null && wrapper.users != null && userID == null)
        {
            foreach (User user in wrapper.users)
            {
                if (user.name == inventory.username) userID = user.id;
                userList.Add(user);
            }
        }

        UnityWebRequest friendListGet = UnityWebRequest.Get($"{url}friends/{userID}");
        yield return friendListGet.SendWebRequest();
        string flrawJson = friendListGet.downloadHandler.text;
        string flwrappedJson = "{ \"friends\": " + flrawJson + " }";
        FriendsListWrapper flwrapper = JsonUtility.FromJson<FriendsListWrapper>(flwrappedJson);

        if (flwrapper != null && flwrapper.friends != null)
        {
            UpdateFriends(flwrapper.friends, userList, (int)userID);
        }

        UnityWebRequest friendRequestsGet = UnityWebRequest.Get($"{url}friend_requests/user/{userID}");
        yield return friendRequestsGet.SendWebRequest();
        string frrawJson = friendRequestsGet.downloadHandler.text;
        string frwrappedJson = "{ \"friendRequests\": " + frrawJson + " }";
        FriendRequestListWrapper frwrapper = JsonUtility.FromJson<FriendRequestListWrapper>(frwrappedJson);

        if (frwrapper != null && frwrapper.friendRequests != null)
        {
            UpdateRequests(frwrapper.friendRequests, userList, (int)userID);
        }
    }
    private IEnumerator SendFriendRequest()
    {
        UnityWebRequest request = UnityWebRequest.Get($"{url}users/");
        yield return request.SendWebRequest();

        string rawJson = request.downloadHandler.text;

        // Manually wrap the array in an object
        string wrappedJson = "{ \"users\": " + rawJson + " }";

        UserListWrapper wrapper = JsonUtility.FromJson<UserListWrapper>(wrappedJson);

        int? userID = null;

        if (wrapper != null && wrapper.users != null)
        {
            int? targetID = null;

            foreach (User user in wrapper.users)
            {
                if (user.name == inventory.username)
                {
                    userID = user.id;
                }
                else if (user.name == targetUser)
                {
                    targetID = user.id;
                }
            }

            if (userID != null && targetID != null)
            {
                FriendRequest friendRequest = new FriendRequest
                {
                    sender = (int)userID,
                    receiver = (int)targetID
                };
                string json = JsonUtility.ToJson(friendRequest);

                UnityWebRequest FRrequest = new UnityWebRequest($"{url}friend_requests/", "POST");
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
                FRrequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
                FRrequest.downloadHandler = new DownloadHandlerBuffer();
                FRrequest.SetRequestHeader("Content-Type", "application/json");

                yield return FRrequest.SendWebRequest();
            }
        }

        UpdateFriendsTab(userID);
    }
    private IEnumerator AcceptRequest(int senderID, int receiverID)
    {
        UnityWebRequest get = UnityWebRequest.Get($"{url}friend_requests/{receiverID}");
        yield return get.SendWebRequest();
        string rawJson = get.downloadHandler.text;
        string wrappedJson = "{ \"friendRequests\": " + rawJson + " }";
        FriendRequestListWrapper wrapper = JsonUtility.FromJson<FriendRequestListWrapper>(wrappedJson);

        foreach(FriendRequest friendRequest in wrapper.friendRequests)
        {
            if (friendRequest.sender == senderID && friendRequest.receiver == receiverID)
            {
                // Add friends
                Friends friends = new Friends
                {
                    user1 = senderID,
                    user2 = receiverID
                };
                string json = JsonUtility.ToJson(friends);

                UnityWebRequest post = new UnityWebRequest($"{url}friends/", "POST");
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
                post.uploadHandler = new UploadHandlerRaw(bodyRaw);
                post.downloadHandler = new DownloadHandlerBuffer();
                post.SetRequestHeader("Content-Type", "application/json");

                yield return post.SendWebRequest();

                // Delete request
                string endpoint = $"{url}friend_requests/{friendRequest.id}";
                using (UnityWebRequest delete = UnityWebRequest.Delete(endpoint))
                {
                    // A DownloadHandler is required to receive response text (if any)
                    delete.downloadHandler = new DownloadHandlerBuffer();

                    yield return delete.SendWebRequest();
                    break;
                }
            }
        }

        UpdateFriendsTab(receiverID);
    }
    private IEnumerator DenyRequest(int senderID, int receiverID)
    {
        UnityWebRequest get = UnityWebRequest.Get($"{url}friend_requests/{receiverID}");
        yield return get.SendWebRequest();
        string rawJson = get.downloadHandler.text;
        string wrappedJson = "{ \"friendRequests\": " + rawJson + " }";
        FriendRequestListWrapper wrapper = JsonUtility.FromJson<FriendRequestListWrapper>(wrappedJson);

        foreach (FriendRequest friendRequest in wrapper.friendRequests)
        {
            if (friendRequest.sender == senderID && friendRequest.receiver == receiverID)
            {
                // Delete request
                string endpoint = $"{url}friend_requests/{friendRequest.id}";
                using (UnityWebRequest delete = UnityWebRequest.Delete(endpoint))
                {
                    // A DownloadHandler is required to receive response text (if any)
                    delete.downloadHandler = new DownloadHandlerBuffer();

                    yield return delete.SendWebRequest();
                    break;
                }
            }
        }

        UpdateFriendsTab(receiverID);
    }
    private IEnumerator CancelRequest(int senderID, int receiverID)
    {
        UnityWebRequest get = UnityWebRequest.Get($"{url}friend_requests/{receiverID}");
        yield return get.SendWebRequest();
        string rawJson = get.downloadHandler.text;
        string wrappedJson = "{ \"friendRequests\": " + rawJson + " }";
        FriendRequestListWrapper wrapper = JsonUtility.FromJson<FriendRequestListWrapper>(wrappedJson);

        foreach (FriendRequest friendRequest in wrapper.friendRequests)
        {
            if (friendRequest.sender == senderID && friendRequest.receiver == receiverID)
            {
                // Delete request
                string endpoint = $"{url}friend_requests/{friendRequest.id}";
                using (UnityWebRequest delete = UnityWebRequest.Delete(endpoint))
                {
                    // A DownloadHandler is required to receive response text (if any)
                    delete.downloadHandler = new DownloadHandlerBuffer();

                    yield return delete.SendWebRequest();
                    break;
                }
            }
        }

        UpdateFriendsTab(senderID);
    }
    private void UpdateFriends(Friends[] friends, List<User> users, int userID)
    {
        // Remove friends
        for (int i = _friends.Count - 1; i >= 0; i--)
        {
            UserDisplay friend = _friends[i];
            _friends.Remove(friend);
            Destroy(friend);
        }

        // Add friends
        foreach(Friends friend in friends)
        {
            string name = null;
            int id = 0;
            if (friend.user1 != userID)
            {
                name = users.Find(s => s.id == friend.user1).name;
                id = friend.user1;
            }

            else if (friend.user2 != userID)
            {
                name = users.Find(s => s.id == friend.user2).name;
                id = friend.user2;
            }

            UserDisplay display = Instantiate(userFramePrefab, friendList.transform);
            display.SetUserName(name);
            display.userID = id;
        }

        friendList.sizeDelta = new Vector2(friendList.sizeDelta.x, userFramePrefab.CardHeight() * friends.Length);
    }
    private void UpdateRequests(FriendRequest[] requests, List<User> users, int userID)
    {
        // Remove requests
        for (int i = _requestsIncoming.Count - 1; i >= 0; i--)
        {
            UserDisplay friend = _friends[i];
            _requestsIncoming.Remove(friend);
            Destroy(friend);
        }

        for (int i = _requestsOutgoing.Count - 1; i >= 0; i--)
        {
            UserDisplay friend = _friends[i];
            _requestsOutgoing.Remove(friend);
            Destroy(friend);
        }

        // Add requests
        foreach (FriendRequest fr in requests)
        {
            string name = null;
            RectTransform parent = null;
            if (fr.sender != userID)
            {
                name = users.Find(s => s.id == fr.sender).name;
                parent = requestIncomingList;
            }

            else if (fr.receiver != userID)
            {
                name = users.Find(s => s.id == fr.receiver).name;
                parent = requestOutgoingList;
            }
            UserDisplay display = Instantiate(userFramePrefab, parent.transform);
            display.SetUserName(name);
            display.senderID = fr.sender;
            display.receiverID = fr.receiver;

            if (parent == requestIncomingList) _requestsIncoming.Add(display);
            else _requestsOutgoing.Add(display);
        }

        requestIncomingList.sizeDelta = new Vector2(requestIncomingList.sizeDelta.x, userFramePrefab.CardHeight() * _requestsIncoming.Count);
        requestOutgoingList.sizeDelta = new Vector2(requestOutgoingList.sizeDelta.x, userFramePrefab.CardHeight() * _requestsOutgoing.Count);
    }
    private void Awake()
    {
        if (instance == null && instance != this) instance = this;
        else Destroy(gameObject);
    }
    #endregion
}