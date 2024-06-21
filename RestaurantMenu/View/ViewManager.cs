using RestaurantMenu.View.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RestaurantMenu.View
{
    public class ViewManager
    {
        private static Database.RestaurantEntities database;

        private static MenuView menuView;
        private static IngredientView ingredientView;


        private static Database.RestaurantEntities Database
        {
            get
            {
                if (database == null)
                {
                    database = new Database.RestaurantEntities();
                    if (database.Database.Exists() == false)
                    {
                        MessageBox.Show("Подключения к базе данных не было выполнено. Приложения будет завершено.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
                return database;
            }
        }

        public static MenuView MenuView
        {
            get
            {
                if (menuView == null)
                {
                    menuView = new MenuView(Database);
                }
                return menuView;
            }
        }
        
        public static IngredientView IngredientView
        {
            get
            {
                if (ingredientView == null)
                {
                    ingredientView = new IngredientView(Database);
                }
                return ingredientView;
            }
        }


    }
}
