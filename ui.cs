using Godot;
using System;
using System.Numerics;

public partial class ui : CanvasLayer
{
    private Label scoreLabel;
    private Label message;
    private Button startButton;
    private Timer messageTimer;
    private string scoreLabelDefaultText = "0";
    private string messageDefaultText= "Dodge the Creeps!";
    private float messageTimerWaitTime = 2.0f;

    [Signal]
    public delegate void StartGameEventHandler();

    public override void _Ready()
    {
        scoreLabel = GetNode<Label>("ScoreLabel");
        message = GetNode<Label>("Message");
        startButton = GetNode<Button>("StartButton");
        messageTimer = GetNode<Timer>("MessageTimer");

        // score label style
        scoreLabel.Text = scoreLabelDefaultText;
        scoreLabel.VerticalAlignment = VerticalAlignment.Center;
        scoreLabel.HorizontalAlignment = HorizontalAlignment.Center;
        scoreLabel.AddThemeFontSizeOverride("font_size", 64);
        scoreLabel.AnchorsPreset = (int)Control.LayoutPreset.CenterTop;
        
        // message style
        message.Text = messageDefaultText;
        message.HorizontalAlignment = HorizontalAlignment.Center;
        message.VerticalAlignment = VerticalAlignment.Center;
        message.AutowrapMode = TextServer.AutowrapMode.Word;
        message.Size = new Godot.Vector2(480, 0);
        message.AddThemeFontSizeOverride("font_size", 64);
        message.AnchorsPreset = (int)Control.LayoutPreset.Center;
        
        // message timer
        messageTimer.OneShot = true;
        messageTimer.WaitTime = messageTimerWaitTime;

        // button style
        startButton.Text = "Start";
        startButton.Size = new Godot.Vector2(200,100);
        startButton.AnchorsPreset = (int)Control.LayoutPreset.CenterBottom;
        startButton.AddThemeFontSizeOverride("font_size", 64);
        startButton.Position = new Godot.Vector2(140, 500);
    }

    public void OnStartButtonPressed()
    {
        startButton.Hide();
        EmitSignal(SignalName.StartGame);
    }

    public void ShowMessage(string newText)
    {
        message.Text = newText;
        message.Show();

        messageTimer.Start();
    }

    async public void ShowGameOver()
    {
        ShowMessage("Game Over");

        await ToSignal(messageTimer, Timer.SignalName.Timeout);

        message.Text = "Dodge the\nCreeps!";
        message.Show();

        await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
        startButton.Show();
    }

    public void UpdateScore(int newScore)
    {
        scoreLabel.Text = newScore.ToString();
    }

    public void OnMessageTimerTimeout()
    {
        message.Hide();
    }
}
