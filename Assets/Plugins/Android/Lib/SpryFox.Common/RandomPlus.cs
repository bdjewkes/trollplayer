using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SpryFox;

public static class RandomPlus {
    // returns a random item from the collection; it throws an exception if the collection is empty
    public static T Choose<T>(ICollection<T> collection) {
        if (collection.Count == 0) throw new System.ArgumentOutOfRangeException("collection", "is empty");
        int i = Random.Range(0, collection.Count);
        return collection.ElementAt<T>(i);
    }

    public static T Choose<T>(ICollection<T> collection, RandomGenerator rng) {
        if (collection.Count == 0) throw new System.ArgumentOutOfRangeException("collection", "is empty");
        int i = rng.GetNext(collection.Count);
        return collection.ElementAt<T>(i);
    }

    public static T ChooseListXor<T>(List<T> collection, SpryFox.Common.Xorshift rng) {
        if (collection.Count == 0) throw new System.ArgumentOutOfRangeException("collection", "is empty");
        int i = rng.GetNext(collection.Count);
        return collection[i];
    }

    public static T ChooseListPcg<T>(List<T> collection, SpryFox.Common.Pcg rng) {
        if (collection.Count == 0) throw new System.ArgumentOutOfRangeException("collection", "is empty");
        int i = rng.GetNext(collection.Count);
        return collection[i];
    }

    // returns a random enum value
    public static T ChooseEnum<T>() {
        System.Array values = System.Enum.GetValues(typeof(T));
        int i = Random.Range(0, values.Length);
        return (T)values.GetValue(i);
    }


    // shuffle a list.
    // requires a ToArray() of the list first, but works as an iterator block so 
    // constant-sized allocs only.
    // http://stackoverflow.com/a/1665080/11801
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, 
                                            System.Random rng) {

        T[] elements = source.ToArray();
        // Note i > 0 to avoid final pointless iteration
        for (int i = elements.Length - 1; i > 0; i--)
        {
            // Swap element "i" with a random earlier element it (or itself)
            int swapIndex = rng.Next(i + 1);
            yield return elements[swapIndex];
            elements[swapIndex] = elements[i];
            // we don't actually perform the swap, we can forget about the
            // swapped element because we already returned it.
        }

        // there is one item remaining that was not returned - we return it now
        yield return elements[0]; 
    }

    // returns 0..cardCount in random order
    public static IEnumerable<int> Shuffle(this System.Random rng, int cardCount)
    {
        var cards = Enumerable.Range(0, cardCount);
        var shuffledCards = Shuffle(cards, rng);
        return shuffledCards;
    }

    public static void ShuffleInPlace<T>(this List<T> elements, System.Random rng) {
        for (int i = elements.Count - 1; i > 0; i--) {
            // Swap element "i" with a random earlier element it (or itself)
            int swapIndex = rng.Next(i + 1);
            var tmp = elements[i];
            elements[i] = elements[swapIndex];
            elements[swapIndex] = tmp;
        }
    }

    // picks a random object from a weighted list
    // the chance of picking a particular object from the list is proportional to its weight
    public static T PickRandomWeighted<T>(List<WeightedPair<T>> weights) {
        float sum = 0;
        foreach (var wp in weights) {
            sum += wp.Weight;
        }
        float val = Random.value * sum;
        float accum = 0;
        foreach (var wp in weights) {
            accum += wp.Weight;
            if (accum >= val) {
                return wp.Value;
            }
        }
        return default(T);
    }

    //////////////////////// random number generation functions ///////////////////

    // generates floats evenly distributed in the set [Start, End)
    public static float Range(float start, float end, RandomGenerator generator) {
        return start + generator.GetNextFloat() * (end - start);
    }

    // generates numbers between M - (M*P) and M + (M*P)
    public static float Vary(float mean, float proportion, RandomGenerator generator) {
        float hdev = mean * proportion;
        return mean + (generator.GetNextFloat() * hdev * 2) - hdev;
    }

    // generates numbers in a normal distribution around the mean
    public static float Gaussian(float mean, float stddev, RandomGenerator generator) {
        // this is the polar Box-Muller transform
        float u;
        float v;
        float s;
        while (true) {
            u = generator.GetNextFloat() * 2 - 1;
            v = generator.GetNextFloat() * 2 - 1;
            s = u * u + v * v;
            if (s < 1 && s != 0) break;
        }
        // discarding the spare because we don't want to have any state over here that would
        // carry over to another call after Random.seed changed
        //float spare = v * Mathf.Sqrt(-2.0f * Mathf.Log(s) / s);
        return mean + stddev * u * Mathf.Sqrt(-2.0f * Mathf.Log(s) / s);
    }

    // generates two gaussian-distribution numbers at once
    public static void Gaussian2(float mean, float stddev, RandomGenerator generator, out float f1, out float f2) {
        // this is the polar Box-Muller transform
        float u;
        float v;
        float s;
        while (true) {
            u = generator.GetNextFloat() * 2 - 1;
            v = generator.GetNextFloat() * 2 - 1;
            s = u * u + v * v;
            if (s < 1 && s != 0) break;
        }
        var s_s = Mathf.Sqrt(-2.0f * Mathf.Log(s) / s);
        f1 = mean + stddev * v * s_s;
        f2 = mean + stddev * u * s_s;
    }

    // generates the sum of successes from a binomial experiment, stopping at max.
    // If max == 1, it returns 1 with probability P, 0 with probability P-1
    public static float Binomial(float flipprob, float max, RandomGenerator generator) {
        int outcome = 0;
        for (int i = 0; i < max; i++) {
            if (generator.GetNextFloat() < flipprob) {
                outcome++;
            } else {
                break;
            }
        }
        return (float)outcome;
    }

    // exponential distribution with lambda = 1/invl.
    // invl is the average interarrival time for events in a Poisson process
    public static float Exponential(float invlambda, float minimum, RandomGenerator generator) {
        return Mathf.Max(minimum, -Mathf.Log(generator.GetNextFloat()) * invlambda);
    }
}


// for use with PickRandomWeighted
public class WeightedPair<T> : System.IComparable<WeightedPair<T>> {
    public T Value;
    public float Weight;
    public int CompareTo(WeightedPair<T> other) {
        if (other == null) return 1;
        return other.Weight.CompareTo(Weight); // lowest-to-highest by weight
    }
}


public interface RandomGenerator {
    float GetNextFloat();
    // returns next int >= 0 && < upperBoundExc
    int GetNext(int upperBoundExc);
}

public struct UnityRandomGenerator : RandomGenerator {
    public float GetNextFloat() {
        return Random.value;
    }

    public int GetNext(int upperBoundExc) {
        return Random.Range(0, upperBoundExc);
    }
}

public struct NativeRandomGenerator : RandomGenerator {
    public NativeRandomGenerator(int seed) {
        rng = new System.Random(seed);
    }
    public float GetNextFloat() {
        return (float)rng.NextDouble();
    }

    public int GetNext(int upperBoundExc) {
        return rng.Next(upperBoundExc);
    }

    System.Random rng;
}