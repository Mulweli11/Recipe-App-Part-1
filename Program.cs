using System;

namespace RecipeApp
{
    class Program
    {
        static void Main(string[] args)
        {
           
            Console.WriteLine("*****Welcome to RecipeApp!******");
            Console.WriteLine("_________________________________");
            Recipe recipe = GetRecipeDetailsFromUser();
            DisplayRecipe(recipe);

            Console.WriteLine("Enter the scaling factor (0.5, 2, or 3):");
            string input = Console.ReadLine();
            if (double.TryParse(input, out double scale))
            {
                if (scale == 0.5 || scale == 2 || scale == 3)
                {
                    recipe.ScaleIngredients(scale);
                    DisplayRecipe(recipe);
                }
                else
                {
                    Console.WriteLine("Invalid scaling factor. Please enter 0.5, 2, or 3.");
                }
            }
            else if (input.ToLower() == "reset")
            {
                recipe.ResetQuantities(recipe.OriginalQuantities);
                DisplayRecipe(recipe);
            }

            else
            {
                Console.WriteLine("Invalid input.");
            }

            Console.WriteLine("Enter 'reset' to reset quantities:");
            string input1 = Console.ReadLine();
            if (input1.ToLower() == "reset")
            {
                recipe.ResetQuantities(recipe.OriginalQuantities);
                DisplayRecipe(recipe);
            }
            
            
                Console.WriteLine("Press any key to clear data and enter a new recipe...");
                Console.ReadKey();
                Console.Clear();
                Main(args); // Recursive call to start over
            
        }

        static Recipe GetRecipeDetailsFromUser()
        {
            Console.WriteLine("Enter the number of ingredients:");
            Console.WriteLine("_________________________________");
            int numIngredients = int.Parse(Console.ReadLine());
            Ingredient[] ingredients = new Ingredient[numIngredients];

            for (int i = 0; i < numIngredients; i++)
            {
                Console.WriteLine($"Enter the name of ingredient {i + 1}:");
                string name = Console.ReadLine();

                Console.WriteLine($"Enter the quantity of {name}:");
                double quantity = double.Parse(Console.ReadLine());

                Console.WriteLine($"Enter the unit of measurement for {name}:");
                string unit = Console.ReadLine();

                ingredients[i] = new Ingredient(name, quantity, unit);
            }

            Console.WriteLine("Enter the number of steps:");
            int numSteps = int.Parse(Console.ReadLine());
            Step[] steps = new Step[numSteps];

            for (int i = 0; i < numSteps; i++)
            {
                Console.WriteLine($"Enter step {i + 1}:");
                string description = Console.ReadLine();
                steps[i] = new Step(description);
            }

            return new Recipe(ingredients, steps);
        }

        static void DisplayRecipe(Recipe recipe)
        {
            Console.WriteLine("|");
            
            Console.WriteLine("_______________________________________");
            Console.WriteLine("******Recipe:******");
            Console.WriteLine("_______________________________________");
            Console.WriteLine("Ingredients:");
            foreach (var ingredient in recipe.Ingredients)
            {
                Console.WriteLine($"{ingredient.Quantity} {ingredient.Unit} of {ingredient.Name}");
            }
            Console.WriteLine("Steps:");
            for (int i = 0; i < recipe.Steps.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {recipe.Steps[i].Description}");
            }
            Console.WriteLine("_______________________________________");
        }
    }

    class Recipe
    {
        public Ingredient[] Ingredients { get; set; }
        public Step[] Steps { get; set; }
        public double[] OriginalQuantities { get; set; }

        public Recipe(Ingredient[] ingredients, Step[] steps)
        {
            Ingredients = ingredients;
            Steps = steps;
            OriginalQuantities = new double[ingredients.Length];
            for (int i = 0; i < ingredients.Length; i++)
            {
                OriginalQuantities[i] = ingredients[i].Quantity;
            }
        }

        public void ScaleIngredients(double factor)
        {
            foreach (var ingredient in Ingredients)
            {
                ingredient.Quantity *= factor;
            }
        }

        public void ResetQuantities(double[] originalQuantities)
        {
            for (int i = 0; i < Ingredients.Length; i++)
            {
                Ingredients[i].Quantity = originalQuantities[i];
            }
        }
    }

    class Ingredient
    {
        public string Name { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }

        public Ingredient(string name, double quantity, string unit)
        {
            Name = name;
            Quantity = quantity;
            Unit = unit;
        }
    }

    class Step
    {
        public string Description { get; set; }

        public Step(string description)
        {
            Description = description;
        }
    }
}