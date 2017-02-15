using InControl;
using UnityEngine;

public class Controls : PlayerActionSet
{
  public PlayerAction Jump;
  public PlayerAction Interact;

  public PlayerAction SetRespawn;
  public PlayerAction GoToRespawn;

  public PlayerAction Reset;
  public PlayerAction Confirm;
  public PlayerAction Pause;

  public PlayerAction Brake;
  public PlayerAction Push;

  public PlayerAction Left;
  public PlayerAction Right;
  public PlayerAction Up;
  public PlayerAction Down;

  public PlayerAction LookLeft;
  public PlayerAction LookRight;

  public PlayerAction RollLeft;
  public PlayerAction RollRight;

  public PlayerTwoAxisAction Move;
  public PlayerOneAxisAction Look;
  public PlayerOneAxisAction Roll;


  public Controls()
  {
    Jump = CreatePlayerAction( "Jump" );
    Interact = CreatePlayerAction( "Interact" );

    SetRespawn = CreatePlayerAction( "Set Respawn" );
    GoToRespawn = CreatePlayerAction( "Go To Respawn" );

    Reset = CreatePlayerAction( "Reset" );
    Confirm = CreatePlayerAction( "Confirm" );
    Pause = CreatePlayerAction( "Pause" );
    Brake = CreatePlayerAction( "Brake" );
    Push = CreatePlayerAction( "Push" );
    Left = CreatePlayerAction( "Move Left" );
    Right = CreatePlayerAction( "Move Right" );
    Up = CreatePlayerAction( "Move Up" );
    Down = CreatePlayerAction( "Move Down" );
    LookLeft = CreatePlayerAction( "Look Left" );
    LookRight = CreatePlayerAction( "Look Right" );
    RollLeft = CreatePlayerAction( "Roll Left" );
    RollRight = CreatePlayerAction( "Roll Right" );
    Move = CreateTwoAxisPlayerAction( Left, Right, Down, Up );
    Look = CreateOneAxisPlayerAction( LookLeft, LookRight);
    Roll = CreateOneAxisPlayerAction( RollLeft, RollRight);
  }


  public static Controls DefaultBindings()
  {
    var controls = new Controls();

    controls.Jump.AddDefaultBinding( Key.Space );
    controls.Jump.AddDefaultBinding( InputControlType.Action1 );

    controls.Interact.AddDefaultBinding( Key.E );
    controls.Interact.AddDefaultBinding( InputControlType.Action2 );

    controls.SetRespawn.AddDefaultBinding( Key.O );
    controls.SetRespawn.AddDefaultBinding( InputControlType.LeftBumper );

    controls.GoToRespawn.AddDefaultBinding( Key.P );
    controls.GoToRespawn.AddDefaultBinding( InputControlType.RightBumper );

    controls.Interact.AddDefaultBinding( Key.E );
    controls.Interact.AddDefaultBinding( InputControlType.Action2 );

    controls.Reset.AddDefaultBinding( Key.R );
    controls.Reset.AddDefaultBinding( InputControlType.Action3 );

    controls.Confirm.AddDefaultBinding( Key.Return );
    controls.Confirm.AddDefaultBinding( InputControlType.Action2 );

    controls.Pause.AddDefaultBinding( Key.Escape );
    controls.Pause.AddDefaultBinding( InputControlType.Command );

    controls.Brake.AddDefaultBinding( Key.LeftShift );
    controls.Brake.AddDefaultBinding( InputControlType.Action4 );

    controls.Push.AddDefaultBinding( Key.Space );
    controls.Push.AddDefaultBinding( InputControlType.Action1 );

    // controls.Jump.AddDefaultBinding( Mouse.LeftButton );

    // controls.Jump.AddDefaultBinding( Key.Space );
    // controls.Jump.AddDefaultBinding( InputControlType.Action3 );
    // controls.Jump.AddDefaultBinding( InputControlType.Back );

    controls.Up.AddDefaultBinding( Key.W );
    controls.Down.AddDefaultBinding( Key.S );
    controls.Left.AddDefaultBinding( Key.A );
    controls.Right.AddDefaultBinding( Key.D );

    controls.Left.AddDefaultBinding( InputControlType.LeftStickLeft );
    controls.Right.AddDefaultBinding( InputControlType.LeftStickRight );
    controls.Up.AddDefaultBinding( InputControlType.LeftStickUp );
    controls.Down.AddDefaultBinding( InputControlType.LeftStickDown );

    controls.Left.AddDefaultBinding( InputControlType.DPadLeft );
    controls.Right.AddDefaultBinding( InputControlType.DPadRight );
    controls.Up.AddDefaultBinding( InputControlType.DPadUp );
    controls.Down.AddDefaultBinding( InputControlType.DPadDown );

    // controls.Up.AddDefaultBinding( Mouse.PositiveY );
    // controls.Down.AddDefaultBinding( Mouse.NegativeY );
    controls.LookLeft.AddDefaultBinding( Mouse.NegativeX );
    controls.LookRight.AddDefaultBinding( Mouse.PositiveX );
    controls.LookLeft.AddDefaultBinding( InputControlType.RightStickLeft );
    controls.LookRight.AddDefaultBinding( InputControlType.RightStickRight );

    controls.RollLeft.AddDefaultBinding( Key.Q );
    controls.RollRight.AddDefaultBinding( Key.E );
    // controls.RollLeft.AddDefaultBinding( InputControlType.RightStickLeft );
    // controls.RollRight.AddDefaultBinding( InputControlType.RightStickRight );


    controls.ListenOptions.IncludeUnknownControllers = true;
    controls.ListenOptions.MaxAllowedBindings = 8;
    //controls.ListenOptions.MaxAllowedBindingsPerType = 1;
    //controls.ListenOptions.UnsetDuplicateBindingsOnSet = true;
    //controls.ListenOptions.IncludeMouseButtons = true;
    //controls.ListenOptions.IncludeModifiersAsFirstClassKeys = true;

    controls.ListenOptions.OnBindingFound = ( action, binding ) => {
      if (binding == new KeyBindingSource( Key.Escape ))
      {
        action.StopListeningForBinding();
        return false;
      }
      return true;
    };

    controls.ListenOptions.OnBindingAdded += ( action, binding ) => {
      Debug.Log( "Binding added... " + binding.DeviceName + ": " + binding.Name );
    };

    controls.ListenOptions.OnBindingRejected += ( action, binding, reason ) => {
      Debug.Log( "Binding rejected... " + reason );
    };

    return controls;
  }
}
