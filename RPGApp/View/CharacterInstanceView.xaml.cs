﻿using RPGApp.ViewModel;
using RPGSystem.Characters;
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

namespace RPGApp.View
{
    /// <summary>
    /// Interaction logic for CharacterInstanceView.xaml
    /// </summary>
    public partial class CharacterInstanceView : UserControl
    {
        public CharacterInstanceViewModel ViewModel { get; }

        public CharacterInstanceView()
        {
            InitializeComponent();
            ViewModel = new CharacterInstanceViewModel();
            DataContext = ViewModel;
        }

        //public CharacterInstance Character
        //{
        //    get => ViewModel.Character;
        //    set => ViewModel.Character = value;
        //}
    }
}