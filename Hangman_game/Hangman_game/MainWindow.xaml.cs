using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Hangman_game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // simulate a key press to start the game
            var result = MessageBox.Show("Welcome! Press OK to start the game, and then press any key between A-Z.", "Hangman", MessageBoxButton.OKCancel);

            if (result != MessageBoxResult.OK)
            {
                Close();
            }

        }


        private string word;
        private string Word
        {
            get
            {
                // if is first time and word-variable is null, takes a word with random from the list.
                if (word == null)
                {
                    string[] words = { "BUTTERFLY", "LEMON", "TORTOISE", "ELEPHANT", "COMPUTER", "GARDEN", "WINDOW", "DICTIONARY", "CHRISTMAS", "SEAHORSE", "TEACHER" };
                    // TESTAUSTA VARTEN string[] words = {"ELEPHANT"};
                    Random rnd = new Random();
                    int wordnum = rnd.Next(0, words.Length);
                    word = words[wordnum];
                }

                return word;
            }
            set { }
        }

        // Private variables and initialize them.
        private string gameword = "";
        private bool isFirsTime = true;
        private int wrongCount = 0;
        private string wrongLetter = "";
        private int lives = 6;


        private void window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e?.Key != null)
            {


                // variable ch takes value of the key
                char ch = ' ';

                // Check if the key pressed is a letter key
                if (e.Key >= Key.A && e.Key <= Key.Z)
                {
                    // Get the letter from the key and convert it to uppercase
                    ch = Convert.ToChar(e.Key.ToString().ToUpper());

                    // Check if the letter is a Finnish alphabet letter
                    switch (ch)
                    {
                        case 'A':
                        case 'O':
                        case 'U':
                        case 'Y':
                        case 'I':
                        case 'E':
                        case 'H':
                        case 'J':
                        case 'K':
                        case 'L':
                        case 'M':
                        case 'N':
                        case 'P':
                        case 'R':
                        case 'S':
                        case 'T':
                        case 'V':
                        case 'Z':
                        case 'B':
                        case 'C':
                        case 'D':
                        case 'F':
                        case 'G':
                        case 'Q':
                        case 'W':
                        case 'X':
                            // Do something with the valid letter input
                            break;
                        default:
                            // Ignore invalid letter input
                            return;
                    }
                }

                else
                {
                    // Ignore non-letter key input
                    return;
                }

                //if is first time that press the key, do this.
                if (isFirsTime == true)
                {
                    // gameword -variable have star-char same amount than right word have letters.
                    for (int i = 0; i < Word.Length; i++)
                    {
                        char star = '*';
                        gameword += star;
                    }
                    // label.content is gameword
                    label.Content = gameword;
                    // no first time anymore
                    isFirsTime = false;

                }
                // if not first time
                else
                {
                    // word2 variable is same than right word
                    string word2 = Word;

                    // result tells true if ch -char is in word, else, result is false
                    Boolean result = word2.Contains(ch);


                    if (result == true)
                    {
                        foreach (char h in word2)
                        {
                            // tells the index of the ch of word
                            int index = word2.IndexOf(ch.ToString());

                            //while true, if index is more than -1
                            while (index > -1)
                            {
                                // gameword remove to * and insert the right char
                                gameword = gameword.Remove(index, 1).Insert(index, ch.ToString());

                                //index change because every duplicates need noticed
                                index = word2.IndexOf(ch.ToString(), index + 1);
                            }
                            // label content is gameword
                            label.Content = gameword;
                        }
                    }
                    //if result not true
                    else
                    {
                        //wrongletter add ch in the list on the gametable
                        wrongLetter += (ch + " ,");
                        label2.Content = wrongLetter;

                        //wrongcount tells building steps for the hangman
                        wrongCount++;

                        //Lives falling down for every wrong answers and lives are in the gametable
                        lives--;
                        label5.Content = lives;

                        //create color
                        SolidColorBrush blackcolor = new SolidColorBrush();
                        blackcolor.Color = Colors.Black;


                        // building the hangman
                        switch (wrongCount)
                        {
                            case 1:
                                {
                                    //create head
                                    Ellipse ellipse = new Ellipse();
                                    ellipse.Height = 40;
                                    ellipse.Width = 40;
                                    ellipse.Fill = Brushes.Beige;
                                    ellipse.Stroke = blackcolor;
                                    ellipse.StrokeThickness = 3;
                                    Canvas.SetLeft(ellipse, 308);
                                    Canvas.SetTop(ellipse, 149);
                                    canvas.Children.Add(ellipse);
                                    break;
                                }

                            case 2:
                                {
                                    // create bodyline
                                    Line body = new Line();
                                    body.Y2 = 70;
                                    body.Stroke = blackcolor;
                                    body.StrokeThickness = 3;
                                    Canvas.SetLeft(body, 327);
                                    Canvas.SetTop(body, 187);
                                    canvas.Children.Add(body);
                                    break;
                                }

                            case 3:
                                {
                                    // create right hand
                                    Line rightHand = new Line();
                                    rightHand.Y2 = 50;
                                    rightHand.X1 = 25;
                                    rightHand.Stroke = blackcolor;
                                    rightHand.StrokeThickness = 3;
                                    Canvas.SetLeft(rightHand, 302);
                                    Canvas.SetTop(rightHand, 187);
                                    canvas.Children.Add(rightHand);
                                    break;
                                }

                            case 4:
                                {
                                    // create left hand
                                    Line leftHand = new Line();
                                    leftHand.Y2 = 50;
                                    leftHand.X2 = 25;
                                    leftHand.Stroke = blackcolor;
                                    leftHand.StrokeThickness = 3;
                                    Canvas.SetLeft(leftHand, 327);
                                    Canvas.SetTop(leftHand, 187);
                                    canvas.Children.Add(leftHand);
                                    break;
                                }

                            case 5:
                                {
                                    // create right leg
                                    Line rightFoot = new Line();
                                    rightFoot.Y2 = 50;
                                    rightFoot.X2 = 25;
                                    rightFoot.Stroke = blackcolor;
                                    rightFoot.StrokeThickness = 3;
                                    Canvas.SetLeft(rightFoot, 327);
                                    Canvas.SetTop(rightFoot, 254);
                                    canvas.Children.Add(rightFoot);
                                    break;
                                }

                            case 6:
                                {
                                    // Create left leg
                                    Line leftFoot = new Line();
                                    leftFoot.Y2 = 50;
                                    leftFoot.X1 = 25;
                                    leftFoot.Stroke = blackcolor;
                                    leftFoot.StrokeThickness = 3;
                                    Canvas.SetLeft(leftFoot, 302);
                                    Canvas.SetTop(leftFoot, 254);
                                    canvas.Children.Add(leftFoot);




                                    // Show messagebox if hangman is ready before you
                                    MessageBox.Show("Sorry, you lost the game! :( \nThe word to guess was: " + word2);
                                    Close();
                                    break;
                                }
                        }
                    }
                }

                // if guess word is ready before hanmgman
                if (label.Content.ToString() == Word)
                {
                    MessageBox.Show("Congratulations, you won the game! :) \nRemaining lives: " + lives);
                    Close();
                }
            }
        }
    }
}
