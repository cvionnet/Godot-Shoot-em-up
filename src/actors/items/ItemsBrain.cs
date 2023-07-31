using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BulletBallet.utils.NucleusFW.SpawnFactory;

namespace BulletBallet.actors.items;

/// <summary>
/// Responsible for :
/// - initialize properties for a new random item
/// - adding random items to the scene
/// - timing to create a new item
/// </summary>
public partial class ItemsBrain : Node2D
{
    //! How to add a new item : see file "README.drawio"  (How to)

    [Export] private PackedScene ItemGenericScene;
    [Export] private float MinTimerNewItem = 30.0f;
    [Export] private float MaxTimerNewItem = 60.0f;

    private Spawn_Factory _spawnItems;
    private Timer _timerNewItem;

    private List<ItemGeneric> _listItems = new List<ItemGeneric>();     // TODO : performance => use an ARRAY ?  (et ajouter l'id dans les propriétés du ItemGeneric)
    private List<classes.Item> _listUniqueItems = new List<classes.Item>();           // items that appear only once during the game
    private ItemGeneric _itemTouched = new ItemGeneric();

//*-------------------------------------------------------------------------*//

    #region GODOT METHODS

    public override void _Ready()
    {
        _spawnItems = GetNode<Spawn_Factory>("Spawn_Items");
        _timerNewItem = GetNode<Timer>("TimerNewItem");

        _timerNewItem.Connect("timeout", new Callable(this, nameof(onNewItemTimer_Timeout)));

        Nucleus.SignalManager.Connect(SignalManager.SignalName.ItemGeneric_ItemBrain_Touched, new Callable(this, nameof(onItem_Touched)));

        Initialize_ItemsBrain();
    }

    // Use to add warning in the Editor   (must add the [Tool] attribute on the class)
    public override string[] _GetConfigurationWarnings()
    {
        return (ItemGenericScene == null) ? new [] { "The object ItemGenericScene must not be empty !" } : new [] { "" };
    }

    #endregion

//*-------------------------------------------------------------------------*//

    #region SIGNAL CALLBACKS

    /// <summary>
    /// Time to create a new item and start a new timer
    /// </summary>
    private void onNewItemTimer_Timeout()
    {
        Create_Item();
        Initialize_TimerNewItem();
    }

    /// <summary>
    /// When a player / pnj has collided with an item, send information to final receiver
    /// </summary>
    /// <param name="itemName">The name of the item node</param>
    private void onItem_Touched(string itemName)
    {
        // Get the touched item properties
        _itemTouched = _listItems.Find(i => i.Name == itemName);

        _listItems.Remove(_itemTouched);
    }

    #endregion

//*-------------------------------------------------------------------------*//

    #region USER METHODS

    private void Initialize_ItemsBrain()
    {
        _spawnItems.Load_NewScene(ItemGenericScene.ResourcePath);

        Initialize_TimerNewItem();
    }

    /// <summary>
    /// Create a new item instance
    /// </summary>
    private void Create_Item()
    {
        // Select a random item
        classes.Item itemProperties = new classes.Item();
        var random_item = (GameManager.ItemsActionList) Nucleus_Maths.Rnd.RandiRange(0, Enum.GetNames(typeof(GameManager.ItemsActionList)).Length-1);

        // Apply item properties
        switch (random_item)
        {
            case GameManager.ItemsActionList.CHARACTER_FASTER:
                itemProperties.SpritePath = "res://src/actors/items/itemSprites/ItemSprite_Player_GainSpeed.tscn";
                itemProperties.SendTo = GameManager.ItemsSendTo.CHARACTER;
                itemProperties.ActionName = random_item;
                itemProperties.ActionDuration = 3.0f;
                itemProperties.MaxVisibleInstance = 2;
                itemProperties.Ttl = 10.0f;
                itemProperties.OptionalValue = 1.5f;    // the percent to apply on MaxSpeed's character
                break;
            case GameManager.ItemsActionList.OTHER_CHARACTERS_SLOWER:
                itemProperties.SpritePath = "res://src/actors/items/itemSprites/ItemSprite_OtherPlayers_LooseSpeed.tscn";
                itemProperties.SendTo = GameManager.ItemsSendTo.OTHER_CHARACTERS;
                itemProperties.ActionName = random_item;
                itemProperties.ActionDuration = 5.0f;
                itemProperties.MaxVisibleInstance = 2;
                itemProperties.Ttl = 10.0f;
                itemProperties.OptionalValue = 0.5f;    // the percent to apply on MaxSpeed's characters
                break;
            case GameManager.ItemsActionList.ALL_CHARACTERS_FASTER:
                itemProperties.SpritePath = "res://src/actors/items/itemSprites/ItemSprite_AllPlayers_GainSpeed.tscn";
                itemProperties.SendTo = GameManager.ItemsSendTo.ALL_CHARACTERS;
                itemProperties.ActionName = random_item;
                itemProperties.ActionDuration = 5.0f;
                itemProperties.MaxVisibleInstance = 2;
                itemProperties.Ttl = 10.0f;
                itemProperties.OptionalValue = 1.5f;    // the percent to apply on MaxSpeed's characters
                break;
        }

        // Only load the item if it is unique and already in the game AND its max instance number is not reached
        if (!_listUniqueItems.Any(i => i.ActionName == itemProperties.ActionName)
            && _listItems.Count(i => i.ItemProperties.ActionName == random_item) < itemProperties.MaxVisibleInstance)
        {
            // Initialize the item
            if (itemProperties.SpritePath != null)
            {
                Vector2 newPosition = new Vector2(Nucleus_Maths.Rnd.RandfRange(40.0f, Nucleus.ScreenWidth-40.0f), Nucleus_Maths.Rnd.RandfRange(40.0f, Nucleus.ScreenHeight-40.0f));
                //ItemGeneric instance = new ItemGeneric();
                ItemGeneric instance = _spawnItems.Add_Instance<ItemGeneric>(null, newPosition);
                _listItems.Add(instance);
                if (itemProperties.UniqueInstance)
                    _listUniqueItems.Add(itemProperties);

                instance.Initialize_ItemProperties(itemProperties);
            }
            else
            {
                Nucleus.Logs.Error($"Error while loading Item Action '{random_item}' (path is null))", new NullReferenceException(), GetType().Name, MethodBase.GetCurrentMethod().Name);
            }
        }
        else
        {
            itemProperties = null;
            //GD.Print($"{random_item} max number");
        }
    }

    private void Initialize_TimerNewItem()
    {
        _timerNewItem.WaitTime = Nucleus_Maths.Rnd.RandfRange(MinTimerNewItem, MaxTimerNewItem);
        _timerNewItem.Start();
    }

    #endregion
}