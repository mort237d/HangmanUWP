using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel.Channels;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace HangmanUWP
{
    public sealed partial class MainPage : Page
    {
        private string selectedWord = "ABC";

        public MainPage()
        {
            this.InitializeComponent();
            for (int i = 0; i < selectedWord.Length; i++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Height = 30;
                textBlock.Width = 30;
                textBlock.Text = selectedWord[i].ToString();
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
                }
                else
                {
                    Debug.WriteLine("Char is NOT in the word");
                }
                btn.Background = new SolidColorBrush(Colors.Red);
            }
        }
    }
}
