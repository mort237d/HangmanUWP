using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
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
        private string selectedWord = "hejsameddejsa";
        string[] words = new string[] {"Bil", "Cykel", "Marmelade", "Syltetøj", "Kaffe", "Brød", "Medister", "Akvarie", "Spade", "Jord", "Moderjord" };
        private string[] stringArray;
        private StackPanel pTBStackPanel, topStackPanel;
        private Image img;
        private TextBox newWordTextBox, playAgainNewWordTextBox;
        private List<char> ABC = new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'x', 'y', 'z', 'æ', 'ø', 'å' };

        public MainPage()
        {
            this.InitializeComponent();

            Setup();
        }

        private void Setup()
        {
            lifes = 6;
            
            //Selects a random word
            if (selectedWord == "hejsameddejsa")
            {
                Random rnd = new Random();
                selectedWord = words[rnd.Next(0, words.Length)];
            }

            //Makes char array in length of selected word
            stringArray = new string[selectedWord.Length];

            //fills stringArray with empty indexes and show them in textblocks
            for (int i = 0; i < selectedWord.Length; i++) stringArray[i] = "_";

            GUISetup();
        }

        private void GUISetup()
        {
            topStackPanel = new StackPanel();
            topStackPanel.Name = "topStackPanel";
            topStackPanel.Orientation = Orientation.Horizontal;
            topStackPanel.HorizontalAlignment = HorizontalAlignment.Center;
            topStackPanel.Margin = new Thickness(0, 50, 0, 0);

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
            btnTopStackPanel.Margin = new Thickness(0, 50, 0, 0);

            StackPanel btnBottomStackPanel = new StackPanel();
            btnBottomStackPanel.Name = "btnBottomStackPanel";
            btnBottomStackPanel.Orientation = Orientation.Horizontal;

            bottomStackPanel.Children.Add(btnTopStackPanel);
            bottomStackPanel.Children.Add(btnBottomStackPanel);

            newWordTextBox = new TextBox();

            Button newWordbtn = new Button();
            newWordbtn.Content = "Nyt spil med indtastede ord";
            newWordbtn.Click += AddWord_Click;

            StackPanel lastStackPanel = new StackPanel();
            lastStackPanel.Name = "btnBottomStackPanel";
            lastStackPanel.Orientation = Orientation.Horizontal;
            lastStackPanel.Margin = new Thickness(0, 50, 0, 0);
            lastStackPanel.HorizontalAlignment = HorizontalAlignment.Center;

            lastStackPanel.Children.Add(newWordTextBox);
            lastStackPanel.Children.Add(newWordbtn);

            mainStackPanel.Children.Add(topStackPanel);
            mainStackPanel.Children.Add(bottomStackPanel);
            mainStackPanel.Children.Add(lastStackPanel);

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

            foreach (var character in stringArray)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Height = 30;
                textBlock.Width = 30;
                textBlock.Text = character;
                textBlock.TextDecorations = TextDecorations.Underline;
                textBlock.TextAlignment = TextAlignment.Center;
                pTBStackPanel.Children.Add(textBlock);
            }
        }

        public void OnABCbtnClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if ((btn.Background as SolidColorBrush).Color == Windows.UI.Colors.Red || (btn.Background as SolidColorBrush).Color == Windows.UI.Colors.GreenYellow)
            {
            }
            else
            {
                if (selectedWord.ToUpper().Contains(btn.Content.ToString()))
                {
                    for (int i = 0; i < selectedWord.Length; i++)
                    {
                        if (selectedWord[i].ToString().ToUpper() == btn.Content.ToString())
                        {
                            stringArray[i] = btn.Content.ToString().ToUpper();
                        }
                    }
                    pTBStackPanel.Children.Clear();

                    foreach (var character in stringArray)
                    {
                        TextBlock textBlock = new TextBlock();
                        textBlock.Height = 30;
                        textBlock.Width = 30;
                        textBlock.Text = character;
                        textBlock.TextDecorations = TextDecorations.Underline;
                        textBlock.TextAlignment = TextAlignment.Center;
                        pTBStackPanel.Children.Add(textBlock);
                    }

                    if (!stringArray.Contains("_"))
                    {
                        mainStackPanel.Children.Clear();

                        TextBlock textBlock = new TextBlock
                        {
                            Text = "WINNER!",
                            FontSize = 70,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center
                        };
                        mainStackPanel.Children.Add(textBlock);

                        TextBlock textBlock2 = new TextBlock
                        {
                            Text = "The word was: " + selectedWord,
                            FontSize = 70,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center
                        };
                        mainStackPanel.Children.Add(textBlock2);

                        playAgainNewWordTextBox = new TextBox
                        {
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Width = 100
                        };
                        mainStackPanel.Children.Add(playAgainNewWordTextBox);

                        Button playAgainButton = new Button
                        {
                            Content = "Play Again",
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Margin = new Thickness(0, 20, 0, 0),
                            Width = 100
                        };
                        playAgainButton.Click += playAgain_Click;
                        mainStackPanel.Children.Add(playAgainButton);

                        Button exitButton = new Button
                        {
                            Content = "Exit",
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Margin = new Thickness(0, 20, 0, 0),
                            Width = 100
                        };
                        exitButton.Click += exit_Click;
                        mainStackPanel.Children.Add(exitButton);
                    }
                    btn.Background = new SolidColorBrush(Colors.GreenYellow);
                }
                else
                {
                    lifes--;
                    switch (lifes)
                    {
                        case 5:
                            img.Source = new BitmapImage(new Uri(base.BaseUri, "/Assets/Galge2.png"));
                            break;
                        case 4:
                            img.Source = new BitmapImage(new Uri(base.BaseUri, "/Assets/Galge3.png"));
                            break;
                        case 3:
                            img.Source = new BitmapImage(new Uri(base.BaseUri, "/Assets/Galge4.png"));
                            break;
                        case 2:
                            img.Source = new BitmapImage(new Uri(base.BaseUri, "/Assets/Galge5.png"));
                            break;
                        case 1:
                            img.Source = new BitmapImage(new Uri(base.BaseUri, "/Assets/Galge6.png"));
                            break;
                        case 0:
                        {
                            mainStackPanel.Children.Clear();

                            TextBlock textBlock = new TextBlock
                            {
                                Text = "GAME OVER!",
                                FontSize = 70,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center
                            };
                            mainStackPanel.Children.Add(textBlock);

                            TextBlock textBlock2 = new TextBlock
                            {
                                Text = "The word was: " + selectedWord,
                                FontSize = 70,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center
                            };
                            mainStackPanel.Children.Add(textBlock2);

                            img = new Image
                            {
                                Source = new BitmapImage(new Uri(base.BaseUri, "/Assets/Galge7.png")),
                                Height = 300,
                                Width = 300
                            };
                            mainStackPanel.Children.Add(img);

                            playAgainNewWordTextBox = new TextBox
                            {
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center,
                                Margin = new Thickness(0, 20, 0, 0),
                                Width = 100
                            };
                            mainStackPanel.Children.Add(playAgainNewWordTextBox);

                                Button playAgainButton = new Button
                            {
                                Content = "Play Again",
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center,
                                Margin = new Thickness(0, 20, 0, 0),
                                Width = 100
                                };
                            playAgainButton.Click += playAgain_Click;
                            mainStackPanel.Children.Add(playAgainButton);

                            Button exitButton = new Button
                            {
                                Content = "Exit",
                                HorizontalAlignment = HorizontalAlignment.Center,
                                VerticalAlignment = VerticalAlignment.Center,
                                Margin = new Thickness(0, 20, 0, 0),
                                Width = 100
                            };
                            exitButton.Click += exit_Click;
                            mainStackPanel.Children.Add(exitButton);
                            break;
                        }
                    }
                    btn.Background = new SolidColorBrush(Colors.Red);
                }
            }
        }

        private void playAgain_Click(object sender, RoutedEventArgs e)
        {
            mainStackPanel.Children.Clear();
            if (playAgainNewWordTextBox.Text.Length != 0) selectedWord = playAgainNewWordTextBox.Text;
            else selectedWord = "hejsameddejsa";
            Setup();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            CoreApplication.Exit();
        }

        private void AddWord_Click(object sender, RoutedEventArgs e)
        {
            mainStackPanel.Children.Clear();
            selectedWord = newWordTextBox.Text;
            Setup();
        }
    }
}
