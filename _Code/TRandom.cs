using System.Collections;
using System.Collections.Generic;
using System;

public static class TRandom
{
    public static int RandomIndex(int minValue, int maxValue)
    {
        Random random = new Random();
        int randomIndex = random.Next(minValue, maxValue);

        return randomIndex;
    }

    public static int RandomNumber(int minValue, int maxValue)
    {
        Random random = new Random();
        int randomNumber = random.Next(minValue, maxValue);

        return randomNumber;
    }

    public static T RandomItem<T>(T[] array)
    {
        Random random = new Random();

        T randomItem = array[random.Next(0, array.Length)];

        return randomItem;
    }

    public static T[] RandomItems<T>(T[] array)
    {
        Random random = new Random();

        for (var i = array.Length - 1; i >= 1; i--)
        {
            int j = random.Next(i + 1);

            var temp = array[j];
            array[j] = array[i];
            array[i] = temp;
        }

        return array;
    }
}
