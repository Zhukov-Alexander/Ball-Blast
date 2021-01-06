using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LibraryOfUsefulAlgorithms
{
    public static class FirstPage
    {
        static Random random = new Random();

        //возвращает лист определенного размера, заполненный связками индекс-тип
        public static List<int> GetListOfTypedIndexes(List<float> typeProbabilities, int baseAmount, out int outputAmount)
        {
            List<int> output = new List<int>();
            for (int i = 0; i < baseAmount; )
            {
                float sumOfProbabilities = typeProbabilities.Sum();
                float threshold = (float)random.NextDouble() * sumOfProbabilities;
                float accumulatedValue = 0;
                for (int a = 0; a < typeProbabilities.Count; a++)
                {
                    accumulatedValue += typeProbabilities[a];
                    if (accumulatedValue >= threshold)
                    {
                        output.Add(a);
                        i += TreeOfPow(2,a);
                    }
                }
            }
            outputAmount = output.Count;
            return output;
        }
        //возвращает лист с усредненными значениями в зависимости параметра размеров
        public static List<float> GetListWithAveragedValues(List<float> initialList, List<int> sizes, out float averageValue)
        {
            List<float> output = new List<float>();
            int b = 0;
            for (int i = 0; i < sizes.Count; i++)
            {
                List<float> avr = new List<float>();
                for (int a = 0; a <= sizes[i]; a++)
                {
                    avr.Add(initialList[b]);
                    b++;
                }
                output.Add(avr.Average());
            }
            averageValue = output.Average();
            return output;
        }
        //определяет количество элементов дерева возведения числа в степень
        public static int TreeOfPow(int baseInt, int powerInt)
        {
            int output = 0;
            for (int b = 0; b <= powerInt; b++)
            {
                output += (int)Math.Pow(baseInt, b);
            }
            return output;
        }

        //выдает определенное количество случайных объектов на основе списка объектов
        public static List<T> GetRandomObjectsFromList<T>(List<T> objects, int amount = 1)
        {
            List<T> objectsCopy = objects.Copy();
            if (amount < 1 || amount > objectsCopy.Count)
            {
                throw new Exception("wrong amount");
            }

            List<T> output = new List<T>();
            for (int i = 0; i < amount; i++)
            {
                int index = random.Next(objectsCopy.Count);
                output.Add(objectsCopy[index]);
                objectsCopy.RemoveAt(index);
            }
            return output;
        }

        //выдает лист определенного размера с индексами в значении
        public static List<int> GetListOfIndexes(int size)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < size; i++)
            {
                list.Add(i);
            }
            return list;
        }
        //выдает лист определенного размера с базовыми значениями
        public static List<T> GetListOfBaseValues<T>(int size) where T : new()
        {
            List<T> list = new List<T>();
            for (int i = 0; i < size; i++)
            {
                list.Add(new T());
            }
            return list;
        }

        //преобразует лист пар ключ-значение в листы ключей и значений
        public static void ConvertListOfKeyValuePairsToLists<T, V>(this List<KeyValuePair<T, V>> listOfKeyValuePairs, out List<T> keys, out List<V> values)
        {
            keys = new List<T>();
            values = new List<V>();
            foreach (KeyValuePair<T, V> pair in listOfKeyValuePairs)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        //преобразует лист в лист пар индекс-значение
        public static void ConvertListToListOfIndexValuePairs<V>(this List<V> list, out List<KeyValuePair<int, V>> listOfKeyValuePairs)
        {
            listOfKeyValuePairs = new List<KeyValuePair<int, V>>();
            for (int i = 0; i < list.Count; i++)
            {
                listOfKeyValuePairs.Add(new KeyValuePair<int, V>(i, list[i]));
            }
        }

        //преобразует листы ключей и значений в лист пар ключ-значение
        public static void ConvertListsToListOfKeyValuePairs<T, V>(this List<T> keys, List<V> values, out List<KeyValuePair<T, V>> listOfKeyValuePairs)
        {
            if (keys.Count != values.Count)
            {
                throw new Exception("different amount of keys and values");
            }

            listOfKeyValuePairs = new List<KeyValuePair<T, V>>();
            for (int i = 0; i < keys.Count; i++)
            {
                listOfKeyValuePairs.Add(new KeyValuePair<T, V>(keys[i], values[i]));
            }
        }

        //преобразует размер листа и пропорции типов в лист типов значений
        public static List<int> GetListOfIndexTypePairs(int count, List<float> typeProportiones) 
        {
            List<int> list = GetListOfIndexes(count);
            List<int> output = new List<int>(new int[count]);
            int currentCount = 0;
            for (int i = 0; i < typeProportiones.Count; i++)
            {
                int partCount = (int)Math.Ceiling(typeProportiones[i] * list.Count);
                for (int b = 0; b < partCount; b++)
                {
                    if (currentCount >= list.Count)
                        break;

                    int index = random.Next(list.Count);
                    output[list[index]] = i;
                    list.RemoveAt(index);
                    currentCount++;
                }
            }
            return output;
        }
        //делит лист классов на части из случайно выбранных значений в зависимости от указанных пропорций
        public static List<List<T>> DivideListIntoParts<T>(List<T> list, List<float> partsProportiones) 
        {
            List<T> listCopy = list.Copy();
            List<List<T>> output = new List<List<T>>();
            int currentCount = 0;
            for (int i = 0; i < partsProportiones.Count; i++)
            {
                List<T> part = new List<T>();
                int partCount = (int)Math.Ceiling(partsProportiones[i] * list.Count);
                for (int b = 0; b < partCount; b++)
                {
                    int index = random.Next(listCopy.Count);
                    part.Add(listCopy[index]);
                    listCopy.RemoveAt(index);
                    currentCount++;
                    if (currentCount >= list.Count)
                        break;
                }
                output.Add(part);
            }
            return output;
        }
        //делит число на части в зависимости от указанных пропорций
        public static List<int> DivideNumberIntoParts(int number, List<float> partsProportiones)
        {
            List<int> output = new List<int>();
            int currentNumber = 0;
            for (int i = 0; i < partsProportiones.Count; i++)
            {
                int amountOfBricksByTypeI = (int)Math.Floor(partsProportiones[i] * number);
                output.Add(amountOfBricksByTypeI);
                currentNumber += amountOfBricksByTypeI;
            }
            if (currentNumber < number)
            {
                int maxValue = output.Max();
                int maxIndex = output.IndexOf(maxValue);
                output[maxIndex] += number - currentNumber;
            }
            return output;
        }

        //сравнивает листы пар объект-количество и возвращает объекты первого листа, количество которых меньше, чем на втором листе
        public static List<T> GetUnfilledObjects<T>(List<KeyValuePair<T, float>> currentList, List<KeyValuePair<T, float>> totalList)
        {
            if (currentList.Count != totalList.Count)
                throw new Exception("lists have different length");
            for(int i = 0; i < currentList.Count; i++)
            {              
                if (EqualityComparer<T>.Default.Equals(currentList[i].Key, totalList[i].Key))
                {
                    throw new Exception("lists have different objects");
                }
            }
            return (from a in currentList
                    from b in totalList
                    where a.Value < b.Value
                    select a.Key).ToList();
        }

        //возвращает лист с усредненными значениями
        public static List<float> GetListWithEqualValues(float totalValue, int lengh, out float mediumIndexValue)
        {
            List<float> output = new List<float>();
            mediumIndexValue = totalValue / lengh;
            for (int i = 0; i < lengh; i++)
            {
                output.Add(mediumIndexValue);
            }
            return output;
        }

        //возвращает лист с линейно возрастающими значениями с учетом их относительного веса
        public static List<float> GetListWithLinearlyIncreasingWeightedValues(float totalValue, int size, float variationCoef)
        {
            List<float> output = new List<float>();
            float indexValue = totalValue / size;
            float variation = indexValue * variationCoef;
            for (int i = 0; i < size; i++)
            {
                output.Add(indexValue - variation + variation / size * 2 * i);
            }
            return output;
        }

        //возвращает лист с перемешанными значениями
        public static List<float> MixValues(List<float> valuesToMix, int amountOfValuesToMixSimultaneosly, float mixCoef, int mixCycles = 1)
        {
            int amountOfGroupsToMix = (int)Math.Ceiling((float)valuesToMix.Count / amountOfValuesToMixSimultaneosly);
            List<float> output = new List<float>();
            for (int i = 0; i < amountOfGroupsToMix; i++)
            {
                List<float> GroupToMix = new List<float>();
                for (int a = 0; a < amountOfValuesToMixSimultaneosly; a++)
                {
                    if (i * amountOfValuesToMixSimultaneosly + a >= valuesToMix.Count)
                        break;
                    GroupToMix.Add(valuesToMix[i * amountOfValuesToMixSimultaneosly + a]);
                }
                for (int b = 0; b < mixCycles; b++)
                {
                    for (int c = 0; c < GroupToMix.Count; c++)
                    {
                        float healthToMix = mixCoef / mixCycles * GroupToMix[c];
                        GroupToMix[c] -= healthToMix;
                        GroupToMix[random.Next(0, GroupToMix.Count)] += healthToMix;
                    }
                }
                output.AddRange(GroupToMix);
            }
            return output;
        }
        //возвращает ряд случайных значений в пределах указанного размера и значений
        public static List<int> GetListOfRandomInts(int count, int min, int max)
        {
            List<int> output = new List<int>();
            for (int i = 0; i < count; i++)
            {
                int number = random.Next(min, max+1);
                output.Add(number);
            }
            return output;
        }

    }
}
