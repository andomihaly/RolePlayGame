﻿using RandomDice;
using RolePlayEntity;
using System;
using System.Collections.Generic;

namespace RolePlaySet.Core
{
    public class RolePlayGameCoordinator : RolePlayGame
    {
        private StoreGateway storeGateway;
        private TurnEventHandler turnHandle;
        private Dice[] dices;

        private Story story = new Story();
        private Player[] players;
        private string gameName;
        private string defaultImage = "";


        public RolePlayGameCoordinator(StoreGateway storeGateway, Dice[] dices)
        {
            this.storeGateway = storeGateway;
            this.dices = dices;
            turnHandle = new TurnEventHandler(dices, new NewTurnHuTextBuilder());
        }

        public void generateNewGame(string gameName)
        {
            checkGameName(gameName);
            this.gameName = reformatGameName(gameName);

            storeGateway.createNewGame(gameName);
        }

        public string[] getAvailableDiceName()
        {
            List<string> diceName = new List<string>();
            foreach (Dice dice in dices)
            {
                diceName.Add(dice.getName());
            }
            return diceName.ToArray();
        }

        public TaskType[] getTaskTypeList()
        {
            return EventTaskGenerator.generateEventTasksList().ToArray();
        }


        public void loadGame(string gameName)
        {
            checkGameName(gameName);
            this.gameName = reformatGameName(gameName);
            defaultImage = storeGateway.loadDefaultImage(gameName);
            loadPlayers(gameName);
            loadStory(gameName);
        }

        public Player[] getPlayers()
        {
            return players;
        }

        public string[] getStory()
        {
            return story.events.ToArray();
        }

        public string getDefaultImage()
        {
            return defaultImage;
        }


        public Player getPlayerByName(string playerName)
        {
            if (!(players == null))
            {
                foreach (Player onePlayer in players)
                {
                    if (onePlayer.name.Equals(playerName))
                    {
                        return onePlayer;
                    }
                }
            }
            return null;
        }

        public void addTurnTaskEvent(string actualEventDescription, string playerName, int basePoint, int extraPoint, int numberOfDice, string diceType, TaskType evenetPoint)
        {
            story.events.Add(turnHandle.generateTurnTaskEvent(actualEventDescription, playerName, basePoint, extraPoint, numberOfDice, diceType, evenetPoint));
            storeGateway.saveGame(story, gameName);
        }

        public void addTurnOpponentEvent(string actualEventDescription, string playerName, int basePoint, int extraPoint, int numberOfDice, string diceType, int opponentPoint, bool isOpponentThrowToo)
        {
            story.events.Add(turnHandle.generateTurnOpponentEvent(actualEventDescription, playerName, basePoint, extraPoint, numberOfDice, diceType, opponentPoint, isOpponentThrowToo));
            storeGateway.saveGame(story, gameName);
        }

        private void checkGameName(string gameName)
        {
            if (gameName == null)
            {
                throw new GameNameIsNotValid(gameName);
            }
            gameName = gameName.Trim();
            if (gameName.Equals(""))
            {
                throw new GameNameIsNotValid(gameName);
            }
        }

        private string reformatGameName(String gameName)
        {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                gameName = gameName.Replace(c, '_');
            }
            return gameName;
        }

        private void loadPlayers(string gameName)
        {
            try
            {
                players = storeGateway.loadPlayers(gameName);
            }
            catch (Exception)
            {
                players = null;
            }
        }

        private void loadStory(string gameName)
        {
            Story teamStory = storeGateway.loadStory(gameName);
            if (teamStory != null)
            {
                story = teamStory;
            }
        }
    }
}