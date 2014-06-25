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

    class OrthoDawnBringer
    {
        private static float[][] colors = new float[][]
        {
            new float[] {0.08F, 0.05F, 0.11F, 1F},
            new float[] {0.08F, 0.05F, 0.11F, 1F},
            new float[] {0.08F, 0.05F, 0.11F, 1F},
            new float[] {0.08F, 0.05F, 0.11F, 1F},
            new float[] {0.08F, 0.05F, 0.11F, 1F},
            new float[] {0.08F, 0.05F, 0.11F, 1F},
            new float[] {0.08F, 0.05F, 0.11F, 1F},
            new float[] {0.08F, 0.05F, 0.11F, 1F},

            new float[] {0.27F, 0.14F, 0.20F, 1F},
            new float[] {0.27F, 0.14F, 0.20F, 1F},
            new float[] {0.27F, 0.14F, 0.20F, 1F},
            new float[] {0.27F, 0.14F, 0.20F, 1F},
            new float[] {0.27F, 0.14F, 0.20F, 1F},
            new float[] {0.27F, 0.14F, 0.20F, 1F},
            new float[] {0.27F, 0.14F, 0.20F, 1F},
            new float[] {0.27F, 0.14F, 0.20F, 1F},

            new float[] {0.19F, 0.20F, 0.43F, 1F},
            new float[] {0.19F, 0.20F, 0.43F, 1F},
            new float[] {0.19F, 0.20F, 0.43F, 1F},
            new float[] {0.19F, 0.20F, 0.43F, 1F},
            new float[] {0.19F, 0.20F, 0.43F, 1F},
            new float[] {0.19F, 0.20F, 0.43F, 1F},                                                                                                        
            new float[] {0.19F, 0.20F, 0.43F, 1F},
            new float[] {0.19F, 0.20F, 0.43F, 1F},

            new float[] {0.31F, 0.29F, 0.31F, 1F},
            new float[] {0.31F, 0.29F, 0.31F, 1F},
            new float[] {0.31F, 0.29F, 0.31F, 1F},
            new float[] {0.31F, 0.29F, 0.31F, 1F},
            new float[] {0.31F, 0.29F, 0.31F, 1F},
            new float[] {0.31F, 0.29F, 0.31F, 1F},
            new float[] {0.31F, 0.29F, 0.31F, 1F},
            new float[] {0.31F, 0.29F, 0.31F, 1F},

            new float[] {0.52F, 0.30F, 0.19F, 1F},
            new float[] {0.52F, 0.30F, 0.19F, 1F},
            new float[] {0.52F, 0.30F, 0.19F, 1F},
            new float[] {0.52F, 0.30F, 0.19F, 1F},
            new float[] {0.52F, 0.30F, 0.19F, 1F},
            new float[] {0.52F, 0.30F, 0.19F, 1F},
            new float[] {0.52F, 0.30F, 0.19F, 1F},
            new float[] {0.52F, 0.30F, 0.19F, 1F},

            new float[] {0.20F, 0.40F, 0.14F, 1F},
            new float[] {0.20F, 0.40F, 0.14F, 1F},
            new float[] {0.20F, 0.40F, 0.14F, 1F},
            new float[] {0.20F, 0.40F, 0.14F, 1F},
            new float[] {0.20F, 0.40F, 0.14F, 1F},
            new float[] {0.20F, 0.40F, 0.14F, 1F},
            new float[] {0.20F, 0.40F, 0.14F, 1F},
            new float[] {0.20F, 0.40F, 0.14F, 1F},

            new float[] {0.82F, 0.27F, 0.28F, 1F},
            new float[] {0.82F, 0.27F, 0.28F, 1F},
            new float[] {0.82F, 0.27F, 0.28F, 1F},
            new float[] {0.82F, 0.27F, 0.28F, 1F},
            new float[] {0.82F, 0.27F, 0.28F, 1F},
            new float[] {0.82F, 0.27F, 0.28F, 1F},
            new float[] {0.82F, 0.27F, 0.28F, 1F},
            new float[] {0.82F, 0.27F, 0.28F, 1F},

            new float[] {0.46F, 0.44F, 0.38F, 1F},
            new float[] {0.46F, 0.44F, 0.38F, 1F},
            new float[] {0.46F, 0.44F, 0.38F, 1F},
            new float[] {0.46F, 0.44F, 0.38F, 1F},
            new float[] {0.46F, 0.44F, 0.38F, 1F},
            new float[] {0.46F, 0.44F, 0.38F, 1F},
            new float[] {0.46F, 0.44F, 0.38F, 1F},
            new float[] {0.46F, 0.44F, 0.38F, 1F},

            new float[] {0.35F, 0.49F, 0.81F, 1F},
            new float[] {0.35F, 0.49F, 0.81F, 1F},
            new float[] {0.35F, 0.49F, 0.81F, 1F},
            new float[] {0.35F, 0.49F, 0.81F, 1F},
            new float[] {0.35F, 0.49F, 0.81F, 1F},
            new float[] {0.35F, 0.49F, 0.81F, 1F},
            new float[] {0.35F, 0.49F, 0.81F, 1F},
            new float[] {0.35F, 0.49F, 0.81F, 1F},

            new float[] {0.82F, 0.49F, 0.17F, 1F},
            new float[] {0.82F, 0.49F, 0.17F, 1F},
            new float[] {0.82F, 0.49F, 0.17F, 1F},
            new float[] {0.82F, 0.49F, 0.17F, 1F},
            new float[] {0.82F, 0.49F, 0.17F, 1F},
            new float[] {0.82F, 0.49F, 0.17F, 1F},
            new float[] {0.82F, 0.49F, 0.17F, 1F},
            new float[] {0.82F, 0.49F, 0.17F, 1F},

            new float[] {0.52F, 0.58F, 0.63F, 1F},
            new float[] {0.52F, 0.58F, 0.63F, 1F},
            new float[] {0.52F, 0.58F, 0.63F, 1F},
            new float[] {0.52F, 0.58F, 0.63F, 1F},
            new float[] {0.52F, 0.58F, 0.63F, 1F},
            new float[] {0.52F, 0.58F, 0.63F, 1F},
            new float[] {0.52F, 0.58F, 0.63F, 1F},
            new float[] {0.52F, 0.58F, 0.63F, 1F},

            new float[] {0.43F, 0.67F, 0.17F, 1F},
            new float[] {0.43F, 0.67F, 0.17F, 1F},
            new float[] {0.43F, 0.67F, 0.17F, 1F},
            new float[] {0.43F, 0.67F, 0.17F, 1F},
            new float[] {0.43F, 0.67F, 0.17F, 1F},
            new float[] {0.43F, 0.67F, 0.17F, 1F},
            new float[] {0.43F, 0.67F, 0.17F, 1F},
            new float[] {0.43F, 0.67F, 0.17F, 1F},

            new float[] {0.82F, 0.67F, 0.60F, 1F},
            new float[] {0.82F, 0.67F, 0.60F, 1F},
            new float[] {0.82F, 0.67F, 0.60F, 1F},
            new float[] {0.82F, 0.67F, 0.60F, 1F},
            new float[] {0.82F, 0.67F, 0.60F, 1F},
            new float[] {0.82F, 0.67F, 0.60F, 1F},
            new float[] {0.82F, 0.67F, 0.60F, 1F},
            new float[] {0.82F, 0.67F, 0.60F, 1F},

            new float[] {0.43F, 0.76F, 0.79F, 1F},
            new float[] {0.43F, 0.76F, 0.79F, 1F},
            new float[] {0.43F, 0.76F, 0.79F, 1F},
            new float[] {0.43F, 0.76F, 0.79F, 1F},
            new float[] {0.43F, 0.76F, 0.79F, 1F},
            new float[] {0.43F, 0.76F, 0.79F, 1F},
            new float[] {0.43F, 0.76F, 0.79F, 1F},
            new float[] {0.43F, 0.76F, 0.79F, 1F},

            new float[] {0.85F, 0.83F, 0.37F, 1F},
            new float[] {0.85F, 0.83F, 0.37F, 1F},
            new float[] {0.85F, 0.83F, 0.37F, 1F},
            new float[] {0.85F, 0.83F, 0.37F, 1F},
            new float[] {0.85F, 0.83F, 0.37F, 1F},
            new float[] {0.85F, 0.83F, 0.37F, 1F},
            new float[] {0.85F, 0.83F, 0.37F, 1F},
            new float[] {0.85F, 0.83F, 0.37F, 1F},

            new float[] {0.87F, 0.93F, 0.84F, 1F},
            new float[] {0.87F, 0.93F, 0.84F, 1F},
            new float[] {0.87F, 0.93F, 0.84F, 1F},
            new float[] {0.87F, 0.93F, 0.84F, 1F},
            new float[] {0.87F, 0.93F, 0.84F, 1F},
            new float[] {0.87F, 0.93F, 0.84F, 1F},
            new float[] {0.87F, 0.93F, 0.84F, 1F},
            new float[] {0.87F, 0.93F, 0.84F, 1F},
            
            //128 garbage
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
        };
        public struct MagicaVoxelData
        {
            public byte x;
            public byte y;
            public byte z;
            public byte color;

            public MagicaVoxelData(BinaryReader stream, bool subsample)
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
        public static MagicaVoxelData[] FromMagica(BinaryReader stream)
        {
            // check out http://voxel.codeplex.com/wikipage?title=VOX%20Format&referringTitle=Home for the file format used below
            // we're going to return a voxel chunk worth of data
            ushort[] data = new ushort[32 * 128 * 32];
            
            MagicaVoxelData[] voxelData = null;

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
                        voxelData = new MagicaVoxelData[numVoxels];
                        for (int i = 0; i < voxelData.Length; i++)
                            voxelData[i] = new MagicaVoxelData(stream, subsample);
                    }
                    else if (chunkName == "RGBA")
                    {
//                        colors = new float[256][];

                        for (int i = 0; i < 256; i++)
                        {
                            byte r = stream.ReadByte();
                            byte g = stream.ReadByte();
                            byte b = stream.ReadByte();
                            byte a = stream.ReadByte();

                            //colors[i] = new float[] { r / 256.0f, g / 256.0f, b / 256.0f, a / 256.0f};
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
        private static void drawPixels(MagicaVoxelData[] voxels)
        {
            Bitmap b = new Bitmap(80, 80);
            Graphics g = Graphics.FromImage((Image)b);
            Image image = new Bitmap("cube.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 4;
            int height = 3;

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
            foreach (MagicaVoxelData vx in voxels.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
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


        public static Bitmap drawPixelsS(MagicaVoxelData[] voxels)
        {
            Bitmap b = new Bitmap(22,44);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_ortho.png");
            //Image reversed = new Bitmap("cube_reversed.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 1;
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
            foreach (MagicaVoxelData vx in voxels.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {

                int current_color = 249 - vx.color;
                if (current_color > 128)
                    current_color = 24;

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
                   image, //(vx.y, 40 - 20 - 2 + vx.x - vx.z, width, height)
                   new Rectangle(vx.y, 44 - 1 - 20 - 2 + vx.x - vx.z, width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }

        public static Bitmap drawPixelsW(MagicaVoxelData[] voxels)
        {
            Bitmap b = new Bitmap(22,44);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_ortho.png");
            //Image reversed = new Bitmap("cube_reversed.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 1;
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
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            for (int i = 0; i < voxels.Length; i++)
            {
                byte tempX = (byte)(voxels[i].x - 11);
                byte tempY = (byte)(voxels[i].y - 11);
                vls[i].x = (byte)((tempY) + 11);
                vls[i].y = (byte)((tempX * -1) + 11 - 1);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;

            }
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {


                int current_color = 249 - vx.color;
                if (current_color > 128)
                    current_color = 24;

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
                   new Rectangle(vx.y, 44 - 1 - 20 - 2 + vx.x - vx.z, width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }

        public static Bitmap drawPixelsE(MagicaVoxelData[] voxels)
        {
            Bitmap b = new Bitmap(22,44);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_ortho.png");
            //Image reversed = new Bitmap("cube_reversed.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 1;
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
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            for (int i = 0; i < voxels.Length; i++)
            {
                byte tempX = (byte)(voxels[i].x - 11);
                byte tempY = (byte)(voxels[i].y - 11);
                vls[i].x = (byte)((tempY * -1) + 11 - 1);
                vls[i].y = (byte)(tempX + 11);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;

            }
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {

                int current_color = 249 - vx.color;
                if (current_color > 128)
                    current_color = 24;
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
                   new Rectangle(vx.y, 44 - 1 - 20 - 2 + vx.x - vx.z, width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }

        public static Bitmap drawPixelsN(MagicaVoxelData[] voxels)
        {
            Bitmap b = new Bitmap(22,44);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_ortho.png");
            //Image reversed = new Bitmap("cube_reversed.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 1;
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
            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            for (int i = 0; i < voxels.Length; i++)
            {
                byte tempX = (byte)(voxels[i].x - 11);
                byte tempY = (byte)(voxels[i].y - 11);
                vls[i].x = (byte)((tempX * -1) + 11 - 1);
                vls[i].y = (byte)((tempY * -1) + 11 - 1);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;

            }
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {

                int current_color = 249 - vx.color;
                if (current_color > 128)
                    current_color = 24;

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
                   new Rectangle(vx.y, 44 - 1 - 20 - 2 + vx.x - vx.z, width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }


        private static void processUnitBasic(string u)
        {
            BinaryReader bin = new BinaryReader(File.Open(u + "_DB.vox", FileMode.Open));
            MagicaVoxelData[] parsed = FromMagica(bin);

                System.IO.Directory.CreateDirectory(u);

                Bitmap bS = drawPixelsS(parsed);
                bS.Save(u + "/default_S" + ".png", ImageFormat.Png);
                Bitmap bW = drawPixelsW(parsed);
                bW.Save(u + "/default_W" + ".png", ImageFormat.Png);
                Bitmap bN = drawPixelsN(parsed);
                bN.Save(u + "/default_N" + ".png", ImageFormat.Png);
                Bitmap bE = drawPixelsE(parsed);
                bE.Save(u + "/default_E" + ".png", ImageFormat.Png);

            bin.Close();

        }

        static void Main(string[] args)
        {
            processUnitBasic("Swordsman");
        }
    }
}
