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
using System.Windows.Shapes;
using RomanNumerals;

namespace BossFighter
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public GameWindow()
        {
            InitializeComponent();
            Jmeno.Text = Player.PlayerName;
            if (Player.PlayerName == "Solaire")
            {
                PlayerHP.Maximum = 500;
                Player.PlayerHP += 500;
                Player.PlayerAttack += 100;
            }
            PlayerHP.Value = Player.PlayerHP;
            EnemyHP.Value = Enemy.EnemyHP;
            PlayerFP.Value = Player.PlayerFP;
            Round.Text = RomanNumerals.Convert.ToRomanNumerals(Player.round, RomanNumerals.Numerals.NumeralFlags.Unicode);
        }

        public bool attacked = false;
        public bool spellbuff = false;
        public bool storm_charge = false;

        public int spell_cooldown = 5;
        public int storm_cooldown = 2;

        public static double dmg_buff = Player.PlayerAttack / 100;
        public double weapon_buff = dmg_buff * 20;

        public double storm_dmg;

        public static double def_buff = Enemy.EnemyAttack / 100;
        public double df_buff = def_buff * 20;

        public int pivko = 6;

        private void Attack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ( attacked == false)
            {
                EnemyHP.Value -= Player.PlayerAttack;
                attacked = true;
                MessageBox.Show($"You deal {Player.PlayerAttack} DMG.");
            }
        }

        private void Storm_ruler_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(Player.round == 4 && storm_charge == false) 
            {
                Player.PlayerAttack += storm_dmg;
                storm_charge = true;
                MessageBox.Show("You charge the storm ruler.\nYour attack has been greatly enchanced.");
            }
            else
            {
                MessageBox.Show("Nothing happened yet.");
            }
        }

        private void Spell_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (spellbuff == false && PlayerFP.Value > 0) 
            {
                if (attacked == false)
                {
                    PlayerFP.Value -= 25;
                    Player.PlayerAttack += weapon_buff;
                    Enemy.EnemyAttack -= df_buff;
                    attacked = true;
                    spellbuff = true;
                    MessageBox.Show($"You used spellbuff.\nYour attack and defense are enhanced.");
                }
            }
        }

        private void Heal_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (attacked == false && pivko >= 0)
            {
                PlayerHP.Value += 35;
                PlayerFP.Value += 15;
                pivko -= 1;
                attacked = true;
                MessageBox.Show($"You use pivko and you restore 35HP and 15FP.\n{pivko} pivko remaining.");
            }
        }

        private void NextRound_Click(object sender, RoutedEventArgs e)
        {
            if (EnemyHP.Value <= 0 && Player.round == 5)
            {
                WinWindow winWindow = new WinWindow();
                winWindow.Show();
                Close();
                InitializeComponent();
            }
            else if(PlayerHP.Value <= 0)
            {
                LoseWindow loseWindow = new LoseWindow();
                loseWindow.Show();
                Close();
                InitializeComponent();
            }
            else if(EnemyHP.Value <= 0)
            {
                Player.round += 1;
                switch (Player.round)
                {
                    case 2:
                        EnemyHP.Maximum = 125;
                        EnemyHP.Value = 125;
                        Enemy.EnemyAttack = 12;
                        EnemyName.Text = "Chaos Eater";
                        Enemak.Source = new BitmapImage(new Uri("Obrazky/Chaos_Eater.png", UriKind.Relative));
                        Player.PlayerAttack = 14;
                        dmg_buff = Player.PlayerAttack / 100;
                        weapon_buff = dmg_buff * 20;
                        def_buff = Enemy.EnemyAttack / 100;
                        df_buff = def_buff * 20;
                        break;
                    case 3:
                        EnemyHP.Maximum = 150;
                        EnemyHP.Value = 150;
                        Enemy.EnemyAttack = 14;
                        EnemyName.Text = "Jailer";
                        Enemak.Source = new BitmapImage(new Uri("Obrazky/Jailer.png", UriKind.Relative));
                        Player.PlayerAttack = 16;
                        dmg_buff = Player.PlayerAttack / 100;
                        weapon_buff = dmg_buff * 20;
                        def_buff = Enemy.EnemyAttack / 100;
                        df_buff = def_buff * 20;
                        PlayerHP.Value = 100;
                        break;
                    case 4:
                        EnemyHP.Maximum = 250;
                        EnemyHP.Value = 250;
                        Enemy.EnemyAttack = 20;
                        EnemyName.Text = "Yhorm The Giant";
                        Enemak.Source = new BitmapImage(new Uri("Obrazky/yhorm.jpg", UriKind.Relative));
                        Player.PlayerAttack = 25;
                        dmg_buff = Player.PlayerAttack / 100;
                        weapon_buff = dmg_buff * 20;
                        def_buff = Enemy.EnemyAttack / 100;
                        df_buff = def_buff * 20;
                        storm_dmg = EnemyHP.Value / 3;
                        break;
                }
                if(spellbuff == true)
                {
                    Player.PlayerAttack += weapon_buff;
                    Enemy.EnemyAttack -= df_buff;
                }
                Round.Text = RomanNumerals.Convert.ToRomanNumerals(Player.round, RomanNumerals.Numerals.NumeralFlags.Unicode);
                attacked = false;
            }
            else
            {
                PlayerHP.Value -= Enemy.EnemyAttack;
                attacked = false;
                MessageBox.Show($"Enemy deals {Enemy.EnemyAttack} DMG.");
                if (spellbuff == true)
                {
                    spell_cooldown -= 1;
                    if (spell_cooldown <= 0)
                    {
                        spellbuff = false;
                        spell_cooldown = 5;
                        Player.PlayerAttack -= weapon_buff;
                        Enemy.EnemyAttack += df_buff;
                    }
                }
                if (storm_charge == true) 
                { 
                    storm_cooldown -= 1;
                    if(storm_cooldown <= 0) 
                    {
                        storm_charge = false;
                        storm_cooldown = 2;
                        Player.PlayerAttack -= storm_dmg;
                    }
                }
            }
        }
    }
}
//STORM RULER !!!!!!!!!!!!!!!