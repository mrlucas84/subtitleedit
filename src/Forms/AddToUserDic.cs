﻿using System;
using System.Drawing;
using System.Windows.Forms;
using Nikse.SubtitleEdit.Logic;
using System.Collections.Generic;

namespace Nikse.SubtitleEdit.Forms
{
    public partial class AddToUserDic : Form
    {

        public AddToUserDic()
        {
            InitializeComponent();
            Text = Configuration.Settings.Language.AddToUserDictionary.Title;
            labelDescription.Text = Configuration.Settings.Language.AddToUserDictionary.Description;
            buttonCancel.Text = Configuration.Settings.Language.General.Cancel;
            buttonOK.Text = Configuration.Settings.Language.General.Ok;
            FixLargeFonts();
        }

        private void FixLargeFonts()
        {
            Graphics graphics = this.CreateGraphics();
            SizeF textSize = graphics.MeasureString(buttonOK.Text, this.Font);
            if (textSize.Height > buttonOK.Height - 4)
            {
                int newButtonHeight = (int)(textSize.Height + 7 + 0.5);
                Utilities.SetButtonHeight(this, newButtonHeight, 1);
            }
        }

        private void AddToUserDic_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                DialogResult = DialogResult.Cancel;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            NewWord = textBoxAddName.Text.Trim().ToLower();
            if (NewWord.Length == 0)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }

            string language = comboBoxDictionaries.Text;
            if (language.IndexOf('[') > 0)
                language = language.Substring(language.IndexOf('[')).TrimStart('[');
            if (language.IndexOf(']') > 0)
                language = language.Substring(0, language.IndexOf(']'));

            var userWordList = new List<string>();

            Utilities.LoadUserWordList(userWordList, language);
            if (!string.IsNullOrEmpty(language) && NewWord.Length > 0 && !userWordList.Contains(NewWord))
            {
                Utilities.AddToUserDictionary(NewWord, language);
                DialogResult = DialogResult.OK;
                return;
            }
            DialogResult = DialogResult.Cancel;
        }

        internal void Initialize(string hunspellName, string text)
        {
            if (!string.IsNullOrEmpty(text))
                textBoxAddName.Text = text.Trim().TrimEnd('.', '!', '?');

            comboBoxDictionaries.Items.Clear();
            foreach (string name in Utilities.GetDictionaryLanguages())
            {
                comboBoxDictionaries.Items.Add(name);
                if (hunspellName != null && name.Equals(hunspellName, StringComparison.OrdinalIgnoreCase))
                    comboBoxDictionaries.SelectedIndex = comboBoxDictionaries.Items.Count - 1;
            }
        }

        public string NewWord { get; private set; }
    }
}
