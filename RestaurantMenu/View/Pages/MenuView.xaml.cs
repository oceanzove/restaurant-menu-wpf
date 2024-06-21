using Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace RestaurantMenu.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для MenuView.xaml
    /// </summary>
    public partial class MenuView : Page
    {
        private readonly Database.RestaurantEntities database;

        public MenuView(Database.RestaurantEntities entities)
        {
            InitializeComponent();

            database = entities;
            DataContext = this;

        }

        private void GoToIngridient_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(ViewManager.IngredientView);
        }

        public void GoToCreateRecipe_Click(object sender, RoutedEventArgs e)
        {
            var recipeView = new RecipeView(database);
            NavigationService.Navigate(recipeView);
        }

    }
}
