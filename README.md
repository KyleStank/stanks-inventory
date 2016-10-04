# stanks-inventory
### Summary
stanks-inventory is a elegant Unity3D inventory system. It is simple and easy to use, yet highly flexible. If you don't want to waste your time creating items
through files, don't worry, stanks-inventory will do it for you! You can easily create items through Unity's editor.

### Intergrating into other frameworks
Using a big framework for your game? No problem! With a little bit of programming, you can intergrate stanks-inventory into a big framework, such as Deftly. All you need to do
is add an Inventory component to your Player, and change the values of the framework with items that you created.

Say that you have a big Shoot script attached to your Player that handles shooting. It has 700 lines of code, and you really don't want to re-write it, but you want a Gun that you created in stanks-inventory to be used. All you have to do is open the Shoot script, find what you want to change, and change it!<br />
#### Example
```csharp
using KStank.stanks_inventory;

public class Shoot : MonoBehaviour {
    Inventory inventory;
    float damage = 10.0f;

    void Awake() {
        inventory = GetComponent<Inventory>();
    }

    void Shoot() {
        //Raycast forward, blah blah...
        
        /*
        In your block of code somewhere, when you have a value that you want to change,
        just use the reference to the Inventory component.
        
        Here, we are going to find a Gun class,
        which inherits from Item(or simply, is a custom object).
        You could also do this at the start of the script,
        which would probably make more sense.
        
        The Item's ID is 3.
        */
        Gun gun = inventory.Find(3) as Gun;
        damage = gun.Damage;
        
        //Do you regular stuff after the values are modified...
        hitObject.SendMessage("Damage", damage, SendMessageOptions.DontRequireReceiver);
        //...
    }
}

//Rest of script...
```

## Features
- Simple, easy, and minimal
- Drag and drop items
- Easy to save and load inventories
- Swappable items
- Custom items
- Stackable items
- Basic editor support

## Plans
- Better Editor Support
- Easier Configuring