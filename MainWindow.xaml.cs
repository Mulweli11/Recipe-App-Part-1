using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace RecipeApp
{
    public partial class MainWindow : Window
    {
        static List<Recipe> recipes = new List<Recipe>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddRecipe_Click(object sender, RoutedEventArgs e)
        {
            AddRecipe();
        }

        private void DisplayRecipes_Click(object sender, RoutedEventArgs e)
        {
            DisplayRecipes();
        }

        private void CreateMenu_Click(object sender, RoutedEventArgs e)
        {
            CreateMenu();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        static void AddRecipe()
        {
            string name = GetStringFromUser("Enter Recipe Name:");

            List<Ingredient> ingredients = GetIngredientsFromUser();
            List<Step> steps = GetStepsFromUser();

            recipes.Add(new Recipe(name, ingredients, steps));
            MessageBox.Show("Recipe added successfully!");
        }

        static List<Ingredient> GetIngredientsFromUser()
        {
            List<Ingredient> ingredients = new List<Ingredient>();

            int numIngredients = GetIntFromUser("Enter the number of ingredients:");

            for (int i = 0; i < numIngredients; i++)
            {
                string name = GetStringFromUser($"Enter the name of ingredient {i + 1}:");
                double quantity = GetDoubleFromUser($"Enter the quantity of {name}:");
                string unit = GetStringFromUser($"Enter the unit of measurement for {name}:");
                double calories = GetDoubleFromUser($"Enter the number of calories for {name}:");
                string foodGroup = GetStringFromUser($"Enter the food group for {name}:");

                ingredients.Add(new Ingredient(name, quantity, unit, calories, foodGroup));
            }

            return ingredients;
        }

        static List<Step> GetStepsFromUser()
        {
            List<Step> steps = new List<Step>();

            int numSteps = GetIntFromUser("Enter the number of steps:");

            for (int i = 0; i < numSteps; i++)
            {
                string description = GetStringFromUser($"Enter step {i + 1}:");
                steps.Add(new Step(description));
            }

            return steps;
        }

        static void DisplayRecipes()
        {
            if (recipes.Count == 0)
            {
                MessageBox.Show("No recipes available.");
                return;
            }

            string recipesList = "Recipes:\n";
            foreach (var recipe in recipes.OrderBy(r => r.Name))
            {
                recipesList += recipe.Name + "\n";
            }

            string recipeName = GetStringFromUser("Enter the name of the recipe to display:\n" + recipesList);

            Recipe selectedRecipe = recipes.FirstOrDefault(r => r.Name.Equals(recipeName, StringComparison.OrdinalIgnoreCase));

            if (selectedRecipe != null)
            {
                string recipeDetails = $"Recipe: {selectedRecipe.Name}\nIngredients:\n";
                foreach (var ingredient in selectedRecipe.Ingredients)
                {
                    recipeDetails += $"{ingredient.Quantity} {ingredient.Unit} of {ingredient.Name} ({ingredient.Calories} calories, Food Group: {ingredient.FoodGroup})\n";
                }
                recipeDetails += "Steps:\n";
                for (int i = 0; i < selectedRecipe.Steps.Count; i++)
                {
                    recipeDetails += $"{i + 1}. {selectedRecipe.Steps[i].Description}\n";
                }
                recipeDetails += $"Total Calories: {selectedRecipe.GetTotalCalories()}\n";
                if (selectedRecipe.GetTotalCalories() > 300)
                {
                    recipeDetails += "Warning: Total calories exceed 300!\n";
                }

                MessageBox.Show(recipeDetails);
                string input = GetStringFromUser("Enter the scaling factor (0.5, 2, or 3) or 'reset' to reset quantities:");
                if (double.TryParse(input, out double scale))
                {
                    if (scale == 0.5 || scale == 2 || scale == 3)
                    {
                        selectedRecipe.ScaleIngredients(scale);
                        string scaledRecipeDetails = "Scaled Recipe:\n";
                        foreach (var ingredient in selectedRecipe.Ingredients)
                        {
                            scaledRecipeDetails += $"{ingredient.Quantity} {ingredient.Unit} of {ingredient.Name}\n";
                        }
                        scaledRecipeDetails += $"Total Calories after scaling: {selectedRecipe.GetTotalCalories()}";
                        MessageBox.Show(scaledRecipeDetails);
                    }
                    else
                    {
                        MessageBox.Show("Invalid scaling factor. Please enter 0.5, 2, or 3.");
                    }
                }
                else if (input.ToLower() == "reset")
                {
                    selectedRecipe.ResetQuantities();
                    MessageBox.Show("Quantities reset to original values.");
                }
                else
                {
                    MessageBox.Show("Invalid input.");
                }
            }
            else
            {
                MessageBox.Show("Recipe not found.");
            }
        }

        static void CreateMenu()
        {
            if (recipes.Count == 0)
            {
                MessageBox.Show("No recipes available.");
                return;
            }

            List<Recipe> selectedRecipes = new List<Recipe>();
            while (true)
            {
                string recipesList = "Available Recipes:\n";
                foreach (var recipe in recipes.OrderBy(r => r.Name))
                {
                    recipesList += recipe.Name + "\n";
                   
                }

                string recipeName = GetStringFromUser("Enter the name of the recipe to add to the menu (or type 'done' to finish):\n" + recipesList);

                if (recipeName.ToLower() == "done")
                {
                    break;
                }

                Recipe selectedRecipe = recipes.FirstOrDefault(r => r.Name.Equals(recipeName, StringComparison.OrdinalIgnoreCase));
                if (selectedRecipe != null)
                {
                    selectedRecipes.Add(selectedRecipe);
                }
                else
                {
                    MessageBox.Show("Recipe not found.");
                }
            }

            if (selectedRecipes.Count > 0)
            {
                DisplayFoodGroupChart(selectedRecipes);
            }
            else
            {
                MessageBox.Show("No recipes selected for the menu.");
            }
        }

        static void DisplayFoodGroupChart(List<Recipe> selectedRecipes)
        {
            Dictionary<string, double> foodGroupContributions = new Dictionary<string, double>();

            foreach (var recipe in selectedRecipes)
            {
                var contributions = recipe.GetFoodGroupContributions();
                foreach (var contribution in contributions)
                {
                    if (foodGroupContributions.ContainsKey(contribution.Key))
                    {
                        foodGroupContributions[contribution.Key] += contribution.Value;
                    }
                    else
                    {
                        foodGroupContributions[contribution.Key] = contribution.Value;
                    }
                }
            }

            Piechart pieChartWindow = new Piechart(foodGroupContributions);
            pieChartWindow.Show();
        }

        static string GetStringFromUser(string prompt)
        {
            InputDialog inputDialog = new InputDialog(prompt);
            if (inputDialog.ShowDialog() == true)
            {
                return inputDialog.ResponseText;
            }
            return string.Empty;
        }

        static int GetIntFromUser(string prompt)
        {
            string input = GetStringFromUser(prompt);
            int.TryParse(input, out int result);
            return result;
        }

        static double GetDoubleFromUser(string prompt)
        {
            string input = GetStringFromUser(prompt);
            double.TryParse(input, out double result);
            return result;
        }
    }

    class Recipe
    {
        public string Name { get; }
        public List<Ingredient> Ingredients { get; }
        public List<Step> Steps { get; }

        public Recipe(string name, List<Ingredient> ingredients, List<Step> steps)
        {
            Name = name;
            Ingredients = ingredients;
            Steps = steps;
        }

        public double GetTotalCalories()
        {
            return Ingredients.Sum(i => i.Calories);
        }

        public void ScaleIngredients(double factor)
        {
            foreach (var ingredient in Ingredients)
            {
                ingredient.Quantity *= factor;
            }
        }

        public void ResetQuantities()
        {
            foreach (var ingredient in Ingredients)
            {
                ingredient.ResetQuantity();
            }
        }

        public Dictionary<string, double> GetFoodGroupContributions()
        {
            return Ingredients
                .GroupBy(i => i.FoodGroup)
                .ToDictionary(g => g.Key, g => g.Sum(i => i.Calories));
        }
    }

    class Ingredient
    {
        public string Name { get; }
        public double Quantity { get; set; }
        public string Unit { get; }
        public double Calories { get; }
        public string FoodGroup { get; }
        private double OriginalQuantity { get; }

        public Ingredient(string name, double quantity, string unit, double calories, string foodGroup)
        {
            Name = name;
            Quantity = quantity;
            Unit = unit;
            Calories = calories;
            FoodGroup = foodGroup;
            OriginalQuantity = quantity;
        }

        public void ResetQuantity()
        {
            Quantity = OriginalQuantity;
        }
    }

    class Step
    {
        public string Description { get; }

        public Step(string description)
        {
            Description = description;
        }
    }
}

