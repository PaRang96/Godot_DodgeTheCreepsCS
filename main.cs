using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public partial class main : Node
{
    [Export]
    public PackedScene mobScene { get; set; }

    private int score;

    private Timer mobTimer;
    private Timer scoreTimer;
    private Timer startTimer;

    private Player player;

    private Marker2D startLocation;
    private Vector2 startPosition;
    private PathFollow2D enemySpawnLocation;
    private Path2D enemyPath;

    private ColorRect colorRect;

    private ui UI;
    private AudioStreamPlayer musicPlayer;
    private AudioStreamPlayer deathSoundPlayer;

    public override void _Ready()
    {
        mobTimer = GetNode<Timer>("MobTimer");
        scoreTimer = GetNode<Timer>("ScoreTimer");
        startTimer = GetNode<Timer>("StartTimer");

        enemyPath = GetNode<Path2D>("EnemyPath");
        enemySpawnLocation = GetNode<PathFollow2D>("EnemyPath/EnemySpawnLocation");

        mobTimer.WaitTime = 0.5f;
        scoreTimer.WaitTime = 1.0f;
        startTimer.WaitTime = 2.0f;

        startTimer.OneShot = true;

        player = GetNode<Player>("Player");

        startLocation = GetNode<Marker2D>("StartLocation");
        startPosition.X = 240;
        startPosition.Y = 450;
        startLocation.Position = startPosition;

        UI = GetNode<ui>("HUD");

        colorRect = GetNode<ColorRect>("ColorRect");
        colorRect.AnchorsPreset = (int)Control.LayoutPreset.FullRect;
        colorRect.ZIndex = -100;
        colorRect.Color = new Color(0.1f, 0.1f, 0.1f);

        musicPlayer = GetNode<AudioStreamPlayer>("Music");
        deathSoundPlayer = GetNode<AudioStreamPlayer>("DeathSound");

        musicPlayer.Stream = (AudioStream)GD.Load("res://art/House In a Forest Loop.ogg");
        
        deathSoundPlayer.Stream = (AudioStream)GD.Load("res://art/gameover.wav");
        //NewGame();
    }

    private void onMobTimerTimeout()
    {
        enemy Enemy = mobScene.Instantiate<enemy>();

        enemySpawnLocation.ProgressRatio = GD.Randf();

        float direction = enemySpawnLocation.Rotation + Mathf.Pi / 2;

        Enemy.Position = enemySpawnLocation.Position;

        direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi/4);

        Enemy.Rotation = direction;

        Vector2 velocity = new Vector2((float)GD.RandRange(150.0, 250.0) , 0);

        Enemy.LinearVelocity = velocity.Rotated(direction);

        AddChild(Enemy);
    }

    private void onScoreTimerTimeout()
    {
        score++;
        UI.UpdateScore(score);
    }

    private void onStartTimerTimeout()
    {
        mobTimer.Start();
        scoreTimer.Start();

        UI.UpdateScore(score);
    }

    public void GameOver()
    {
        GD.Print("Game Over called");

        mobTimer.Stop();
        scoreTimer.Stop();

        UI.ShowGameOver();

        GetTree().CallGroup("enemies", Node.MethodName.QueueFree);

        musicPlayer.Stop();
        deathSoundPlayer.Play();
    }

    public void NewGame()
    {
        score = 0;

        player.Start(startLocation.Position);

        startTimer.Start();

        UI.UpdateScore(score);
        UI.ShowMessage("Get Ready!");

        GetTree().CallGroup("enemies", Node.MethodName.QueueFree);

        musicPlayer.Play();
    }
}
