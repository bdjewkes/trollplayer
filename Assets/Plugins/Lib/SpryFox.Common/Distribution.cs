using UnityEngine;
using System.Collections.Generic;
using SpryFox.Common;

using DarkConfig;

public abstract class Distribution {
    public abstract float Generate(RandomGenerator generator);
    public float Generate() {
        return Generate(new UnityRandomGenerator());
    }

    public static Distribution FromDoc(Distribution existing, DocNode doc) {
        // a single number means we want just a fixed value instead of a random distribution
        if (doc.Type == DocNodeType.Scalar) {
            var fixedValue = System.Convert.ToSingle(doc.StringValue);
            if (existing != null && existing is Range) {
                ((Range)existing).Start = fixedValue;
                ((Range)existing).End = fixedValue;
                return existing;
            } else {
                return new Range(fixedValue, fixedValue);
            }
        }

        var first = doc[0].StringValue;
        var resultType = typeof(Range);
        int startingIndex = 1;
        if (first == "vary") {
            resultType = typeof(Vary);
        } else if (first == "gaussian") {
            resultType = typeof(Gaussian);
        } else if (first == "binomial") {
            resultType = typeof(Binomial);
        } else if (first == "exponential") {
            resultType = typeof(Exponential);
        } else if (first == "range") {
            resultType = typeof(Range);
        } else {
            // we haven't found a tag we know about, so assume that the first value is a number and it's a Range
            startingIndex = 0;
        }

        // all these distributions take two floats as arguments, so let's parse those out first
        float firstNum = System.Convert.ToSingle(doc[startingIndex].StringValue);
        float secondNum = System.Convert.ToSingle(doc[startingIndex + 1].StringValue);

        // set the numbers appropriate to the class
        if (resultType == typeof(Range)) {
            if (existing != null && existing is Range) {
                ((Range)existing).Start = firstNum;
                ((Range)existing).End = secondNum;
            } else {
                existing = new Range(firstNum, secondNum);
            }
        } else if (resultType == typeof(Vary)) {
            if (existing != null && existing is Vary) {
                ((Vary)existing).Mean = firstNum;
                ((Vary)existing).Proportion = secondNum;
            } else {
                existing = new Vary(firstNum, secondNum);
            }
        } else if (resultType == typeof(Gaussian)) {
            if (existing != null && existing is Gaussian) {
                ((Gaussian)existing).Mean = firstNum;
                ((Gaussian)existing).StdDev = secondNum;
            } else {
                existing = new Gaussian(firstNum, secondNum);
            }
        } else if (resultType == typeof(Binomial)) {
            if (existing != null && existing is Binomial) {
                ((Binomial)existing).FlipProb = firstNum;
                ((Binomial)existing).Max = secondNum;
            } else {
                existing = new Binomial(firstNum, secondNum);
            }
        } else if (resultType == typeof(Exponential)) {
            if (existing != null && existing is Exponential) {
                ((Exponential)existing).InvLambda = firstNum;
                ((Exponential)existing).Minimum = secondNum;
            } else {
                existing = new Exponential(firstNum, secondNum);
            }
        }
        return existing;
    }
}


[System.Serializable]
public class Range : Distribution {
    public float Start;
    public float End;
    public Range(float start, float end) {
        Set(start, end);
    }

    public float Lerp(float t) {
        return Mathf.Lerp(Start, End, t);
    }

    public void Set(float a, float b) {
        if (a <= b) {
            Start = a;
            End = b;
        } else {
            End = a;
            Start = b;
        }
    }
    public override float Generate(RandomGenerator generator) {
        return RandomPlus.Range(Start, End, generator);
    }
    public override string ToString() {
        return "Range(" + Start + ", " + End + ")";
    }

    public static Range FromDoc(Range existing, DocNode doc) {
        Assert.True(doc.Type == DocNodeType.List,
                    "Range config expected list, got ", doc.Type);
        Assert.True(doc.Count == 2,
                    "Range config expected 2 items, got ", doc.Count);

        float min = System.Convert.ToSingle(doc[0].StringValue);
        float max = System.Convert.ToSingle(doc[1].StringValue);

        if (existing == null) {
            return new Range(min, max);
        } else {
            existing.Start = min;
            existing.End = max;
            return existing;
        }
    }
}


[System.Serializable]
public class Vary : Distribution {
    public float Mean;
    public float Proportion;
    public Vary(float m, float p) {
        Mean = m;
        Proportion = p;
    }
    public override float Generate(RandomGenerator generator) {
        return RandomPlus.Vary(Mean, Proportion, generator);
    }
    public override string ToString() {
        return "Vary(" + Mean + ", " + Proportion + ")";
    }
}

[System.Serializable]
public class Gaussian : Distribution {
    public float Mean;
    public float StdDev;
    public Gaussian(float m, float s) {
        Mean = m;
        StdDev = s;
    }
    public override float Generate(RandomGenerator generator) {
        return RandomPlus.Gaussian(Mean, StdDev, generator);
    }
}

[System.Serializable]
public class Binomial : Distribution {
    public float FlipProb;
    public float Max;
    public Binomial(float p, float m) {
        FlipProb = p;
        Max = m;
    }
    public override float Generate(RandomGenerator generator) {
        return RandomPlus.Binomial(FlipProb, Max, generator);
    }
}

[System.Serializable]
public class Exponential : Distribution {
    public float InvLambda;
    public float Minimum;
    public Exponential(float invl, float min) {
        InvLambda = invl;
        Minimum = min;
    }
    public override float Generate(RandomGenerator generator) {
        return RandomPlus.Exponential(InvLambda, Minimum, generator);
    }
}
