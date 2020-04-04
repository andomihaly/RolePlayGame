﻿using RolePlaySet;
using RolePlaySet.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RolePlayFileBasedStorage
{
    public class RolePlayFileStorage : StoreGateway
    {
        private static string PLAYER_FILE_NAME = "players.txt";
        private static string STORY_FILE_NAME = "story.txt";
        private static string PLAYER_NAME_FLAG = "+";
        private static string SKILL_FLAG = "-";
        private static string SKILL_SEPARATOR = "|";
        private static string IMAGE_FLAG = "*";

        private string gameName;
        private string path;
        private int rowIndex;
        private List<Player> players;
        string[] playerFileLines;



        public void createNewGame(string gameName)
        {
            checkGameName(gameName);
            this.gameName = gameName;
            generateGameStructure();
        }

        public Player[] loadPlayers(string gameName)
        {
            checkGameName(gameName);
            this.gameName = gameName;
            return loadPlayersFromFile();
        }

        public string loadDefaultImage(string gameName)
        {
            checkGameName(gameName);
            this.gameName = gameName;

            string defaultImage = generatePath() + "\\default.png";
            if (File.Exists(defaultImage))
            {
                return defaultImage;
            }
            defaultImage = generatePath() + "\\default.jpg";
            if (File.Exists(defaultImage))
            {
                return defaultImage;
            }

            return "";
        }

        public Story loadStory(string gameName)
        {
            checkGameName(gameName);
            this.gameName = gameName;
            path = generatePath();
            String storyFile = path + "\\" + STORY_FILE_NAME;
            Story story = new Story();
            story.events = System.IO.File.ReadAllLines(storyFile).OfType<string>().ToList();
            return story;

        }

        public void saveGame(Story story, string gameName)
        {
            checkGameName(gameName);
            this.gameName = gameName;
            path = generatePath();
            String storyFile = path + "\\" + STORY_FILE_NAME;
            System.IO.File.WriteAllLines(storyFile, story.events);
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

        private void generateGameStructure()
        {
            try
            {
                path = generatePath();
                createGameFolder();
                createPlayerExample();
                createStoryFile();
            }
            catch (Exception)
            {
                throw new CouldNotCreateNewGameFileStructure("\"" + gameName + "\" structure creation is faild.");
            }
        }

        private string generatePath()
        {
            return Directory.GetCurrentDirectory() + "\\" + gameName;
        }

        private void createGameFolder()
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        private void createPlayerExample()
        {
            String playerFile = path + "\\" + PLAYER_FILE_NAME;
            if (!File.Exists(playerFile))
            {
                using (StreamWriter sw = File.CreateText(playerFile))
                {
                    sw.WriteLine(PLAYER_NAME_FLAG + "Name1");
                    sw.WriteLine(SKILL_FLAG + "Skill1Name" + SKILL_SEPARATOR + "+1");
                    sw.WriteLine(SKILL_FLAG + "Skill2Name" + SKILL_SEPARATOR + "+3");
                    sw.WriteLine(PLAYER_NAME_FLAG + "Name2");
                    sw.WriteLine(SKILL_FLAG + "Skill1Name" + SKILL_SEPARATOR + "+2");
                    sw.WriteLine(SKILL_FLAG + "Skill2Name" + SKILL_SEPARATOR + "+3");
                    sw.WriteLine(SKILL_FLAG + "Skill3Name" + SKILL_SEPARATOR + "0");
                    sw.WriteLine(SKILL_FLAG + "Skill4Name" + SKILL_SEPARATOR + "-1");
                    sw.WriteLine(IMAGE_FLAG + "actor400pxHeigh.jpg");
                    sw.WriteLine(PLAYER_NAME_FLAG + "Name3");
                    sw.WriteLine(IMAGE_FLAG + "image.png");
                    sw.WriteLine(PLAYER_NAME_FLAG + "Name4");                    
                }
            }
        }

        private void createStoryFile()
        {
            String storyFile = path + "\\" + STORY_FILE_NAME;
            if (!File.Exists(storyFile))
            {
                File.CreateText(storyFile);

            }
        }

        private Player[] loadPlayersFromFile()
        {
            path = generatePath();
            String playerFile = path + "\\" + PLAYER_FILE_NAME;

            playerFileLines = System.IO.File.ReadAllLines(playerFile);
            players = new List<Player>();
            rowIndex = 0;

            while (rowIndex < playerFileLines.Length)
            {
                parseAndAddNextPlayerToList();
                rowIndex++;
            }
            return players.ToArray();
        }

        private void parseAndAddNextPlayerToList()
        {
            string tempLine = playerFileLines[rowIndex];
            if (tempLine[0].Equals(PLAYER_NAME_FLAG[0]))
            {
                Player player = new Player();
                player.name = tempLine.Substring(1);
                while ((rowIndex + 1) < playerFileLines.Length && playerFileLines[rowIndex + 1][0].Equals(SKILL_FLAG[0]))
                {
                    rowIndex++;
                    parseAndAddNextSkillToPlayer(player);
                }
                if ((rowIndex + 1) < playerFileLines.Length && playerFileLines[rowIndex + 1][0].Equals(IMAGE_FLAG[0]))
                {
                    rowIndex++;
                    player.image = path + "\\" + playerFileLines[rowIndex].Substring(1);
                    if (!File.Exists(player.image))
                    {
                        player.image = "";
                    }
                }
                players.Add(player);
            }
        }

        private void parseAndAddNextSkillToPlayer(Player player)
        {
            string tempLine = playerFileLines[rowIndex].Substring(1);
            string[] skillArray = tempLine.Split(SKILL_SEPARATOR[0]);
            if (skillArray.Length == 2)
            {
                Skill skill = new Skill();
                skill.name = skillArray[0];
                if (isConverttableToInt(skillArray[1]))
                {
                    skill.score = Convert.ToInt32(skillArray[1]);
                }
                else
                {
                    skill.score = 0;
                }
                player.skills.Add(skill);
            }
        }

        private bool isConverttableToInt(String number)
        {
            try
            {
                Convert.ToInt32(number);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }


    }
}
