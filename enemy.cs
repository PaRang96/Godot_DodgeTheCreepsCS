using Godot;
using System;

public partial class enemy : RigidBody2D
{
    protected string[] mobTypes;

    // random type of animation
    // among walk, swim, fly
    public override void _Ready()
    {
        AnimatedSprite2D animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
        animatedSprite2D.Play(mobTypes[GD.Randi() % mobTypes.Length]);
    }

    // delete scene and child nodes
    // when instance hits the edge of screen
    private void OnVisibleOnScreenNotifier2DScreenExited()
    {
        QueueFree();
    }
}
