
using Microsoft.Maui.Controls.Shapes;


namespace Whack_a_mole
{
    public partial class MainPage : ContentPage
    {
        private int _score=0;
        private int bigestScoreThisFar = 0;
        private int _timeRemaining=30;
        private IDispatcherTimer _gameTimer;
        private bool _gameRunning=false;
        private Random _random;
        int _moleRow;
        int _moleCol;

        public MainPage()
        {
            InitializeComponent();
            _random = new Random();
            CreatGameGrid();
            _gameTimer = Dispatcher.CreateTimer();
            _gameTimer.Interval = TimeSpan.FromSeconds(1);
            _gameTimer.Tick += OnGameTimerTick;
            Mole.Opacity = 1;
            Mole.InputTransparent = false;

        }

        private void OnGameTimerTick(object? sender, EventArgs e)
        {
            _timeRemaining--;
           Timer.Text =$"Remaining time : {_timeRemaining }" ;
            if (_timeRemaining < 1)
            {
                Timer.Text = $"End of game ! ";
                _gameTimer.Stop();
                StratButton.IsEnabled = true;
                _gameRunning = false;
                if(bigestScoreThisFar < _score)
                {
                    bigestScoreThisFar = _score;
                    ScoreLabel.Text = $"Highest Score updated! {bigestScoreThisFar}";
                    ;
                }
                _score = 0;
            }
            if (_timeRemaining > 1)
            {
                StratButton.IsEnabled = false;
            }

        }
        private void MoveMole()
        {
             _moleRow = _random.Next(3);
             _moleCol = _random.Next(3);
            Grid.SetRow(Mole, _moleRow);
            Grid.SetColumn(Mole, _moleCol);
        }


        private async void StratButton_Clicked(object sender, EventArgs e)
        {
            _score = 0;
            _gameTimer.Start();
            startGame();
            _gameRunning = true;
            await ShowMole();
        }

        private async Task ShowMole()
        {
            while (_gameRunning)
            {
                MoveMole();
                Mole.Opacity = 1;
                Mole.Scale = 0;
                Mole.Rotation = 0;
                Mole.InputTransparent= false;
                await Mole.ScaleToAsync(1, 750);
                await Mole.RotateToAsync(1, 750);
                await Mole.FadeToAsync(0, 1500);
                await Task.Yield();



            }
        }

        private void startGame()
        {
            _timeRemaining = 30;
            ScoreLabel.Text = $"{_score}";
            _gameRunning = true;
        }

        private void CreatGameGrid()
        {
            for(int row = 0; row <3; row++)
            {
                GameGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int column = 0; column < 3; column++)
            {
                GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for(int col = 0; col < 3; col++)
            {
                for(int row =0; row <3; row++)
                {
                    Border border = new Border
                    {
                        BackgroundColor = Colors.LightGreen,
                        Stroke = Colors.DarkGreen,
                        StrokeThickness = 3,
                        StrokeShape = new RoundRectangle
                        {
                            CornerRadius = 15
                        },
                        Margin = new Thickness(5)
                    };
                    GameGrid.Add(border, col, row);
                }
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            _score++;
            ScoreLabel.Text = $"Score {_score} ";
            Mole.CancelAnimations();

        }


    }
}
