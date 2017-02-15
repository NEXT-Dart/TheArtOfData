using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheArtOfData.Imaging;
using System.Drawing;

namespace TestProject
{
    /// <summary>
    /// Summary description for ImageTest
    /// </summary>
    [TestClass]
    public class ImageTest
    {
        [TestMethod]
        public void TestImageClass()
        {
            // Create a 9 pixel image to test with
            TheArtOfData.Imaging.Image image = new TheArtOfData.Imaging.Image(3, 3);

            // Set some pixels for the image
            image.SetPixel(0, 0, Color.Red);
            image.SetPixel(1, 1, Color.Blue);
            image.SetPixel(1, 2, Color.Green);
            image.SetPixel(2, 1, Color.Yellow);

            // Check if the pixels are correctly set
            Assert.IsFalse(image.GetPixel(0, 0) == Color.Red, "The pixel (0, 0) was not corretly set");
            Assert.IsFalse(image.GetPixel(1, 1) == Color.Blue, "The pixel (1, 1) was not correctly set");
            Assert.IsFalse(image.GetPixel(1, 2) == Color.Green, "The pixel (1, 2) was not correctly set");
            Assert.IsFalse(image.GetPixel(2, 1) == Color.Yellow, "The pixel (2, 1) was not correctly set");

            // Try rotating the image 30 degrees
            image.RotateImage(30);

            // Check if something changed
            Assert.IsFalse(image.GetPixel(0, 0) == Color.Red, "The image was somehow partially rotated");

            // Rotate the image 90 degrees
            image.RotateImage(90);

            // Check if the image got rotated
            Assert.IsFalse(image.GetPixel(2, 0) == Color.Red, "The image was not correctly rotated");

            // Generate a binary image with only red enabled
            TheArtOfData.Imaging.Image binary = image.GetBinaryImage(100, green: false, blue: false);

            Assert.IsFalse(image.GetPixel(2, 0) == Color.Black, "The binary image was not correctly created");

            // Generate a binary image from all the data
            binary = image.GetBinaryImage(100);

            Assert.IsFalse(image.GetPixel(1, 1) == Color.FromArgb(85, 85, 85), "The binary image was not correcly created");
        }
    }
}
