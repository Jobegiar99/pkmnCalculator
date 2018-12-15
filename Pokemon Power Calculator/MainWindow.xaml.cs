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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Results_Click(object sender, RoutedEventArgs e)
        {
            double levelData = 0d, attackDefense = 0d, power = 0d, Modifier = 0d;
            float targets=0f, weather = 0f, critical = 0f, random = 0f, stab = 0f, type = 0f, burn = 0f, other = 0f;
            int level_attackdefense_power = 0; /* ((level data * attackDenfese*power) /50)+2*/


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

            level_attackdefense_power =(int) ((levelData * attackDefense * power) / 50)+2;

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

            try //Calculates Weather value. It can be 0.5,1 or 1.5.
            {

            }
            catch (Exception)
            {

            }

             
        }

        public double NegativeToPositive(double value)
        {
            if (value < 0)
            {
                value *= -1;
            }
            return value;
        }
    }
}
