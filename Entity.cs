using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test4
{
    internal class Entity
    {
        public int thisX;
        public int thisY;
        public int backX;
        public int backY;
        public int hp;
        public int damage;
        public virtual char visibleCharOfEntity { get; set; } 

        public Entity(int thisX, int thisY, int hp, int damage)
        {
            this.thisX = thisX;
            this.thisY = thisY;
            this.backX = thisX;
            this.backY = thisY;
            this.hp = hp;
            this.damage = damage;
        }

        public virtual void MoveAi(ConsoleKey word) { }

        public virtual void Folow(List<Entity> entities, Hero hero, int i) { }
    }

    internal class Hero : Entity
    {
        private char charOfEntity = 'H';
        public override char visibleCharOfEntity
        {
            get { return charOfEntity; }
            set { charOfEntity = value; }
        }

        Weapon curentWeapon;
        Armor curentArmor;
        public Hero(int thisX, int thisY, int hp, int damage) : base(thisX, thisY, hp, damage)
        {
            curentWeapon = null;
            curentArmor = null;
        }

        private static void use(List<Items> inventory, int i, ref int hp, ref int damage)
        {
            try
            {
                if (inventory[i].GetType() == typeof(Consumable))
                {
                    hp += inventory[i].hp;
                    damage += inventory[i].damage;
                    inventory.Remove(inventory[i]);
                }
            }
            catch (Exception)
            {

                Console.Clear();
            }
        }

        private static void equipOnHero(List<Items> inventory, int i, ref int hp, ref int damage, ref Weapon curentWeapon, ref Armor curentArmor)
        {
            try
            {
                if (inventory[i].GetType() == typeof(Weapon))
                {
                    if (curentWeapon != null)
                    {
                        inventory.Add(curentWeapon);
                        damage -= curentWeapon.damage;
                        hp -= curentWeapon.hp;
                    }
                    curentWeapon = (Weapon)inventory[i];
                    damage += curentWeapon.damage;
                    hp += curentWeapon.hp;
                    inventory.Remove(inventory[i]);
                }

                if (inventory[i].GetType() == typeof(Armor))
                {
                    if (curentArmor != null)
                    {
                        inventory.Add(curentArmor);
                        damage -= curentArmor.damage;
                        hp -= curentArmor.hp;
                    }
                    curentArmor = (Armor)inventory[i];
                    damage += curentArmor.damage;
                    hp += curentArmor.hp;
                    inventory.Remove(inventory[i]);
                }
            }
            catch (Exception)
            {

                Console.Clear();
            }
            
        }

        public void Move(ConsoleKeyInfo word, char[,] Map, List<Items> inventory, List<Items> itemsInRegion)
        {
            
            switch (word.Key)
            {
                case ConsoleKey.W:
                    thisY--;
                    break;
                case ConsoleKey.S:
                    thisY++;
                    break;
                case ConsoleKey.A:
                    thisX--;
                    break;
                case ConsoleKey.D:
                    thisX++;
                    break;
                case ConsoleKey.Z:
                    for (int s = 0; s < itemsInRegion.Count; s++)
                    {
                        if ((thisX == itemsInRegion[s].thisX) && (thisY == itemsInRegion[s].thisY))
                        {
                            inventory.Add(itemsInRegion[s]);
                            itemsInRegion.Remove(itemsInRegion[s]);
                            Map[thisX, thisY] = ' ';
                            break;
                        }
                    }
                    break;
                case ConsoleKey.I:
                    Console.SetCursorPosition(0, Map.GetLength(1) - 2);
                    string inventoryLine = "";
                    for (int i = 0; i < inventory.Count; i++)
                    {
                        inventoryLine += (i + " " + inventory[i].name + " " + inventory[i].Equip + "\n");
                    }
                    
                    try
                    {
                        Console.Write("{0}\n", inventoryLine);
                        int n = Convert.ToInt32(Console.ReadLine());
                        if (inventory[n].Equip == "Consume")
                        {
                            use(inventory, n, ref hp, ref damage);
                        }
                        else
                        {
                            equipOnHero(inventory, n, ref hp, ref damage, ref curentWeapon, ref curentArmor);
                        }
                        
                    }
                    catch (Exception)
                    {

                        Console.Clear();
                    }
                    break;
            }
        }
    }

    internal class Enemy : Entity
    {
        private char charOfEntity = ' ';
        public override char visibleCharOfEntity
        {
            get { return charOfEntity; }
            set { charOfEntity = value; }
        }

        public Enemy(int thisX, int thisY, int hp, int damage) : base(thisX, thisY, hp, damage)
        {
        }

        public override void MoveAi(ConsoleKey word)
        {
            switch (word)
            {
                case ConsoleKey.W:
                    thisY--;
                    break;
                case ConsoleKey.S:
                    thisY++;
                    break;
                case ConsoleKey.A:
                    thisX--;
                    break;
                case ConsoleKey.D:
                    thisX++;
                    break;
            }
        }

        public virtual int someRandomAiMoves(List<Entity> entities, int i, char direct)
        {
            Random rnd = new Random();
            if (direct == 'X')
            {
                switch (rnd.Next(0, 4))
                {
                    case 0:
                        return entities[i].thisX--;
                    case 1:
                        return entities[i].thisX++;
                    default:
                        return entities[i].thisX;
                }
            }
            else
            {
                switch (rnd.Next(0, 4))
                {
                    case 0:
                        return entities[i].thisY--;
                    case 1:
                        return entities[i].thisY++;
                    default:
                        return entities[i].thisY;
                }
            }
        }

        public override void Folow(List<Entity> entities, Hero hero, int i)
        {
            char direct2 = 'Y';
            char direct1 = 'X';
            if (thisY > hero.thisY)
            {
                thisY--;
                someRandomAiMoves(entities, i, direct1);

            }
            else if (thisY < hero.thisY)
            {
                thisY++;
                someRandomAiMoves(entities, i, direct1);
            }
            else
            {
                if (thisX > hero.thisX)
                {
                    thisX--;
                    someRandomAiMoves(entities, i, direct2);
                }
                if (thisX < hero.thisX)
                {
                    thisX++;
                    someRandomAiMoves(entities, i, direct2);
                }
            }
        }
    }

    internal class Goblin : Enemy
    {
        public Goblin(int thisX, int thisY, int hp, int damage) : base(thisX, thisY, hp, damage)
        {
        }

        private char charOfEntity = 'G';
        public override char visibleCharOfEntity
        {
            get { return charOfEntity; }
            set { charOfEntity = value; }
        }
    }

    internal class Slime : Enemy
    {
        public Slime(int thisX, int thisY, int hp, int damage) : base(thisX, thisY, hp, damage)
        {
        }

        private char charOfEntity = 'S';
        public override char visibleCharOfEntity
        {
            get { return charOfEntity; }
            set { charOfEntity = value; }
        }
    }

    internal class Boss : Enemy
    {
        public Boss(int thisX, int thisY, int hp, int damage) : base(thisX, thisY, hp, damage)
        {
        }
        
        private char charOfEntity = 'B';
        public override char visibleCharOfEntity
        {
            get { return charOfEntity; }
            set { charOfEntity = value; }
        }

        public override int someRandomAiMoves(List<Entity> entities, int i, char direct)
        {
            Random rnd = new Random();
            if (direct == 'X')
            {
                switch (rnd.Next(0, 6))
                {
                    case 0:
                        return entities[i].thisX--;
                    case 1:
                        return entities[i].thisX++;
                    default:
                        return entities[i].thisX;
                }
            }
            else
            {
                switch (rnd.Next(0, 6))
                {
                    case 0:
                        return entities[i].thisY--;
                    case 1:
                        return entities[i].thisY++;
                    default:
                        return entities[i].thisY;
                }
            }
        }


    }
}
