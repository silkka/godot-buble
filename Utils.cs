using System;
using System.Collections.Generic;

class Utils
{
    // Method to pick a random element from a list
    public static T PickRandomElement<T>(Array array)
    {
        // Check if the list is not empty
        if (array.Length == 0)
        {
            throw new InvalidOperationException("The list is empty.");
        }

        // Generate a random index
        Random random = new Random();
        int randomIndex = random.Next(array.Length);

        // Return the random element
        return (T)array.GetValue(randomIndex);
    }
}
