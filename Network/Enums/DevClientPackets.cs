﻿namespace DevProLauncher.Network.Enums
{
    public enum DevClientPackets
    {
        GameList = 0,
        RemoveRoom = 1,
        UpdatePlayers = 2,
        LoginAccepted = 3,
        LoginFailed = 4,
        ServerMessage = 5,
        Banned = 6,
        Kicked = 7,
        RegisterAccept = 8,
        RegisterFailed = 9,
        Pong = 10,
        RoomStart = 11,
        UserList = 12,
        UpdateUserInfo = 13,
        FriendList = 15,
        JoinChannelAccept = 16,
        Message = 17,
        DuelRequest = 18,
        DevPoints = 19,
        UserStats = 20,
        TeamStats = 21,
        TeamList = 22,
        AcceptDuelRequest = 23,
        RefuseDuelRequest = 24,
        StartDuel = 25,
        TeamRequest = 26,
        GameServers = 29,
        RemoveServer = 30,
        AddServer = 31,
        ChannelList = 32,
        TournamentList = 33,
        TournamentRoomList = 34,
        TournamentRommUpdate = 35,
        TournamentMessage = 36,
        CreateRoom = 37,
        ChannelUsers = 38,
        AddChannelUser = 39,
        RemoveChannelUser = 40,
        Ranking = 41,
        Invalid = 42,
        InvalidTemp = 99,
        ValidateAccept = 43,
        ValidateFailed = 44,
        ResendAccept = 45,
        ResendFailed = 46,
        ChangeAccept = 47,
        ChangeFailed = 48,
        DuplicateMail = 49,
        BlacklistMail = 50,
        MailFormat = 51
    }
}
