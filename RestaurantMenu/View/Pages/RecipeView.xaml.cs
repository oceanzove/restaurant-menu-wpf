using Database;
using RestaurantMenu.Middleware;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RestaurantMenu.View.Pages
{
    /// <summary>
    /// Логика взаимодействия для RecipeView.xaml
    /// </summary>
    public partial class RecipeView : Page
    {
        private readonly Database.RestaurantEntities database;

        public ObservableCollection<Ingredient> Ingredients { get; set; }
        public ObservableCollection<Ingredient> SelectedIngredients { get; set; }
        public Database.Menu Menu { get; set; }
        public ObservableCollection<MenuIngredient> MenuIngredients { get; set; }


        public decimal Weight { get; set; }
        public decimal Kcal { get; set; }
        public decimal Protein { get; set; }
        public decimal Fat { get; set; }
        public decimal Carbohydrate { get; set; }

        private string defaultName = "Поменяйте название";
        public RecipeView(Database.RestaurantEntities entities)
        {
            InitializeComponent();

            database = entities;
            DataContext = this;

            MenuIngredients = new ObservableCollection<MenuIngredient>(database.MenuIngredients);


            SetNewRecipe();
            UpdateIngridients();
            UpdateSelectedIngredients();
            SetMenuInfo();
        }

        private void SaveRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (tbRecipeName.Text != this.defaultName)
            {
                database.SaveChanges();
                MessageBox.Show("Рецепт успешно сохранен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.GoBack();
            }
        }

        private void SetMenuInfo()
        {
            if (Menu == null) return;

            var selectedMenu = MenuIngredients.Where(menu => menu.menu_id == Menu.id).ToList();

            this.Weight = selectedMenu.Sum(menu => menu.weight);
            rInfoWeight.Text = this.Weight.ToString("N2", CultureInfo.InvariantCulture);

            var proteintPerHundredGrams = this.SelectedIngredients.Sum(ingridient => ingridient.protein);
            decimal protein = 0;
            foreach (var elem in this.SelectedIngredients)
            {
                var product = this.MenuIngredients.Where(menu => menu.menu_id == this.Menu.id && elem.id == menu.ingredient_id).FirstOrDefault();
                protein += (elem.protein * 10) * product.weight;
            }
            this.Protein = protein;
            rInfoProtein.Text = this.Protein.ToString("N2", CultureInfo.InvariantCulture);

            var fatPerHundredGrams = this.SelectedIngredients.Sum(ingridient => ingridient.fat);
            decimal fat = 0;
            foreach (var elem in this.SelectedIngredients)
            {
                var product = this.MenuIngredients.Where(menu => menu.menu_id == this.Menu.id && elem.id == menu.ingredient_id).FirstOrDefault();
                fat += (elem.fat * 10) * product.weight;
            }
            this.Fat = fat;
            rInfoFat.Text = this.Fat.ToString("N2", CultureInfo.InvariantCulture);

            var carbohydratePerHundredGrams = this.SelectedIngredients.Sum(ingridient => ingridient.carbohydrate);
            decimal carbohydrate = 0;
            foreach (var elem in this.SelectedIngredients)
            {
                var product = this.MenuIngredients.Where(menu => menu.menu_id == this.Menu.id && elem.id == menu.ingredient_id).FirstOrDefault();
                carbohydrate += (elem.carbohydrate * 10) * product.weight;
            }
            this.Carbohydrate = carbohydrate;
            rInfoCarbohydrate.Text = this.Carbohydrate.ToString("N2", CultureInfo.InvariantCulture);


            this.Kcal = CaloriesCalculator.CalculateCalories(this.Protein, this.Fat, this.Carbohydrate);
            rInfoKcal.Text = this.Kcal.ToString("N2", CultureInfo.InvariantCulture);
        }

        private void CreateDefaultName()
        {
            Menu = new Database.Menu();
            Menu.name = this.defaultName;
            Menu.approved = false;
            database.Menus.Add(Menu);
        }

        private void SetNewRecipe()
        {
            CreateDefaultName();
            bSaveRecipe.IsEnabled = false;
        }

        private void GoToBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void UpdateIngridients()
        {
            Ingredients = new ObservableCollection<Ingredient>(database.Ingredients);
        }

        private void UpdateSelectedIngredients()
        {
            var selectedMenu = MenuIngredients.Where(menu => menu.menu_id == Menu.id).ToList();
            var ingredientIds = selectedMenu.Select(selectetMenu => selectetMenu.ingredient_id).ToArray();

            var filteredIngredients = database.Ingredients.Where(ingredient => ingredientIds.Contains(ingredient.id)).ToList();

            SelectedIngredients = new ObservableCollection<Ingredient>(filteredIngredients);
        }

        private void bInMenu_Click(object sender, RoutedEventArgs e)
        {
            var selectedIngridient = lvIngridients.SelectedItem as Database.Ingredient;

            var menuIngredient = new Database.MenuIngredient();

            menuIngredient.menu_id = this.Menu.id;
            menuIngredient.ingredient_id = selectedIngridient.id;
            menuIngredient.weight = 0.100M;

            SelectedIngredients.Add(selectedIngridient);

            database.MenuIngredients.Add(menuIngredient);
            this.MenuIngredients.Add(menuIngredient);

            SetMenuInfo();
        }
        private void lvIngridients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bInMenu.IsEnabled = lvIngridients.SelectedItem != null;
        }

        private void lvSelectedIngridients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bOutMenu.IsEnabled = lvSelectedIngridients.SelectedItem != null;
            bWeightIngridient.IsEnabled = lvSelectedIngridients.SelectedItem != null;

            var selectedIngridientFromMenu = GetSelectedIngridientFromMenu();

            if (selectedIngridientFromMenu == null) return;

            tbWeightIngridient.Text = selectedIngridientFromMenu.weight.ToString();
        }

        private void bOutMenu_Click(object sender, RoutedEventArgs e)
        {
            var selectedIngridientFromMenu = GetSelectedIngridientFromMenu();

            if (selectedIngridientFromMenu == null) return;

            bWeightIngridient.IsEnabled = false;

            var selectedIngridient = lvSelectedIngridients.SelectedItem as Database.Ingredient;

            SelectedIngredients.Remove(selectedIngridient);

            database.MenuIngredients.Remove(selectedIngridientFromMenu);
            this.MenuIngredients.Remove(selectedIngridientFromMenu);

            SetMenuInfo();
        }

        private MenuIngredient GetSelectedIngridientFromMenu()
        {
            var selectedIngridient = lvSelectedIngridients.SelectedItem as Database.Ingredient;

            if (selectedIngridient == null)
                return new MenuIngredient();

            return this.MenuIngredients.Where(menu => menu.menu_id == this.Menu.id && selectedIngridient.id == menu.ingredient_id).FirstOrDefault();
        }

        private void tbWeightIngridient_TextChanged(object sender, TextChangedEventArgs e)
        {
            var selectedIngridientFromMenu = GetSelectedIngridientFromMenu();

            if (selectedIngridientFromMenu == null) return;

            if (tbWeightIngridient.Text.Trim().Length > 0)
            {
                decimal weight;
                if (!decimal.TryParse(tbWeightIngridient.Text.Trim().Replace('.', ','), out weight))
                {
                    MessageBox.Show("Введите корректное значение для веса.");
                    return;
                }

                if (selectedIngridientFromMenu.weight != weight)
                {
                    selectedIngridientFromMenu.weight = weight;

                }
                SetMenuInfo();
            }
        }

        private void tbRecipeName_TextChanged(object sender, TextChangedEventArgs e)
        {
            bSaveRecipe.IsEnabled = tbRecipeName.Text.Trim() != this.defaultName;
            if (tbRecipeName.Text.Trim() == this.defaultName)
            {
                tbRecipeName.Background = new SolidColorBrush(Colors.Red);
                tbRecipeName.Foreground = new SolidColorBrush(Colors.White);
            } else
            {
                tbRecipeName.Background = new SolidColorBrush(Colors.White);
                tbRecipeName.Foreground = new SolidColorBrush(Colors.Black);

            }
        }
    }
}
