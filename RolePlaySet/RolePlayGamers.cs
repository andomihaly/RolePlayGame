﻿using RolePlaySet.Entity;

namespace RolePlaySet
{
    public interface RolePlayGamers
    {
        Player[] getPlayers();
        void loadGame(string gameName);
        Player getPlayerByName(string playerName);
        void generateNewGame(string gameName);
        void AddTurn(string actualEventDescription, string playerName, int basePoint, int extraPoint, int numberOfDice, string diceType, int opponentPoint, bool isOpponentThrowToo);
        string[] getStory();
        string getDefaultImage();
    }
}
