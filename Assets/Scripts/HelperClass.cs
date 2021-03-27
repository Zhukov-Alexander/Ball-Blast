using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public static class HelperClass
{
    static Random random = new Random();
    public static float RandomFloat(float min, float max)
    {
        double range = max - min;
        double sample = random.NextDouble();
        double scaled = (sample * range) + min;
        return (float)scaled;
    }
    public static Vector3 Multiply(this Vector3 multiplied, Vector3 multiplier)
    {
        return new Vector3(multiplied.x * multiplier.x, multiplied.y * multiplier.y, multiplied.z * multiplied.z);
    }
    public static Vector3 GetVector3(float xyz)
    {
        return new Vector3(xyz, xyz, xyz);
    }
    public static Vector2 RandomDirecton()
    {
        return new Vector2(RandomFloat(-1, 1), RandomFloat(-1, 1)).normalized;
    }
    public static bool TryGetComponentsFromChildren<T>(this GameObject parent, out List<T> components, bool includeParent = true)
    {
        return TryGetComponents(GetChildren(parent, includeParent), out components);
    }
    public static List<GameObject> GetChildren(this GameObject parent, bool includeParent = true)
    {
        List<GameObject> gameObjects = new List<GameObject>();
        gameObjects = parent.GetComponentsInChildren<Transform>().Select(a => a.gameObject).ToList();
        if (!includeParent) gameObjects.Remove(parent);
        return gameObjects;
    }
    public static bool TryGetComponents<T>(List<GameObject> gameObjects, out List<T> components)
    {
        components = new List<T>();
        foreach (var item in gameObjects)
        {
            if (item.TryGetComponent(out T component))
                components.Add(component);
        }
        if (components.Count == 0)
            return false;
        else
            return true;
    }

    public static void Change(this ref bool input)
    {
        if (input)
            input = false;
        else
            input = true;
    }
    public static List<T> GetComponentsInChildrenNonRecursive<T>(this GameObject parent) where T : class
    {
        if (parent.transform.childCount <= 0) return null;

        var output = new List<T>();

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            var component = parent.transform.GetChild(i).GetComponent<T>();
            if (component != null)
                output.Add(component);
        }
        if (output.Count > 0)
            return output.ToList();

        return null;
    }
    public static Vector3 GetTouchPoint(this Touch touch)
    {
        Ray ray = Camera.current.ScreenPointToRay(touch.position);
        new Plane(Vector3.up, 0).Raycast(ray, out float enter);
        return ray.GetPoint(enter);
    }
    //сравнивает doubles с погрешностью
    public static bool NearlyEqual(double a, double b, double epsilon)
    {
        double diff = Math.Abs(a - b);
        return diff < epsilon;
    }
    public static string MultiplyerToPercent(float multiplyer)
    {
        return ((int)(multiplyer*100)).ToString() + "%";
    }
    public static Vector2 VectorFromAngle(float angle, bool bUseRadians = false)
    {
        return RotateBy(Vector2.up, angle, bUseRadians);
    }
    public static Vector2 RotateBy(Vector2 initialVector, float angle, bool bUseRadians = false)
    {
        if (!bUseRadians) angle *= Mathf.Deg2Rad;
        var ca = Math.Cos(angle);
        var sa = Math.Sin(angle);
        var rx = initialVector.x * ca - initialVector.y * sa;
        return new Vector2((float)rx, (float)(initialVector.x * sa + initialVector.y * ca));
    }
    //возвращает лист x или y значений из листа вектор2
    public static List<float> ToListX(this List<Vector2> vector2s)
    {
        List<float> output = new List<float>();
        vector2s.ForEach(a => output.Add(a.x));
        return output;
    }
    public static List<float> ToListY(this List<Vector2> vector2s)
    {
        List<float> output = new List<float>();
        vector2s.ForEach(a => output.Add(a.y));
        return output;
    }

    //возвращает лист связок индекс-тип на основе общего веса и веса индексов различного типа
    public static List<int> GetListOfTypedIndexes(List<float> typeProbabilities, List<float> typeAmounts, float wholeAmount)
    {
        List<int> output = new List<int>();
        for (float i = 0; i < wholeAmount; )
        {
            float threshold = (float)random.NextDouble() * typeProbabilities.Sum();
            float accumulatedValue = 0;
            for (int a = 0; a < typeProbabilities.Count; a++)
            {
                accumulatedValue += typeProbabilities[a];
                if (accumulatedValue >= threshold)
                {
                    output.Add(a);
                    i += typeAmounts[a];
                    break;
                }
            }
        }
        return output;
    }
    public static Func<int, int> BasedTreeOfPow(int baseInt)
    {
        return (i) => TreeOfPow(baseInt, (int)i);
    }
    //возвращает лист весов типов на основании количества типов и метода их расчета
    public static List<float> GetTypeWeights(int amountOfTypes, Func<int, int> calcFunc)
    {
        List<float> output = new List<float>();
        for (int i = 0; i < amountOfTypes; i++)
        {
            output.Add(calcFunc(i));
        }
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
                if (b >= initialList.Count) break;
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

        List<T> output = new List<T>();
        for (int i = 0; i < amount; i++)
        {
            int index = random.Next(objectsCopy.Count);
            output.Add(objectsCopy[index]);
            //objectsCopy.RemoveAt(index);
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

    //возвращает лист с линейно возрастающими значениями с учетом их суммы, количества и вариации
    public static List<float> GetListWithLinearlyIncreasingValuesBasedOnTotalValue(float totalValue, int size, float variationCoef)
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
    //возвращает лист с линейно возрастающими значениями с учетом их начального значения, количества и вариации
    public static List<float> GetListWithLinearlyIncreasingValuesBasedOnStartValue(float startValue, int size, float variationCoef)
    {
        List<float> output = new List<float>();
        float variation = startValue * variationCoef;
        for (int i = 0; i < size; i++)
        {
            output.Add(startValue + variation / size * i);
        }
        return output;
    }
    //умное округление
    public static List<int> SmartRound(List<float> values, int totalValue)
    {
        for (int i = 0; i < values.Count; i++)
        {
            if (values.Sum() >= totalValue)
                values[i] = Mathf.FloorToInt(values[i]);
            else
                values[i] = Mathf.CeilToInt(values[i]);
        }
        List<int> output = new List<int>();
        values.ForEach(a => output.Add((int)a));
        if (output.Sum() != totalValue) throw new Exception("smartRoundFailed: sum of values is " + output.Sum() + " while total value is " + totalValue);
        return output;
    }
    //создать лист определенного размера
    public static List<T> GetListOfSize<T>(int count)
    {
        List<T> ret = new List<T>(count);
        ret.AddRange(Enumerable.Repeat(default(T), count));
        return ret;
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
