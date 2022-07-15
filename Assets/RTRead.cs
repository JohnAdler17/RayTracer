using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Text.RegularExpressions;
using System.IO;



public class RTRead : MonoBehaviour
{
    string[] entries; // Will contain the entries of the read in file (to be Parsed)

    private Texture2D myImage;

    ArrayList sphereList = new ArrayList(); // List of spheres

    int screenWidth; // Width of the image to be read in from file

    int screenHeight; // Height of the image to be read in from file

    Color backgroundColor; // Background color of the image to be read in from file

    // A struct to hold some of the ray tracing information
    public struct record
    {
        public double t;
        public Vector3 normal;
        public Color color;
    }

    public record Rec;

    public InputField FileRead;

    public InputField Save;


    // Start is called before the first frame update
    // Sets Texture2D myImage to a new texture before the first frame update
    void Start()
    {
        myImage = new Texture2D((int)(Screen.width), (int)(Screen.height));
    }

    // Draws a blank texture when the program is started
    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, myImage.width, myImage.height), myImage);
    }

    // Reads the input from the user when the "Open" button is pressed
    public void readInput()
    {
        sphereList.Clear();
        string filePath;
        filePath = FileRead.text;
        ReadIn(filePath);
        RayTraceImage();
        
    }


    // Save input not functional
    public void saveInput()
    {
        string[] ppmvalues;
        int i = 0;
        string path;
        path = Save.text;
        string fullPath = @"C: \Users\nmnm2\Downloads\WindowsFilesSAMPLE_CLI\" + path;
        using (StreamWriter writetext = new StreamWriter(fullPath))
        {
            for (int x = 0; x < screenWidth; x++)
            {
                for (int y = 0; y < screenHeight; y++)
                {
                    //ppmvalues[i] = string.Format(myImage.GetPixel(x, y).r);// convert float to string
                    i++;
                    //ppmvalues[i] = (String)myImage.GetPixel(x, y).g;
                    i++;
                    //ppmvalues[i] = (String)myImage.GetPixel(x, y).b;
                }
                
            }
        }

    }

    // Shoots a ray through each pixel and sets the appropriate color of the pixel
    void RayTraceImage()
    {
        Vector3 direction = new Vector3(0, 0, -1);
        Color acolor;
        for (int i = 0; i < screenWidth; i++)
        {
            for (int j = 0; j < screenHeight; j++)
            {
                Vector3 origin = new Vector3(i, j, 0);
                acolor = trace(origin, direction);
                myImage.SetPixel(i, j, acolor);
            }
        }
        myImage.Apply();
    }

    // Determines the ambient color of a sphere
    Color trace(Vector3 origin, Vector3 direction)
    {
        float t0 = 0.001f;
        float t1 = 100000;
        double minsofar = 100000000;
       // float currmin = minsofar;
        bool hit = false;
        //Color* myhit = null;
        Color myColor = new Color();
        
        for (int x = 0; x < sphereList.Count; x++)
        {
            
            if (findHit(x, origin, direction, t0, t1, Rec))
            {

                hit = true;
            }
            if ((hit == true) && (minsofar > Rec.t))
            {
                myColor = ((Sphere)sphereList[x]).aColor;
                minsofar = Rec.t;

            }
        }
        if (hit == true)
        {
            return myColor;
        }
        return backgroundColor;
    
    }

    // Finds a hit between the current sphere and the ray
    bool findHit(int iteration, Vector3 o, Vector3 direction, float t0, float t1, record rec)
    {
        double A = Vector3.Dot(direction, direction);
        double B = 2 * Vector3.Dot(direction, o - ((Sphere)sphereList[iteration]).center);
        double C = Vector3.Dot(o - ((Sphere)sphereList[iteration]).center, o - ((Sphere)sphereList[iteration]).center) - ((Sphere)sphereList[iteration]).radius * ((Sphere)sphereList[iteration]).radius;
        double discrim = B * B - 4 * A * C;

        if (discrim >= 0)
        {
            double sqrtd = Math.Sqrt(discrim);
            double t = (-B - sqrtd)/(2 * A);
            if (t < t0)
            {
                t = (-B - sqrtd) / (2 * A);
            }
            if (t < t0 || t > t1)
            {
                return false;
            }
            rec.t = t;
            //rec.normal = unitVector(origin + t * direction - center);
            rec.color = ((Sphere)sphereList[iteration]).aColor;
            return true;
        }
        else
        {
            return false;
        }
    }


    public void ReadIn(string filePath)
    {
        Debug.Log("Start");
        string allText = System.IO.File.ReadAllText(@"C: \Users\nmnm2\Downloads\WindowsFilesSAMPLE_CLI\" + filePath);
        string replaceText = Regex.Replace(allText, "[^a-zA-Z0-9% .+-_]", " "); //replace all non-word characters with spaces 
        string[] entries = replaceText.Split(new string[] {" "}, System.StringSplitOptions.RemoveEmptyEntries);
        for (int x = 0; x < entries.Length; x++)
        {
            Debug.Log(entries[x]); // prints out all entries in the input file to the debug log
        }
        screenWidth = int.Parse(entries[1]);
        screenHeight = int.Parse(entries[2]);

        myImage = new Texture2D((int)(screenWidth), (int)(screenHeight));

        float backgroundRed = float.Parse(entries[4]);
        float backgroundGreen = float.Parse(entries[5]);
        float backgroundBlue = float.Parse(entries[6]);
        backgroundColor = new Color(backgroundRed, backgroundGreen, backgroundBlue);

        Sphere s1 = new Sphere();
        Sphere s2 = new Sphere();
        Sphere s3 = new Sphere();
        Sphere s4 = new Sphere();
        Sphere s5 = new Sphere();
        Sphere s6 = new Sphere();
        Sphere s7 = new Sphere();

        int radius = 0;

        int centerX = 0;
        int centerY = 0;
        int centerZ = 0;

        float aRed = 0.0f;
        float aGreen = 0.0f;
        float aBlue = 0.0f;

        int rRed = 0;
        int rGreen = 0;
        int rBlue = 0;

        radius = int.Parse(entries[8]);

        centerX = int.Parse(entries[9]);
        centerY = int.Parse(entries[10]);
        centerZ = int.Parse(entries[11]);

        aRed = float.Parse(entries[12]);
        aGreen = float.Parse(entries[13]);
        aBlue = float.Parse(entries[14]);

        rRed = int.Parse(entries[15]);
        rGreen = int.Parse(entries[16]);
        rBlue = int.Parse(entries[17]);

        sphereList.Add(s1);

        s1.createSphere(radius, centerX, centerY, centerZ, aRed, aGreen, aBlue, rRed, rGreen, rBlue);

        s1.display();

        radius = int.Parse(entries[19]);

        centerX = int.Parse(entries[20]);
        centerY = int.Parse(entries[21]);
        centerZ = int.Parse(entries[22]);

        aRed = float.Parse(entries[23]);
        aGreen = float.Parse(entries[24]);
        aBlue = float.Parse(entries[25]);

        rRed = int.Parse(entries[26]);
        rGreen = int.Parse(entries[27]);
        rBlue = int.Parse(entries[28]);

        s2.createSphere(radius, centerX, centerY, centerZ, aRed, aGreen, aBlue, rRed, rGreen, rBlue);
        sphereList.Add(s2);

        radius = int.Parse(entries[30]);

        centerX = int.Parse(entries[31]);
        centerY = int.Parse(entries[32]);
        centerZ = int.Parse(entries[33]);

        aRed = float.Parse(entries[34]);
        aGreen = float.Parse(entries[35]);
        aBlue = float.Parse(entries[36]);

        rRed = int.Parse(entries[37]);
        rGreen = int.Parse(entries[38]);
        rBlue = int.Parse(entries[39]);

        s3.createSphere(radius, centerX, centerY, centerZ, aRed, aGreen, aBlue, rRed, rGreen, rBlue);
        sphereList.Add(s3);

        radius = int.Parse(entries[41]);

        centerX = int.Parse(entries[42]);
        centerY = int.Parse(entries[43]);
        centerZ = int.Parse(entries[44]);

        aRed = float.Parse(entries[45]);
        aGreen = float.Parse(entries[46]);
        aBlue = float.Parse(entries[47]);

        rRed = int.Parse(entries[48]);
        rGreen = int.Parse(entries[49]);
        rBlue = int.Parse(entries[50]);

        s4.createSphere(radius, centerX, centerY, centerZ, aRed, aGreen, aBlue, rRed, rGreen, rBlue);
        sphereList.Add(s4);

        radius = int.Parse(entries[52]);

        centerX = int.Parse(entries[53]);
        centerY = int.Parse(entries[54]);
        centerZ = int.Parse(entries[55]);

        aRed = float.Parse(entries[56]);
        aGreen = float.Parse(entries[57]);
        aBlue = float.Parse(entries[58]);

        rRed = int.Parse(entries[59]);
        rGreen = int.Parse(entries[60]);
        rBlue = int.Parse(entries[61]);

        s5.createSphere(radius, centerX, centerY, centerZ, aRed, aGreen, aBlue, rRed, rGreen, rBlue);
        sphereList.Add(s5);

        radius = int.Parse(entries[63]);

        centerX = int.Parse(entries[64]);
        centerY = int.Parse(entries[65]);
        centerZ = int.Parse(entries[66]);

        aRed = float.Parse(entries[67]);
        aGreen = float.Parse(entries[68]);
        aBlue = float.Parse(entries[69]);

        rRed = int.Parse(entries[70]);
        rGreen = int.Parse(entries[71]);
        rBlue = int.Parse(entries[72]);

        s6.createSphere(radius, centerX, centerY, centerZ, aRed, aGreen, aBlue, rRed, rGreen, rBlue);
        sphereList.Add(s6);

        radius = int.Parse(entries[74]);

        centerX = int.Parse(entries[75]);
        centerY = int.Parse(entries[76]);
        centerZ = int.Parse(entries[77]);

        aRed = float.Parse(entries[78]);
        aGreen = float.Parse(entries[79]);
        aBlue = float.Parse(entries[80]);

        rRed = int.Parse(entries[81]);
        rGreen = int.Parse(entries[82]);
        rBlue = int.Parse(entries[83]);

        s7.createSphere(radius, centerX, centerY, centerZ, aRed, aGreen, aBlue, rRed, rGreen, rBlue);
        sphereList.Add(s7);

        s2.display();
        s3.display();
        s4.display();
        s5.display();
        s6.display();
        s7.display();
                
    }
    
    
}


// Class that contains the information for a sphere
public class Sphere
{
    public int radius;

    // center coordinates
    public int centerX;
    public int centerY;
    public int centerZ;

    // ambient RGB values
    public float aRed;
    public float aGreen;
    public float aBlue;

    public int rRed;
    public int rGreen;
    public int rBlue;

    public Vector3 center; // vector containing center coordinates

    public Color aColor; // Color containing ambient RGB values

    public void createSphere(int R, int cX, int cY, int cZ, float aR, float aG, float aB, 
        int rR, int rG, int rB)
    {
        radius = R;

        centerX = cX;
        centerY = cY;
        centerZ = cZ;

        aRed = aR;
        aGreen = aG;
        aBlue = aB;

        rRed = rR;
        rGreen = rG;
        rBlue = rB;

        center = new Vector3(centerX, centerY, centerZ);
        aColor = new Color(aRed, aGreen, aBlue);
    }

    // Displays contents of a sphere for testing purposes
    public void display()
    {
        Debug.Log(radius + "|" + centerX + "|" + centerY + "|" + centerZ + "|" + aRed + "|" + aGreen
            + "|" + aBlue + "|" + rRed + "|" + rGreen + "|" + rBlue);
    }
}
