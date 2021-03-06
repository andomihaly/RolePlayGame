﻿using RolePlayGUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace RolePlayGUI
{
    public partial class RolePlayBoard
    {

        public void refillStoryBox(String[] story)
        {
            storyBox.Clear();
            for (int i = story.Length; i > 0; i--)
            {
                storyBox.Text += i.ToString() + ". lépés:" + (story[i - 1] + NEW_LINE);
                if (i == story.Length)
                {
                    storyBox.Text += (NEW_LINE);
                }
            }


        }
        public void VisualizeLastDiceRolls(RolledDiceInTurn rolledDices)
        {
            if (rolledDices.opponent.Count != 0)
            {
                opponentDiceLabel.Visible = true;
                opponenetDicesPictureBox.Visible = true;
                opponenetDicesPictureBox.Image = ImageCreator.generateDiceImage(rolledDices.opponent);
            }
            else
            {
                opponentDiceLabel.Visible = false;
                opponenetDicesPictureBox.Visible = false;
            }
            playerDiceLabel.Visible = true;
            playerDicesPictureBox.Visible = true;
            playerDicesPictureBox.Image = ImageCreator.generateDiceImage(rolledDices.player);
        }


        private void nextTurn()
        {
            if (isPointsAndNumbersConvertable())
            {
                try
                {
                    prepareNextTurnAndSend();
                }
                catch (Exception)
                {
                    notSavedGameLabel.Visible = true;
                }
            }
        }

        private void prepareNextTurnAndSend()
        {
            string actualPlayerName = "";
            if (playersComboBox.SelectedItem != null)
            {
                actualPlayerName = playersComboBox.SelectedItem.ToString();
            }
            else if (playersComboBox.Text != null && !playersComboBox.Text.ToString().Equals(resourceManager.GetString("playerName", actualCultureInfo)))
            {
                actualPlayerName = playersComboBox.Text.ToString();
            }
            if (opponentRadioButton.Checked)
            {
                sendOpponentEvent(actualPlayerName);
            }
            else
            {
                sendTaskEven(actualPlayerName);

            }
            eventDescription.Text = "";
        }

        private void sendOpponentEvent(string actualName)
        {
            if (isConverttableToInt(opponentPoint.Text))
            {
                gameCoordinator.addTurnOpponentEvent(eventDescription.Text, actualName,
                    Convert.ToInt32(playerBasedPoint.Text), Convert.ToInt32(playerExtraPoint.Text),
                    Convert.ToInt32(numberOfDice.Text), diceType.SelectedItem.ToString(),
                    Convert.ToInt32(opponentPoint.Text), opponenetThrowDiceToo.Checked);
            }
        }

        private void sendTaskEven(string actualName)
        {
            String taskName = findEventTaskBasedOnEventTaskName(ladderComboBox.SelectedItem.ToString());
            if (taskName != null)
            {
                gameCoordinator.addTurnTaskEvent(eventDescription.Text, actualName,
                        Convert.ToInt32(playerBasedPoint.Text), Convert.ToInt32(playerExtraPoint.Text),
                        Convert.ToInt32(numberOfDice.Text), diceType.SelectedItem.ToString(), taskName);
            }
        }


        private void loadAndFillEventTasks()
        {
            ladderComboBox.Items.Clear();
            foreach (Task task in gameCoordinator.taskList)
            {
                ladderComboBox.Items.Add(task.name);
            }
            ladderComboBox.Text = resourceManager.GetString("ladderTask", actualCultureInfo);
        }

        private string findEventTaskBasedOnEventTaskName(string eventTaskName)
        {
            foreach (Task task in gameCoordinator.taskList)
            { 
                if (task.name.Equals(eventTaskName))
                    return task.name;
            }
            return null;
        }




        private void fillGUIWithGame()
        {
            notSavedGameLabel.Visible = false;
            foreach (GamePlayer player in gameCoordinator.gamePlayers)
            {
                playersComboBox.Items.Add(player.name);
            }
            playersComboBox.Items.Add("-");
            /*
            else
            {
            TODO: átvinni a helyére   
            createNotificationFormFauilt(rm.GetString("errorNotLoad", actualCultureInfo) +
                                                rolePlayGameName.Text +
                                                rm.GetString("errorFromGame", actualCultureInfo));
            }*/
            reloadDefaultImage();
        }

        private void reloadSkillList(List<GamePlayerSkill> skills)
        {
            playerSkillComboBox.SelectedItem = null;
            playerSkillComboBox.Items.Clear();
            foreach (GamePlayerSkill skill in skills)
            {
                playerSkillComboBox.Items.Add(skill.gamePlayerSkillName);
            }
            playerSkillComboBox.Text = resourceManager.GetString("skill", actualCultureInfo);
        }

        private void reloadImage(string playerName)
        {
            GamePlayer player = gameCoordinator.findGamePlayer(playerName);
            reloadDefaultImage();
            if (player != null && !player.imagePath.Equals(""))
            {
                playerPicture.Image = Image.FromFile(player.imagePath);
            }
        }
        private void reloadDefaultImage()
        {
            if (!gameCoordinator.defaultImagePath.Equals(""))
            {
                playerPicture.Image = Image.FromFile(gameCoordinator.defaultImagePath);
            }
        }




        private void calculateSumPlayerPoint()
        {
            if (!isConverttableToInt(playerBasedPoint.Text))
            {
                playerBasedPoint.Text = ZERO.ToString();
            }
            if (!isConverttableToInt(playerExtraPoint.Text))
            {
                playerExtraPoint.Text = ZERO.ToString();
            }
            sumPlayerPoint.Text = (Convert.ToInt32(playerBasedPoint.Text) + Convert.ToInt32(playerExtraPoint.Text)).ToString();
        }




        private void loadLanguageTexts()
        {
            this.Text = resourceManager.GetString("rolePlayBoard", actualCultureInfo);
            narrationButton.Text = resourceManager.GetString("narration", actualCultureInfo);
            actualEvent.Text = resourceManager.GetString("actualEvent", actualCultureInfo);
            diceLabel.Text = resourceManager.GetString("diceInstruction", actualCultureInfo);
            throwDice.Text = resourceManager.GetString("throwDice", actualCultureInfo);
            basePontLabel.Text = resourceManager.GetString("basePoint", actualCultureInfo);
            extraPointLabel.Text = resourceManager.GetString("extraPoint", actualCultureInfo);
            sumPointLabel.Text = resourceManager.GetString("sumPoint", actualCultureInfo);
            opponentPointLabel.Text = resourceManager.GetString("opponentPoint", actualCultureInfo);
            vsLabel.Text = resourceManager.GetString("vs", actualCultureInfo);
            opponenetThrowDiceToo.Text = resourceManager.GetString("opponentThrowToo", actualCultureInfo);
            languageGroupBox.Text = resourceManager.GetString("languageTag", actualCultureInfo);
            languageRadioButtonHu.Text = resourceManager.GetString("hu", actualCultureInfo);
            languageRadioButtonEn.Text = resourceManager.GetString("en", actualCultureInfo);
            loadGame.Text = resourceManager.GetString("loadGame", actualCultureInfo); ;
            generateGame.Text = resourceManager.GetString("generateGame", actualCultureInfo);
            historyLabel.Text = resourceManager.GetString("story", actualCultureInfo);
            playersComboBox.Text = resourceManager.GetString("playerName", actualCultureInfo);
            playerSkillComboBox.Text = resourceManager.GetString("skill", actualCultureInfo);
            notSavedGameLabel.Text = resourceManager.GetString("gameIsNotSaved", actualCultureInfo);

            playerDiceLabel.Text = resourceManager.GetString("playerDiceRoll", actualCultureInfo);
            opponentDiceLabel.Text = resourceManager.GetString("opponentDiceRoll", actualCultureInfo);

            ladderRadioButton.Text = resourceManager.GetString("task", actualCultureInfo);
            opponentRadioButton.Text = resourceManager.GetString("opponent", actualCultureInfo);
            ladderComboBox.Text = resourceManager.GetString("ladderTask", actualCultureInfo);
            opponentGroupBox.Text = resourceManager.GetString("eventType", actualCultureInfo);

            diceType.Items.Clear();
            foreach (string actualDiceType in gameCoordinator.dicesList)
            {
                diceType.Items.Add(actualDiceType);
            }
            diceType.SelectedIndex = 0;
            playerBasedPoint.Text = ZERO.ToString();
            playerExtraPoint.Text = ZERO.ToString();
            opponentPoint.Text = ZERO.ToString();
            numberOfDice.Text = ZERO.ToString();
            reloadDefaultImage();
        }




        private bool isPointsAndNumbersConvertable()
        {
            return (isConverttableToInt(playerBasedPoint.Text) && isConverttableToInt(playerExtraPoint.Text) &&
                    isConverttableToInt(numberOfDice.Text));
        }


        private bool isConverttableToInt(string number)
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
