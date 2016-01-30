
using UnityEngine;
using System;
using System.Linq;
using System.Text;

namespace SpryFox.Common {
    
public class Assert
{
    private const string s_assertGuard = "ASSERTS_ENABLED";

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void True(bool test, object msg1) {
        if (test == false) {
            Fail(msg1.ToString());
        }
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void True(bool test, object msg1, object msg2) {
        if (test == false) {
            Fail(msg1.ToString() + " " + msg2.ToString());
        }
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void True(bool test, object msg1, object msg2, object msg3) {
        if (test == false) {
            var sb = new StringBuilder();
            sb.Append(msg1); sb.Append(" ");
            sb.Append(msg2); sb.Append(" ");
            sb.Append(msg3);
            Fail(sb.ToString());
        }
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void True(bool test, object msg1, object msg2, object msg3, object msg4) {
        if (test == false) {
            var sb = new StringBuilder();
            sb.Append(msg1); sb.Append(" ");
            sb.Append(msg2); sb.Append(" ");
            sb.Append(msg3); sb.Append(" ");
            sb.Append(msg4);
            Fail(sb.ToString());
        }
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void True(bool test, object msg1, object msg2, object msg3, object msg4, object msg5) {
        if (test == false) {
            var sb = new StringBuilder();
            sb.Append(msg1); sb.Append(" ");
            sb.Append(msg2); sb.Append(" ");
            sb.Append(msg3); sb.Append(" ");
            sb.Append(msg4); sb.Append(" ");
            sb.Append(msg5);
            Fail(sb.ToString());
        }
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void True(bool test, object msg1, object msg2, object msg3, object msg4, object msg5, object msg6) {
        if (test == false) {
            var sb = new StringBuilder();
            sb.Append(msg1); sb.Append(" ");
            sb.Append(msg2); sb.Append(" ");
            sb.Append(msg3); sb.Append(" ");
            sb.Append(msg4); sb.Append(" ");
            sb.Append(msg5); sb.Append(" ");
            sb.Append(msg6);
            Fail(sb.ToString());
        }
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void True(bool test, object msg1, object msg2, object msg3, object msg4, object msg5, object msg6, object msg7) {
        if (test == false) {
            var sb = new StringBuilder();
            sb.Append(msg1); sb.Append(" ");
            sb.Append(msg2); sb.Append(" ");
            sb.Append(msg3); sb.Append(" ");
            sb.Append(msg4); sb.Append(" ");
            sb.Append(msg5); sb.Append(" ");
            sb.Append(msg6); sb.Append(" ");
            sb.Append(msg7);
            Fail(sb.ToString());
        }
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void True(bool test, object msg1, object msg2, object msg3, object msg4, object msg5, object msg6, object msg7, object msg8) {
        if (test == false) {
            var sb = new StringBuilder();
            sb.Append(msg1); sb.Append(" ");
            sb.Append(msg2); sb.Append(" ");
            sb.Append(msg3); sb.Append(" ");
            sb.Append(msg4); sb.Append(" ");
            sb.Append(msg5); sb.Append(" ");
            sb.Append(msg6); sb.Append(" ");
            sb.Append(msg7); sb.Append(" ");
            sb.Append(msg8);
            Fail(sb.ToString());
        }
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void True(bool test, params object[] messages)
    {
        if(test == false)
        {
            Fail(messages);
        }
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void False(bool test, params object[] messages)
    {
        if(test)
        {
            Fail(messages);
        }
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void ValueInRange(int value, 
                                    int minInclusive, 
                                    int maxExclusive,
                                    string msg) {
        
        bool isOutOfRange = value < minInclusive || value >= maxExclusive;
        if (isOutOfRange)
        {
            Fail("value ", value, " not in range [", minInclusive, ", ", maxExclusive, 
                 ")\n", msg);
        }
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void ValueInRange<T>(T value, T minInclusive, T maxExclusive) 
        where T : IComparable<T>
    {   
        bool compare = minInclusive.CompareTo(value) <= 0 && maxExclusive.CompareTo(value) > 0;
        if(!compare) {
            True(compare, 
                 "value ", value, " not in range [", minInclusive, ", ", maxExclusive, ")");
        }
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void ValueInRange<T>(T value, T minInclusive, T maxExclusive,
                                       params object[] messages) 
        where T : IComparable<T>
    {
        bool compare = minInclusive.CompareTo(value) <= 0 && maxExclusive.CompareTo(value) > 0;
        if(!compare) {
            var sb = new StringBuilder();
            foreach(var msg in messages) {
                sb.Append(msg);
            }
            True(compare,
                 "value ", value, " not in range [", minInclusive, ", ", maxExclusive, ")\n",
                 sb);
        }
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void IsNotNull(System.Object obj, object msg1)
    {
        Assert.True(obj != null, msg1);
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void IsNotNull(System.Object obj, object msg1, object msg2)
    {
        Assert.True(obj != null, msg1, msg2);
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void IsNotNull(System.Object obj, params object[] msgs)
    {
        Assert.True(obj != null, msgs);
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void IsNull(System.Object obj, params object[] msgs)
    {
        Assert.True(obj == null, msgs);
    }


    // tests for qnans
    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void IsValid(float f, params object[] msgs)
    {
        Assert.True(float.IsNaN(f) == false, msgs);
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void IsValid(float f, object msg)
    {
        Assert.True(float.IsNaN(f) == false, msg);
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void IsValid(float f, object msg1, object msg2)
    {
        Assert.True(float.IsNaN(f) == false, msg1, msg2);
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void IsValid(float f, object msg1, object msg2, object msg3)
    {
        Assert.True(float.IsNaN(f) == false, msg1, msg2, msg3);
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void IsValid(float f, object msg1, object msg2, object msg3, object msg4)
    {
        Assert.True(float.IsNaN(f) == false, msg1, msg2, msg3, msg4);
    }

    // tests for qnans
    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void IsValid(Vector3 v, params object[] msgs)
    {
        Assert.IsValid(v.x, msgs);
        Assert.IsValid(v.y, msgs);
        Assert.IsValid(v.z, msgs);
    }

    // tests for qnans
    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void IsValid(Vector2 v, params object[] msgs)
    {
        bool isValid = float.IsNaN(v.x) == false && float.IsNaN(v.y) == false;
        Assert.True(isValid, msgs);
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void IsValid(Vector2 v, object msg)
    {
        bool isValid = float.IsNaN(v.x) == false && float.IsNaN(v.y) == false;
        Assert.True(isValid, msg);
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void IsValid(Vector2 v, object msg1, object msg2)
    {
        bool isValid = float.IsNaN(v.x) == false && float.IsNaN(v.y) == false;
        Assert.True(isValid, msg1, msg2);
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void IsValid(Vector2 v, object msg1, object msg2, object msg3)
    {
        bool isValid = float.IsNaN(v.x) == false && float.IsNaN(v.y) == false;
        Assert.True(isValid, msg1, msg2, msg3);
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void IsValid(Vector2 v, object msg1, object msg2, object msg3, object msg4)
    {
        bool isValid = float.IsNaN(v.x) == false && float.IsNaN(v.y) == false;
        Assert.True(isValid, msg1, msg2, msg3, msg4);
    }


    // tests for qnans
    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void IsValid(Quaternion q, params object[] msgs)
    {
        Assert.IsValid(q.x, msgs);
        Assert.IsValid(q.y, msgs);
        Assert.IsValid(q.z, msgs);
        Assert.IsValid(q.w, msgs);
    }

    // tests for qnans
    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void IsValid(Transform t, params object[] msgs)
    {
        Assert.IsValid(t.position, msgs);
        Assert.IsValid(t.rotation, msgs);
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void Fail(params object[] messages)
    {
        // concatenate the messages here, when we know the test has failed
        string message = string.Join(" ", messages.Select(x => x.ToString()).ToArray());
        Fail(message);
    }

    [System.Diagnostics.Conditional(s_assertGuard)]
    public static void Fail(string message) {
        if (Application.isEditor || Application.isPlaying == false) {
            // exceptions are best because they abort execution immediately,
            // but can't be depended on in a build. 
            throw new UnityException(message);
        } else {
            Debug.LogError(message);
            // debug break aborts at the end of the frame,
            // won't get you out of an inifinite loop
            Debug.Break();
        }
    }
}

}

