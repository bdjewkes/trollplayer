using UnityEngine;
using System.Collections;
using SpryFox.Common;

/// <summary>
/// Class containing parameters for a call to Perlin.FractalNoise.
/// </summary>
public class FractalSimplex {
    public int Seed = 0;
    public float LargestFeature = 10f;
    public int Octaves = 3;
    public float Persistence = 0.7f;
    public float Lacunarity = 2.1245187f;

    public float GetValue(int baseSeed, float x, float y) {
        return Perlin.FractalNoise(x / LargestFeature, y / LargestFeature, baseSeed + Seed, Octaves, Persistence, Lacunarity);
    }
}

public class Perlin {
    // generates a pseudorandom value for a given pair of coordinates
    public static float RandomMix(int x, int y, int seed) {
        int a = (x + y * 10000) + seed;
        a = (a ^ 61) ^ (a >> 16);
        a = a ^ (seed << 10);
        a = a + (a << 3);
        a = a ^ (a >> 4);
        a = a * 0x27d4eb2d;
        a = a ^ (a >> 15);
        a = a & 0xfffff;
        return ((float)a)/0xfffff;
    }

    // uses cosine to do a curved interpolation between two values
    public static float Cirp(float a, float b, float t) {
        float f = (1 - Mathf.Cos(t * Mathf.PI)) * 0.5f;
        return  a*(1-f) + b*f;
    }

    // uses hermite spline to do a curved interpolation between two values
    // this is basically an excuse to have a function named "herp"
    public static float Herp(float a, float b, float t) {
        // http://cubic.org/docs/hermite.htm
        t = Mathf.Clamp01(t);
        float h = (2 * a - 2 * b) * (t*t*t) +
                  (3 * b - 3 * a) * (t*t) +
                  a;
        return h;
    }

    /// <summary>
    /// Generates fractal simplex noise in the range [0, 1].  The noise is guaranteed to be smooth (i.e. have gradual transitions) as the x y values move around in the coordinate system.  The derivative is also guaranteed to be smooth.
    /// </summary>
    /// <param name="x">x coordinate</param>
    /// <param name="y">y coordinate</param>
    /// <param name="seed">seed value provides an arbitrary perturbation; two calls to FractalNoise that differ only in their seed value should produce different results.</param>
    /// <param name="octaves">The fractal bit is that it sums many different scales of noise.  The first octave is the largest, each successive octave is smaller in scale and therefore adds more texture, at the expense of smoothness.</param>
    /// <param name="persistence">Each successive octave adds successively less to the total in order that they don't drown out the base octave's effects.  Graininess increases the higher it goes.  At 1, it's almost the same as just using the smallest octave.  At 0, it's essentially the same as setting octaves = 1.</param>
    /// <param name="lacunarity">This governs the relative scale of subsequent octaves.  The features in octave 2 are 1/lacunarity as large as the features in octave 1.  Generally a value around 2, but not exactly 2, looks good and avoids having axis-aligned artifacts.  (I don't know where the term came from, I think probably Ken Perlin invented it)</param>
    /// <returns></returns>
    public static float FractalNoise(float x, float y, int seed, int octaves, float persistence = 0.7f, float lacunarity = 2.1245187f) {
        float sum = 0;
        for(int i = 0; i < octaves; i++) {
            float noise = 0;
            noise = SimplexSeeded(x, y, seed * i);
            noise = 2 * noise - 1; // remap to [-1, 1]
            sum += noise * Mathf.Pow(lacunarity, -persistence * i);
            x *= lacunarity;
            y *= lacunarity;
        }
        return Mathf.Clamp01((1 + sum) / 2f);
    }

    public static float SimplexSeeded(float x, float y, int seed) {
        return Simplex(x + (seed & 0xFFFF), y + ((seed & 0xFFFF0000) >> 16));
    }

    // http://webstaff.itn.liu.se/~stegu/simplexnoise/simplexnoise.pdf
    public static float Simplex(float x, float y) {
        const float F2 = 0.366025403f; // F2 = 0.5*(Math.sqrt(3.0)-1.0)
        const float G2 = 0.211324865f; // G2 = (3.0-Math.sqrt(3.0))/6.0

        float n0, n1, n2; // Noise contributions from the three corners

        // Skew the input space to determine which simplex cell we're in
        float s = (x+y)*F2; // Hairy factor for 2D
        float xs = x + s;
        float ys = y + s;
        int i = MathPlus.FastFloor(xs);
        int j = MathPlus.FastFloor(ys);

        float t = (float)(i+j)*G2;
        float X0 = i-t; // Unskew the cell origin back to (x,y) space
        float Y0 = j-t;
        float x0 = x-X0; // The x,y distances from the cell origin
        float y0 = y-Y0;

        // For the 2D case, the simplex shape is an equilateral triangle.
        // Determine which simplex we are in.
        int i1, j1; // Offsets for second (middle) corner of simplex in (i,j) coords
        if(x0>y0) {i1=1; j1=0;} // lower triangle, XY order: (0,0)->(1,0)->(1,1)
        else {i1=0; j1=1;}      // upper triangle, YX order: (0,0)->(0,1)->(1,1)

        // A step of (1,0) in (i,j) means a step of (1-c,-c) in (x,y), and
        // a step of (0,1) in (i,j) means a step of (-c,1-c) in (x,y), where
        // c = (3-sqrt(3))/6

        float x1 = x0 - i1 + G2; // Offsets for middle corner in (x,y) unskewed coords
        float y1 = y0 - j1 + G2;
        float x2 = x0 - 1.0f + 2.0f * G2; // Offsets for last corner in (x,y) unskewed coords
        float y2 = y0 - 1.0f + 2.0f * G2;

        // crop the indices to index into perm
        int ii = i & 255;
        int jj = j & 255;

        // Calculate the contribution from the three corners
        float t0 = 0.5f - x0*x0-y0*y0;
        if(t0 < 0.0f) n0 = 0.0f;
        else {
            t0 *= t0;
            n0 = t0 * t0 * SimplexGrad(perm[ii+perm[jj]], x0, y0);
        }

        float t1 = 0.5f - x1*x1-y1*y1;
        if(t1 < 0.0f) n1 = 0.0f;
        else {
            t1 *= t1; 
            n1 = t1 * t1 * SimplexGrad(perm[ii+i1+perm[jj+j1]], x1, y1);
        }

        float t2 = 0.5f - x2*x2-y2*y2;
        if(t2 < 0.0f) n2 = 0.0f;
        else {
            t2 *= t2;
            n2 = t2 * t2 * SimplexGrad(perm[ii+1+perm[jj+1]], x2, y2);
        }

        // Add contributions from each corner to get the final noise value.
        // The result is scaled to return values in the interval [0,1].
        return (0.5f + (17.5f * (n0 + n1 + n2)));
    }

    // simplex noise values
    static readonly byte[] perm = new byte[512] { 151,160,137,91,90,15,
        131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
        190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
        88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
        77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
        102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
        135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
        5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
        223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
        129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
        251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
        49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
        138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180,
        151,160,137,91,90,15,
        131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
        190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
        88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
        77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
        102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
        135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
        5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
        223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
        129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
        251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
        49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
        138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
    };

    static float SimplexGrad( int hash, float x, float y ) {
        int h = hash & 7;       // Convert low 3 bits of hash code
        float u = h<4 ? x : y;  // into 8 simple gradient directions,
        float v = h<4 ? y : x;  // and compute the dot product with (x,y).
        return ((h&1) != 0 ? -u : u) + ((h&2) != 0 ? -2.0f*v : 2.0f*v);
    }

}
