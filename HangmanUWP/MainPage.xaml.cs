﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace HangmanUWP
{
    public sealed partial class MainPage : Page
    {
        private int lifes = 5;
        private string selectedWord = "Morten";
        private char[] charArray;

        public MainPage()
        {
            this.InitializeComponent();

            charArray = new char[selectedWord.Length];

            for (int i = 0; i < selectedWord.Length; i++)
            {
                charArray[i] = ' ';
            }

            foreach (var character in charArray)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Height = 30;
                textBlock.Width = 30;
                textBlock.Text = character.ToString();
                textBlock.TextAlignment = TextAlignment.Center;
                pTBStackPanel.Children.Add(textBlock);
            }

            List<char> ABC = new List<char>(){ 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'x', 'y', 'z', 'æ', 'ø', 'å' };
            foreach (var character in ABC)
            {
                Button ABCbtn = new Button();
                ABCbtn.Content = character.ToString();
                ABCbtn.Width = 40;
                ABCbtn.Click += new RoutedEventHandler(OnABCbtnClick);
                btnStackPanel.Children.Add(ABCbtn);
            }
        }

        void OnABCbtnClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if ((btn.Background as SolidColorBrush).Color == Windows.UI.Colors.Red)
            {
                Debug.WriteLine("Char already used");
            }
            else
            {
                if (selectedWord.ToLower().Contains(btn.Content.ToString()))
                {
                    Debug.WriteLine("Char is in the word");
                    for (int i = 0; i < selectedWord.Length; i++)
                    {
                        if (selectedWord[i].ToString().ToLower() == btn.Content.ToString())
                        {
                            Debug.WriteLine(btn.Content + " is in index: " + i);
                            charArray[i] = Convert.ToChar(btn.Content.ToString().ToLower());
                        }
                    }
                    pTBStackPanel.Children.Clear();

                    foreach (var character in charArray)
                    {
                        TextBlock textBlock = new TextBlock();
                        textBlock.Height = 30;
                        textBlock.Width = 30;
                        textBlock.Text = character.ToString();
                        textBlock.TextAlignment = TextAlignment.Center;
                        pTBStackPanel.Children.Add(textBlock);
                    }

                    if (!charArray.Contains(' '))
                    {
                        Debug.WriteLine("WINNER");
                    }
                }
                else
                {
                    Debug.WriteLine("Char is NOT in the word");
                    lifes--;
                    Debug.WriteLine(lifes);
                    if (lifes == 0)
                    {
                        Debug.WriteLine("GAME OVER!");
                    }
                }
                btn.Background = new SolidColorBrush(Colors.Red);
            }
        }
    }
}
