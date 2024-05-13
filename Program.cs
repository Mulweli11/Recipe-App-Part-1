using System;
using System.Collections.Generic;
using System.Linq;

namespace RecipeApp
{
    class Program
    {
        static List<Recipe> recipes = new List<Recipe>();

        static void Main(string[] args)
        {
            Console.WriteLine("*****Welcome to RecipeApp!******");

            while (true)
            {
                Console.WriteLine("_________________________________");
                Console.WriteLine("1. Add Recipe");
                Console.WriteLine("2. Display Recipes");
                Console.WriteLine("3. Exit");
                Console.WriteLine("_________________________________"); // Added for visual separation
                Console.WriteLine("Enter your choice:");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddRecipe();
                        break;
                    case "2":
                        DisplayRecipes();
                        break;
                    case "3":
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a valid option.");
                        break;
                }
            }
        }

        static void AddRecipe()
        {
            Console.WriteLine("Enter Recipe Name:");
            string name = Console.ReadLine();

            List<Ingredient> ingredients = GetIngredientsFromUser();
            List<Step> steps = GetStepsFromUser();

            recipes.Add(new Recipe(name, ingredients, steps));
            Console.WriteLine("_________________________________");
            Console.WriteLine("Recipe added successfully!");
        }

        static List<Ingredient> GetIngredientsFromUser()
        {
            List<Ingredient> ingredients = new List<Ingredient>();

            Console.WriteLine("Enter the number of ingredients:");
            int numIngredients = int.Parse(Console.ReadLine());

            for (int i = 0; i < numIngredients; i++)
            {
                Console.WriteLine($"Enter the name of ingredient {i + 1}:");
                string name = Console.ReadLine();

                Console.WriteLine($"Enter the quantity of {name}:");
                double quantity = double.Parse(Console.ReadLine());

                Console.WriteLine($"Enter the unit of measurement for {name}:");
                string unit = Console.ReadLine();

                Console.WriteLine($"Enter the number of calories for {name}:");
                double calories = double.Parse(Console.ReadLine());

                Console.WriteLine($"Enter the food group for {name}:");
                string foodGroup = Console.ReadLine();

                ingredients.Add(new Ingredient(name, quantity, unit, calories, foodGroup));
            }

            return ingredients;
        }

        static List<Step> GetStepsFromUser()
        {
            List<Step> steps = new List<Step>();

            Console.WriteLine("Enter the number of steps:");
            int numSteps = int.Parse(Console.ReadLine());

            for (int i = 0; i < numSteps; i++)
            {
                Console.WriteLine($"Enter step {i + 1}:");
                string description = Console.ReadLine();
                steps.Add(new Step(description));
            }

            return steps;
        }

        static void DisplayRecipes()
        {
            if (recipes.Count == 0)
            {
                Console.WriteLine("No recipes available.");
                return;
            }
            Console.WriteLine("_________________________________");
            Console.WriteLine("Recipes:");
            foreach (var recipe in recipes.OrderBy(r => r.Name))
            {
                Console.WriteLine(recipe.Name);
            }
            Console.WriteLine("________________________________________");
            Console.WriteLine("Enter the name of the recipe to display:");
            Console.WriteLine("________________________________________");
            string recipeName = Console.ReadLine();

            Recipe selectedRecipe = recipes.FirstOrDefault(r => r.Name.Equals(recipeName, StringComparison.OrdinalIgnoreCase));

            if (selectedRecipe != null)
            {
                Console.WriteLine($"Recipe: {selectedRecipe.Name}");
                Console.WriteLine("Ingredients:");
                foreach (var ingredient in selectedRecipe.Ingredients)
                {
                    Console.WriteLine($"{ingredient.Quantity} {ingredient.Unit} of {ingredient.Name} ({ingredient.Calories} calories, Food Group: {ingredient.FoodGroup})");
                }
                Console.WriteLine("Steps:");
                for (int i = 0; i < selectedRecipe.Steps.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {selectedRecipe.Steps[i].Description}");
                }
                Console.WriteLine($"Total Calories: {selectedRecipe.GetTotalCalories()}");
                if (selectedRecipe.GetTotalCalories() > 300)
                {
                    Console.WriteLine("__");
                    Console.WriteLine("Warning: Total calories exceed 300!");
                }

                Console.WriteLine("________________________________________");
                Console.WriteLine("Enter the scaling factor (0.5, 2, or 3) or 'reset' to reset quantities:");
                string input = Console.ReadLine();
                if (double.TryParse(input, out double scale))
                {
                    if (scale == 0.5 || scale == 2 || scale == 3)
                    {
                        selectedRecipe.ScaleIngredients(scale);
                        Console.WriteLine("Scaled Recipe:");
                        foreach (var ingredient in selectedRecipe.Ingredients)
                        {
                            Console.WriteLine($"{ingredient.Quantity} {ingredient.Unit} of {ingredient.Name}");
                        }
                        Console.WriteLine($"Total Calories after scaling: {selectedRecipe.GetTotalCalories()}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid scaling factor. Please enter 0.5, 2, or 3.");
                    }
                }
                else if (input.ToLower() == "reset")
                {
                    // Reset quantities
                    selectedRecipe.ResetQuantities();
                    Console.WriteLine("Quantities reset to original values.");
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                }
            }
            else
            {
                Console.WriteLine("_________________________________");
                Console.WriteLine("Recipe not found.");
            }
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
    }

    class Ingredient
    {
        public string Name { get; }
        public double Quantity { get; set; }
        public string Unit { get; }
        public double Calories { get; }
        public string FoodGroup { get; }

        public Ingredient(string name, double quantity, string unit, double calories, string foodGroup)
        {
            Name = name;
            Quantity = quantity;
            Unit = unit;
            Calories = calories;
            FoodGroup = foodGroup;
        }

        public void ResetQuantity()
        {
            
            Quantity = 0;
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

