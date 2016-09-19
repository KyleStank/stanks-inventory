using UnityEngine;
using System.Collections;

namespace KStank.stanks_inventory.Example {
    /// <summary>
    /// Custom item(in this case, a gun) that can be saved to inventory.
    /// </summary>
    public class Gun : Item {
        float damage = 0.0f;

        /// <summary>
        /// Amount of damage a weapon outputs.
        /// </summary>
        public float Damage {
            get { return damage; }
            set { damage = value; }
        }

        public Gun(string name, int position, float damage) : base(name, position) {
            Name = name;
            Position = position;
            this.damage = damage;
        }
    }
}
