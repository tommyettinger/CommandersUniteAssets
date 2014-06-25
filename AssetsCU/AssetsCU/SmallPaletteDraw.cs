using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace AssetsCU
{

    class SmallPaletteDraw
    {
        private static float[][] colors = null;
        public struct MagicaVoxelDataPaletted
        {
            public byte x;
            public byte y;
            public byte z;
            public byte color;

            public MagicaVoxelDataPaletted(BinaryReader stream, bool subsample)
            {
                x = (byte)(subsample ? stream.ReadByte() / 2 : stream.ReadByte());
                y = (byte)(subsample ? stream.ReadByte() / 2 : stream.ReadByte());
                z = (byte)(subsample ? stream.ReadByte() / 2 : stream.ReadByte());
                color = stream.ReadByte();
            }
        }
        private static int sizex = 0, sizey = 0, sizez = 0;
        /// <summary>
        /// Load a MagicaVoxel .vox format file into a MagicaVoxelData[] that we use for voxel chunks.
        /// </summary>
        /// <param name="stream">An open BinaryReader stream that is the .vox file.</param>
        /// <param name="overrideColors">Optional color lookup table for converting RGB values into my internal engine color format.</param>
        /// <returns>The voxel chunk data for the MagicaVoxel .vox file.</returns>
        public static MagicaVoxelDataPaletted[] FromMagica(BinaryReader stream)
        {
            // check out http://voxel.codeplex.com/wikipage?title=VOX%20Format&referringTitle=Home for the file format used below
            // we're going to return a voxel chunk worth of data
            ushort[] data = new ushort[32 * 128 * 32];
            
            MagicaVoxelDataPaletted[] voxelData = null;

            string magic = new string(stream.ReadChars(4));
            int version = stream.ReadInt32();

            // a MagicaVoxel .vox file starts with a 'magic' 4 character 'VOX ' identifier
            if (magic == "VOX ")
            {
                bool subsample = false;

                while (stream.BaseStream.Position < stream.BaseStream.Length)
                {
                    // each chunk has an ID, size and child chunks
                    char[] chunkId = stream.ReadChars(4);
                    int chunkSize = stream.ReadInt32();
                    int childChunks = stream.ReadInt32();
                    string chunkName = new string(chunkId);

                    // there are only 2 chunks we only care about, and they are SIZE and XYZI
                    if (chunkName == "SIZE")
                    {
                        sizex = stream.ReadInt32();
                        sizey = stream.ReadInt32();
                        sizez = stream.ReadInt32();

                        if (sizex > 32 || sizey > 32) subsample = true;

                        stream.ReadBytes(chunkSize - 4 * 3);
                    }
                    else if (chunkName == "XYZI")
                    {
                        // XYZI contains n voxels
                        int numVoxels = stream.ReadInt32();
                        int div = (subsample ? 2 : 1);

                        // each voxel has x, y, z and color index values
                        voxelData = new MagicaVoxelDataPaletted[numVoxels];
                        for (int i = 0; i < voxelData.Length; i++)
                            voxelData[i] = new MagicaVoxelDataPaletted(stream, subsample);
                    }
                    else if (chunkName == "RGBA")
                    {
                        colors = new float[256][];

                        for (int i = 0; i < 256; i++)
                        {
                            byte r = stream.ReadByte();
                            byte g = stream.ReadByte();
                            byte b = stream.ReadByte();
                            byte a = stream.ReadByte();

                            colors[i] = new float[] { r / 256.0f, g / 256.0f, b / 256.0f, a / 256.0f};
                        }
                    }
                    else stream.ReadBytes(chunkSize);   // read any excess bytes
                }
                
                if (voxelData.Length == 0) return voxelData; // failed to read any valid voxel data

                // now push the voxel data into our voxel chunk structure
                for (int i = 0; i < voxelData.Length; i++)
                {
                    // do not store this voxel if it lies out of range of the voxel chunk (32x128x32)
                    if (voxelData[i].x > 31 || voxelData[i].y > 31 || voxelData[i].z > 127) continue;
                    
                    // use the voxColors array by default, or overrideColor if it is available
                    int voxel = (voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128);
                    //data[voxel] = (colors == null ? voxColors[voxelData[i].color - 1] : colors[voxelData[i].color - 1]);
                }
            }

            return voxelData;
        }
        private static void drawPixels(MagicaVoxelDataPaletted[] voxels)
        {
            Bitmap b = new Bitmap(80, 80);
            Graphics g = Graphics.FromImage((Image)b);
            Image image = new Bitmap("cube.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 2;
            int height = 2;

            //g.DrawImage(image, 10, 10, width, height);

            float[][] colorMatrixElements = { 
   new float[] {1F,  0,  0,  0, 0},
   new float[] {0,  1F,  0,  0, 0},
   new float[] {0,  0,  1F,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            foreach (MagicaVoxelDataPaletted vx in voxels.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {

                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {colors[vx.color - 1][0],  0,  0,  0, 0},
   new float[] {0,  colors[vx.color - 1][1],  0,  0, 0},
   new float[] {0,  0,  colors[vx.color - 1][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);

                g.DrawImage(
                   image,
                   new Rectangle((vx.x + vx.y) * 2, 80 - 3 - 32 - vx.y + vx.x - 2 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            b.Save("output.png", ImageFormat.Png);
        }


        public static Bitmap drawPixelsSE(MagicaVoxelDataPaletted[] voxels)
        {
            Bitmap b = new Bitmap(44,44);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_gray_small.png");
            //Image reversed = new Bitmap("cube_reversed.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 2;
            int height = 2;

            //g.DrawImage(image, 10, 10, width, height);

            float[][] colorMatrixElements = { 
   new float[] {1F,  0,  0,  0, 0},
   new float[] {0,  1F,  0,  0, 0},
   new float[] {0,  0,  1F,  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            foreach (MagicaVoxelDataPaletted vx in voxels.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = vx.color - 1;
                
                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {colors[current_color ][0],  0,  0,  0, 0},
   new float[] {0,  colors[current_color][1],  0,  0, 0},
   new float[] {0,  0,  colors[current_color][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(
                   image,
                   new Rectangle((vx.x + vx.y), 33 - 1 - 22 - vx.y + vx.x - vx.z, width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }

        public static Bitmap drawPixelsSW(MagicaVoxelDataPaletted[] voxels)
        {
            Bitmap b = new Bitmap(44,44);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_gray_small.png");
            //Image reversed = new Bitmap("cube_reversed.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 2;
            int height = 2;

            //g.DrawImage(image, 10, 10, width, height);

            float[][] colorMatrixElements = { 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            MagicaVoxelDataPaletted[] vls = new MagicaVoxelDataPaletted[voxels.Length];
            for (int i = 0; i < voxels.Length; i++)
            {
                byte tempX = (byte)(voxels[i].x - 11);
                byte tempY = (byte)(voxels[i].y - 11);
                vls[i].x = (byte)((tempY) + 11);
                vls[i].y = (byte)((tempX * -1) + 11 - 1);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;

            }
            foreach (MagicaVoxelDataPaletted vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {

                int current_color = vx.color - 1;

                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {colors[current_color ][0],  0,  0,  0, 0},
   new float[] {0,  colors[current_color][1],  0,  0, 0},
   new float[] {0,  0,  colors[current_color][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(
                   image,
                   new Rectangle((vx.x + vx.y), 33 - 1 - 22 - vx.y + vx.x - vx.z, width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }

        public static Bitmap drawPixelsNE(MagicaVoxelDataPaletted[] voxels)
        {
            Bitmap b = new Bitmap(44,44);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_gray_small.png");
            //Image reversed = new Bitmap("cube_reversed.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 2;
            int height = 2;

            //g.DrawImage(image, 10, 10, width, height);

            float[][] colorMatrixElements = { 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            MagicaVoxelDataPaletted[] vls = new MagicaVoxelDataPaletted[voxels.Length];
            for (int i = 0; i < voxels.Length; i++)
            {
                byte tempX = (byte)(voxels[i].x - 11);
                byte tempY = (byte)(voxels[i].y - 11);
                vls[i].x = (byte)((tempY * -1) + 11 - 1);
                vls[i].y = (byte)(tempX + 11);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;

            }
            foreach (MagicaVoxelDataPaletted vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = vx.color - 1;

                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {colors[current_color ][0],  0,  0,  0, 0},
   new float[] {0,  colors[current_color][1],  0,  0, 0},
   new float[] {0,  0,  colors[current_color][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(
                   image,
                   new Rectangle((vx.x + vx.y), 33 - 1 - 22 - vx.y + vx.x - vx.z, width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }

        public static Bitmap drawPixelsNW(MagicaVoxelDataPaletted[] voxels)
        {
            Bitmap b = new Bitmap(44,44);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_gray_small.png");
            //Image reversed = new Bitmap("cube_reversed.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 2;
            int height = 2;

            //g.DrawImage(image, 10, 10, width, height);

            float[][] colorMatrixElements = { 
   new float[] {1F, 0,  0,  0,  0},
   new float[] {0, 1F,  0,  0,  0},
   new float[] {0,  0,  1F, 0,  0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0,  0,  0,  0, 1F}};

            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);

            imageAttributes.SetColorMatrix(
               colorMatrix,
               ColorMatrixFlag.Default,
               ColorAdjustType.Bitmap);
            MagicaVoxelDataPaletted[] vls = new MagicaVoxelDataPaletted[voxels.Length];
            for (int i = 0; i < voxels.Length; i++)
            {
                byte tempX = (byte)(voxels[i].x - 11);
                byte tempY = (byte)(voxels[i].y - 11);
                vls[i].x = (byte)((tempX * -1) + 11 - 1);
                vls[i].y = (byte)((tempY * -1) + 11 - 1);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;

            }
            foreach (MagicaVoxelDataPaletted vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = vx.color - 1;

                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {colors[current_color ][0],  0,  0,  0, 0},
   new float[] {0,  colors[current_color][1],  0,  0, 0},
   new float[] {0,  0,  colors[current_color][2],  0, 0},
   new float[] {0,  0,  0,  1F, 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(
                   image,
                   new Rectangle((vx.x + vx.y), 33 - 1 - 22 - vx.y + vx.x - vx.z, width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }


        static void Main(string[] args)
        {
            BinaryReader bin = new BinaryReader(File.Open("Grass_P.vox", FileMode.Open));
            drawPixels(FromMagica(bin));
        }
    }
}
