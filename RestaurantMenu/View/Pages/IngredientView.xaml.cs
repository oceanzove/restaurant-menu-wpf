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
    /// Логика взаимодействия для IngredientView.xaml
    /// </summary>
    public partial class IngredientView : Page
    {
        private readonly Database.RestaurantEntities database;
        public ObservableCollection<Ingredient> Ingredients { get; set; }

        public IngredientView(Database.RestaurantEntities entities)
        {
            InitializeComponent();

            database = entities;
            DataContext = this;

            UpdateIngridients();
        }

        private void UpdateIngridients()
        {
            Ingredients = new ObservableCollection<Ingredient>(database.Ingredients);
        }

        private void GoToBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        
        // Редактирование

        private void EnableEditIngridient_Click(object sender, RoutedEventArgs e)
        {
            bIngridientEdit.IsEnabled = true;

            var selectedIngridient = lvIngridients.SelectedItem as Database.Ingredient;
            tbEditName.Text = selectedIngridient.name;
            tbEditProtein.Text = selectedIngridient.protein.ToString();
            tbEditFat.Text = selectedIngridient.fat.ToString();
            tbEditCarbohydrate.Text = selectedIngridient.carbohydrate.ToString();
        }

        private void SaveEditIngridient_Click(object sender, RoutedEventArgs e)
        {
            var name = tbEditName.Text.Trim();
            decimal protein;
            if (!decimal.TryParse(tbEditProtein.Text.Trim().Replace('.', ','), out protein))
            {
                MessageBox.Show("Введите корректное значение для белка.");
                return;
            }
            decimal fat;
            if (!decimal.TryParse(tbEditFat.Text.Trim().Replace('.', ','), out fat))
            {
                MessageBox.Show("Введите корректное значение для жира.");
                return;
            }
            decimal carbohydrate;
            if (!decimal.TryParse(tbEditCarbohydrate.Text.Trim().Replace('.', ','), out carbohydrate))
            {
                MessageBox.Show("Введите корректное значение для углевода.");
                return;
            }

            ClearTbEditIngridient();

            var selectedIngridient = lvIngridients.SelectedItem as Database.Ingredient;

            if (name != selectedIngridient.name)
            {
                selectedIngridient.name = name;
            }
            if (protein != selectedIngridient.protein)
            {
                selectedIngridient.protein = protein;
            }
            if (fat != selectedIngridient.fat)
            {
                selectedIngridient.fat = fat;
            }
            if (carbohydrate != selectedIngridient.carbohydrate)
            {
                selectedIngridient.carbohydrate = carbohydrate;
            }

            database.SaveChanges();

            lvIngridients.ItemsSource = null;
            lvIngridients.ItemsSource = database.Ingredients.ToList();

            bIngridientEdit.IsEnabled = false;
        }

        private void CancelEditIngridient_Click(object sender, RoutedEventArgs e)
        {
            bIngridientEdit.IsEnabled = false;
            ClearTbEditIngridient();
        }
        private void lvIngridients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bEditMode.IsEnabled = lvIngridients.SelectedItem != null;
            bIngridientEdit.IsEnabled = false;
            ClearTbEditIngridient();
        }

        private void ClearTbEditIngridient()
        {
            tbEditName.Clear();
            tbEditProtein.Clear();
            tbEditFat.Clear();
            tbEditCarbohydrate.Clear();
        }

        // Создание
        private void EnableCreateIngridient_Click(object sender, RoutedEventArgs e)
        {
            ToggleCreateMode();
        }

        private void CreateIngridient_Click(object sender, RoutedEventArgs e)
        {
            var name = tbCreateName.Text.Trim();
            decimal protein;
            if (!decimal.TryParse(tbCreateProtein.Text.Trim().Replace('.', ','), out protein))
            {
                MessageBox.Show("Введите корректное значение для белка.");
                return;
            }
            decimal fat;
            if (!decimal.TryParse(tbCreateFat.Text.Trim().Replace('.', ','), out fat))
            {
                MessageBox.Show("Введите корректное значение для жира.");
                return;
            }
            decimal carbohydrate;
            if (!decimal.TryParse(tbCreateCarbohydrate.Text.Trim().Replace('.', ','), out carbohydrate))
            {
                MessageBox.Show("Введите корректное значение для углевода.");
                return;
            }
            ClearTbCreateIngridient();


            Database.Ingredient ingredient = new Database.Ingredient();
            ingredient.name = name;
            ingredient.protein = protein;
            ingredient.fat = fat;
            ingredient.carbohydrate = carbohydrate;

            
            database.Ingredients.Add(ingredient);
            Ingredients.Add(ingredient);
            database.SaveChanges();

            ToggleCreateMode();
        }

        private void CanselIngridient_Click(object sender, RoutedEventArgs e)
        {
            ClearTbCreateIngridient();
            ToggleCreateMode();
        }

        private void tbCreateIngridient_TextChanged(object sender, TextChangedEventArgs e)
        {
            bSaveNewIngridient.IsEnabled =
                tbCreateName.Text.Trim().Length > 0
                && tbCreateProtein.Text.Trim().Length > 0
                && tbCreateFat.Text.Trim().Length > 0
                && tbCreateCarbohydrate.Text.Trim().Length > 0;
        }

        private void ClearTbCreateIngridient()
        {
            tbCreateName.Clear();
            tbCreateProtein.Clear();
            tbCreateFat.Clear();
            tbCreateCarbohydrate.Clear();
        }

        private void ToggleCreateMode()
        {
            if (bIngridientCreate.IsEnabled)
            {
                bIngridientCreate.IsEnabled = false;
                bCreateMode.IsEnabled = true;
            } else
            {
                bIngridientCreate.IsEnabled = true;
                bCreateMode.IsEnabled = false;
            }
        }

    }
}
