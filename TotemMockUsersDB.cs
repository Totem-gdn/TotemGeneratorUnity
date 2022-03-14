using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TotemEntities;

public class TotemMockUsersDB
{
    private List<TotemUser> _users;

    public TotemMockUsersDB()
    {
        _users = new List<TotemUser>()
        {
            new TotemUser("user1", "pa$$1"),
            new TotemUser("user2", "pa$$2"),
            new TotemUser("user3", "pa$$3")
        };
    }

    public void AddNewUser(string uName, string uPwd)
    {
        var curr = _users.Find(u => u.GetUserName() == uName);
        if (curr != null)
        {
            throw new Exception("User with that name already exists!");
        }
        var newUser = new TotemUser(uName, uPwd);
        _users.Add(newUser);
    }

    public bool AuthenticateUser(string uName, string uPwd)
    {
        var auth = _users.Find(u => u.Authenticate(uName, uPwd));
        return auth != null;
    }

    [CanBeNull]
    public TotemUser GetUser(string uName)
    {
        return _users.Find(u => u.GetUserName() == uName);
    }

    public void AddAvatarToUser(string userName, TotemAvatar a)
    {
        var currUser = _users.Find(user => user == a.GetCurrentOwner());
        var user = _users.Find(user => user.GetUserName() == userName);
        currUser?.RemoveAvatar(a);
        a.SetOwner(user);
        user.AddAvatar(a);
    }
        
    public void AddSpearToUser(string userName, TotemSpear s)
    {
        var currUser = _users.Find(user => user == s.GetCurrentOwner());
        var user = _users.Find(user => user.GetUserName() == userName);
        currUser?.RemoveSpear(s);
        s.SetOwner(user);
        user.AddSpear(s);
    }
}