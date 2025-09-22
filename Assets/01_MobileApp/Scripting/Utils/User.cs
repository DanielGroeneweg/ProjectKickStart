using UnityEngine;
using System;
using JetBrains.Annotations;
[Serializable]
public class User
{
    public string name;
    public string skinID;
    public int id;
    public bool hasWalked;
}
[Serializable]
public class UserListWrapper
{
    public User[] users;
}
[Serializable]
public class FriendRequest
{
    public int sender;
    public int receiver;
    public int id;
}
[Serializable]
public class FriendRequestListWrapper
{
    public FriendRequest[] friendRequests;
}
[Serializable]
public class Friends
{
    public int user1;
    public int user2;
    public int id;
}
[Serializable]
public class FriendsListWrapper
{
    public Friends[] friends;
}
[Serializable]
public class FlowerSkin
{
    public Skin flower;
    public Skin stem;
    public Skin pot;
    public Skin hat;
    public Skin scarf;
    public Skin glasses;
}