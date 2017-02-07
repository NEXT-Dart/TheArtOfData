using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheArtOfData.Art
{
    class MondriaanArtGenerator : ArtGenerator
    {
        #region "Fields"

        private List<byte> data;
        private Random rand;
        private string currentByte;

        #endregion

        #region "Constructors"

        public MondriaanArtGenerator()
        {
            data = new List<byte>();
        }

        #endregion

        #region "Properties"



        #endregion

        #region "Methods"

        private int[] GetRandomNumbers(int amount, int min, int max)
        {
            List<int> numbers = new List<int>();
            for (int i = 0; i < amount * 2; i++)
            {
                numbers.Add(rand.Next(min, max));
            }

            // Filter any double numbers
            numbers = numbers.Distinct().ToList();

            // Check if there are any following numbers
            List<int> toRemove = new List<int>();
            foreach (int val in numbers)
            {
                if (numbers.Contains(val + 1))
                    toRemove.Add(val + 1);
            }

            // Remove first and last
            numbers.Remove(0);
            numbers.Remove(max - 1);

            foreach (int val in toRemove)
            {
                numbers.Remove(val);
            }

            // Remove any leftover numbers
            while (numbers.Count > amount)
            {
                numbers.RemoveAt(rand.Next(0, numbers.Count));
            }

            // Check if we still have enough numbers
            if (numbers.Count < amount)
            {
                numbers = new List<int>(GetRandomNumbers(amount, min, max));
            }

            return numbers.OrderBy(x => x).ToArray();
        }

        private string GetNextBit()
        {
            if (currentByte.Length == 0)
            {
                currentByte = Convert.ToString(data[0], 2);
                while (currentByte.Length != 8)
                {
                    currentByte = "0" + currentByte;
                }
                data.RemoveRange(0, 1);
            }

            string bit = currentByte.Substring(0, 1);
            currentByte = currentByte.Remove(0, 1);
            return bit;
        }

        #endregion

        #region "Abstract/Virtual Methods"



        #endregion

        #region "Inherited Methods"

        public override void AddBytes(byte[] data)
        {
            this.data.AddRange(data);
        }

        public override Image GetImage()
        {
            if (data.Count == 0)
                return new Bitmap(1, 1);

            const int size = 1000;
            const int imgSize = 80;

            rand = new Random(0);

            // Get the rows and columns to color yellow
            int[] rows = GetRandomNumbers((int)(imgSize * 0.2), 0, imgSize);
            int[] columns = GetRandomNumbers((int)(imgSize * 0.2), 0, imgSize);

            // Create the image grid
            ImageGrid ig = new ImageGrid(imgSize, rand);
            foreach (int row in rows)
            {
                ig.SetRow(row);
            }

            foreach (int col in columns)
            {
                ig.SetColumn(col);
            }

            // Create the bits on the image grid
            int totalBitCount = data.Count * 8;


            return ig.CreateBitmap(size);
        }

        #endregion

        #region "Static Methods"



        #endregion

        #region "Operators"



        #endregion
    }
}
