﻿using System;
using RolePlaySet;
using RolePlaySet.Entity;

namespace RolePlaySetTests
{
    class StubStoreGateway : StoreGateway
    {
        public void createNewGame(string gameName)
        {
            
        }

        public Player[] loadPlayers(string gameName)
        {
            if (gameName.Equals("ValidGame"))
            {
                Player aPlayer = new Player();
                aPlayer.name = "A Player";
                Player[] players = { aPlayer, aPlayer };
                return players;
            }
            return null;
        }

        public Story loadStory(string gameName)
        {
            if (gameName.Equals("ValidGame"))
            {
                Story story = new Story();
                story.events.Add("1");
                story.events.Add("2");
                story.events.Add("3");
                return story;
            }
            return null;
        }

        public void saveGame(Story story, string gameName)
        {
            
        }
    }
}