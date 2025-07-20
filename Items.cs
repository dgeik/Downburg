using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test4
{
    internal class Items
    {
        public int thisX;
        public int thisY;
        public int hp;
        public int damage;
        public string name;
        public virtual char visibleCharOfItem { get; set; }
        public virtual string Equip { get; set; }

        public Items(int thisX, int thisY, int hp, int damage, string name)
        {
            this.thisX = thisX;
            this.thisY = thisY;
            this.hp = hp;
            this.damage = damage;
            this.name = name;
        }
    }

    internal class Consumable : Items
    {
        public Consumable(int thisX, int thisY, int hp, int damage, string name) : base(thisX, thisY, hp, damage, name)
        {
        }

        private char charOfItem = 'C';
        public override char visibleCharOfItem
        {
            get { return charOfItem; }
            set { charOfItem = value; }
        }

        private string canEquip = "Consume";
        public override string Equip
        {
            get { return canEquip; }
            set { canEquip = value; }
        }
    }

    internal class Equipment : Items
    {
        public Equipment(int thisX, int thisY, int hp, int damage, string name) : base(thisX, thisY, hp, damage, name)
        {
        }

        private char charOfItem = 'E';
        public override char visibleCharOfItem
        {
            get { return charOfItem; }
            set { charOfItem = value; }
        }

        private string canEquip = "Equipment";
        public override string Equip
        {
            get { return canEquip; }
            set { canEquip = value; }
        }
    }

    internal class Weapon : Equipment
    {
        
        public Weapon(int thisX, int thisY, int hp, int damage, string name) : base(thisX, thisY, hp, damage, name)
        {
        }
    }

    internal class Armor : Equipment
    {
        
        public Armor(int thisX, int thisY, int hp, int damage, string name) : base(thisX, thisY, hp, damage, name)
        {
        }
    }
}
