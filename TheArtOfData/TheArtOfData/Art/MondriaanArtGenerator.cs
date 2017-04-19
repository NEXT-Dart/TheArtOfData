using Imaging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            currentByte = "";
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
                if (data.Count > 0)
                {
                    currentByte = Convert.ToString(data[0], 2);
                    data.RemoveRange(0, 1);
                }
                while (currentByte.Length != 8)
                {
                    currentByte = "0" + currentByte;
                }

            }

            string bit = currentByte.Substring(0, 1);
            currentByte = currentByte.Remove(0, 1);
            return bit;
        }

        private string GetBits(int amount)
        {
            string bits = "";
            for (int i = 0; i < amount; i++)
            {
                bits += GetNextBit();
            }
            return bits;
        }

        #endregion

        #region "Abstract/Virtual Methods"



        #endregion

        #region "Inherited Methods"

        public override void AddBytes(byte[] data)
        {
            this.data.AddRange(data);
        }

        public CustomImage GetImage1()
        {
            //if (data.Count == 0)
            return new CustomImage(1, 1);

            //// Create the bits on the image grid
            //int totalBitCount = data.Count * 8;
            //int imgSize = 10 + (int)(totalBitCount * 0.5);

            //rand = new Random(data[0]);

            //// Get the rows and columns to color yellow
            //int[] rows = GetRandomNumbers((int)(imgSize * 0.15), 0, imgSize);
            //int[] columns = GetRandomNumbers((int)(imgSize * 0.15), 0, imgSize);

            //// Create the image grid
            //ImageGrid ig = new ImageGrid(imgSize, rand);
            //foreach (int row in rows)
            //{
            //    ig.SetRow(row);
            //}

            //foreach (int col in columns)
            //{
            //    ig.SetColumn(col);
            //}

            //ig.CheckCollisions();
            //LineData[] lines = ig.Lines;

            //// Get the total amount of bits
            //int totalLineBits = 0;
            //foreach (LineData line in lines)
            //{
            //    totalLineBits += line.Area.Length / 2;
            //}

            //// Give each line an amount of bits
            //foreach (LineData line in lines)
            //{
            //    int length = line.Area.Length / 2;
            //    float percentage = 1f / (float)totalLineBits * length;
            //    line.SetBitsOnLine(ig, GetBits(Ceil((float)totalBitCount * percentage)));
            //}

            //return ig.CreateBitmap();
        }

        public override CustomImage GetImage(object[] values)
        {
            if (data.Count == 0)
                return new CustomImage(1, 1);

            long totalBits = data.Count * 8;
            rand = new Random();
            int size = Ceil((float)Math.Sqrt(totalBits));

            ImageGrid grid = new ImageGrid(rand.Next(size * 4, size * 6));

            int[] rows = GetRandomNumbers(size, 0, grid.Size);
            int[] cols = GetRandomNumbers(size, 0, grid.Size);

            int bitsPerLine = Ceil((float)totalBits / (float)(rows.Length + cols.Length));
            foreach (int row in rows)
            {
                LineData line = new LineData(rand.Next(), bitsPerLine, grid.Size, row: row);
                line.ImageGrid = grid;
                grid.AddLine(line);
                line.SetBitsOnLine(GetBits(bitsPerLine));
            }
            foreach (int col in cols)
            {
                LineData line = new LineData(rand.Next(), bitsPerLine, grid.Size, col: col);
                line.ImageGrid = grid;
                grid.AddLine(line);
                line.SetBitsOnLine(GetBits(bitsPerLine));
            }

            CustomImage image = grid.CreateImage();

            image.Optimize();
            return image;
        }

        private int Ceil(float value)
        {
            if (value % 1 == 0)
                return (int)value;
            return (int)(value + 1);
        }

        #endregion

        #region "Static Methods"



        #endregion

        #region "Operators"



        #endregion
    }
}
