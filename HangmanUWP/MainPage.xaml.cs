using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace HangmanUWP
{
    public sealed partial class MainPage : Page
    {
        private int lifes;
        private string selectedWord;
        string[] words = new string[] {"Morten", "David", "Sofie"};
        private string[] charArray;
        private StackPanel pTBStackPanel, topStackPanel;
        private Image img;

        public MainPage()
        {
            this.InitializeComponent();

            Setup();
        }

        private void Setup()
        {
            lifes = 6;

            topStackPanel = new StackPanel();
            topStackPanel.Name = "topStackPanel";
            topStackPanel.Orientation = Orientation.Horizontal;
            topStackPanel.HorizontalAlignment = HorizontalAlignment.Center;

            pTBStackPanel = new StackPanel();
            pTBStackPanel.Name = "pTBStackPanel";
            pTBStackPanel.Orientation = Orientation.Horizontal;

            img = new Image();
            img.Source = new BitmapImage(new Uri(base.BaseUri, "/Assets/Galge1.png"));

            topStackPanel.Children.Add(pTBStackPanel);
            topStackPanel.Children.Add(img);

            StackPanel bottomStackPanel = new StackPanel();
            bottomStackPanel.Name = "bottomStackPanel";
            bottomStackPanel.Orientation = Orientation.Vertical;
            bottomStackPanel.HorizontalAlignment = HorizontalAlignment.Center;

            StackPanel btnTopStackPanel = new StackPanel();
            btnTopStackPanel.Name = "btnTopStackPanel";
            btnTopStackPanel.Orientation = Orientation.Horizontal;
            btnTopStackPanel.Margin = new Thickness(0,50,0,0);

            StackPanel btnBottomStackPanel = new StackPanel();
            btnBottomStackPanel.Name = "btnBottomStackPanel";
            btnBottomStackPanel.Orientation = Orientation.Horizontal;

            bottomStackPanel.Children.Add(btnTopStackPanel);
            bottomStackPanel.Children.Add(btnBottomStackPanel);

            mainStackPanel.Children.Add(topStackPanel);
            mainStackPanel.Children.Add(bottomStackPanel);

            //Selects a random word
            Random rnd = new Random();
            selectedWord = words[rnd.Next(0, words.Length)];

            //Makes char array in length of selected word
            charArray = new string[selectedWord.Length];

            //fills charArray with empty indexes and show them in textblocks
            for (int i = 0; i < selectedWord.Length; i++) charArray[i] = "_";
            foreach (var character in charArray)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Height = 30;
                textBlock.Width = 30;
                textBlock.Text = character.ToString();
                textBlock.TextDecorations = TextDecorations.Underline;
                textBlock.TextAlignment = TextAlignment.Center;
                pTBStackPanel.Children.Add(textBlock);
            }

            List<char> ABC = new List<char>()
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v',
                'x', 'y', 'z', 'æ', 'ø', 'å'
            };
            int count = 0;
            foreach (var character in ABC)
            {
                Button ABCbtn = new Button();
                ABCbtn.Content = character.ToString().ToUpper();
                ABCbtn.Width = 40;
                ABCbtn.Click += new RoutedEventHandler(OnABCbtnClick);

                if (count < 14) btnTopStackPanel.Children.Add(ABCbtn);
                else btnBottomStackPanel.Children.Add(ABCbtn);

                count++;
            }
        }

        void OnABCbtnClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if ((btn.Background as SolidColorBrush).Color == Windows.UI.Colors.Red || (btn.Background as SolidColorBrush).Color == Windows.UI.Colors.GreenYellow)
            {
                Debug.WriteLine("Char already used");
            }
            else
            {
                if (selectedWord.ToUpper().Contains(btn.Content.ToString()))
                {
                    Debug.WriteLine("Char is in the word");
                    for (int i = 0; i < selectedWord.Length; i++)
                    {
                        if (selectedWord[i].ToString().ToUpper() == btn.Content.ToString())
                        {
                            Debug.WriteLine(btn.Content + " is in index: " + i);
                            charArray[i] = btn.Content.ToString().ToUpper();
                        }
                    }
                    pTBStackPanel.Children.Clear();

                    foreach (var character in charArray)
                    {
                        TextBlock textBlock = new TextBlock();
                        textBlock.Height = 30;
                        textBlock.Width = 30;
                        textBlock.Text = character;
                        textBlock.TextDecorations = TextDecorations.Underline;
                        textBlock.TextAlignment = TextAlignment.Center;
                        pTBStackPanel.Children.Add(textBlock);
                    }

                    if (!charArray.Contains("_"))
                    {
                        Debug.WriteLine("WINNER");
                        mainStackPanel.Children.Clear();

                        TextBlock textBlock = new TextBlock();
                        textBlock.Text = "WINNER!";
                        textBlock.FontSize = 70;
                        textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                        textBlock.VerticalAlignment = VerticalAlignment.Center;
                        mainStackPanel.Children.Add(textBlock);

                        TextBlock textBlock2 = new TextBlock();
                        textBlock2.Text = "The word was: " + selectedWord;
                        textBlock2.FontSize = 70;
                        textBlock2.HorizontalAlignment = HorizontalAlignment.Center;
                        textBlock2.VerticalAlignment = VerticalAlignment.Center;
                        mainStackPanel.Children.Add(textBlock2);

                        Button playAgainButton = new Button();
                        playAgainButton.Content = "Play Again";
                        playAgainButton.HorizontalAlignment = HorizontalAlignment.Center;
                        playAgainButton.VerticalAlignment = VerticalAlignment.Center;
                        playAgainButton.Click += playAgain_Click;
                        mainStackPanel.Children.Add(playAgainButton);

                        Button exitButton = new Button();
                        exitButton.Content = "Exit";
                        exitButton.HorizontalAlignment = HorizontalAlignment.Center;
                        exitButton.VerticalAlignment = VerticalAlignment.Center;
                        exitButton.Click += exit_Click;
                        mainStackPanel.Children.Add(exitButton);
                    }
                    btn.Background = new SolidColorBrush(Colors.GreenYellow);
                }
                else
                {
                    Debug.WriteLine("Char is NOT in the word");
                    lifes--;
                    Debug.WriteLine(lifes);
                    if (lifes == 5)img.Source = new BitmapImage(new Uri(base.BaseUri, "/Assets/Galge2.png"));
                    else if (lifes == 4) img.Source = new BitmapImage(new Uri(base.BaseUri, "/Assets/Galge3.png"));
                    else if (lifes == 3) img.Source = new BitmapImage(new Uri(base.BaseUri, "/Assets/Galge4.png"));
                    else if (lifes == 2) img.Source = new BitmapImage(new Uri(base.BaseUri, "/Assets/Galge5.png"));
                    else if (lifes == 1) img.Source = new BitmapImage(new Uri(base.BaseUri, "/Assets/Galge6.png"));
                    else if (lifes == 0)
                    {
                        Debug.WriteLine("GAME OVER!");
                        mainStackPanel.Children.Clear();

                        TextBlock textBlock = new TextBlock();
                        textBlock.Text = "GAME OVER!";
                        textBlock.FontSize = 70;
                        textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                        textBlock.VerticalAlignment = VerticalAlignment.Center;
                        mainStackPanel.Children.Add(textBlock);

                        TextBlock textBlock2 = new TextBlock();
                        textBlock2.Text = "The word was: " + selectedWord;
                        textBlock2.FontSize = 70;
                        textBlock2.HorizontalAlignment = HorizontalAlignment.Center;
                        textBlock2.VerticalAlignment = VerticalAlignment.Center;
                        mainStackPanel.Children.Add(textBlock2);

                        Image img = new Image();
                        img.Source = new BitmapImage(new Uri(base.BaseUri, "/Assets/Galge7.png"));
                        mainStackPanel.Children.Add(img);

                        Button playAgainButton = new Button();
                        playAgainButton.Content = "Play Again";
                        playAgainButton.HorizontalAlignment = HorizontalAlignment.Center;
                        playAgainButton.VerticalAlignment = VerticalAlignment.Center;
                        playAgainButton.Click += playAgain_Click;
                        mainStackPanel.Children.Add(playAgainButton);

                        Button exitButton = new Button();
                        exitButton.Content = "Exit";
                        exitButton.HorizontalAlignment = HorizontalAlignment.Center;
                        exitButton.VerticalAlignment = VerticalAlignment.Center;
                        exitButton.Click += exit_Click;
                        mainStackPanel.Children.Add(exitButton);
                    }
                    btn.Background = new SolidColorBrush(Colors.Red);
                }
            }
        }

        private void playAgain_Click(object sender, RoutedEventArgs e)
        {
            mainStackPanel.Children.Clear();
            Setup();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            CoreApplication.Exit();
        }
    }
}
