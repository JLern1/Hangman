﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace HangmanApp
{
    public partial class FormMain : Form
    {
        private List<string> _Words = new List<string>();
        private List<WordItem> _Letters = new List<WordItem>();
        string _word = string.Empty;
        int _guesses = 7;
        bool _isGameInSession = false;

        public FormMain()
        {
            InitializeComponent();
        }
        private void Btn_Click(object sender, EventArgs e)
        {
            lblLetterCorrect.Text = "";
            PromptUserToStartAGame();
            var btn = (Button)sender;
            char letterPressed = Convert.ToChar(btn.Text.ToLower());

            btn.Enabled = false;
            bool isLetterPresentInWord = IsLetterInWord(letterPressed);
            ShowMessages(letterPressed, isLetterPresentInWord);
            if (isLetterPresentInWord)
            {
                //replace _ with a letter
                var pressedLetter = _Letters.Where(x => x.Letter == letterPressed);
                var listOfDashes = lblCurrentWord.Text.Split(' ');

                foreach (var item in pressedLetter)
                {
                    listOfDashes[item.Index] = item.Letter.ToString();
                }
                string currentText = string.Empty;
                foreach (var item in listOfDashes)
                {
                    currentText += item + " ";
                }
                lblCurrentWord.Text = currentText;
            }

            var isGameNotFinished = _Letters.Any(x => x.IsGuesed == false);
            if (isGameNotFinished) return;

            EnableButtons(false);
            lblMessage.Text = "Congratulations, you just WON, press" +
                " \"New Game\" to continue";
        }

        private void ShowMessages(char letterPressed, bool isLetterPresentInWord)
        {
            if (isLetterPresentInWord)
            {
                lblLetterCorrect.Text = $"Your guess of \"{letterPressed.ToString().ToUpper()}\" is correct!";
                MarkGuessedLetters(letterPressed);
            }
            else
            {
                _guesses--;
                lblGuesses.Text = _guesses + " Remaining Guesses";
            }
        }

        private void MarkGuessedLetters(char letterPressed)
        {
            var listOfMatchingLetters = _Letters.Where(x => x.Letter == letterPressed);
            foreach (var item in listOfMatchingLetters)
            {
                item.IsGuesed = true;
            }
        }

        private bool IsLetterInWord(char letterPressed)
        {
            var result = from w in _Letters
                         where w.Letter == letterPressed
                         select w.Letter;

            return result.Any();
        }

        private void BtnNewGame_Click(object sender, EventArgs e)
        {
            _isGameInSession = true;
            this.lblMessage.Text = "";

            _Words.Add("Apple");
            _Words.Add("Orange");
            _Words.Add("Peach");

            StartNewGame();

        }

        private void StartNewGame()
        {
            Random rnd = new Random();
            var indexToPlay = rnd.Next(0, _Words.Count);
            _word = _Words[indexToPlay];
            lblCurrentWord.Text = "";
            CreateUnderscoreList();
            CreateListOfLetters();
            if (_isGameInSession) EnableButtons(true);
            lblWordGuessing.Text = _word;
        }

        private void CreateListOfLetters()
        {
            int i = 0;
            foreach (var l in _word.ToLower())
            {
                _Letters.Add(new
                    WordItem
                { Letter = l, IsGuesed = false, Index = i });
                i++;
            }
        }

        private void CreateUnderscoreList()
        {
            for (int i = 0; i < _word.Length; i++)
            {
                lblCurrentWord.Text += "_ ";
            }
        }

        private void EnableButtons(bool showHide)
        {

            foreach (var c in this.Controls)
            {
                if (c is Button)
                {
                    var btn = (Button)c;
                    if (btn.Name.StartsWith("btn"))
                    {
                        btn.Enabled = showHide;
                    }
                }
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.lblMessage.Text = "Welcome User, press \"New Game\" to begin!";
            this.lblMessage.ForeColor = Color.Blue;
        }

        private void PromptUserToStartAGame()
        {
            if (!_isGameInSession)
            {
                lblMessage.Text = "Please start a new game to play";
                lblMessage.ForeColor = Color.Red;
                return;
            }
        }
    }
}
