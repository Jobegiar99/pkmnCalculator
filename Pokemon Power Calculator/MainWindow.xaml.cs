using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pokemon_Power_Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool criticalHasBeenSelected = false,isBurnHasBeenSelected=false,attackTypeHasBeenSelected=false;
        string criticalAnswer="no", burnAnswer="no",attackType="physical";

        public MainWindow()
        {
            InitializeComponent();
            button_YesBurned.Background = Brushes.LightSlateGray;
            button_NoBurned.Background = Brushes.LightSlateGray;
            button_Critical.Background = Brushes.LightSlateGray;
            button_NoCritical.Background = Brushes.LightSlateGray;
            button_PhysicalAttack.Background = Brushes.LightSlateGray;
            button_SpecialAttack.Background = Brushes.LightSlateGray;
            button_Results.Background = Brushes.LightSlateGray;
            button_Reset.Background = Brushes.LightSlateGray;
        }

        private void Button_Results_Click(object sender, RoutedEventArgs e)
        {
            double levelData = 0d, attackDefense = 0d, power = 0d, modifier = 0d;
            float targets=0f, weather = 0f, critical = 0f, randomMin=0.85f,randomMax=1, stab = 0f, type = 0f, burn = 0f, other = 1f;
            Random randomGenerator = new Random();
            int level_attackdefense_power = 0, /* ((level data * attackDenfese*power) /50)+2*/ damage;
            string minValue, maxValue;

            if (string.IsNullOrEmpty(combo_attackElement.Text))
            {
                MessageBox.Show("Choose an attack element.", "Damage Calculator", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            } //Checks if any combo box is null or if a button has not been clicked.
            else if (string.IsNullOrEmpty(combo_PlayerPkmnElement1.Text) && string.IsNullOrEmpty(combo_PlayerPkmnElement2.Text))
            {
                MessageBox.Show("Choose at least one element for your pokemon.", "Damage Calculator", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (string.IsNullOrEmpty(combo_FoePkmnElement1.Text) && string.IsNullOrEmpty(combo_FoePkmnElement2.Text))
            {
                MessageBox.Show("Choose at least one element for your foe's pokemon.", "Damage Calculator", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (string.IsNullOrEmpty(combo_PowerBoost.Text))
            {
                MessageBox.Show("Choose an option for power boost. If there is no boost, choose None.", "Damage Calculator", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (string.IsNullOrEmpty(combo_Weather.Text))
            {
                MessageBox.Show("Choose and option for the weather. Select \"Clear Skies\" if there is no weather condition","Damage Calculator",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            else if (criticalHasBeenSelected == false)
            {
                MessageBox.Show("Please click Yes or No for critical hit.", "Damage Calculator", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (isBurnHasBeenSelected == false)
            {
                MessageBox.Show("Please click Yes or No for \"Is your Pokemon Burned?\"","Damage Calculator",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            else if(attackTypeHasBeenSelected==false)
            {
                MessageBox.Show("Please click Physical if your attack is a physical attack or Special if it's a Special Attack.", "Damage Calculator", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                levelData = double.Parse(txt_pkmnLevel.Text);
                levelData=NegativeToPositive(levelData);
                levelData = (levelData * 2) / 5 + 2;   
            }
            catch (Exception)
            {
                MessageBox.Show("Write a numerical value for pokemon level.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            } //Calculate levelData (with pokemon level)

            try
            {
                try
                {
                    attackDefense = double.Parse(txt_pkmnAttack.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Write a numerical value for your pokemon attack.","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                    return;
                }

                attackDefense /= double.Parse(txt_pkmnDefense.Text);
                
            }
            catch (Exception)
            {
                MessageBox.Show("Write a numerical value for your foe's pokemon defense.","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            } // Calculate pokemon attack power divided by foe's pokemon defense.

            try
            {
                power =double.Parse(txt_attackPower.Text);
                NegativeToPositive(levelData);
                switch (combo_PowerBoost.SelectedIndex) //Calculates Power. Power= The effective power of the used move (Bulbapedia,2018).
                {
                    case 0: //No move
                        break;
                    case 1: //Helping Hand
                    case 3://Me first
                        power *= 1.5;
                        break;
                    case 2://Charge (electric attack move boost)
                        if (combo_attackElement.SelectedIndex==3)
                        {
                            power *= 2;
                        }
                        break;
                        //Terian Boosts
                    case 4: //Grassy Terrian. Grass-type moves 1.5*, BUlldoze,Magnitude and Earthquake 0.5*.
                        if (combo_attackElement.SelectedIndex == 4)
                        {

                            dynamic MsgResult = MessageBox.Show("Is the attack Bulldoze, Magnitude or Earthquake?", "Damage Calculator", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
                            if (MsgResult == System.Windows.MessageBoxResult.Yes)
                            {
                                power *= 0.5;
                            }
                        }
                        else if (combo_attackElement.SelectedIndex == 10)
                        {
                            power *= 1.5;

                        }
                        break;
                    case 5://Electric Terrian. Electric-type moves 1.5*.
                        if (combo_attackElement.SelectedIndex == 2)
                        {
                            power *= 1.5;
                        }
                        break;
                    case 6://Psychic Terrian. Psychic-type moves *1.5.
                        if (combo_attackElement.SelectedIndex == 5){
                            power *= 1.5;
                        }
                        break;
                    case 7: //Misty Terrian
                        if (combo_attackElement.SelectedIndex == 16)
                        {
                            power *= 0.5;
                        }
                        break;
                    case 8://Mud Sport
                        if (combo_attackElement.SelectedIndex == 2)
                        {
                            power *= .33;
                        }
                        break;
                    case 9: //Water Sport
                        if (combo_attackElement.SelectedIndex == 9)
                        {
                            power *= 0.33;
                        }
                        break;
                    
                }
                
            }  catch (Exception)
            {
                MessageBox.Show("Write a numerical value for power.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            } //Calculate power (with power attack and in some cases also with attack element).

            try
            {
                targets = float.Parse(txt_Targets.Text);
                if (targets>5 && targets!=1)
                {
                    MessageBox.Show("Write a numerical value from 1 to 5 for the amount of targets","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                    return;
                }else if (targets > 1 && targets<6)
                {
                    targets = 0.75f;
                }
                else if (targets < 1)
                {
                    MessageBox.Show("Write a numerical value from 1 to 5 for the amount of targets", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }else
                {
                    targets = 1;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Write a numerical value from 1 to 5 for the amount of targets", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            } //Calculates target value. It can be 0.75 or 1.


            /*Calculates Weather value. It can be 0.5,1 or 1.5. Click on + to see the code.*/
            { 
            if (combo_Weather.SelectedIndex == 1)//sunny day
                    {
                        if (combo_attackElement.SelectedIndex == 9)//If sunny day and fire element
                        {
                            weather = 1.5f;
                        }
                        else if (combo_attackElement.SelectedIndex == 1) //if sunny day and water element
                        {
                            weather = 0.5f;
                        }
                        else //anything else
                        {
                            weather = 1;
                        }
                    }
                else if (combo_Weather.SelectedIndex == 2)//rain dance
                    {
                        if (combo_attackElement.SelectedIndex == 1)//if rain dance and water element
                        {
                            weather = 1.5f;
                        }
                        else if (combo_attackElement.SelectedIndex == 9) //if rain dance and fire element
                        {
                            weather = 0.5f;
                        }
                        else //anythign else
                        {
                            weather = 1;
                        }
                    }
                else if (combo_Weather.SelectedIndex==3)//If sandstorm.
                {
                    weather = 1;
                    power = IsSolarBeamBlade(power); //

                }else if (combo_Weather.SelectedIndex == 4)//If hail.
                {
                    weather = 1;
                    power = IsSolarBeamBlade(power);
                }
                else
                {
                    weather = 1;
                }
                   
            }

            critical = (criticalAnswer == "yes") ? 1.5f : 1; //Calculates critical Value. If it's a critical hit, it's 1.5, if not it's 1;

            /*Calculates STAB (same type attack boost) It is 1.5 if attack element matches any of the player's pokemon type,else it is 1. Click on + to see the code.*/
            {
                if (combo_attackElement.Text.Equals(combo_PlayerPkmnElement1.Text) || combo_attackElement.Text.Equals(combo_PlayerPkmnElement2.Text))
                {
                    stab = 1.5f;
                }
                else
                {
                    stab = 1;
                }
            }

            /*Calculates type bonus based on foe's pokemon type and attack element type*/
            {
                ElementDamageTable();
            }

            type = ElementDamageTable();
            burn = BurnCalculation();
            other = OtherCalculation(type);
            level_attackdefense_power = (int)((levelData * attackDefense * power) / 50) + 2;
            modifier = targets * weather * critical * randomMin * stab * type * burn * other;
            damage =(int) (level_attackdefense_power * modifier);
            minValue = "Minimum damage your pokemon can inflict to enemy is: " + damage;
            modifier = targets * weather * critical * randomMax * stab * type * burn * other;
            damage = (int)(level_attackdefense_power * modifier);
            maxValue = "\nMaximum damage your pokemon can inflict to enemy is: " + damage;
            MessageBox.Show(minValue+maxValue, "Damage Calculator", MessageBoxButton.OK);


        }

        private double NegativeToPositive(double value)
        {
            if (value < 0)
            {
                value *= -1;
            }
            return value;
        } //Checks if user input is negative and makes it a positive number.


        private double IsSolarBeamBlade(double value) //Checks if the attack element is a grass-type attack. If it is and the conditions are met it asks if it is a specific attack.
        {
            if (combo_attackElement.SelectedIndex == 10)
            {
                dynamic myResult = MessageBox.Show("Is the attack Solar Beam or Solar Blade?", "Damage Calculator", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (myResult == System.Windows.MessageBoxResult.Yes)
                {
                    value /= 2;
                    return value;
                }
                else
                {
                    return value;
                }
            }
            else
            {
                return value;
            }
        }

        private void Button_YesBurned_Click(object sender, RoutedEventArgs e)
        {
            isBurnHasBeenSelected = true;
            burnAnswer = "yes";
            button_YesBurned.Background = Brushes.ForestGreen;
            button_NoBurned.Background = Brushes.LightSlateGray;
        } //is called when is burned Yes button is Clicked.

        private void Button_NoBurned_Click(object sender, RoutedEventArgs e)
        {
            isBurnHasBeenSelected = true;
            burnAnswer = "no";
            button_YesBurned.Background = Brushes.LightSlateGray;
            button_NoBurned.Background = Brushes.IndianRed;

        } //Is called when is burned No button is Clicked.

        private void Button_Critical_Click(object sender, RoutedEventArgs e)
        {
            criticalHasBeenSelected = true;
            criticalAnswer = "yes";
            button_Critical.Background = Brushes.ForestGreen;
            button_NoCritical.Background = Brushes.LightSlateGray;
        } //Is called when is critical hit Yes button is Clicked.

        private void Button_PhysicalAttack_Click(object sender, RoutedEventArgs e)
        {
            attackTypeHasBeenSelected = true;
            attackType = "physical";
            button_PhysicalAttack.Background = Brushes.IndianRed;
            button_SpecialAttack.Background = Brushes.LightSlateGray;
        }

        private void Button_Reset_Click(object sender, RoutedEventArgs e)
        {
            txt_attackPower.Text = null;
            txt_pkmnAttack.Text = null;
            txt_pkmnDefense.Text = null;
            txt_pkmnLevel.Text = null;
            txt_Targets.Text = null;
            isBurnHasBeenSelected = false;
            criticalHasBeenSelected = false;
            attackTypeHasBeenSelected = false;
            combo_Abilities.Text = null;
            combo_attackElement.Text = null;
            combo_FoePkmnElement1.Text = null;
            combo_FoePkmnElement2.Text = null;
            combo_PlayerPkmnElement1.Text = null;
            combo_PlayerPkmnElement2.Text = null;
            combo_PowerBoost.Text = null;
            combo_Weather.Text = null;
            button_Critical.Background = Brushes.LightSlateGray;
            button_NoBurned.Background = Brushes.LightSlateGray;
            button_NoCritical.Background = Brushes.LightSlateGray;
            button_PhysicalAttack.Background = Brushes.LightSlateGray;
            button_SpecialAttack.Background = Brushes.LightSlateGray;
            button_YesBurned.Background = Brushes.LightSlateGray;
         

        }

        private void Button_SpecialAttack_Click(object sender, RoutedEventArgs e)
        {
            attackTypeHasBeenSelected = true;
            attackType = "special";
            button_PhysicalAttack.Background = Brushes.LightSlateGray;
            button_SpecialAttack.Background = Brushes.ForestGreen;

        }

        private void Button_NoCritical_Click(object sender, RoutedEventArgs e)
        {
            criticalHasBeenSelected = true;
            criticalAnswer = "no";
            button_Critical.Background = Brushes.LightSlateGray;
            button_NoCritical.Background = Brushes.IndianRed;
        } //Is called when is critical hit No button is Clicked.

        private float ElementDamageTable()// Calculates Element Damage based on Element Strengths and Weaknesses.
        {
            string element1, element2, attackE;
            byte positionX1 = 0, positionX2 = 0, positionY = 0;
            float total;
            string[,] defendingType =
            {        //                      D       E          F      E       N       D        I       N       G                T          Y       P       E
                //______________________________________________________________________________________________________________________________________________________________vertical length=19. horizontal length=19
                {null,     "Normal","Fighting","Flying","Poison","Ground","Rock" , "Bug" ,"Ghost","Steel","Fire" ,"Water","Grass","Electric","Psychic","Ice","Dragon","Dark" ,"Fairy"},
                {"Normal"  ,  "1"  ,  "1"  ,   "1"  ,  "1"   ,   "1"  , "0.5" ,  "1"  ,  "0"  , "0.5" ,  "1"  ,  "1"  ,  "1"  ,    "1"   ,   "1"   , "1" ,   "1"  ,  "1"  ,  "1"  },
             {"Fighting"   ,  "2"  ,  "1"  ,  "0.5" , "0.5"  ,   "1"  ,  "2"  , "0.5" ,  "0"  ,  "2"  ,  "1"  ,  "1"  ,  "1"  ,    "1"   ,  "0.5"  , "2" ,   "1"  ,  "2"  , "0.5" },//A
                {"Flying"  ,  "1"  ,  "2"  ,   "1"  ,  "1"   ,   "1"  , "0.5" ,  "2"  ,  "1"  , "0.5" ,  "1"  ,  "1"  ,  "2"  ,   "0.5"  ,   "1"   , "1" ,   "1"  ,  "1"  ,  "1"  },//T
                {"Poison"  ,  "1"  ,  "1"  ,   "1"  , "0.5"  ,  "0.5" , "0.5" ,  "1"  , "0.5" ,  "0"  ,  "1"  ,  "1"  ,  "2"  ,    "1"   ,   "1"   , "1" ,   "1"  ,  "1"  ,  "2"  },//T
                {"Ground"  ,  "1"  ,  "1"  ,   "0"  ,  "2"   ,   "1"  ,  "2"  , "0.5" ,  "1"  ,  "2"  ,  "2"  ,  "1"  , "0.5" ,    "2"   ,   "1"   , "1" ,   "1"  ,  "1"  ,  "1"  },//A
                {"Rock"    ,  "1"  , "0.5"  ,  "2"  ,  "1"   ,  "0.5"  , "1"  ,  "2"  ,  "1"  , "0.5" ,  "2"  ,  "1"  ,  "1"  ,    "1"   ,   "1"   , "2" ,   "1"  ,  "1"  ,  "1"  },//C
                {"Bug"     ,  "1"  , "0.5" ,  "0.5" , "0.5"  ,   "1"  ,  "1"  ,  "1"  , "0.5" , "0.5" , "0.5" ,  "1"  ,  "2"  ,    "1"   ,   "2"   , "1" ,   "1"  ,  "2"  , "0.5" },//K
                {"Ghost"   ,  "0"  ,  "1"  ,   "1"  ,  "1"   ,   "1"  ,  "1"  ,  "1"  ,  "2"  ,  "1"  ,  "1"  ,  "1"  ,  "1"  ,    "1"   ,   "2"   , "1" ,   "1"  , "0.5" ,  "1"  },//I
                {"Steel"   ,  "1"  ,  "1"  ,   "1"  ,  "1"   ,   "1"  ,  "2"  ,  "1"  ,  "1"  , "0.5" , "0.5" , "0.5" ,  "1"  ,   "0.5"  ,   "1"   , "2" ,   "1"  ,  "1"  ,  "2"  },//N
                {"Fire"    ,  "1"  ,  "1"  ,   "1"  ,  "1"   ,   "1"  , "0.5" ,  "2"  ,  "1"  ,  "2"  , "0.5" , "0.5" ,  "2"  ,    "1"   ,   "1"   , "2" ,  "0.5" ,  "1"  ,  "1"  },//G
                {"Water"   ,  "1"  ,  "1"  ,   "1"  ,  "1"   ,   "2"  ,  "2"  ,  "1"  ,  "1"  ,  "1"  ,  "2"  , "0.5" , "0.5" ,    "1"   ,   "1"   , "1" ,  "0.5" ,  "1"  ,  "1"  },
                {"Grass"   ,  "1"  ,  "1"  ,  "0.5" , "0.5"  ,   "2"  ,  "2"  , "0.5" ,  "1"  , "0.5" , "0.5" ,  "2"  , "0.5" ,    "1"   ,   "1"   , "1" ,  "0.5" ,  "1"  ,  "1"  },
              {"Electric"  ,  "1"  ,  "1"  ,   "2"  ,  "1"   ,   "0"  ,  "1"  ,  "1"  ,  "1"  ,  "1"  ,  "1"  ,  "2"  , "0.5" ,   "0.5"  ,   "1"   , "1" ,  "0.5" ,  "1"  ,  "1"  },//T
               {"Psychic"  ,  "1"  ,  "2"  ,   "1"  ,  "2"   ,   "1"  ,  "1"  ,  "1"  ,  "1"  , "0.5" ,  "1"  ,  "1"  ,  "1"  ,    "1"   ,  "0.5"  , "1" ,   "1"  ,  "0"  ,  "1"  },//Y
                {"Ice"     ,  "1"  ,  "1"  ,   "2"  ,  "1"   ,   "2"  ,  "1"  ,  "1"  ,  "1"  , "0.5" , "0.5" , "0.5" ,  "2"  ,    "1"   ,   "1"   ,"0.5",   "2"  ,  "1"  ,  "1"  },//P
                {"Dragon"  ,  "1"  ,  "1"  ,   "1"  ,  "1"   ,   "1"  ,  "1"  ,  "1"  ,  "1"  , "0.5" ,  "1"  ,  "1"  ,  "1"  ,    "1"   ,   "1"   , "1" ,   "2"  ,  "1"  ,  "0"  },//E
                {"Dark"    ,  "1"  , "0.5" ,   "1"  ,  "1"   ,   "1"  ,  "1"  ,  "1"  ,  "2"  ,  "1"  ,  "1"  ,  "1"  ,  "1"  ,    "1"   ,   "2"   , "1" ,   "1"  , "0.5" , "0.5" },
                {"Fairy"   ,  "1"  ,  "2"  ,   "1"  , "0.5"  ,   "1"  ,  "1"  ,  "1"  ,  "1"  , "0.5" , "0.5" ,  "1"  ,  "1"  ,    "1"   ,   "1"   , "1" ,   "2"  ,  "2"  ,  "1"  },
                //*-------------------------------------------------------------------------------------------------------------------------------------------------------------
            };
            attackE = combo_attackElement.Text;
            for (byte i = 0; i < 19; i++)
            {
                if (defendingType[i, 0] == attackE)
                {
                    positionY =i; 
                    break;
                }
            }

            element1 = combo_FoePkmnElement1.Text;
            element2 = combo_FoePkmnElement2.Text;

            if (string.Equals(element1,"")) //Sets x position for element2 if element1 is null.
            {
                
                for (byte i = 1; i < 19; i++)
                {
                    if (defendingType[0, i] == element2)
                    {
                        positionX1 = i;
                        break;
 
                    }
                }
                return float.Parse(defendingType[positionY, positionX1]);
            }
            else if (string.Equals("",element2) || string.Equals(element2,"None") || string.Equals(element2,null)) //Sets x position for element1 if element2 is null.
            {
               
                for (byte i = 1; i < 19; i++)
                {
                    if (string.Equals(defendingType[0, i],element1))
                    {
                        positionX1 = i;
                        break;
                    }
                }
                return float.Parse(defendingType[positionY, positionX1]);
            }
            else //Sets x position for element1 and element2
            {
                for (byte i = 1; i < 19; i++)
                {
                    if (defendingType[i, 0] == element1)
                    {
                        positionX1 = i;
                    }
                }
                for (byte i = 1; i < 19; i++)
                {
                    if (defendingType[i, 0] == element2)
                    {
                        positionX2 = i;
                    }
                }
                total = float.Parse(defendingType[positionY, positionX1]) * float.Parse(defendingType[positionY, positionX2]);
               
                return total;

            }  
        }

        private float BurnCalculation() //Calculates burn value. If pokemon is burned, its ability is not Guts and uses a physical move, burn is 0.5, otherwhise it's 1.
        {
            if (burnAnswer == "yes")
            {
                if (attackType == "physical")
                {
                    dynamic answer = MessageBox.Show("Is your pokemon ability \"Guts\"?", "Damage Calculator", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (answer == System.Windows.MessageBoxResult.Yes)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0.5f;
                    }
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
           
        }

        private float OtherCalculation(float type)
        {
            if (combo_Abilities.SelectedIndex != 0)
            {
                switch (combo_Abilities.SelectedIndex)
                {
                    case 1://Fluffy
                        if (combo_attackElement.SelectedIndex==9 && attackType=="physical")
                        {
                            
                            return 1;
                        }else if(combo_attackElement.SelectedIndex==9 && attackType=="special")
                        {
                            return 2;

                        }
                        else if (attackType=="special")
                        {
                            return 1;
                        }
                        else
                        {
                            return 0.5f;
                        }
                    case 2://Filer
                        if (type>1)
                        {
                            return 0.75f;
                        }
                        else
                        {
                            return 1;
                        }
                    case 3: //Friend Guard
                        return 0.75f;
                    case 4: //Multiscale
                            return 0.5f;
                        
                    case 5: //Prism Armor
                        if (type > 1)
                        {
                            return 0.75f;
                        }
                        else
                        {
                            return 1;
                        }
                    case 6://Shadow Shield
                        return 0.5f;
                    case 7: //Sniper
                        if (criticalAnswer == "yes")
                        {
                            return 1.5f;
                        }else
                        {
                            return 1;
                        }
                    case 8://Solid Rock
                        if (type > 1)
                        {
                            return 0.75f;
                        }
                        else
                        {
                            return 1;
                        }
                    case 9://Tinted Lens
                        if (type < 1)
                        {
                            return 2;
                        }
                        else
                        {
                            return 1;
                        }


                }
            }

            return 1;
        }
    }

}
