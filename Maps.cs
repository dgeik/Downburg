using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test4
{
    internal class Maps
    {
        Random rnd = new Random();
        public virtual int countOfEnemyInRegion { get; set; }
        public virtual int countOfItemsInRegion { get; set; }

        public virtual List<(int, int)> Positions { get; set; }
        List<(int, int)> notUsedPositions = new List<(int, int)> { };
        public virtual List<Enemy> Enemies { get; set; }
        public List<Enemy> enemyInRegion = new List<Enemy> { };
        public virtual List<Items> Items { get; set; }
        public List<Items> itemsInRegion = new List<Items> { };
        public virtual List<string> RegionAllMaps { get; set; }
        List<string> region = new List<string> { };

        public void placePosotions()
        {
            notUsedPositions.Clear();
            for (int i = 0; i < Positions.Count; i++)
            {
                notUsedPositions.Add(Positions[i]);
            }
        }
        public void choosePositions(int i, string type)
        {
            try
            {
                if (type == "Entity")
                {
                    (int, int) curentPosition = notUsedPositions[rnd.Next(0, notUsedPositions.Count)];
                    enemyInRegion[i].thisX = curentPosition.Item1;
                    enemyInRegion[i].thisY = curentPosition.Item2;
                    notUsedPositions.Remove(curentPosition);
                }

                if (type == "Items")
                {
                    (int, int) curentPosition = notUsedPositions[rnd.Next(0, notUsedPositions.Count)];
                    itemsInRegion[i].thisX = curentPosition.Item1;
                    itemsInRegion[i].thisY = curentPosition.Item2;
                    notUsedPositions.Remove(curentPosition);
                }
            }
            catch (Exception)
            {

                ;
            }
            
        }
        public void placeEnemiesInRegion() 
        {
            enemyInRegion.Clear();
            for (int i = 0; i < Enemies.Count; i++)
            {
                enemyInRegion.Add(Enemies[i]);
            }
        }
        public Enemy chooseOfEnemy()
        {
            try
            {
                Enemy curentEnemy = enemyInRegion[rnd.Next(0, enemyInRegion.Count)];
                enemyInRegion.Remove(curentEnemy);
                return curentEnemy;
            }
            catch (Exception)
            {

                return new Enemy(0, 0, 0, 0);
            }
        }
        public void placeItemsInRegion()
        {
            itemsInRegion.Clear();
            for (int i = 0; i < Items.Count; i++)
            {
                itemsInRegion.Add(Items[i]);
            }
        }
        public Items chooseOfItems(char[,] Map)
        {
            try
            {
                int i = rnd.Next(0, itemsInRegion.Count - 1);
                Items curentItem = itemsInRegion[i];
                Map[itemsInRegion[i].thisX, itemsInRegion[i].thisY] = itemsInRegion[i].visibleCharOfItem;
                itemsInRegion.Remove(itemsInRegion[i]);
                return curentItem;
            }
            catch (Exception)
            {

                return new Items(0, 0, 0, 0, "");
            }
        }
        public void placeMaps()
        {
            region.Clear();
            for (int i = 0; i < RegionAllMaps.Count; i++)
            {
                region.Add("maps/" + RegionAllMaps[i]);
            }
        }
        public string chooseOfMap()
        {
            string curentMap = region[rnd.Next(0, region.Count)];
            region.Remove(curentMap);
            return curentMap;
        }

    }

    internal class RegionOfMaps_1 : Maps
    {
        Random rnd = new Random();
        private int countOfEnemy = 4;
        private int countOfItems = 4;
        public override int countOfEnemyInRegion
        {
            get { return countOfEnemy; }
            set { countOfEnemy = value; }
        }

        public override int countOfItemsInRegion
        {
            get { return countOfItems; }
            set { countOfItems = value; }
        }

        List<(int, int)> positions = new List<(int, int)> { (40, 7), (26, 7), (16, 15), (30, 14), (10, 6), (9, 7), (21, 16), (39, 23), (40, 19), (9, 5),
            (22, 5), (44, 10), (57, 5), (6, 6), (44, 6)};

        List<Enemy> enemies = new List<Enemy> { new Slime(0, 0, 5, 1), new Goblin(0, 0, 7, 2), new Slime(0, 0, 5, 1), new Slime(0, 0, 5, 1), new Slime(0, 0, 5, 2) };
        List<Items> items = new List<Items> { new Consumable(0, 0,2,0,"Potin"), new Consumable(0, 0, 3, 0, "Potin"), new Consumable(0, 0, 3, -1, "Potin"), new Armor(0, 0, 5, 0, "Plate Hp:5 Dmg:0"),
        new Weapon(0, 0, 0, 2, "Sword Hp:0 Dmg:2"), new Armor(0, 0, 2, 0, "Plate Hp:2 Dmg:0"), new Weapon(0, 0, 0, 4, "Halberd Hp:0 Dmg:4"), new Weapon(0, 0, 0, 4, "Halberd Hp:0 Dmg:4"), new Weapon(0, 0, 0, 2, "Sword Hp:0 Dmg:2"), new Weapon(0, 0, 2, 5, "Sword of Frozen Heart Hp:2 Dmg:5")};
        List<string> regionAllMaps = new List<string> { "map.txt", "map1.txt", "map2.txt" };

        public override List<(int, int)> Positions
        {
            get { return positions; }
            set { positions = value; }
        }

        public override List<Enemy> Enemies
        {
            get { return enemies; }
            set { enemies = value; }
        }

        public override List<Items> Items
        {
            get { return items; }
            set { items = value; }
        }

        public override List<string> RegionAllMaps
        {
            get { return regionAllMaps; }
            set { regionAllMaps = value; }
        }
    }

    internal class RegionOfMaps_2 : Maps
    {
        Random rnd = new Random();
        private int countOfEnemy = 4;
        private int countOfItems = 4;
        public override int countOfEnemyInRegion
        {
            get { return countOfEnemy; }
            set { countOfEnemy = value; }
        }
        public override int countOfItemsInRegion
        {
            get { return countOfItems; }
            set { countOfItems = value; }
        }

        List<(int, int)> positions = new List<(int, int)> { (18, 5), (33, 16), (14, 15), (20, 15),
            (22, 22), (9, 17), (9,15), (15, 24), (10, 6), (38, 16), (30, 7), (10, 6), (32, 16), (38, 11), (6, 6), (51, 13)};

        List<Enemy> enemies = new List<Enemy> {new Goblin(0, 0, 7, 2), new Slime(0, 0, 8, 2), new Goblin(0, 0, 7, 3), new Goblin(0, 0, 7, 2), new Slime(0, 0, 5, 1), new Slime(0, 0, 5, 1) };
        List<Items> items = new List<Items> { new Consumable(0, 0,3,0,"Potin"), new Consumable(0, 0, 3, 2, "Potin"), new Consumable(0, 0, 5, 0, "Potin"), new Consumable(0, 0, 5, 0, "Potin"),
        new Weapon(0, 0, 0, 4, "Axe Hp:0 Dmg:4"), new Armor(0, 0, 4, 2, "Spike plate Hp:4 Dmg:2"), new Weapon(0, 0, 0, 4, "Halberd Hp:0 Dmg:4"), new Weapon(0, 0, 0, 4, "Halberd Hp:0 Dmg:4"), new Armor(0, 0, 4, 0, "Chain plate Hp:4 Dmg:0"), new Weapon(0, 0, 2, 5, "Sword of Frozen Heart Hp:2 Dmg:5")};
        List<string> regionAllMaps = new List<string> { "map3.txt", "map4.txt", "map5.txt" };
        public override List<(int, int)> Positions
        {
            get { return positions; }
            set { positions = value; }
        }

        public override List<Enemy> Enemies
        {
            get { return enemies; }
            set { enemies = value; }
        }
        public override List<Items> Items
        {
            get { return items; }
            set { items = value; }
        }

        public override List<string> RegionAllMaps
        {
            get { return regionAllMaps; }
            set { regionAllMaps = value; }
        }
    }

    internal class RegionOfMaps_3 : Maps
    {
        Random rnd = new Random();
        private int countOfEnemy = 1;
        public override int countOfEnemyInRegion
        {
            get { return countOfEnemy; }
            set { countOfEnemy = value; }
        }

        List<(int, int)> positions = new List<(int, int)> { (18, 10) };

        List <Enemy> enemies = new List<Enemy> {new Boss(0, 0, 13, 5)};
        List<Items> items = new List<Items> { };
        List<string> regionAllMaps = new List<string> { "map7.txt" };
        public override List<(int, int)> Positions
        {
            get { return positions; }
            set { positions = value; }
        }
        public override List<Enemy> Enemies
        {
            get { return enemies; }
            set { enemies = value; }
        }
        public override List<Items> Items
        {
            get { return items; }
            set { items = value; }
        }
        public override List<string> RegionAllMaps
        {
            get { return regionAllMaps; }
            set { regionAllMaps = value; }
        }
    }
}
