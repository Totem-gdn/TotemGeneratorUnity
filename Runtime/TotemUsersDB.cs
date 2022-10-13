using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TotemEntities;
using TotemUtils;

public class TotemUsersDB
{
    private List<TotemUser> _users;

    public TotemUsersDB()
    {
        _users = new List<TotemUser>();
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

    public void AddNewUser(TotemUser user)
    {
        var curr = _users.Find(u => u.GetUserName() == user.GetUserName());
        if (curr != null)
        {
            throw new Exception("User with that name already exists!");
        }
        _users.Add(user);
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
        var foundUser = _users.Find(user => user.GetUserName() == userName);
        currUser?.RemoveAvatar(a);
        a.SetOwner(foundUser);
        foundUser.AddAvatar(a);
    }
        
    public void AddSpearToUser(string userName, TotemSpear s)
    {
        var currUser = _users.Find(user => user == s.GetCurrentOwner());
        var foundUser = _users.Find(user => user.GetUserName() == userName);
        currUser?.RemoveSpear(s);
        s.SetOwner(foundUser);
        foundUser.AddSpear(s);
    }

    public void AddSwordToUser(string userName, TotemSword s)
    {
        var currUser = _users.Find(user => user == s.GetCurrentOwner());
        var foundUser = _users.Find(user => user.GetUserName() == userName);
        currUser?.RemoveSword(s);
        s.SetOwner(foundUser);
        foundUser.AddSword(s);
    }
}