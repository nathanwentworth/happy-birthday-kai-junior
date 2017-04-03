using InControl;
using UnityEngine;

public class Controls : PlayerActionSet
{
  public PlayerAction Confirm;
  public PlayerAction Cancel;
  public PlayerAction Pause;

  public PlayerAction Left;
  public PlayerAction Right;
  public PlayerAction Up;
  public PlayerAction Down;

  public PlayerAction LookLeft;
  public PlayerAction LookRight;
  public PlayerAction LookDown;
  public PlayerAction LookUp;

  public PlayerTwoAxisAction Move;
  public PlayerTwoAxisAction Look;


  public Controls()
  {
    Confirm = CreatePlayerAction( "Confirm" );
    Cancel = CreatePlayerAction( "Cancel" );
    Pause = CreatePlayerAction( "Pause" );

    Left = CreatePlayerAction( "Move Left" );
    Right = CreatePlayerAction( "Move Right" );
    Up = CreatePlayerAction( "Move Up" );
    Down = CreatePlayerAction( "Move Down" );

    LookLeft = CreatePlayerAction( "Look Left" );
    LookRight = CreatePlayerAction( "Look Right" );
    LookDown = CreatePlayerAction( "Look Down" );
    LookUp = CreatePlayerAction( "Look Up" );

    Move = CreateTwoAxisPlayerAction( Left, Right, Down, Up );
    Look = CreateTwoAxisPlayerAction( LookLeft, LookRight, LookDown, LookUp );
  }


  public static Controls DefaultBindings()
  {
    var controls = new Controls();

    controls.Confirm.AddDefaultBinding( Key.Return );
    controls.Confirm.AddDefaultBinding( InputControlType.Action1 );

    controls.Cancel.AddDefaultBinding( Key.Delete );
    controls.Cancel.AddDefaultBinding( InputControlType.Action2 );

    controls.Pause.AddDefaultBinding( Key.Escape );
    controls.Pause.AddDefaultBinding( InputControlType.Command );


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


    controls.LookLeft.AddDefaultBinding( Mouse.NegativeX );
    controls.LookRight.AddDefaultBinding( Mouse.PositiveX );
    controls.LookDown.AddDefaultBinding( Mouse.NegativeY );
    controls.LookUp.AddDefaultBinding( Mouse.PositiveY );

    controls.LookLeft.AddDefaultBinding( InputControlType.RightStickLeft );
    controls.LookRight.AddDefaultBinding( InputControlType.RightStickRight );
    controls.LookDown.AddDefaultBinding( InputControlType.RightStickDown );
    controls.LookUp.AddDefaultBinding( InputControlType.RightStickUp);


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
