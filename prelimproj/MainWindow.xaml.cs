

using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Media;

using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Formats.Asn1.AsnWriter;
//using static System.Net.Mime.MediaTypeNames;

namespace prelimproj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //IAMTRYINGTOGITHUB
        //please work

        int currentscore = 0;
        int gamestart = 0;
        string difficulty = "Easy"; //Easy, Medium, Hard, Nightmare
        int round = 0;
        string currentcontrols = "Buttons"; //Buttons and then Textbox.

        private DispatcherTimer timer;
        private int starttime = 60;
        private int roundTime = 0;
        private int currentRound = 1;
        //private int totalplaytime = 0; //placeholder because maybe i shouldnt use int and use something more specific.
        private TimeSpan elapsedTime;

        private SoundPlayer player;
        private SoundPlayer player2;

        private MediaPlayer mediaPlayer1;
        private MediaPlayer mediaPlayer2;

        private string leaderboardFilePath = "C://Users//derrick//Downloads//leaderboard.txt"; // @"C:\MyApp\leaderboard.txt";
        private string savedPreferencesFilePath = "C://Users//derrick//Downloads//savedpreferences.txt";
        //private string leaderboardFilePath = "C://Users//22-0354c//Downloads//leaderboard.txt";
        //private string savedPreferencesFilePath = "C://Users//22-0354c//Downloads//savedpreferences.txt";
        List<List<string>> Easyleaderboard = new List<List<string>>(); //leaderboardData.Add(new List<string> { "Name", "Time", "Score", "EntryID"  });
        List<List<string>> Mediumleaderboard = new List<List<string>>(); //leaderboardData.Add(new List<string> { "Name", "Time", "Score", "EntryID"});
        List<List<string>> Hardleaderboard = new List<List<string>>(); //leaderboardData.Add(new List<string> { "Name", "Time", "Score", "EntryID" });
        List<List<string>> Nightmareleaderboard = new List<List<string>>(); //leaderboardData.Add(new List<string> { "Name", "Time", "Score", "EntryID"});

        //TODO: RESET VARIABLES AND ADD ENDSCREEN VARIABLE. not sure which is done lol

        /// <summary>
        /// this is
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            //Result_Copy.Content = GenerateRandom8BitDecimal();

            screenswitch(0);

            roundTime = starttime;
            InitializeTimer();

            initializesettings();
            EnsureFilesExist(leaderboardFilePath);
            EnsureFilesExist(savedPreferencesFilePath);
            ReadLeaderboard(leaderboardFilePath);

            player = new SoundPlayer();
            player2 = new SoundPlayer();
            InitializeMediaPlayers();

            this.Closing += MainWindow_Closing;
        }


        private void InitializeMediaPlayers()
        {
            // Initialize first media player
            mediaPlayer1 = new MediaPlayer();
            mediaPlayer1.Open(new Uri(@"C:\Users\derrick\source\repos\prelimproj\prelimproj\Sounds\click-21156.mp3"));
            mediaPlayer1.MediaEnded += (sender, e) => ResetMediaPlayer(mediaPlayer1); //this part occurs when the media is finished playing, resetting it so it can be played again

            // Initialize second media player
            mediaPlayer2 = new MediaPlayer();
            //mediaPlayer2.Open(new Uri(@"C:\Users\derrick\Downloads\AUDIO-2024-03-19-14-48-10.wav"));
           // mediaPlayer2.MediaEnded += (sender, e) => ResetMediaPlayer(mediaPlayer2);
        }

        private void PlayBothSounds()
        {
            mediaPlayer1.Play();
            mediaPlayer2.Play();
        }

        private void ResetMediaPlayer(MediaPlayer player)
        {
            player.Stop();
            player.Position = TimeSpan.Zero; // Reset position to beginning
        }

        public void PlaySound(SoundPlayer player, string soundFilePath)
        {
            try
            {
                // Set the sound file path
                player.SoundLocation = soundFilePath;

                // Play the sound
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error playing sound: " + ex.Message);
                //Console.WriteLine("Error playing sound: " + ex.Message);
            }
        }

        private void initializesettings()
        {
            //integrate txt.file reading logic here

            GameControlSelect.Content = currentcontrols;
            swapcontrols();

            DifficultySelect.Content = difficulty;
            swapdifficulty();
        }

        private void swapcontrols()
        {
            if (currentcontrols == "Buttons")
            {
                GameInterfaceOption2.Visibility = Visibility.Hidden;
                GameInterfaceOption1.Visibility = Visibility.Visible;
            }
            if (currentcontrols == "Textbox")
            {
                GameInterfaceOption1.Visibility = Visibility.Hidden;
                GameInterfaceOption2.Visibility = Visibility.Visible;
            }
            GameControlSelect.Content = currentcontrols;
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            roundTime--; // Decrement the round time
            Timer.Content = roundTime.ToString(); // Update the timer label

            elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(1)); //getting totaltime

            // Check if time is up
            if (roundTime <= 0)
            {
                // End the game
                EndGame();
            }
        }

        private void EndGame()
        {
            //dunno what to put here lol

            // Stop the timer
            timer.Stop();

            gamestart = 0; //Resets this



            // Show necessary information on the end screen
            EndScreenTimePlayedLabel.Content = $"Time Played: {elapsedTime.ToString("mm':'ss")}"; // Display the time played
            EndScreenScoreLabel.Content = $"Score: {currentscore}"; // Display the score
            EndScreenDifficultyLabel.Content = $"Difficulty: {difficulty}"; // Display the difficulty
            EndScreenRoundsLabel.Content = $"Rounds: {currentRound}"; // Display the current round

            screenswitch(1); // Show the end screen and menu


            EndScreenLeaderboard.Items.Clear();
            if (difficulty == "Easy")
                PopulateLeaderboardGrid(Easyleaderboard, EndScreenLeaderboard);
            if (difficulty == "Medium")
                PopulateLeaderboardGrid(Mediumleaderboard, EndScreenLeaderboard);
            if (difficulty == "Hard")
                PopulateLeaderboardGrid(Hardleaderboard, EndScreenLeaderboard);
            if (difficulty == "Nightmare")
                PopulateLeaderboardGrid(Nightmareleaderboard, EndScreenLeaderboard);

            currentscore = 0; //these gotta be reset somewhere
            currentRound = 0;

            //writeleaderboard(leaderboardFilePath);
        }

        // Start a new round
        private void StartNewRound()
        {
            Round.Content = currentRound.ToString();

            // Reset round time and update the timer label
            roundTime = CalculateRoundTime(currentRound, starttime);
            //MessageBox.Show(roundTime.ToString() + " " + currentRound.ToString());
            Timer.Content = roundTime.ToString();

            // Generate a new random 8-bit number
            int randomNum = GenerateRandom8BitDecimal();
            GameNum.Content = randomNum.ToString();

            // Start the timer
            timer.Start();
        }

        // End the current round
        private void EndRound()
        {
            // Calculate the points earned based on the time left on the timer
            //int pointsEarned = (int)timer.Content; // Assuming Timer.Content holds the remaining time in seconds
            int pointsEarned = roundTime;
            currentscore += pointsEarned;
            Score.Content = currentscore.ToString();

            // Stop the timer
            timer.Stop();

            // Increment the current round
            currentRound++;

            // Start a new round
            StartNewRound();
        }

        // Calculate round time based on the round number
        // Method to calculate round time based on the round number
        private int CalculateRoundTime(int roundNumber, int startTime)
        {
            // Calculate reduction factor for rounds after 10
            double reductionFactor = (roundNumber > 10) ? 0.66 : 0;

            // Calculate the reduction amount for rounds after 10
            int reductionAmount = (int)Math.Floor(startTime * reductionFactor);

            // Calculate the round time
            int roundTime = startTime - (roundNumber - 1) * (roundNumber > 10 ? reductionAmount : 2);

            return Math.Max(roundTime, 1); // Ensure the round time is at least 1 second
        }

        private void BinaryButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (button != null)
            {
                if (button.Content.ToString() == "0")
                {
                    button.Content = "1";
                }
                else if (button.Content.ToString() == "1")
                {
                    button.Content = "0";
                }
            }

            Result.Content = GetPlayerDecimal();
            if (gamestart == 1)
            {
                if (CheckPlayerInput(GetPlayerDecimal()) == true)
                {
                    EndRound();
                }
            }
        }

        private bool CheckPlayerInput(int playerDecimal)
        {
            // Get the player's decimal input
            //int playerDecimal = GetPlayerDecimal();

            // Get the generated random 8-bit number
            int generatedNumber = int.Parse(GameNum.Content.ToString());

            // Compare the player's input with the generated number
            return playerDecimal == generatedNumber;
        }


        private string GetBinaryNumber()
        {
            string binaryNumber = "";
            binaryNumber += Button1.Content;
            binaryNumber += Button2.Content;
            binaryNumber += Button4.Content;
            binaryNumber += Button8.Content;
            binaryNumber += Button16.Content;
            binaryNumber += Button32.Content;
            binaryNumber += Button64.Content;
            binaryNumber += Button128.Content;

            return binaryNumber;
        }

        private int GetPlayerDecimal()
        {
            int thedecimal = 0;

            // Convert the content of the buttons to integers and compute the decimal number
            thedecimal += (Button1.Content.ToString() == "1") ? 1 : 0;
            thedecimal += (Button2.Content.ToString() == "1") ? 2 : 0;
            thedecimal += (Button4.Content.ToString() == "1") ? 4 : 0;
            thedecimal += (Button8.Content.ToString() == "1") ? 8 : 0;
            thedecimal += (Button16.Content.ToString() == "1") ? 16 : 0;
            thedecimal += (Button32.Content.ToString() == "1") ? 32 : 0;
            thedecimal += (Button64.Content.ToString() == "1") ? 64 : 0;
            thedecimal += (Button128.Content.ToString() == "1") ? 128 : 0;

            return thedecimal;
        }

        public int GenerateRandom8BitDecimal()
        {
            // Initialize a new instance of the Random class
            Random rand = new Random();

            // Generate a random number between 0 and 255 (inclusive)
            int randomNumber = rand.Next(256);

            // Return the generated random number
            return randomNumber;
        }


        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            screenswitch(2);

            gamestart = 1;
            // Start the first round
            StartNewRound();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            screenswitch(3);


        }

        private void BackToMenuButtonClick(object sender, RoutedEventArgs e)
        {
            HideAllGrids();
            GameInterface.Visibility = Visibility.Visible;
            Menu.Visibility = Visibility.Visible;
        }

        private void HideAllGrids()
        {
            foreach (var child in TheThing.Children)
            {
                if (child is Grid grid)
                {
                    grid.Visibility = Visibility.Hidden;
                }
            }
            EndScreen.Visibility = Visibility.Hidden;
        }

        private void textchange(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox != null)
            {
                // Get the text from the textbox
                string text = textBox.Text;

                // Validate the input: Only allow "0" or "1"
                foreach (char c in text)
                {
                    if (c != '0' && c != '1')
                    {
                        // If the input is invalid, remove the last character entered
                        textBox.Text = textBox.Text.Remove(textBox.Text.Length - 1);
                        textBox.SelectionStart = textBox.Text.Length; // Set cursor position to the end
                        return;
                    }
                }

                //this part is copypasted form the above part, hope it works
                Result.Content = BinaryToDecimal(textBox.Text);
                if (gamestart == 1)
                {
                    if (CheckPlayerInput(BinaryToDecimal(textBox.Text)) == true)
                    {
                        EndRound();
                    }
                }
            }
        }

        private int BinaryToDecimal(string binaryInput)
        {
            // Remove leading zeros from the binary input
            binaryInput = binaryInput.TrimStart('0');

            int decimalResult = 0;
            int power = 0;

            // Iterate through the binary string from right to left
            for (int i = binaryInput.Length - 1; i >= 0; i--)
            {
                // If the current character is '1', add the corresponding value to the decimal result
                if (binaryInput[i] == '1')
                {
                    decimalResult += (int)Math.Pow(2, power);
                }
                // Increment the power value for the next position
                power++;
            }

            return decimalResult;
        }

        

        private void GameControlSelectButtonclick(object sender, RoutedEventArgs e)
        {
            if (currentcontrols == "Buttons")
            {
                currentcontrols = "Textbox";
            }
            else if (currentcontrols == "Textbox")
            {
                currentcontrols = "Buttons";
            }
            swapcontrols();
        }

        private string[] ReadEncryptedText(string filepath, string encryptionkey)
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "0123456789";
            string[] decryptedText = null;
            try
            {
                // Create a StreamReader to read the contents of the file
                using (StreamReader sr = new StreamReader(filepath))
                {
                    // Read the entire contents of the file
                    string encryptedText = sr.ReadToEnd();

                    // Decrypt the encrypted text using the provided encryption key
                    decryptedText = new string[encryptedText.Length];
                    for (int i = 0; i < encryptedText.Length; i++)
                    {
                        char character = encryptedText[i];
                        int index;

                        // Decrypt letters only
                        if (char.IsLetter(character))
                        {
                            index = (alphabet.IndexOf(character) - int.Parse(encryptionkey) + alphabet.Length) % alphabet.Length;
                            decryptedText[i] = alphabet[index].ToString();
                        }
                        // Keep numbers as they are
                        else if (char.IsDigit(character))
                        {
                            index = (numbers.IndexOf(character) - int.Parse(encryptionkey) + numbers.Length) % numbers.Length;
                            decryptedText[i] = numbers[index].ToString();
                        }
                        // Keep other characters as they are
                        else
                        {
                            decryptedText[i] = character.ToString();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Handle exceptions, e.g., file not found, file is empty, etc.
                MessageBox.Show("An error occurred: " + e.Message);
            }
            return decryptedText;
        }
        private void WriteEncryptedText(string[] text, string filepath, string encryptionkey)
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "0123456789";

            try
            {
                // Create a StreamWriter to write the encrypted text into the file
                using (StreamWriter sw = new StreamWriter(filepath))
                {
                    foreach (string line in text)
                    {
                        foreach (char character in line)
                        {
                            int index;

                            // Encrypt letters only
                            if (char.IsLetter(character))
                            {
                                index = (alphabet.IndexOf(character) + int.Parse(encryptionkey)) % alphabet.Length;
                                sw.Write(alphabet[index]);
                            }
                            // Keep numbers as they are
                            else if (char.IsDigit(character))
                            {
                                index = (numbers.IndexOf(character) + int.Parse(encryptionkey)) % numbers.Length;
                                sw.Write(numbers[index]);
                            }
                            // Keep other characters as they are
                            else
                            {
                                sw.Write(character);
                            }
                        }
                        sw.WriteLine(); // Write a new line after each line of text
                    }
                }
            }
            catch (Exception e)
            {
                // Handle exceptions, e.g., file not found, permission denied, etc.
                MessageBox.Show("An error occurred: " + e.Message);
            }
        }


        public void EnsureFilesExist(string filepath)
        {
            if (!File.Exists(filepath))
            {
                try
                {
                    using (StreamWriter sw = File.CreateText(filepath))
                    {
                        //hmm
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating file: {ex.Message}");
                }
            }
        }

        private void DifficultySelectButtonclick(object sender, RoutedEventArgs e)
        {
            if (difficulty == "Easy")
            {
                difficulty = "Medium";
            }
            else if (difficulty == "Medium")
            {
                difficulty = "Hard";
            }
            else if (difficulty == "Hard")
            {
                difficulty = "Nightmare";
            }
            else if (difficulty == "Nightmare")
            {
                difficulty = "Easy"; // Wrap back to Easy if it's Nightmare
            }
            DifficultySelect.Content = difficulty;
            swapdifficulty();

            //this is here because i was testing the soundstuff
            //PlaySound(player2, @"C:\Users\derrick\Downloads\Car_RRRRR.wav");
            //PlaySound(player, @"C:\Users\derrick\Downloads\AUDIO-2024-03-19-14-48-10.wav");
            //PlayBothSounds();
        }
        

        private void swapdifficulty()
        {
            if (difficulty == "Easy")
            {
                starttime = 60;
            }
            if (difficulty == "Medium")
            {
                starttime = 45;
            }
            if (difficulty == "Hard")
            {
                starttime = 30;
            }
            if (difficulty == "Nightmare")
            {
                starttime = 10;
            }
        }


        private void EnterLeaderBoardButtonClick(object sender, RoutedEventArgs e)
        {
            // Create a new entry for the leaderboard
            List<string> newEntry = new List<string>();
            newEntry.Add(EndScreenTextBox.Text); // Add name
            newEntry.Add(EndScreenTimePlayedLabel.Content.ToString().Substring(13)); // Add time played
            newEntry.Add(EndScreenScoreLabel.Content.ToString().Substring(7)); // Add score
            newEntry.Add(GenerateEntryID()); // Add entry ID

            // Add the new entry to the respective leaderboard list based on difficulty
            switch (difficulty)
            {
                case "Easy":
                    Easyleaderboard.Add(newEntry);
                    Easyleaderboard.Sort((x, y) => int.Parse(y[2]).CompareTo(int.Parse(x[2]))); // Sort by score
                    break;
                case "Medium":
                    Mediumleaderboard.Add(newEntry);
                    Mediumleaderboard.Sort((x, y) => int.Parse(y[2]).CompareTo(int.Parse(x[2]))); // Sort by score
                    break;
                case "Hard":
                    Hardleaderboard.Add(newEntry);
                    Hardleaderboard.Sort((x, y) => int.Parse(y[2]).CompareTo(int.Parse(x[2]))); // Sort by score
                    break;
                case "Nightmare":
                    Nightmareleaderboard.Add(newEntry);
                    Nightmareleaderboard.Sort((x, y) => int.Parse(y[2]).CompareTo(int.Parse(x[2]))); // Sort by score
                    break;
                default:
                    break;
            }

            // Update the leaderboard grids
            LeaderboardButton_Click(null, null);
            LeaderBoard.Visibility = Visibility.Hidden;

            // Clear the textbox after adding the entry
            EndScreenTextBox.Text = "";

            EndScreen.Visibility = Visibility.Hidden;
            GameInterface.Visibility = Visibility.Visible;
            TheGameParts.Visibility = Visibility.Visible; //Not sure about "TheGameParts" in general
            Menu.Visibility = Visibility.Visible;
            //MessageBox.Show(Nightmareleaderboard.Count.ToString());
            //PopulateLeaderboard();
        }


        // Method to generate a unique entry ID 
        private string GenerateEntryID()
        {
            // just usin timestamp rn mang
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }


        // Method to populate a ListBox with leaderboard data
        private void PopulateLeaderboardGrid(List<List<string>> leaderboard, ListBox listBox)
        {
            // Add each entry in the leaderboard list to the ListBox
            foreach (var entry in leaderboard)
            {
                listBox.Items.Add($"{entry[0]} | Score: {entry[2]} | Time: {entry[1]}");
            }
        }

        private void LeaderboardButton_Click(object sender, RoutedEventArgs e)
        {
            // Make the LeaderBoard grid visible and hide other grids
            LeaderBoard.Visibility = Visibility.Visible;
            GameInterface.Visibility = Visibility.Hidden;
            Settings.Visibility = Visibility.Hidden;
            Menu.Visibility = Visibility.Hidden;

            // Clear existing items in the leaderboard grids
            FirstLeaderboard.Items.Clear();

            if (difficulty == "Easy")
                PopulateLeaderboardGrid(Easyleaderboard, FirstLeaderboard);
            else if (difficulty == "Medium")
                PopulateLeaderboardGrid(Mediumleaderboard, FirstLeaderboard);
            else if (difficulty == "Hard")
                PopulateLeaderboardGrid(Hardleaderboard, FirstLeaderboard);
            else if (difficulty == "Nightmare")
                PopulateLeaderboardGrid(Nightmareleaderboard, FirstLeaderboard);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);

            // Check if the ESC key is pressed
            if (e.Key == Key.Escape)
            {
                // Call a method to handle the ESC key press
                HandleEscKeyPress();
            }
        }

        private void HandleEscKeyPress()
        {
            if (gamestart == 1)
            {
                timer.Stop();
                MessageBox.Show("Paused");
                timer.Start();
            }
            if (gamestart == 0)
            {
                Application.Current.Shutdown();
            }
        }

        private void writeleaderboard(string filepath)
        {
            using (StreamWriter sw = new StreamWriter(filepath))
            {
                sw.WriteLine("EASY:");
                for (int i = 0; i < Easyleaderboard.Count; i++)
                {
                    //for(int ii = 0; ii < Easyleaderboard[ii].Count; ii++)
                    //{
                    //    sw.Write(Easyleaderboard[ii][0].ToString() + ",");
                    //}
                    string aga = (Easyleaderboard[i][0]).ToString() + "," + (Easyleaderboard[i][1]).ToString() + "," + (Easyleaderboard[i][2]).ToString() + "," + (Easyleaderboard[i][3]).ToString();
                    sw.WriteLine(aga);
                }
                sw.WriteLine("MEDIUM:");
                for (int i = 0; i < Mediumleaderboard.Count; i++)
                {
                    string aga = (Mediumleaderboard[i][0]).ToString() + "," + (Mediumleaderboard[i][1]).ToString() + "," + (Mediumleaderboard[i][2]).ToString() + "," + (Mediumleaderboard[i][3]).ToString();
                    sw.WriteLine(aga);
                }
                sw.WriteLine("HARD:");
                for (int i = 0; i < Hardleaderboard.Count; i++)
                {
                    string aga = (Hardleaderboard[i][0]).ToString() + "," + (Hardleaderboard[i][1]).ToString() + "," + (Hardleaderboard[i][2]).ToString() + "," + (Hardleaderboard[i][3]).ToString();
                    sw.WriteLine(aga);
                }
                sw.WriteLine("NIGHTMARE:");
                for (int i = 0; i < Nightmareleaderboard.Count; i++)
                {
                    string aga = (Nightmareleaderboard[i][0]).ToString() + "," + (Nightmareleaderboard[i][1]).ToString() + "," + (Nightmareleaderboard[i][2]).ToString() + "," + (Nightmareleaderboard[i][3]).ToString();
                    sw.WriteLine(aga);
                }
            }
        }

        private void ReadLeaderboard(string filepath)
        {
            using (var sr = new StreamReader(filepath))
            {
                string line;
                string currentDifficulty = string.Empty;

                while ((line = sr.ReadLine()) != null)
                {
                    // Remove any leading/trailing whitespace
                    line = line.Trim();

                    // Check if the line indicates a difficulty level
                    if (line.EndsWith(":"))
                    {
                        currentDifficulty = line.TrimEnd(':');
                    }
                    else if (!string.IsNullOrEmpty(currentDifficulty))
                    {
                        // Split the line into fields (assuming comma-separated values)
                        string[] fields = line.Split(',');

                        // Ensure we have at least 4 fields (Name, Time, Score, EntryID)
                        if (fields.Length >= 4)
                        {
                            List<string> leaderboardEntry = new List<string>
                        {
                            fields[0], // Name
                            fields[1], // Time
                            fields[2], // Score
                            fields[3]  // EntryID
                        };

                            // Add the entry to the appropriate leaderboard
                            switch (currentDifficulty.ToUpper())
                            {
                                case "EASY":
                                    Easyleaderboard.Add(leaderboardEntry);
                                    Easyleaderboard.Sort((x, y) => int.Parse(y[2]).CompareTo(int.Parse(x[2]))); // Sort by score
                                    break;
                                case "MEDIUM":
                                    Mediumleaderboard.Add(leaderboardEntry);
                                    Mediumleaderboard.Sort((x, y) => int.Parse(y[2]).CompareTo(int.Parse(x[2]))); // Sort by score
                                    break;
                                case "HARD":
                                    Hardleaderboard.Add(leaderboardEntry);
                                    Hardleaderboard.Sort((x, y) => int.Parse(y[2]).CompareTo(int.Parse(x[2]))); // Sort by score
                                    break;
                                case "NIGHTMARE":
                                    Nightmareleaderboard.Add(leaderboardEntry);
                                    Nightmareleaderboard.Sort((x, y) => int.Parse(y[2]).CompareTo(int.Parse(x[2]))); // Sort by score
                                    break;
                                default:
                                    // Unknown difficulty level
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void screenswitch(int ehhhh)
        {
            if (ehhhh == 0) //at the start
            {
                HideAllGrids();
                GameInterface.Visibility = Visibility.Visible;
                Menu.Visibility = Visibility.Visible;
                //TheGameParts.Visibility = Visibility.Visible;
            }
            if (ehhhh == 1) //at endgame
            {
                HideAllGrids();
                TheGameParts.Visibility = Visibility.Hidden;
                GameInterface.Visibility = Visibility.Visible;
                EndScreen.Visibility = Visibility.Visible;
                Menu.Visibility = Visibility.Visible;
            }
            if (ehhhh == 2) //at playbuttonclick
            {
                HideAllGrids();
                // Make the GameInterface grid visible and hide other grids
                GameInterface.Visibility = Visibility.Visible;
                Settings.Visibility = Visibility.Hidden;
                LeaderBoard.Visibility = Visibility.Hidden;
                Menu.Visibility = Visibility.Hidden;

                TheGameParts.Visibility = Visibility.Visible;
            }
            if (ehhhh == 3) //at settingsbuttonclick
            {
                HideAllGrids();
                // Make the Settings grid visible and hide other grids
                Settings.Visibility = Visibility.Visible;
                GameInterface.Visibility = Visibility.Hidden;
                LeaderBoard.Visibility = Visibility.Hidden;
                Menu.Visibility = Visibility.Hidden;
            }

        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            writeleaderboard(leaderboardFilePath);
        }

    }
}