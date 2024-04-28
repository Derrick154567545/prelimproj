using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace prelimproj
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {

        int currentscore = 0;
        int gamestart = 0;
        string difficulty = "Easy"; //Easy, Medium, Hard, Nightmare
        int round = 0;
        string currentcontrols = "Buttons"; //Buttons and then Textbox.

        private DispatcherTimer timer;
        private int starttime = 60;
        private int roundTime = 0;
        private int currentRound = 1;
        private TimeSpan elapsedTime;

        private MediaPlayer Clickplayer;
        private MediaPlayer Musicplayer;
        private MediaPlayer Throatplayer;

        double musicvolumevariable = 0;
        double soundvolumevariable = 0.5;

        List<List<string>> Easyleaderboard = new List<List<string>>(); //leaderboardData.Add(new List<string> { "Name", "Time", "Score", "EntryID"  });
        List<List<string>> Mediumleaderboard = new List<List<string>>(); //leaderboardData.Add(new List<string> { "Name", "Time", "Score", "EntryID"});
        List<List<string>> Hardleaderboard = new List<List<string>>(); //leaderboardData.Add(new List<string> { "Name", "Time", "Score", "EntryID" });
        List<List<string>> Nightmareleaderboard = new List<List<string>>(); //leaderboardData.Add(new List<string> { "Name", "Time", "Score", "EntryID"});

        int[] noteunlock = { 1, 0, 0, 0, 0, 0 };
        string[] notetext = { "", "", "", "", "", "" };
        private string noteFilePath = "C://Users//derrick//Downloads//gamelore.txt";

        private string leaderboardFilePath = "C://Users//derrick//Downloads//leaderboard.txt"; // @"C:\MyApp\leaderboard.txt";
        private string savedPreferencesFilePath = "C://Users//derrick//Downloads//savedpreferences.txt";
        //private string leaderboardFilePath = "C://Users//22-0354c//Downloads//leaderboard.txt";
        //private string savedPreferencesFilePath = "C://Users//22-0354c//Downloads//savedpreferences.txt";
        //maybe notes filepath

        public Window1()
        {
            InitializeComponent();
            //MessageBox.Show(currentcontrols);
            SwitchScreens("Menuscreen");

            //initialize mediaplayers
            Clickplayer = new MediaPlayer();
            Musicplayer = new MediaPlayer();
            Throatplayer = new MediaPlayer();
            AudioMethod("Loonboon");


            this.Closing += MainWindow_Closing;

            //reading from txt files
            EnsureFilesExist(leaderboardFilePath);
            EnsureFilesExist(savedPreferencesFilePath);
            EnsureFilesExist(noteFilePath);
            ReadLeaderboard(leaderboardFilePath);
            ReadNoteFile(noteFilePath);

            //MessageBox.Show(notetext[1].ToString());

            ReadFromCSVFile(savedPreferencesFilePath);

            //MessageBox.Show(currentcontrols);

            DifficultyButton.Content = difficulty;
            swapcontrols();
            swapdifficulty();
            Clickplayer.Volume = soundvolumevariable;
            Throatplayer.Volume = soundvolumevariable;
            Musicplayer.Volume = musicvolumevariable;
            MusicVolumeSlider.Value = musicvolumevariable;
            SoundVolumeSlider.Value = soundvolumevariable;  

            roundTime = starttime;
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            GameNum.Visibility = Visibility.Visible;

            Random rand = new Random();
            int chance = rand.Next(1, 101); 
            if (roundTime == 10 && chance <= 50) 
            {
                AudioMethod("10SecondsHavePast");
            }
            else if (roundTime == 20 && chance <= 30)
            {
                AudioMethod("20seconds");
            }
            else if (roundTime == 30 && chance <= 20) 
            {
                AudioMethod("Hurry_up");
            }

            roundTime--; 
            Timer.Content = roundTime.ToString(); 

            elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(1)); //getting totaltime

            AudioMethod("Switch");

            if (roundTime < 10) // Check if elapsed time is 10 seconds
            {
                hi.Visibility = Visibility.Visible; 
            }
            if (roundTime > 10) 
            {
                hi.Visibility = Visibility.Hidden; 
            }

            if(elapsedTime.TotalSeconds > 5 && difficulty == "Hard")
            {
                GameNum.Visibility = Visibility.Hidden;
            }


            if (roundTime <= 0)
            {
                EndGame();
            }
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

            ResultDisplay.Content = GetPlayerDecimal();
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

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {

            base.OnPreviewKeyDown(e);
            //MessageBox.Show(e.Key.ToString());

            // Check if the ESC key is pressed
            if (e.Key == Key.Escape)
            {
                // Call a method to handle the ESC key press
                HandleEscKeyPress();
            }
            if (currentcontrols == "Buttons")
            {
                switch (e.Key)
                {
                    case Key.D1:
                        ToggleButtonState(Button128, 0);
                        break;
                    case Key.D2:
                        ToggleButtonState(Button64, 0);
                        break;
                    case Key.D3:
                        ToggleButtonState(Button32, 0);
                        break;
                    case Key.D4:
                        ToggleButtonState(Button16, 0);
                        break;
                    case Key.D5:
                        ToggleButtonState(Button8, 0);
                        break;
                    case Key.D6:
                        ToggleButtonState(Button4, 0);
                        break;
                    case Key.D7:
                        ToggleButtonState(Button2, 0);
                        break;
                    case Key.D8:
                        ToggleButtonState(Button1, 0);
                        break;
                    case Key.D9:
                        ToggleButtonState(Button64, 1);
                        break;
                    case Key.D0:
                        ToggleButtonState(Button64, 2);
                        break;

                    case Key.NumPad1:
                        ToggleButtonState(Button128, 0);
                        break;
                    case Key.NumPad2:
                        ToggleButtonState(Button64, 0);
                        break;
                    case Key.NumPad3:
                        ToggleButtonState(Button32, 0);
                        break;
                    case Key.NumPad4:
                        ToggleButtonState(Button16, 0);
                        break;
                    case Key.NumPad5:
                        ToggleButtonState(Button8, 0);
                        break;
                    case Key.NumPad6:
                        ToggleButtonState(Button4, 0);
                        break;
                    case Key.NumPad7:
                        ToggleButtonState(Button2, 0);
                        break;
                    case Key.NumPad8:
                        ToggleButtonState(Button1, 0);
                        break;
                    case Key.NumPad9:
                        ToggleButtonState(Button64, 1);
                        break;
                    case Key.NumPad0:
                        ToggleButtonState(Button64, 2);
                        break;

                    default:
                        break;
                }
                ResultDisplay.Content = GetPlayerDecimal();
                if (gamestart == 1)
                {
                    if (CheckPlayerInput(GetPlayerDecimal()) == true)
                    {
                        EndRound();
                    }
                }
            }

        }

        private void ToggleButtonState(Button button, int nine)
        {
            if (nine == 0)
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
            if (nine == 1) //switches all buttons
            {
                foreach (var child in TheButtons.Children)
                {
                    if (child is Button button1)
                    {
                        ToggleButtonState(button1, 0);
                    }
                }
            }
            if (nine == 2) //clear
            {
                foreach (var child in TheButtons.Children)
                {
                    if (child is Button button1)
                    {
                        button1.Content = "0";
                    }
                }
            }

            ResultDisplay.Content = GetPlayerDecimal();
            if (gamestart == 1)
            {
                //if (CheckPlayerInput(GetPlayerDecimal()) == true)
                {
                    //EndRound();
                }
            }
        }

        private void HandleEscKeyPress()
        {
            if (gamestart == 1)
            {
                int wasTimerRunning = 0;
                if (timer.IsEnabled)
                    wasTimerRunning = 1;

                if (wasTimerRunning == 1)
                {
                    timer.Stop();
                }

                //MessageBox.Show("Paused");

                PauseScreen.Visibility = Visibility.Visible;

                //if (wasTimerRunning == 1)
                //{
                //    timer.Start();
                //}
            }
            else if (gamestart == 0)
            {
                Application.Current.Shutdown();
            }
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

        private void PlayClickSound_MouseEnter(object sender, MouseEventArgs e)
        {
            // Your code here to handle the mouse hover
            //AudioMethod("Click");
        }

        private void SwitchScreens(string screen)
        {
            // Hide all screens
            Gamescreen.Visibility = Visibility.Hidden;
            Menuscreen.Visibility = Visibility.Hidden;
            OptionsScreen.Visibility = Visibility.Hidden;
            LeaderBoardScreen.Visibility = Visibility.Hidden;
            NoteScreen.Visibility = Visibility.Hidden;
            PauseScreen.Visibility = Visibility.Hidden;
            TutorialScreen.Visibility = Visibility.Hidden;
            BinaryInput.Visibility = Visibility.Hidden;
            GameEndScreen.Visibility = Visibility.Hidden;

            // Show the desired screen 
            //screenToShow.Visibility = Visibility.Visible; //Grid screenToShow,
            //AudioMethod("Click");
            //MouseEnter = "PlayClickSound_MouseEnter"


            switch (screen)
            {
                case "Gamescreen":
                    Gamescreen.Visibility = Visibility.Visible;
                    BinaryInput.Visibility = Visibility.Visible;
                    gamestart = 1;
                    // Start the first round
                    AudioMethod("Loonboon");
                    StartNewRound();
                    break;
                case "Menuscreen":
                    Menuscreen.Visibility = Visibility.Visible;
                    BinaryInput.Visibility = Visibility.Visible;
                    ResultDisplay.Visibility = Visibility.Visible; //huhhm
                    break;
                case "OptionsScreen":
                    OptionsScreen.Visibility = Visibility.Visible;
                    break;
                case "LeaderBoardScreen":
                    LeaderBoardScreen.Visibility = Visibility.Visible;

                    Leaderboardscreenleaderboard.Items.Clear();
                    LeaderboardDifficulty.Content = difficulty;
                    if (difficulty == "Easy")
                        PopulateLeaderboardGrid(Easyleaderboard, Leaderboardscreenleaderboard);
                    else if (difficulty == "Medium")
                        PopulateLeaderboardGrid(Mediumleaderboard, Leaderboardscreenleaderboard);
                    else if (difficulty == "Hard")
                        PopulateLeaderboardGrid(Hardleaderboard, Leaderboardscreenleaderboard);
                    else if (difficulty == "Nightmare")
                        PopulateLeaderboardGrid(Nightmareleaderboard, Leaderboardscreenleaderboard);
                    break;
                case "NoteScreen":
                    NoteScreen.Visibility = Visibility.Visible;
                    PopulateListOfAvailableNotes();

                    break;
                case "PauseScreen":
                    PauseScreen.Visibility = Visibility.Visible;
                    break;
                case "TutorialScreen":
                    TutorialScreen.Visibility = Visibility.Visible;
                    break;
                case "GameEndScreen":
                    GameEndScreen.Visibility = Visibility.Visible;

                    break;
                default:
                    //hmmm
                    break;
            }
        }

        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchScreens("OptionsScreen");
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            // Implement functionality for the Play button here
            SwitchScreens(Gamescreen.Name);
        }

        private void LeaderboardsButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchScreens("LeaderBoardScreen");
        }

        private void NotesButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchScreens("NoteScreen");
        }

        private void BackToMenuButton_Click(object sender, RoutedEventArgs e)
        {
            SwitchScreens("Menuscreen");
        }

        private void AudioMethod(string soundtoplay)
        {
            switch (soundtoplay)
            {
                case "Click":
                    PlaySound(Throatplayer, @"C:\Users\derrick\Downloads\click-21156.mp3", 1);
                    //
                    break;
                case "Loonboon":
                    PlaySound(Musicplayer, @"C:\Users\derrick\Downloads\Loonboon.mp3", 0);
                    break;
                case "Switch":
                    PlaySound(Clickplayer, @"C:\Users\derrick\Downloads\light-switch-81967.mp3", 1);
                    break;
                case "10SecondsHavePast":
                    PlaySound(Throatplayer, @"C:\Users\derrick\Downloads\10SecondsHavePast.mp3", 1);
                    break;
                case "20seconds":
                    PlaySound(Throatplayer, @"C:\Users\derrick\Downloads\20seconds.mp3", 1);
                    break;
                case "Hurry_up":
                    PlaySound(Throatplayer, @"C:\Users\derrick\Downloads\Hurry_up.mp3", 1);
                    break;
                case "Good_job":
                    PlaySound(Throatplayer, @"C:\Users\derrick\Downloads\Good_job.mp3", 1);
                    break;
                case "Uh_oh":
                    PlaySound(Throatplayer, @"C:\Users\derrick\Downloads\Uh_oh.mp3", 1);
                    break;
                case "10_seconds_left":
                    PlaySound(Throatplayer, @"C:\Users\derrick\Downloads\10_seconds_left.mp3", 1);
                    break;
                case "Nice_Starts_here":
                    PlaySound(Throatplayer, @"C:\Users\derrick\Downloads\Nice_Starts_here.mp3", 1);
                    break;
                case "tap-notification":
                    //PlaySound(Throatplayer, @"C:\Users\derrick\Downloads\tap-notification-180637.mp3", 1);
                    PlaySound(Throatplayer, @"C:\Users\derrick\Downloads\click-21156.mp3", 1);
                    break;
                default:
                    MessageBox.Show("Sound not found.");
                    break;
            }
        }


        public void PlaySound(MediaPlayer player, string soundFilePath, int musicorsounds)
        {
            try
            {
                // Set the sound file path
                player.Open(new Uri(soundFilePath));

                // Set the volume
                if (musicorsounds == 1)
                {
                    player.Volume = SoundVolumeSlider.Value; // Set the desired volume level here
                }
                if (musicorsounds == 0)
                {
                    player.Volume = MusicVolumeSlider.Value; // Set the desired volume level here
                }

                // Play the sound
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error playing sound: " + ex.Message);
            }
        }

        private void MusicVolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Update the volume of the media player //Do we need another method for sounds? nah, soundsvolume slider has more work if we add more mediaplayers
            Musicplayer.Volume = MusicVolumeSlider.Value;
        }

        private void SoundVolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Update the volume of the media player //Do we need another method for sounds? nah, soundsvolume slider has more work if we add more mediaplayers
            //Clickplayer.Volume = SoundVolumeSlider.Value;
        }

        private void EndGame()
        {
            //dunno what to put here lol
            Noteunlocker();

            SwitchScreens("GameEndScreen");
            // Stop the timer
            timer.Stop();

            gamestart = 0; //Resets this



            // Show necessary information on the end screen
            EndScreenTimePlayedLabel.Content = $"Time Played: {elapsedTime.ToString("mm':'ss")}"; // Display the time played
            EndScreenScoreLabel.Content = $"Score: {currentscore}"; // Display the score
            EndScreenDifficultyLabel.Content = $"Difficulty: {difficulty}"; // Display the difficulty
            EndScreenRoundsLabel.Content = $"Rounds: {currentRound}"; // Display the current round


            GameEndscreenleaderboard.Items.Clear();
            if (difficulty == "Easy")
                PopulateLeaderboardGridButOnlyTenEntries(Easyleaderboard, GameEndscreenleaderboard);
            if (difficulty == "Medium")
                PopulateLeaderboardGridButOnlyTenEntries(Mediumleaderboard, GameEndscreenleaderboard);
            if (difficulty == "Hard")
                PopulateLeaderboardGridButOnlyTenEntries(Hardleaderboard, GameEndscreenleaderboard);
            if (difficulty == "Nightmare")
                PopulateLeaderboardGridButOnlyTenEntries(Nightmareleaderboard, GameEndscreenleaderboard);

            currentscore = 0; //these gotta be reset somewhere
            currentRound = 1;

            
            //writeleaderboard(leaderboardFilePath);
        }

        private void PopulateLeaderboardGridButOnlyTenEntries(List<List<string>> leaderboardData, ListBox leaderboardListBox)
        {
            // Clear the items of the leaderboard ListBox
            leaderboardListBox.Items.Clear();


            int counter = 0;

            foreach (var entry in leaderboardData)
            {
                counter = counter + 1;
                leaderboardListBox.Items.Add($"{entry[0]} | {entry[2]} | {entry[1]}");
                if (counter == 10)
                    break;
            }
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

            AudioMethod("tap-notification");

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

            AudioMethod("Uh_oh");

            // Start a new round
            StartNewRound();
        }

        private int CalculateRoundTime(int roundNumber, int startTime)
        {
            // if roundnumber is greater than 10, reductionfactor is 0.66, if not then its 0
            double reductionFactor = (roundNumber > 10) ? 0.66 : 0;

            // multiply
            int reductionAmount = (int)Math.Floor(startTime * reductionFactor);

            // Calculate the round time, if its less than 10 then its just reduced by 2, for nightmare difficulty
            int roundTime = startTime - (roundNumber - 1) * (roundNumber > 10 ? reductionAmount : 2);

            return Math.Max(roundTime, 1); // round time is at least 1 second
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
            DifficultyButton.Content = difficulty;
            swapdifficulty();
        }

        private void swapdifficulty()
        {
            if (difficulty == "Easy")
            {
                starttime = 60;
                ResultDisplay.Visibility = Visibility.Visible;
                DifficultyLabel.Content = "Easy mode!, 60 seconds to 20 seconds!";
            }
            if (difficulty == "Medium")
            {
                starttime = 45;
                ResultDisplay.Visibility = Visibility.Visible;
                DifficultyLabel.Content = "Medium!, 45 seconds to 15 seconds AND you cant see the decimal of your \n binary";
            }
            if (difficulty == "Hard")
            {
                starttime = 30;
                ResultDisplay.Visibility = Visibility.Hidden;
                DifficultyLabel.Content = "Hard!, 30 seconds to 10 seconds! AND you cant see the decimal of your \n binary AND the number disappears after 5 seconds";
            }
            if (difficulty == "Nightmare")
            {
                starttime = 10;
                ResultDisplay.Visibility = Visibility.Hidden;
                DifficultyLabel.Content = "10 seconds";
            }
        }

        private string GenerateEntryID()
        {
            // just usin timestamp rn mang
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        private void EnterLeaderBoardButtonClick(object sender, RoutedEventArgs e)
        {
            List<string> newEntry = new List<string>();

            //this is so that players cant be smart and mess with the filereading method
            string thename = NameInput.Text;
            if(thename == "EASY:" || (thename == "MEDIUM:"))
            {
                thename = "EASYorMEDIUM";
            }
            if (thename == "HARD:" || (thename == "NIGHTMARE:"))
            {
                thename = "HARDorNIGHTMARE";
            }


            newEntry.Add(thename); // Add name
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
            //EnterLeaderBoardButtonClick(null, null);

            // Clear the textbox after adding the entry
            NameInput.Text = "";

            SwitchScreens("Menuscreen");
            //MessageBox.Show(Nightmareleaderboard.Count.ToString());
            //PopulateLeaderboard();
        }

        private void PopulateLeaderboardGrid(List<List<string>> leaderboard, ListBox listBox)
        {
            // Add each entry in the leaderboard list to the ListBox
            foreach (var entry in leaderboard)
            {
                listBox.Items.Add($"{entry[0]} | {entry[2]} | {entry[1]}");
            }
        }

        private void PopulateLeaderboardGridPointsPerSeconds(List<List<string>> leaderboard, ListBox listBox)
        {
            // Create a new list to store the modified entries
            List<(string, double)> modifiedEntries = new List<(string, double)>();

            foreach (var entry in leaderboard)
            {
                int entry2 = int.Parse(entry[2]);

                // Parse entry1 in the format "minute:seconds"
                string[] timeComponents = entry[1].Split(':');
                int minutes = int.Parse(timeComponents[0]);
                int seconds = int.Parse(timeComponents[1]);
                double entry1 = minutes * 60 + seconds;

                double result = entry2 / entry1;

                modifiedEntries.Add((entry[0], result));
            }

            // Sort the modified entries by the result (average per second) in descending order
            modifiedEntries = modifiedEntries.OrderByDescending(e => e.Item2).ToList();

            foreach (var entry in modifiedEntries)
            {
                listBox.Items.Add($"{entry.Item1} | {entry.Item2}");
            }
        }

        private void LeaderboardDifficulty_Click(object sender, RoutedEventArgs e)
        {
            Leaderboardscreenleaderboard.Items.Clear();

            if (PointDisplay.Content.ToString() == "Score")
            {
                if ((string)LeaderboardDifficulty.Content == "Easy")
                {
                    PopulateLeaderboardGrid(Mediumleaderboard, Leaderboardscreenleaderboard);
                    LeaderboardDifficulty.Content = "Medium";
                }
                else if ((string)LeaderboardDifficulty.Content == "Medium") // Added parentheses around the condition
                {
                    PopulateLeaderboardGrid(Hardleaderboard, Leaderboardscreenleaderboard);
                    LeaderboardDifficulty.Content = "Hard"; // Added the assignment to change the difficulty
                }
                else if ((string)LeaderboardDifficulty.Content == "Hard") // Added parentheses around the condition
                {
                    PopulateLeaderboardGrid(Nightmareleaderboard, Leaderboardscreenleaderboard);
                    LeaderboardDifficulty.Content = "Nightmare"; // Added the assignment to change the difficulty
                }
                else if ((string)LeaderboardDifficulty.Content == "Nightmare") // Added parentheses around the condition
                {
                    PopulateLeaderboardGrid(Easyleaderboard, Leaderboardscreenleaderboard);
                    LeaderboardDifficulty.Content = "Easy"; // Added the assignment to change the difficulty
                }
            }
            

            if(PointDisplay.Content.ToString() == "ScorePerSecond")
            {
                if ((string)LeaderboardDifficulty.Content == "Easy")
                {
                    PopulateLeaderboardGridPointsPerSeconds(Mediumleaderboard, Leaderboardscreenleaderboard);
                    LeaderboardDifficulty.Content = "Medium";
                }
                else if ((string)LeaderboardDifficulty.Content == "Medium") // Added parentheses around the condition
                {
                    PopulateLeaderboardGridPointsPerSeconds(Hardleaderboard, Leaderboardscreenleaderboard);
                    LeaderboardDifficulty.Content = "Hard"; // Added the assignment to change the difficulty
                }
                else if ((string)LeaderboardDifficulty.Content == "Hard") // Added parentheses around the condition
                {
                    PopulateLeaderboardGridPointsPerSeconds(Nightmareleaderboard, Leaderboardscreenleaderboard);
                    LeaderboardDifficulty.Content = "Nightmare"; // Added the assignment to change the difficulty
                }
                else if ((string)LeaderboardDifficulty.Content == "Nightmare") // Added parentheses around the condition
                {
                    PopulateLeaderboardGridPointsPerSeconds(Easyleaderboard, Leaderboardscreenleaderboard);
                    LeaderboardDifficulty.Content = "Easy"; // Added the assignment to change the difficulty
                }

            }

        }

        private void ControlsButtonClick(object sender, RoutedEventArgs e)
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

        private void swapcontrols()
        {
            if (currentcontrols == "Buttons")
            {
                TheTextbox.Visibility = Visibility.Hidden;
                TheButtons.Visibility = Visibility.Visible;
                ControlsLabel.Content = "Click on the buttons or use numpad! 9 swaps all and 0 clears all";
            }
            if (currentcontrols == "Textbox")
            {
                TheButtons.Visibility = Visibility.Hidden;
                TheTextbox.Visibility = Visibility.Visible;
                ControlsLabel.Content = "Type into the textbox, it only accepts 1s and 0s";
            }
            ControlsButton.Content = currentcontrols;
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
                ResultDisplay.Content = BinaryToDecimal(textBox.Text);
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

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            writeleaderboard(leaderboardFilePath);
            musicvolumevariable = Musicplayer.Volume;
            soundvolumevariable = Clickplayer.Volume;
            WriteToPreferencesFile(savedPreferencesFilePath);
        }

        private void ReadNoteFile(string filepath)
        {
            try
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    string line;
                    string lines = "";

                    // Read all lines from the file
                    while ((line = sr.ReadLine()) != null)
                    {
                        lines += line + "\n"; // Add newline to preserve line breaks
                    }

                    // Split notes based on the delimiter "NOTES"
                    string[] notes = lines.Split(new string[] { "NOTE" }, StringSplitOptions.None);
                    //MessageBox.Show(notes[0]);

                    // If notes array has enough elements
                    if (notes.Length >= 6)
                    {
                        // Store notes in notetext array
                        for (int i = 0; i < 6; i++)
                        {
                            // Trim each note to remove leading/trailing whitespace
                            notetext[i] = notes[i + 1].Trim(); //i dont know why i need the +1 but aight
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error: Not enough notes found in the file.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reading note file: " + ex.Message);
            }
        }

        private void Noteunlocker()
        {
            // Unlock notes based on game progress or achievements
            if (currentscore > 0)
            {
                noteunlock[1] = 1; // Unlock note 2 after the first game
            }

            if (currentscore >= 500 && difficulty == "Easy")
            {
                noteunlock[2] = 1;
            }

            if (currentscore >= 500 && difficulty == "Medium")
            {
                noteunlock[3] = 1;
            }

            if (currentscore >= 500 && difficulty == "Hard")
            {
                noteunlock[4] = 1; 
            }

            if (currentscore >= 100 && difficulty == "Nightmare")
            {
                noteunlock[5] = 1; // Unlock note 6 by getting 100 on Nightmare
            }
        }

        private void PopulateListOfAvailableNotes()
        {
            ListOfAvailableNotes.Items.Clear();
            for (int i = 0; i < noteunlock.Length; i++)
            {
                if (noteunlock[i] == 1)
                {
                    ListBoxItem item = new ListBoxItem();
                    item.Content = "Note " + (i + 1);
                    item.Tag = i; // Store the index of the note
                    ListOfAvailableNotes.Items.Add(item);
                }
                else
                {
                    ListBoxItem item = new ListBoxItem();
                    if(i == 0)
                    {
                        item.Content = "Note " + (i + 1) + " - Youre supposed to have this";
                    }
                    if (i == 1)
                    {
                        item.Content = "Note " + (i + 1) + " - Play a game!";
                    }
                    if (i == 2)
                    {
                        item.Content = "Note " + (i + 1) + " - Get 500 on Easy!";
                    }
                    if (i == 3)
                    {
                        item.Content = "Note " + (i + 1) + " - Get 500 on Medium!";
                    }
                    if (i == 4)
                    {
                        item.Content = "Note " + (i + 1) + " - Get 500 on Hard!";
                    }
                    if (i == 5)
                    {
                        item.Content = "Note " + (i + 1) + " - Get 50 on Nightmare!";
                    }
                    //item.Content = "Note " + (i + 1) + " - Unavailable";
                    item.IsEnabled = false; // Disable selection for unavailable notes
                    ListOfAvailableNotes.Items.Add(item);
                }
            }
        }

        private void ListOfAvailableNotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListOfAvailableNotes.SelectedItem != null)
            {
                int selectedIndex = (int)((ListBoxItem)ListOfAvailableNotes.SelectedItem).Tag;
                NoteReader.Text = notetext[selectedIndex];
            }
        }

        public void WriteToPreferencesFile(string filePath)
        {
            try
            {
                // Create or overwrite the preferences file
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine("Difficulty: " + difficulty);
                    sw.WriteLine("Controls: " + currentcontrols);
                    sw.WriteLine("NoteUnlock: " + string.Join(",", noteunlock));
                    sw.WriteLine("MusicVolume: " + musicvolumevariable);
                    sw.WriteLine("SoundVolume: " + soundvolumevariable);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to preferences file: {ex.Message}");
            }
        }

        public void ReadFromCSVFile(string filePath)
        {
            try
            {
                // Check if the preferences file exists
                if (File.Exists(filePath))
                {
                    // Read all lines from the preferences file
                    string[] lines = File.ReadAllLines(filePath);

                    // Loop through each line and extract the variable values
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(':');
                        if (parts.Length == 2)
                        {
                            string variableName = parts[0].Trim();
                            string value = parts[1].Trim();

                            // Set the variables based on the extracted values
                            switch (variableName)
                            {
                                case "Difficulty":
                                    difficulty = value;
                                    break;
                                case "Controls":
                                    currentcontrols = value;
                                    break;
                                case "NoteUnlock":
                                    string[] unlockValues = value.Split(',');
                                    if (unlockValues.Length == noteunlock.Length)
                                    {
                                        for (int i = 0; i < noteunlock.Length; i++)
                                        {
                                            noteunlock[i] = int.Parse(unlockValues[i]);
                                        }
                                    }
                                    break;
                                case "MusicVolume":
                                    musicvolumevariable = double.Parse(value);
                                    break;
                                case "SoundVolume":
                                    soundvolumevariable = double.Parse(value);
                                    break;
                                default:
                                    // Handle unknown variable names (if necessary)
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Preferences file not found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading preferences file: {ex.Message}");
            }
        }

        private void UNPAUSEBUTTON(object sender, RoutedEventArgs e)
        {
            PauseScreen.Visibility = Visibility.Hidden;
            timer.Start();
        }

        private void PointDisplay_Click(object sender, RoutedEventArgs e)
        {
            if(PointDisplay.Content.ToString() == "Score")
            {
                PointDisplay.Content = "ScorePerSecond";
            }
            else if (PointDisplay.Content.ToString() == "ScorePerSecond")
            {
                PointDisplay.Content = "Score";
            }
            if (PointDisplay.Content.ToString() == "ScorePerSecond")
            {
                Leaderboardscreenleaderboard.Items.Clear();

                if ((string)LeaderboardDifficulty.Content == "Easy")
                {
                    
                    PopulateLeaderboardGridPointsPerSeconds(Easyleaderboard, Leaderboardscreenleaderboard);
                }
                else if ((string)LeaderboardDifficulty.Content == "Medium") // Added parentheses around the condition
                {
                    PopulateLeaderboardGridPointsPerSeconds(Mediumleaderboard, Leaderboardscreenleaderboard);
                   
                }
                else if ((string)LeaderboardDifficulty.Content == "Hard") // Added parentheses around the condition
                {
                    PopulateLeaderboardGridPointsPerSeconds(Hardleaderboard, Leaderboardscreenleaderboard);
                    
                }
                else if ((string)LeaderboardDifficulty.Content == "Nightmare") // Added parentheses around the condition
                {
                    PopulateLeaderboardGridPointsPerSeconds(Nightmareleaderboard, Leaderboardscreenleaderboard);
                }
            }
            if (PointDisplay.Content.ToString() == "Score")
            {
                Leaderboardscreenleaderboard.Items.Clear();

                if ((string)LeaderboardDifficulty.Content == "Easy")
                {
                    PopulateLeaderboardGrid(Easyleaderboard, Leaderboardscreenleaderboard);
                }
                else if ((string)LeaderboardDifficulty.Content == "Medium") // Added parentheses around the condition
                {
                    PopulateLeaderboardGrid(Mediumleaderboard, Leaderboardscreenleaderboard);

                }
                else if ((string)LeaderboardDifficulty.Content == "Hard") // Added parentheses around the condition
                {
                    PopulateLeaderboardGrid(Hardleaderboard, Leaderboardscreenleaderboard);

                }
                else if ((string)LeaderboardDifficulty.Content == "Nightmare") // Added parentheses around the condition
                {
                    PopulateLeaderboardGrid(Nightmareleaderboard, Leaderboardscreenleaderboard);
                }
            }

        }
    }

}