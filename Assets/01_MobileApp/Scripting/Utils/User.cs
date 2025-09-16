using UnityEngine;
using System;
[Serializable]
public class User
{
    public string name;
    public string skinID;
    public int id;
}

[Serializable]
public class UserListWrapper
{
    public User[] users;
}