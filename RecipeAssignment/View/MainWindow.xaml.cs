using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using RecipeAssignment.Model;

namespace RecipeAssignment.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DataClasses1DataContext _dataClasses1DataContext = new DataClasses1DataContext();

        private readonly List<Recipe> _filterRecipeList = new List<Recipe>();
        private readonly HashSet<Recipe> _favRecipeSet = new HashSet<Recipe>();

        public MainWindow()

        {
            InitializeComponent();

            LoadRecipes();
            LoadFavRecipes();
        }

        private void LoadRecipes()
        {
            try
            {
                DataGridRecipes.ItemsSource = null;
                DataGridRecipes.ItemsSource = _dataClasses1DataContext.Recipes.ToList();
            }
            catch (Exception ex)
            {
                // Some other error occurred.
                // Report the error to the user.
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void LoadFavRecipes()
        {
            try
            {
                foreach (var recipe in _dataClasses1DataContext.Recipes.ToList().Where(recipe => recipe.is_favorite))
                {
                    _favRecipeSet.Add(recipe);
                }

                DataFavorites.ItemsSource = null;
                DataFavorites.ItemsSource = _favRecipeSet;
            }
            catch (Exception ex)
            {
                // Some other error occurred.
                // Report the error to the user.
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void Add_To_Favorite(Recipe editedRecipe)
        {
            foreach (var recipe in _dataClasses1DataContext.Recipes.ToList()
                .Where(recipe => DataGridRecipes.SelectedItem == recipe))
            {
                switch (editedRecipe.is_favorite)
                {
                    case true when !_favRecipeSet.Contains(editedRecipe):
                        _favRecipeSet.Add(editedRecipe);
                        break;
                    case false when recipe.recipe_id == editedRecipe.recipe_id:
                        _favRecipeSet.Remove(editedRecipe);
                        break;
                }
            }
        }

        private void AddNewRecipe_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newRecipe = new Recipe
                {
                    recipe_name = RecipeNameTextBox.Text,
                    cooking_time = TimeSpan.Parse(CookingTimeTextBox.Text),
                    description = DescriptionTextBox.Text,
                    is_favorite = FavoriteCheckBox.IsChecked != null && (bool) FavoriteCheckBox.IsChecked,
                    ingredients = IngNameTextBox.Text,
                };

                Add_To_Favorite(newRecipe);

                _dataClasses1DataContext.Recipes.InsertOnSubmit(newRecipe);
                _dataClasses1DataContext.SubmitChanges(ConflictMode.FailOnFirstConflict);

                LoadRecipes();
                LoadFavRecipes();

                MessageBox.Show("Recipe Added Successfully");
            }
            catch (FormatException)
            {
                MessageBox.Show(
                    "Invalid Input Values");
            }
            catch (Exception ex)
            {
                // Some other error occurred.
                // Report the error to the user.
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void DeleteRecipe_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in _dataClasses1DataContext.Recipes.ToList()
                    .Where(item => DataGridRecipes.SelectedItem == item))
                {
                    Add_To_Favorite(item);

                    _dataClasses1DataContext.Recipes.DeleteOnSubmit(item);
                    _dataClasses1DataContext.SubmitChanges();

                    LoadRecipes();
                    LoadFavRecipes();


                    MessageBox.Show("Recipe Deleted Successfully");
                    TabItemRecipes.IsSelected = true;
                }
            }
            catch (Exception ex)
            {
                // Some other error occurred.
                // Report the error to the user.
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void UpdateRecipe_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (var item in _dataClasses1DataContext.Recipes.ToList()
                    .Where(item => DataGridRecipes.SelectedItem == item))
                {
                    item.recipe_name = RecipeNameTextBox.Text;
                    item.cooking_time = TimeSpan.Parse(CookingTimeTextBox.Text);
                    item.description = DescriptionTextBox.Text;
                    if (FavoriteCheckBox.IsChecked != null) item.is_favorite = (bool) FavoriteCheckBox.IsChecked;
                    item.ingredients = IngNameTextBox.Text;

                    Add_To_Favorite(item);
                }


                _dataClasses1DataContext.SubmitChanges();

                LoadRecipes();
                LoadFavRecipes();

                MessageBox.Show("Recipe Updated Successfully");


                TabItemRecipes.IsSelected = true;
            }
            catch (Exception ex)
            {
                // Some other error occurred.
                // Report the error to the user.
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void GridRec_MouseDC(object sender, MouseButtonEventArgs e)
        {
            switch (DataGridRecipes.SelectedItem)
            {
                case null:
                    return;
                case Recipe _:
                    TabItemAddEditRecipe.IsSelected = true;
                    break;
            }
        }


        private void GridFavRec_MouseDC(object sender, MouseButtonEventArgs e)
        {
            switch (DataFavorites.SelectedItem)
            {
                case null:
                    return;
                case Recipe recipe:

                    MessageBox.Show(recipe.ToString());
                    break;
            }
        }


        private void GridRec_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in _dataClasses1DataContext.Recipes.ToList()
                .Where(item => DataGridRecipes.SelectedItem == item))
            {
                RecipeNameTextBox.Text = item.recipe_name;
                CookingTimeTextBox.Text = item.cooking_time.ToString();
                IngNameTextBox.Text = item.ingredients;
                DescriptionTextBox.Text = item.description;
                FavoriteCheckBox.IsChecked = item.is_favorite;
                break;
            }
        }


        private void SearchRecipe_Click(object sender, RoutedEventArgs e)
        {
            _filterRecipeList.Clear();

            if (SearchTextBox.Text.Equals(""))
            {
                _filterRecipeList.AddRange(_dataClasses1DataContext.Recipes.ToList());
            }
            else
            {
                foreach (var recipe in _dataClasses1DataContext.Recipes.ToList())
                {
                    if (RbName.IsChecked == true && recipe.recipe_name.Contains(SearchTextBox.Text))
                    {
                        _filterRecipeList.Add(recipe);
                    }

                    try
                    {
                        if (RbCookingTime.IsChecked == true &&
                            recipe.cooking_time == TimeSpan.Parse(SearchTextBox.Text))
                        {
                            _filterRecipeList.Add(recipe);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Some other error occurred.
                        // Report the error to the user.
                        MessageBox.Show(ex.Message, "Error");
                    }

                    if (RbIngredient.IsChecked == true && recipe.ingredients.Contains(SearchTextBox.Text))
                    {
                        _filterRecipeList.Add(recipe);
                    }
                }
            }

            DataGridRecipes.ItemsSource = null;
            DataGridRecipes.ItemsSource = _filterRecipeList;
        }


        private void SaveRecipe_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Save As...",
                Filter = "Binary File (*.bin)|*.bin",
                InitialDirectory = @"C:\"
            };
            if (saveFileDialog.ShowDialog() != true) return;
            var fs = new FileStream(saveFileDialog.FileName, FileMode.Create);
            // Create the writer for data.
            var bw = new BinaryWriter(fs);

            foreach (var item in _favRecipeSet)
            {
                var recId = item.recipe_id;
                var recName = item.recipe_name;
                var recCookingTime = item.cooking_time;
                var recIngredients = item.ingredients;
                var recDescription = item.description;
                var recIsFavorite = item.is_favorite;

                bw.Write(recId);
                bw.Write(recName);
                bw.Write(recCookingTime.ToString());
                bw.Write(recIngredients);
                bw.Write(recDescription);
                bw.Write(recIsFavorite);
            }

            fs.Close();
            bw.Close();
            _favRecipeSet.Clear();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            _favRecipeSet.Clear();
            var openFileDialog = new OpenFileDialog
            {
                Title = "Open File...",
                Filter = "Binary File (*.bin)|*.bin",
                InitialDirectory = @"C:\"
            };
            if (openFileDialog.ShowDialog() != true) return;
            var fs = new FileStream(openFileDialog.FileName, FileMode.Open);
            var br = new BinaryReader(fs);

            var newRecipe = new Recipe
            {
                recipe_id = br.ReadInt32(),
                recipe_name = br.ReadString(),
                cooking_time = TimeSpan.Parse(br.ReadString()),
                ingredients = br.ReadString(),
                description = br.ReadString(),
                is_favorite = br.ReadBoolean(),
            };
            _favRecipeSet.Add(newRecipe);

            fs.Close();
            br.Close();

            DataFavorites.ItemsSource = null;
            DataFavorites.ItemsSource = _favRecipeSet;
        }


        private void AddRecipeMain_Click(object sender, RoutedEventArgs e)
        {
            TabItemAddEditRecipe.IsSelected = true;
        }


        private void RecipeNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((TextBox) sender).Text.Length < 3)
            {
                RecipeNameTextBox.Text = "";
                MessageBox.Show("Recipe name is too short! Should be minimum of 3 characters!");
            }
            else if (((TextBox) sender).Text.Length > 150)
            {
                RecipeNameTextBox.Text = "";
                MessageBox.Show("Recipe name is too long! Should be maximum of 150 characters!");
            }
        }
    }
}