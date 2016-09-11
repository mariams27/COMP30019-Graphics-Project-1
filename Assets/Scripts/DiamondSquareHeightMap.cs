using UnityEngine;
using System.Collections;

/***
 * Generates a height map as a float[,] using the Diamond-Square algorithm.
 */
public class DiamondSquareHeightMap
{

    float[,] data;
    int size;
    float averageHeight;

    public DiamondSquareHeightMap(int recursions, float magnitudeFalloffFactor = 0.5f)
    {
        size = Mathf.FloorToInt(Mathf.Pow(2, recursions + 1) + 1);
        data = new float[size, size];

        data[0, 0] = rValue();
        data[0, size - 1] = rValue();
        data[size - 1, 0] = rValue();
        data[size - 1, size - 1] = rValue();

        // The actual algorithm
        float magnitude = 1;
        for (int stepsize = size; stepsize > 1; stepsize /= 2)
        {
            diamondSquare(stepsize, magnitude);
            magnitude *= magnitudeFalloffFactor;
        }

        // Shift heightmap values from the range [-1, 1] to [0, 1]
        averageHeight = 0f;
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                data[x, y] = (data[x, y] + 1) / 2;
                averageHeight += data[x, y];
            }
        }
        averageHeight /= size * size;
    }

    void diamondSquare(int stepsize, float magnitude)
    {
        // do the diamond and square steps for the given stepsize and magnitude
        int hstep = stepsize / 2;
        for (int x = 0; x + stepsize <= size; x += stepsize)
        {
            for (int y = 0; y + stepsize <= size; y += stepsize)
            {
                // diamond step
                diamondAverage(x + hstep, y + hstep, stepsize, magnitude);

                // square step
                squareAverage(x, y + hstep, stepsize, magnitude);
                squareAverage(x + hstep, y, stepsize, magnitude);
                squareAverage(x + 2 * hstep, y + hstep, stepsize, magnitude);
                squareAverage(x + hstep, y + 2 * hstep, stepsize, magnitude);
            }
        }
    }

    void diamondAverage(int x, int y, int stepsize, float magnitude)
    {
        // sets the value for point (x, y) using the diamond step
        int hs = stepsize / 2;
        set(x, y, magnitude * rValue() + (
            get(x - hs, y - hs) +
            get(x - hs, y + hs) +
            get(x + hs, y + hs) +
            get(x + hs, y - hs)
        ) / 4);
    }

    void squareAverage(int x, int y, int stepsize, float magnitude)
    {
        // sets the value for point (x, y) using the square step
        int hs = stepsize / 2;
        set(x, y, magnitude * rValue() + (
            get(x - hs, y) +
            get(x, y + hs) +
            get(x + hs, y) +
            get(x, y - hs)
        ) / 4);
    }

    float rValue()
    {
        return Random.Range(-1f, 1f);
    }

    // data accessor methods with wrapped indexing

    int wrap(int i)
    {
        i %= size;
        return i < 0 ? i + size : i;
    }

    float get(int x, int y)
    {
        return data[wrap(y), wrap(x)];
    }

    void set(int x, int y, float value)
    {
        data[wrap(y), wrap(x)] = value;
    }

    // public getters

    public float[,] GetData()
    {
        return data;
    }

    public int GetSize()
    {
        return size;
    }

    public float GetAverageHeight()
    {
        return averageHeight;
    }

}