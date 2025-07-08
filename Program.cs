using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test4
{
    internal class Program
    {
        static char[,] create_map(string nameMap)
        {
            string[] map = File.ReadAllLines(nameMap);

            char[,] Map = new char[map[0].Length, map.Length];

            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    Map[i, j] = map[j][i];
                }
            }
            return Map;
        }

        static void print(char[,] Map)
        {
            string scene = "";
            for (int i = 0; i < Map.GetLength(1); i++)
            {
                for (int j = 0; j < Map.GetLength(0); j++)
                {
                    scene += Map[j, i];
                }
                scene += '\n';
            }
            Console.ForegroundColor = ConsoleColor.DarkGray;
            scene = scene.Replace(' ', '∙'/*249*/);
            Console.WriteLine(scene);
            Console.ResetColor();
        }

        static void isWall(char[,] Map, List<Entity> entities, int i)
        {
            if (Map[entities[i].thisX, entities[i].thisY] == '█')
            {
                entities[i].thisX = entities[i].backX;
                entities[i].thisY = entities[i].backY;
            }
        }

        static void addEntitiesToList(ref List<Entity> entities, List<Maps> maps, double numberOfCurentRegion, Hero hero)
        {
            entities.Clear();

            for (int i = 0; i < maps[(int)numberOfCurentRegion].enemyInRegion.Count; i++)
            {
                maps[(int)numberOfCurentRegion].choosePositions(i, "Entity");
            }

            for (int k = 0; k < maps[(int)numberOfCurentRegion].countOfEnemyInRegion; k++)
            {
                entities.Add(maps[(int)numberOfCurentRegion].chooseOfEnemy());
            }
            entities.Add(hero);
        }

        static void addItemsToList(List<Items> itemsInregion, List<Maps> maps, double numberOfCurentRegion, char[,]Map)
        {
            itemsInregion.Clear();

            for (int i = 0; i < maps[(int)numberOfCurentRegion].itemsInRegion.Count; i++)
            {
                maps[(int)numberOfCurentRegion].choosePositions(i, "Items");
            }

            for (int k = 0; k < maps[(int)numberOfCurentRegion].countOfItemsInRegion; k++)
            {
                itemsInregion.Add(maps[(int)numberOfCurentRegion].chooseOfItems(Map));
            }
        }

        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            Random rnd = new Random();
            List<ConsoleKey> directonAi = new List<ConsoleKey>() { ConsoleKey.W, ConsoleKey.S, ConsoleKey.A, ConsoleKey.D };
            Console.SetWindowSize(80,30);
            Console.CursorSize = 24;
            
            
            List<Maps> maps = new List<Maps>() {new RegionOfMaps_1(), new RegionOfMaps_2(), new RegionOfMaps_3()};

            while (true)
            {
                Console.Clear();
                Console.SetCursorPosition(36,9);
                Console.WriteLine("Downburg");
                Console.SetCursorPosition(35, 11);
                Console.WriteLine("WASD - Move");
                Console.SetCursorPosition(35, 12);
                Console.WriteLine("Z - Pick up");
                Console.SetCursorPosition(34, 13);
                Console.WriteLine("I - Inventory");
                Console.SetCursorPosition(18, 14);
                Console.Write("Number of item + Enter - Choose in inventory");
                Console.SetCursorPosition(30, 15);
                Console.Write("F - On 'F' next floor");
                Console.SetCursorPosition(26, 16);
                Console.Write("Else buttons - Nothing/cancel");
                Console.ReadKey();
                Console.SetCursorPosition(0,0);

                int thisX = 4;
                int thisY = 5;
                int countOfRegion = maps.Count();
                double numberOfCurentRegion = 0;
                bool isNear;
                bool win = false;
                bool lose = false;
                ConsoleKeyInfo wordHero = new ConsoleKeyInfo((char)ConsoleKey.F, ConsoleKey.F, false, false, false);

                for (int i = 0; i < countOfRegion; i++)
                {
                    maps[i].placeMaps();
                }

                string curentMap = maps[(int)numberOfCurentRegion].chooseOfMap();
                char[,] Map = create_map(curentMap);
                char[,] Map2 = new char[Map.GetLength(0), Map.GetLength(1)];

                maps[(int)numberOfCurentRegion].placePosotions();

                Hero Hero = new Hero(thisX, thisY, 25, 2);
                List<Entity> entities = new List<Entity>() { };
                maps[(int)numberOfCurentRegion].placeEnemiesInRegion();
                addEntitiesToList(ref entities, maps, numberOfCurentRegion, Hero);

                List<Items> inventory = new List<Items> { };
                List<Items> itemsInregion = new List<Items>();
                maps[(int)numberOfCurentRegion].placeItemsInRegion();
                addItemsToList(itemsInregion, maps, numberOfCurentRegion, Map);



                while (win == false && lose == false)
                {
                    {
                        for (int i = Hero.thisX - 4; i <= Hero.thisX + 4; i++)
                        {
                            for (int j = Hero.thisY - 4; j <= Hero.thisY + 4; j++)
                            {
                                Map2[i, j] = Map[i, j];
                            }
                        }
                    }//исследование карты

                    Console.SetCursorPosition(0, 0);//say no more to Epileptic
                    print(Map2);

                    for (int i = 0; i < entities.Count; i++)
                    {
                        isNear = false;
                        if (entities[i] != Hero && entities[i].hp > 0)
                        {
                            entities[i].backY = entities[i].thisY;
                            entities[i].backX = entities[i].thisX;

                            for (int q = entities[i].thisX - 6; q <= entities[i].thisX + 6; q++)
                            {
                                for (int j = entities[i].thisY - 6; j <= entities[i].thisY + 6; j++)
                                {
                                    if (q == Hero.thisX && j == Hero.thisY)
                                    {
                                        isNear = true;

                                        entities[i].Folow(entities, Hero, i);
                                        break;
                                    }
                                }
                            }//следование

                            if (isNear == false)
                            {
                                var word = directonAi[rnd.Next(0, directonAi.Count)];

                                entities[i].MoveAi(word);
                            }

                            isWall(Map, entities, i);

                            for (int k = 0; k < entities.Count - 1; k++)
                            {
                                if (entities[i] == entities[k])
                                {
                                    continue;
                                }
                                else
                                {
                                    if (entities[i].thisX == entities[k].thisX && entities[i].thisY == entities[k].thisY)
                                    {
                                        entities[i].thisX = entities[i].backX;
                                        entities[i].thisY = entities[i].backY;
                                    }
                                }
                            }//чтоб не были в 1 блоке

                            if (entities[i].thisX == Hero.thisX && entities[i].thisY == Hero.thisY)
                            {
                                Hero.hp = Hero.hp - entities[i].damage;
                                entities[i].thisX = entities[i].backX;
                                entities[i].thisY = entities[i].backY;
                            }//боёвка


                            for (int k = Hero.thisX - 5; k <= Hero.thisX + 5; k++)
                            {
                                for (int j = Hero.thisY - 5; j <= Hero.thisY + 5; j++)
                                {
                                    if (entities[i].thisX == k & entities[i].thisY == j)
                                    {
                                        Console.SetCursorPosition(entities[i].thisX, entities[i].thisY);
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.Write(entities[i].visibleCharOfEntity);
                                        Console.ResetColor();
                                    }
                                }
                            }//видимые только в 11х11 клеток

                        }

                        if (entities[i] == Hero && entities[i].hp > 0)
                        {
                            Console.SetCursorPosition(entities[i].thisX, entities[i].thisY);
                            Console.Write(entities[i].visibleCharOfEntity);
                            Console.SetCursorPosition(0, Map.GetLength(1) - 3);
                            Console.Write($"Hp:{entities[i].hp} Dmg:{entities[i].damage}");

                            for (int s = 0; s < itemsInregion.Count; s++)
                            {
                                if ((entities[i].thisX == itemsInregion[s].thisX) && (entities[i].thisY == itemsInregion[s].thisY))
                                {
                                    Console.SetCursorPosition(0, Map.GetLength(1) - 2);
                                    Console.Write(itemsInregion[s].name + '\n');
                                    break;
                                }
                            }

                            Console.Write(' ');
                            wordHero = Console.ReadKey();

                            entities[i].backY = entities[i].thisY;
                            entities[i].backX = entities[i].thisX;

                            Hero.Move(wordHero, Map, inventory, itemsInregion);

                            isWall(Map, entities, i);

                            for (int k = 0; k < entities.Count - 1; k++)
                            {
                                if (Hero.thisX == entities[k].thisX && Hero.thisY == entities[k].thisY)
                                {
                                    entities[k].hp = entities[k].hp - Hero.damage;
                                    if (entities[k].hp <= 0)
                                    {
                                        entities[k].thisX = 0;
                                        entities[k].thisY = 0;
                                    }
                                    entities[i].thisX = entities[i].backX;
                                    entities[i].thisY = entities[i].backY;
                                }
                            }//боёвка

                            if (numberOfCurentRegion == 2)
                            {
                                if (entities[i - 1].hp <= 0)
                                {
                                    win = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (Hero.hp <= 0)
                    {
                        lose = true;
                        break;
                    }

                    if ((wordHero.Key == ConsoleKey.F) && (Map[Hero.thisX, Hero.thisY] == 'F'))
                    {
                        entities.Clear();

                        numberOfCurentRegion = numberOfCurentRegion + 0.5;
                        curentMap = maps[(int)numberOfCurentRegion].chooseOfMap();
                        Map = create_map(curentMap);
                        Map2 = new char[Map.GetLength(0), Map.GetLength(1)];

                        maps[(int)numberOfCurentRegion].placePosotions();

                        maps[(int)numberOfCurentRegion].placeEnemiesInRegion();
                        addEntitiesToList(ref entities, maps, numberOfCurentRegion, Hero);

                        maps[(int)numberOfCurentRegion].placeItemsInRegion();
                        addItemsToList(itemsInregion, maps, numberOfCurentRegion, Map);
                        Hero.thisX = 4;
                        Hero.thisY = 5;
                        Console.Clear();
                    }//этаж
                    //Console.Clear();//say no more to Epileptic
                }

                if (win == true)
                {
                    Console.Clear();
                    Console.SetCursorPosition(26, 9);
                    Console.WriteLine("You found amulet of Majesty");
                    Console.SetCursorPosition(27, 10);
                    Console.WriteLine("and escaped from Downburg");
                    Console.ReadKey();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("You died in Downburg");
                    Console.ReadKey();
                }
            }
        }
    }   
}
