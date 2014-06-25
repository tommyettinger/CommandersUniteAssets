using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace AssetsCU
{

    class OffVoxels
    {
        private static float[][] colors = new float[][]
        {
            //tires, tread
            new float[] {0.2F,0.2F,0.2F,1F},
            new float[] {0.2F,0.2F,0.2F,1F},
            new float[] {0.2F,0.2F,0.2F,1F},
            new float[] {0.2F,0.2F,0.2F,1F},
            new float[] {0.2F,0.2F,0.2F,1F},
            new float[] {0.2F,0.2F,0.2F,1F},
            new float[] {0.2F,0.2F,0.2F,1F},
            new float[] {0.2F,0.2F,0.2F,1F},
            
            //tire hubcap, tread core
            new float[] {0.3F,0.3F,0.3F,1F},
            new float[] {0.4F,0.4F,0.4F,1F},
            new float[] {0.3F,0.3F,0.3F,1F},
            new float[] {0.3F,0.3F,0.3F,1F},
            new float[] {0.3F,0.3F,0.3F,1F},
            new float[] {0.3F,0.3F,0.3F,1F},
            new float[] {0.3F,0.3F,0.3F,1F},
            new float[] {0.3F,0.3F,0.3F,1F},
            
            //gun barrel
            new float[] {0.4F,0.35F,0.5F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            new float[] {0.3F,0.35F,0.4F,1F},
            
            //gun peripheral (sights, trigger)
            new float[] {0.4F,0.5F,0.4F,1F},
            new float[] {0.6F,0.8F,1F,1F},
            new float[] {0.4F,0.5F,0.4F,1F},
            new float[] {0.4F,0.5F,0.4F,1F},
            new float[] {0.4F,0.5F,0.4F,1F},
            new float[] {0.4F,0.5F,0.4F,1F},
            new float[] {0.4F,0.5F,0.4F,1F},
            new float[] {0.4F,0.5F,0.4F,1F},
            
            //main paint
            new float[] {0.3F,0.3F,0.3F,1F},     //black
            new float[] {1.05F,1.05F,1.05F,1F},  //white
            new float[] {1F,0.1F,0F,1F},         //red
            new float[] {0.95F,0.4F,0.1F,1F},      //orange
            new float[] {1.1F,1.1F,0.2F,1F},         //yellow
            new float[] {0.2F,0.7F,0.1F,1F},    //green
            new float[] {0.2F,0.3F,1F,1F},       //blue
            new float[] {0.7F,0.3F,0.8F,1F},       //purple
            
            //doors
            new float[] {0.8F,0F,0F,1F},         //black
            new float[] {0.8F,0.9F,1F,1F},         //white
            new float[] {0.6F,0.1F,0F,1F},       //red
            new float[] {0.8F,0.3F,0F,1F},       //orange
            new float[] {0.85F,0.8F,0.1F,1F},   //yellow
            new float[] {0.4F,1F,0.35F,1F},     //green
            new float[] {0.2F,0.1F,0.5F,1F},       //blue
            new float[] {0.6F,0.1F,0.55F,1F},       //purple
            
            //cockpit
            new float[] {0.5F,0.5F,0.4F,1F},     //black
            new float[] {0.75F,0.75F,0.9F,1F},   //white
            new float[] {0.9F,0.5F,0.4F,1F},     //red
            new float[] {0.82F,0.4F,0.1F,1F},    //orange
            new float[] {0.9F,0.9F,0.4F,1F},     //yellow
            new float[] {0.5F,0.8F,0.5F,1F},     //green
            new float[] {0.6F,0.5F,0.8F,1F},     //blue
            new float[] {0.9F,0.3F,0.85F,1F},   //purple

            //helmet
            new float[] {0.8F,0.4F,0.2F,1F},     //black
            new float[] {0.85F,0.9F,0.9F,1F},       //white
            new float[] {0.9F,0.3F,0.2F,1F},     //red
            new float[] {1F,0.5F,0.3F,1F},       //orange
            new float[] {0.7F,0.65F,0.5F,1F},         //yellow
            new float[] {0.45F,0.25F,0.0F,1F},       //green
            new float[] {0.5F,0.55F,0.7F,1F},       //blue
            new float[] {0.9F,0.2F,1F,1F},       //purple
            
            //flesh
            new float[] {1.1F,0.89F,0.55F,1F},  //black
            new float[] {0.9F,1.2F,0F,1F},      //white
            new float[] {1.1F,0.89F,0.55F,1F},  //red
            new float[] {1.1F,0.89F,0.55F,1F},  //orange
            new float[] {1.1F,0.89F,0.55F,1F},  //yellow
            new float[] {1.1F,0.89F,0.55F,1F},  //green
            new float[] {1.1F,0.89F,0.55F,1F},  //blue
            new float[] {1.1F,0.89F,0.55F,1F},  //purple
            
            //exposed metal
            new float[] {0.8F,0.8F,0.8F,1F},     //black
            new float[] {0.7F,0.7F,0.9F,1F},     //white
            new float[] {0.8F,0.8F,0.8F,1F},     //red
            new float[] {0.8F,0.8F,0.8F,1F},     //orange
            new float[] {0.8F,0.8F,0.8F,1F},     //yellow
            new float[] {0.8F,0.8F,0.8F,1F},     //green
            new float[] {0.8F,0.8F,0.8F,1F},     //blue
            new float[] {0.8F,0.8F,0.8F,1F},     //purple

            //lights
            new float[] {1.2F,1.2F,0.75F,1F},        //black
            new float[] {0.7F,1.4F,0.8F,1F},         //white
            new float[] {1.4F,1.2F,0.75F,1F},        //red
            new float[] {1.3F,1.3F,0.75F,1F},        //orange
            new float[] {1.2F,0.7F,0.4F,1F},        //yellow
            new float[] {1.3F,1.2F,0.75F,1F},        //green
            new float[] {1.2F,1.3F,0.75F,1F},        //blue
            new float[] {1.3F,1.3F,0.8F,1F},        //purple

            //windows
            new float[] {0.6F,0.9F,0.9F,1F},
            new float[] {0.3F,1.2F,1.2F,1F},
            new float[] {0.6F,0.9F,0.9F,1F},
            new float[] {0.6F,0.9F,0.9F,1F},
            new float[] {0.45F,0.4F,0.4F,1F},
            new float[] {0.45F,0.4F,0.4F,1F},
            new float[] {0.6F,0.9F,0.9F,1F},
            new float[] {0.6F,0.9F,0.9F,1F},

            //shadow (HAS ALPHA)
            new float[] {0F,0F,0F,0.26F},
            new float[] {0F,0F,0F,0.26F},
            new float[] {0F,0F,0F,0.26F},
            new float[] {0F,0F,0F,0.26F},
            new float[] {0F,0F,0F,0.26F},
            new float[] {0F,0F,0F,0.26F},
            new float[] {0F,0F,0F,0.26F},
            new float[] {0F,0F,0F,0.26F},
            
            //whirling rotors (HAS ALPHA)
            new float[] {0.6F,0.6F,0.6F,0.21F},     //black
            new float[] {1.25F,1.35F,1.45F,0.21F},  //white
            new float[] {1.3F,0.25F,0.3F,0.21F},         //red
            new float[] {1.2F,0.6F,0.3F,0.21F},      //orange
            new float[] {1.4F,1.4F,0.6F,0.21F},         //yellow
            new float[] {0.4F,1.5F,0.4F,0.21F},    //green
            new float[] {0.4F,0.4F,1.4F,0.21F},       //blue
            new float[] {1.1F,0.2F,1.1F,0.21F},       //purple
            //120 garbage
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
            //4 final garbage
            new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F}, new float[] {0F,0F,0F,0F},
        };
        private struct MagicaVoxelData
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
        private static MagicaVoxelData[] FromMagica(BinaryReader stream)
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
                        //colors = new float[256][];

                        for (int i = 0; i < 256; i++)
                        {
                            byte r = stream.ReadByte();
                            byte g = stream.ReadByte();
                            byte b = stream.ReadByte();
                            byte a = stream.ReadByte();

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


        private static Bitmap drawPixelsSE(MagicaVoxelData[] voxels, int idx)
        {
            Bitmap b = new Bitmap(80+10+10, 100, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_off.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 5;
            int height = 4;

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
                if (current_color > 112)
                    current_color = 24;
                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {colors[current_color + idx][0],  0,  0,  0, 0},
   new float[] {0,  colors[current_color + idx][1],  0,  0, 0},
   new float[] {0,  0,  colors[current_color + idx][2],  0, 0},
   new float[] {0,  0,  0,  colors[current_color + idx][3], 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(
                   image,
                   new Rectangle(vx.x * (1+1) + vx.y * 2, 100 - 24 - vx.y + vx.x - vx.z * 3, width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }

        private static Bitmap drawPixelsSW(MagicaVoxelData[] voxels, int idx)
        {
            Bitmap b = new Bitmap(80+10+10, 100, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_off.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 5;
            int height = 4;

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
                byte tempX = (byte)(voxels[i].x - 10);
                byte tempY = (byte)(voxels[i].y - 10);
                vls[i].x = (byte)((tempY) + 10);
                vls[i].y = (byte)((tempX * -1) + 10 - 1);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;

            }
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;
                if (current_color > 112)
                    current_color = 24;
                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {colors[current_color + idx][0],  0,  0,  0, 0},
   new float[] {0,  colors[current_color + idx][1],  0,  0, 0},
   new float[] {0,  0,  colors[current_color + idx][2],  0, 0},
   new float[] {0,  0,  0,  colors[current_color + idx][3], 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(
                   image,
                   new Rectangle(vx.x * (1+1) + vx.y * 2, 100 - 24 - vx.y + vx.x - vx.z * 3, width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }

        private static Bitmap drawPixelsNE(MagicaVoxelData[] voxels, int idx)
        {
            Bitmap b = new Bitmap(80+10+10, 100, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_off.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 5;
            int height = 4;

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
                byte tempX = (byte)(voxels[i].x - 10);
                byte tempY = (byte)(voxels[i].y - 10);
                vls[i].x = (byte)((tempY * -1) + 10 - 1);
                vls[i].y = (byte)(tempX + 10);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;

            }
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;
                if (current_color > 112)
                    current_color = 24;
                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {colors[current_color + idx][0],  0,  0,  0, 0},
   new float[] {0,  colors[current_color + idx][1],  0,  0, 0},
   new float[] {0,  0,  colors[current_color + idx][2],  0, 0},
   new float[] {0,  0,  0,  colors[current_color + idx][3], 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(
                   image,
                   new Rectangle(vx.x * (1+1) + vx.y * 2, 100 - 24 - vx.y + vx.x - vx.z * 3, width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }

        private static Bitmap drawPixelsNW(MagicaVoxelData[] voxels, int idx)
        {
            Bitmap b = new Bitmap(80+10+10, 100, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            //Image image = new Bitmap("cube_large.png");
            Image image = new Bitmap("cube_off.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 5;
            int height = 4;

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
                byte tempX = (byte)(voxels[i].x - 10);
                byte tempY = (byte)(voxels[i].y - 10);
                vls[i].x = (byte)((tempX * -1) + 10 - 1);
                vls[i].y = (byte)((tempY * -1) + 10 - 1);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;

            }
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;
                if (current_color > 112)
                    current_color = 24;
                colorMatrix = new ColorMatrix(new float[][]{ 
   new float[] {colors[current_color + idx][0],  0,  0,  0, 0},
   new float[] {0,  colors[current_color + idx][1],  0,  0, 0},
   new float[] {0,  0,  colors[current_color + idx][2],  0, 0},
   new float[] {0,  0,  0,  colors[current_color + idx][3], 0},
   new float[] {0, 0, 0, 0, 1F}});

                imageAttributes.SetColorMatrix(
                   colorMatrix,
                   ColorMatrixFlag.Default,
                   ColorAdjustType.Bitmap);

                g.DrawImage(
                   image,
                   new Rectangle(vx.x * (1+1) + vx.y * 2, 100 - 24 - vx.y + vx.x - vx.z * 3, width, height),  // destination rectangle 
                    //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                   0, 0,        // upper-left corner of source rectangle 
                   width,       // width of source rectangle
                   height,      // height of source rectangle
                   GraphicsUnit.Pixel,
                   imageAttributes);
            }
            return b;
        }







        private static Bitmap drawOutlineSE(MagicaVoxelData[] voxels, int idx)
        {
            Bitmap b = new Bitmap(100+8, 108, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            Image image = new Bitmap("black_outline_off.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 9;
            int height = 8;

            foreach (MagicaVoxelData vx in voxels.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;
                if (current_color > 112)
                    current_color = 24;
                if (colors[current_color + idx][3] == 1F)
                    g.DrawImage(
                       image,
                       new Rectangle(vx.x * (1+1) + vx.y * 2 + 2 , 100 - 20 - 2 - vx.y + vx.x - vx.z * 3, width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                       0, 0,        // upper-left corner of source rectangle 
                       width,       // width of source rectangle
                       height,      // height of source rectangle
                       GraphicsUnit.Pixel,
                       imageAttributes);
            }
            return b;
        }
        private static Bitmap drawOutlineSW(MagicaVoxelData[] voxels, int idx)
        {
            Bitmap b = new Bitmap(100+8, 108, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            Image image = new Bitmap("black_outline_off.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 9;
            int height = 8;

            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            for (int i = 0; i < voxels.Length; i++)
            {
                byte tempX = (byte)(voxels[i].x - 10);
                byte tempY = (byte)(voxels[i].y - 10);
                vls[i].x = (byte)((tempY) + 10);
                vls[i].y = (byte)((tempX * -1) + 10 - 1);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;

            }
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;
                if (current_color > 112)
                    current_color = 24;
                if (colors[current_color + idx][3] == 1F)
                    g.DrawImage(
                       image,
                       new Rectangle(vx.x * (1+1) + vx.y * 2 + 2 , 100 - 20 - 2 - vx.y + vx.x - vx.z * 3, width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                       0, 0,        // upper-left corner of source rectangle 
                       width,       // width of source rectangle
                       height,      // height of source rectangle
                       GraphicsUnit.Pixel,
                       imageAttributes);
            }
            return b;
        }

        private static Bitmap drawOutlineNE(MagicaVoxelData[] voxels, int idx)
        {
            Bitmap b = new Bitmap(100+8, 108, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            Image image = new Bitmap("black_outline_off.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 9;
            int height = 8;

            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            for (int i = 0; i < voxels.Length; i++)
            {
                byte tempX = (byte)(voxels[i].x - 10);
                byte tempY = (byte)(voxels[i].y - 10);
                vls[i].x = (byte)((tempY * -1) + 10 - 1);
                vls[i].y = (byte)(tempX + 10);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;

            }
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;
                if (current_color > 112)
                    current_color = 24;
                if (colors[current_color + idx][3] == 1F)
                    g.DrawImage(
                       image,
                       new Rectangle(vx.x * (1+1) + vx.y * 2 + 2 , 100 - 20 - 2 - vx.y + vx.x - vx.z * 3, width, height),  // destination rectangle 
                        //                   new Rectangle((vx.x + vx.y) * 4, 128 - 6 - 32 - vx.y * 2 + vx.x * 2 - 4 * vx.z, width, height),  // destination rectangle 
                       0, 0,        // upper-left corner of source rectangle 
                       width,       // width of source rectangle
                       height,      // height of source rectangle
                       GraphicsUnit.Pixel,
                       imageAttributes);
            }
            return b;
        }


        private static Bitmap drawOutlineNW(MagicaVoxelData[] voxels, int idx)
        {
            Bitmap b = new Bitmap(100+8, 108, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage((Image)b);
            Image image = new Bitmap("black_outline_off.png");
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = 9;
            int height = 8;

            MagicaVoxelData[] vls = new MagicaVoxelData[voxels.Length];
            for (int i = 0; i < voxels.Length; i++)
            {
                byte tempX = (byte)(voxels[i].x - 10);
                byte tempY = (byte)(voxels[i].y - 10);
                vls[i].x = (byte)((tempX * -1) + 10 - 1);
                vls[i].y = (byte)((tempY * -1) + 10 - 1);
                vls[i].z = voxels[i].z;
                vls[i].color = voxels[i].color;

            }
            foreach (MagicaVoxelData vx in vls.OrderBy(v => v.x * 32 - v.y + v.z * 32 * 128)) //voxelData[i].x + voxelData[i].z * 32 + voxelData[i].y * 32 * 128
            {
                int current_color = 249 - vx.color;
                if (current_color > 112)
                    current_color = 24;
                if (colors[current_color + idx][3] == 1F)
                    g.DrawImage(
                       image,
                       new Rectangle(vx.x * (1+1) + vx.y * 2 + 2 , 100 - 20 - 2 - vx.y + vx.x - vx.z * 3, width, height),  // destination rectangle 
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
            BinaryReader bin = new BinaryReader(File.Open(u + "_X.vox", FileMode.Open));
            MagicaVoxelData[] parsed = FromMagica(bin);

            for (int i = 0; i < 8; i++)
            {
                System.IO.Directory.CreateDirectory(u);

                Bitmap bSE = drawPixelsSE(parsed, i);
                bSE.Save(u + "/color" + i + "_" + u + "_default_SE" + ".png", ImageFormat.Png);
                Bitmap bSW = drawPixelsSW(parsed, i);
                bSW.Save(u + "/color" + i + "_" + u + "_default_SW" + ".png", ImageFormat.Png);
                Bitmap bNW = drawPixelsNW(parsed, i);
                bNW.Save(u + "/color" + i + "_" + u + "_default_NW" + ".png", ImageFormat.Png);
                Bitmap bNE = drawPixelsNE(parsed, i);
                bNE.Save(u + "/color" + i + "_" + u + "_default_NE" + ".png", ImageFormat.Png);

            }
            bin.Close();

        }

        private static void processUnitOutlined(string u)
        {
            BinaryReader bin = new BinaryReader(File.Open(u + "_X.vox", FileMode.Open));
            MagicaVoxelData[] parsed = FromMagica(bin);

            for (int i = 0; i < 8; i++)
            {
                Graphics g;
                System.IO.Directory.CreateDirectory(u);

                Bitmap bSE = drawPixelsSE(parsed, i), oSE = drawOutlineSE(parsed, i);
                g = Graphics.FromImage(oSE);
                g.DrawImage(bSE, 4, 4);
                oSE.Save(u + "/color" + i + "_" + u + "_default_SE" + ".png", ImageFormat.Png);

                Bitmap bSW = drawPixelsSW(parsed, i), oSW = drawOutlineSW(parsed, i);
                g = Graphics.FromImage(oSW);
                g.DrawImage(bSW, 4, 4);
                oSW.Save(u + "/color" + i + "_" + u + "_default_SW" + ".png", ImageFormat.Png);

                Bitmap bNE = drawPixelsNE(parsed, i), oNE = drawOutlineNE(parsed, i);
                g = Graphics.FromImage(oNE);
                g.DrawImage(bNE, 4, 4);
                oNE.Save(u + "/color" + i + "_" + u + "_default_NE" + ".png", ImageFormat.Png);

                Bitmap bNW = drawPixelsNW(parsed, i), oNW = drawOutlineNW(parsed, i);
                g = Graphics.FromImage(oNW);
                g.DrawImage(bNW, 4, 4);
                oNW.Save(u + "/color" + i + "_" + u + "_default_NW" + ".png", ImageFormat.Png);
            }
            bin.Close();

        }
        private static void processBases()
        {
            BinaryReader[] powers = new BinaryReader[8];
            BinaryReader[] speeds = new BinaryReader[8];
            BinaryReader[] techniques = new BinaryReader[8];


            MagicaVoxelData[][] basepowers = new MagicaVoxelData[8][];
            MagicaVoxelData[][] basespeeds = new MagicaVoxelData[8][];
            MagicaVoxelData[][] basetechniques = new MagicaVoxelData[8][];

            for (int i = 0; i < 8; i++)
            {
                powers[i] = new BinaryReader(File.OpenRead(@"Bases\Anim_P_" + i + ".vox"));
                basepowers[i] = FromMagica(powers[i]);
                speeds[i] = new BinaryReader(File.OpenRead(@"Bases\Anim_S_" + i + ".vox"));
                basespeeds[i] = FromMagica(speeds[i]);
                techniques[i] = new BinaryReader(File.OpenRead(@"Bases\Anim_T_" + i + ".vox"));
                basetechniques[i] = FromMagica(techniques[i]);

            }

            System.IO.Directory.CreateDirectory("Power");
            System.IO.Directory.CreateDirectory("Speed");
            System.IO.Directory.CreateDirectory("Technique");
            Graphics g;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {


                    Bitmap power = drawPixelsSE(basepowers[j], i), oPower = drawOutlineSE(basepowers[j], i);
                    g = Graphics.FromImage(oPower);
                    g.DrawImage(power, 2, 2);
                    oPower.Save("Power/color" + i + "_frame_" + j + ".png", ImageFormat.Png);

                    Bitmap speed = drawPixelsSE(basespeeds[j], i), oSpeed = drawOutlineSE(basespeeds[j], i);
                    g = Graphics.FromImage(oSpeed);
                    g.DrawImage(speed, 2, 2);
                    oSpeed.Save("Speed/color" + i + "_frame_" + j + ".png", ImageFormat.Png);

                    Bitmap technique = drawPixelsSE(basetechniques[j], i), oTechnique = drawOutlineSE(basetechniques[j], i);
                    g = Graphics.FromImage(oTechnique);
                    g.DrawImage(technique, 2, 2);
                    oTechnique.Save("Technique/color" + i + "_frame_" + j + ".png", ImageFormat.Png);
                }
            }

            for (int i = 0; i < 8; i++)
            {
                powers[i].Close();
                speeds[i].Close();
                techniques[i].Close();
            }
        }
        private static void processUnit(string u)
        {
            BinaryReader bin = new BinaryReader(File.Open(u + "_X.vox", FileMode.Open));
            MagicaVoxelData[] parsed = FromMagica(bin);

            BinaryReader[] bases = {
                                   new BinaryReader(File.Open("Base_Power.vox", FileMode.Open)),
                                   new BinaryReader(File.Open("Base_Speed.vox", FileMode.Open)),
                                   new BinaryReader(File.Open("Base_Technique.vox", FileMode.Open))
            };
            BinaryReader[] powers = new BinaryReader[8];
            BinaryReader[] speeds = new BinaryReader[8];
            BinaryReader[] techniques = new BinaryReader[8];


            MagicaVoxelData[][] basepowers = new MagicaVoxelData[8][];
            MagicaVoxelData[][] basespeeds = new MagicaVoxelData[8][];
            MagicaVoxelData[][] basetechniques = new MagicaVoxelData[8][];

            for (int i = 0; i < 8; i++)
            {
                powers[i] = new BinaryReader(File.OpenRead(@"Bases\Anim_P_" + i + ".vox"));
                basepowers[i] = FromMagica(powers[i]);
                speeds[i] = new BinaryReader(File.OpenRead(@"Bases\Anim_S_" + i + ".vox"));
                basespeeds[i] = FromMagica(speeds[i]);
                techniques[i] = new BinaryReader(File.OpenRead(@"Bases\Anim_T_" + i + ".vox"));
                basetechniques[i] = FromMagica(techniques[i]);

            }
            for (int i = 0; i < 8; i++)
            {
                System.IO.Directory.CreateDirectory(u);
                System.IO.Directory.CreateDirectory(u + "color" + i);
                System.IO.Directory.CreateDirectory(u + "color" + i + "/power");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/power/SE");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/power/SW");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/power/NW");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/power/NE");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/speed");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/speed/SE");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/speed/SW");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/speed/NW");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/speed/NE");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/technique");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/technique/SE");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/technique/SW");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/technique/NW");
                System.IO.Directory.CreateDirectory(u + "color" + i + "/technique/NE");
                Bitmap bSE = drawPixelsSE(parsed, i);
                bSE.Save(u + "/color" + i + "_" + u + "_default_SE" + ".png", ImageFormat.Png);
                Bitmap bSW = drawPixelsSW(parsed, i);
                bSW.Save(u + "/color" + i + "_" + u + "_default_SW" + ".png", ImageFormat.Png);
                Bitmap bNW = drawPixelsNW(parsed, i);
                bNW.Save(u + "/color" + i + "_" + u + "_default_NW" + ".png", ImageFormat.Png);
                Bitmap bNE = drawPixelsNE(parsed, i);
                bNE.Save(u + "/color" + i + "_" + u + "_default_NE" + ".png", ImageFormat.Png);
                for (int j = 0; j < 8; j++)
                {
                    Bitmap power = drawPixelsSE(basepowers[j], i);
                    Graphics g = Graphics.FromImage(power);
                    g.DrawImage(bSE, 0, 0);
                    power.Save(u + "color" + i + "/power/SE/" + j + ".png", ImageFormat.Png);

                    Bitmap speed = drawPixelsSE(basespeeds[j], i);
                    g = Graphics.FromImage(speed);
                    g.DrawImage(bSE, 0, 0);
                    speed.Save(u + "color" + i + "/speed/SE/" + j + ".png", ImageFormat.Png);

                    Bitmap technique = drawPixelsSE(basetechniques[j], i);
                    g = Graphics.FromImage(technique);
                    g.DrawImage(bSE, 0, 0);
                    technique.Save(u + "color" + i + "/technique/SE/" + j + ".png", ImageFormat.Png);
                }
                for (int j = 0; j < 8; j++)
                {
                    Bitmap power = drawPixelsSE(basepowers[j], i);
                    Graphics g = Graphics.FromImage(power);
                    g.DrawImage(bSW, 0, 0);
                    power.Save(u + "color" + i + "/power/SW/" + j + ".png", ImageFormat.Png);

                    Bitmap speed = drawPixelsSE(basespeeds[j], i);
                    g = Graphics.FromImage(speed);
                    g.DrawImage(bSW, 0, 0);
                    speed.Save(u + "color" + i + "/speed/SW/" + j + ".png", ImageFormat.Png);

                    Bitmap technique = drawPixelsSE(basetechniques[j], i);
                    g = Graphics.FromImage(technique);
                    g.DrawImage(bSW, 0, 0);
                    technique.Save(u + "color" + i + "/technique/SW/" + j + ".png", ImageFormat.Png);
                }
                for (int j = 0; j < 8; j++)
                {
                    Bitmap power = drawPixelsSE(basepowers[j], i);
                    Graphics g = Graphics.FromImage(power);
                    g.DrawImage(bNE, 0, 0);
                    power.Save(u + "color" + i + "/power/NE/" + j + ".png", ImageFormat.Png);

                    Bitmap speed = drawPixelsSE(basespeeds[j], i);
                    g = Graphics.FromImage(speed);
                    g.DrawImage(bNE, 0, 0);
                    speed.Save(u + "color" + i + "/speed/NE/" + j + ".png", ImageFormat.Png);

                    Bitmap technique = drawPixelsSE(basetechniques[j], i);
                    g = Graphics.FromImage(technique);
                    g.DrawImage(bNE, 0, 0);
                    technique.Save(u + "color" + i + "/technique/NE/" + j + ".png", ImageFormat.Png);
                }
                for (int j = 0; j < 8; j++)
                {
                    Bitmap power = drawPixelsSE(basepowers[j], i);
                    Graphics g = Graphics.FromImage(power);
                    g.DrawImage(bNW, 0, 0);
                    power.Save(u + "color" + i + "/power/NW/" + j + ".png", ImageFormat.Png);

                    Bitmap speed = drawPixelsSE(basespeeds[j], i);
                    g = Graphics.FromImage(speed);
                    g.DrawImage(bNW, 0, 0);
                    speed.Save(u + "color" + i + "/speed/NW/" + j + ".png", ImageFormat.Png);

                    Bitmap technique = drawPixelsSE(basetechniques[j], i);
                    g = Graphics.FromImage(technique);
                    g.DrawImage(bNW, 0, 0);
                    technique.Save(u + "color" + i + "/technique/NW/" + j + ".png", ImageFormat.Png);
                }
                ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Program Files\ImageMagick-6.8.9-Q16\convert.EXE");
                startInfo.UseShellExecute = false;
                startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/power/SE/* " + u + "/color" + i + "_" + u + "_power_SE.gif";
                Process.Start(startInfo).WaitForExit();
                startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/speed/SE/* " + u + "/color" + i + "_" + u + "_speed_SE.gif";
                Process.Start(startInfo).WaitForExit();
                startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/technique/SE/* " + u + "/color" + i + "_" + u + "_technique_SE.gif";
                Process.Start(startInfo).WaitForExit();

                startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/power/SW/* " + u + "/color" + i + "_" + u + "_power_SW.gif";
                Process.Start(startInfo).WaitForExit();
                startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/speed/SW/* " + u + "/color" + i + "_" + u + "_speed_SW.gif";
                Process.Start(startInfo).WaitForExit();
                startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/technique/SW/* " + u + "/color" + i + "_" + u + "_technique_SW.gif";
                Process.Start(startInfo).WaitForExit();

                startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/power/NW/* " + u + "/color" + i + "_" + u + "_power_NW.gif";
                Process.Start(startInfo).WaitForExit();
                startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/speed/NW/* " + u + "/color" + i + "_" + u + "_speed_NW.gif";
                Process.Start(startInfo).WaitForExit();
                startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/technique/NW/* " + u + "/color" + i + "_" + u + "_technique_NW.gif";
                Process.Start(startInfo).WaitForExit();

                startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/power/NE/* " + u + "/color" + i + "_" + u + "_power_NE.gif";
                Process.Start(startInfo).WaitForExit();
                startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/speed/NE/* " + u + "/color" + i + "_" + u + "_speed_NE.gif";
                Process.Start(startInfo).WaitForExit();
                startInfo.Arguments = "-dispose background -delay 20 -loop 0 " + u + "color" + i + "/technique/NE/* " + u + "/color" + i + "_" + u + "_technique_NE.gif";
                Process.Start(startInfo).WaitForExit();

            }
            bin.Close();
            bases[0].Close();
            bases[1].Close();
            bases[2].Close();
            for (int i = 0; i < 8; i++)
            {
                powers[i].Close();
                speeds[i].Close();
                techniques[i].Close();
            }
        }

        private static Bitmap[] processFloor(string u)
        {
            BinaryReader bin = new BinaryReader(File.Open(u + "_P.vox", FileMode.Open));
            PlusPaletteDraw.MagicaVoxelDataPaletted[] parsed = PlusPaletteDraw.FromMagica(bin);


            System.IO.Directory.CreateDirectory(u);
            Bitmap[] bits = new Bitmap[] {
                PlusPaletteDraw.drawPixelsSE(parsed),
                PlusPaletteDraw.drawPixelsSW(parsed),
                PlusPaletteDraw.drawPixelsNW(parsed),
                PlusPaletteDraw.drawPixelsNE(parsed)
            };
            /*Random r = new Random();
            Bitmap b = new Bitmap(80,40);
            Graphics tiling = Graphics.FromImage(b);
            tiling.DrawImageUnscaled(bits[r.Next(4)], -40, -20);
            tiling.DrawImageUnscaled(bits[r.Next(4)], 40, -20);
            tiling.DrawImageUnscaled(bits[r.Next(4)], 0, 0);
            tiling.DrawImageUnscaled(bits[r.Next(4)], -40, 20);
            tiling.DrawImageUnscaled(bits[r.Next(4)], 40, 20);*/
            bits[0].Save(u + "/" + u + "_default_SE" + ".png", ImageFormat.Png);
            bits[1].Save(u + "/" + u + "_default_SW" + ".png", ImageFormat.Png);
            bits[2].Save(u + "/" + u + "_default_NW" + ".png", ImageFormat.Png);
            bits[3].Save(u + "/" + u + "_default_NE" + ".png", ImageFormat.Png);
            //b.Save(u + "/tiled.png", ImageFormat.Png);

            bin.Close();
            return bits;
        }
        static Bitmap makeTiling()
        {

            Bitmap[] tilings = new Bitmap[20];
            processFloor("Grass").CopyTo(tilings, 0);
            processFloor("Grass").CopyTo(tilings, 4);
            processFloor("Grass").CopyTo(tilings, 8);
            processFloor("Jungle").CopyTo(tilings, 12);
            processFloor("Forest").CopyTo(tilings, 16);


            Random r = new Random();
            Bitmap[] lines = new Bitmap[20];
            int showRoadsOrRivers = r.Next(1);
            if (showRoadsOrRivers == 0)
            {
                processFloor("Road_Cross").CopyTo(lines, 0);
                processFloor("Road_Curve").CopyTo(lines, 4);
                processFloor("Road_End").CopyTo(lines, 8);
                processFloor("Road_Straight").CopyTo(lines, 12);
                processFloor("Road_Tee").CopyTo(lines, 16);
            }
            else
            {
                processFloor("River_Cross").CopyTo(lines, 0);
                processFloor("River_Curve").CopyTo(lines, 4);
                processFloor("River_End").CopyTo(lines, 8);
                processFloor("River_Straight").CopyTo(lines, 12);
                processFloor("River_Tee").CopyTo(lines, 16);
            }
            Bitmap b = new Bitmap(100+8*9, 44*18);
            Graphics tiling = Graphics.FromImage(b);

            Bitmap[,] grid = new Bitmap[10, 19];
            Bitmap[,] midgrid = new Bitmap[9, 18];
            for (int i = 0; i < 10; i++)
            {
                grid[i, 0] = tilings[r.Next(4)];
                grid[i, 18] = tilings[r.Next(4)];
            }
            for (int i = 0; i < 19; i++)
            {
                grid[0, i] = tilings[r.Next(4)];
                grid[9, i] = tilings[r.Next(4)];
            }
            for (int i = 1; i < 9; i++)
            {
                for (int j = 1; j < 18; j++)
                {
                    grid[i, j] = tilings[r.Next(20)];
                }
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 18; j++)
                {
                    midgrid[i, j] = tilings[r.Next(20)];
                }
            }
            int rl = r.Next(2, 4), rw = r.Next(2, 5);
            int randNorthX = r.Next(rw + 1, 8 - rl);
            int randNorthY = r.Next(3, 15 - rl - rw);
            int randWestX = randNorthX - rw, randWestY = randNorthY + rw;
            int randEastX = randNorthX + rl, randEastY = randNorthY + rl;
            int randSouthX = randNorthX + rl - rw, randSouthY = randNorthY + rl + rw;

            for (int w = 0; w < rw; w++)
            {
                grid[randWestX + w, randWestY - w] = null;
                midgrid[randWestX + w, randWestY - w - 1] = null;
                grid[randSouthX + w, randSouthY - w] = null;
                midgrid[randSouthX + w, randSouthY - w - 1] = null;
            }
            grid[randWestX + rw, randWestY - rw] = null;
            grid[randSouthX + rw, randSouthY - rw] = null;
            for (int l = 0; l < rl; l++)
            {
                grid[randWestX + l, randWestY + l] = null;
                midgrid[randWestX + l, randWestY + l] = null;
                grid[randNorthX + l, randNorthY + l] = null;
                midgrid[randNorthX + l, randNorthY + l] = null;
            }
            grid[randWestX + rl, randWestY + rl] = null;
            grid[randNorthX + rl, randNorthY + rl] = null;
            rl = r.Next(2, 5);
            rw = r.Next(2, 4);

            randNorthX = r.Next(rw + 1, 8 - rl);
            randNorthY = r.Next(3, 16 - rl - rw);
            randWestX = randNorthX - rw;
            randWestY = randNorthY + rw;
            randEastX = randNorthX + rl;
            randEastY = randNorthY + rl;
            randSouthX = randNorthX + rl - rw;
            randSouthY = randNorthY + rl + rw;
            for (int w = 0; w < rw; w++)
            {
                grid[randWestX + w, randWestY - w] = null;
                midgrid[randWestX + w, randWestY - w - 1] = null;
                grid[randSouthX + w, randSouthY - w] = null;
                midgrid[randSouthX + w, randSouthY - w - 1] = null;
            }
            grid[randWestX + rw, randWestY - rw] = null;
            grid[randSouthX + rw, randSouthY - rw] = null;
            for (int l = 0; l < rl; l++)
            {
                grid[randWestX + l, randWestY + l] = null;
                midgrid[randWestX + l, randWestY + l] = null;
                grid[randNorthX + l, randNorthY + l] = null;
                midgrid[randNorthX + l, randNorthY + l] = null;
            }
            grid[randWestX + rl, randWestY + rl] = null;
            grid[randNorthX + rl, randNorthY + rl] = null;
            int[,] adjGrid = new int[10, 19], adjMidGrid = new int[9, 18];
            adjGrid.Initialize();
            adjMidGrid.Initialize();

            for (int i = 1; i < 9; i++)
            {
                for (int j = 1; j < 18; j++)
                {
                    /*if (grid[i, j] == null)
                    {
                        adjGrid[i, j] |= 1;
                    }*/
                    if (midgrid[i, j] == null) // southeast
                    {
                        adjGrid[i, j] |= 2;
                    }
                    if (midgrid[i - 1, j] == null) // southwest
                    {
                        adjGrid[i, j] |= 4;
                    }
                    if (midgrid[i - 1, j - 1] == null) // northwest
                    {
                        adjGrid[i, j] |= 8;
                    }
                    if (midgrid[i, j - 1] == null) // northeast
                    {
                        adjGrid[i, j] |= 16;
                    }
                }
            }

            for (int i = 1; i < 8; i++)
            {
                for (int j = 1; j < 18; j++)
                {
                    /*if (midgrid[i, j] == null)
                    {
                        adjMidGrid[i, j] |= 1;
                    }*/
                    if (grid[i + 1, j + 1] == null) // southeast
                    {
                        adjMidGrid[i, j] |= 2;
                    }
                    if (grid[i, j + 1] == null) // southwest
                    {
                        adjMidGrid[i, j] |= 4;
                    }
                    if (grid[i, j] == null) // northwest
                    {
                        adjMidGrid[i, j] |= 8;
                    }
                    if (grid[i + 1, j] == null) // northeast
                    {
                        adjMidGrid[i, j] |= 16;
                    }
                }
            }
            for (int i = 1; i < 9; i++)
            {
                for (int j = 1; j < 18; j++)
                {
                    if (grid[i, j] == null)
                    {
                        switch (adjGrid[i, j])
                        {
                            case 2: grid[i, j] = lines[8]; //se
                                break;
                            case 4: grid[i, j] = lines[9]; //sw
                                break;
                            case 8: grid[i, j] = lines[10]; //nw
                                break;
                            case 16: grid[i, j] = lines[11]; //ne
                                break;
                            case 6: grid[i, j] = lines[4]; //se sw
                                break;
                            case 10: grid[i, j] = lines[12]; //se nw
                                break;
                            case 12: grid[i, j] = lines[5]; //sw nw
                                break;
                            case 18: grid[i, j] = lines[7]; //ne se
                                break;
                            case 20: grid[i, j] = lines[13]; //sw ne
                                break;
                            case 24: grid[i, j] = lines[6]; //nw ne
                                break;
                            case 14: grid[i, j] = lines[17]; //se sw nw
                                break;
                            case 22: grid[i, j] = lines[16]; //ne se sw
                                break;
                            case 26: grid[i, j] = lines[19]; //se ne nw
                                break;
                            case 28: grid[i, j] = lines[18]; //sw nw ne
                                break;
                            case 30: grid[i, j] = lines[0]; //sw nw ne
                                break;
                            default: grid[i, j] = tilings[0];
                                break;
                        }
                    }
                }
            }

            for (int i = 1; i < 8; i++)
            {
                for (int j = 1; j < 18; j++)
                {
                    if (midgrid[i, j] == null)
                    {
                        switch (adjMidGrid[i, j])
                        {
                            case 2: midgrid[i, j] = lines[8]; //se
                                break;
                            case 4: midgrid[i, j] = lines[9]; //sw
                                break;
                            case 8: midgrid[i, j] = lines[10]; //nw
                                break;
                            case 16: midgrid[i, j] = lines[11]; //ne
                                break;
                            case 6: midgrid[i, j] = lines[4]; //se sw
                                break;
                            case 10: midgrid[i, j] = lines[12]; //se nw
                                break;
                            case 12: midgrid[i, j] = lines[5]; //sw nw
                                break;
                            case 18: midgrid[i, j] = lines[7]; //ne se
                                break;
                            case 20: midgrid[i, j] = lines[13]; //sw ne
                                break;
                            case 24: midgrid[i, j] = lines[6]; //nw ne
                                break;
                            case 14: midgrid[i, j] = lines[17]; //se sw nw
                                break;
                            case 22: midgrid[i, j] = lines[16]; //ne se sw
                                break;
                            case 26: midgrid[i, j] = lines[19]; //se ne nw
                                break;
                            case 28: midgrid[i, j] = lines[18]; //sw nw ne
                                break;
                            case 30: midgrid[i, j] = lines[0]; //sw nw ne
                                break;
                            default: midgrid[i, j] = tilings[0];
                                break;
                        }
                    }
                }
            }
            for (int j = 0; j < 18; j++)
            {
                for (int i = 0; i < 10; i++)
                {
                    tiling.DrawImageUnscaled(grid[i, j], (100+8 * i) - 44, (44 * j) - 22 - 7);
                }
                for (int i = 0; i < 9; i++)
                {
                    tiling.DrawImageUnscaled(midgrid[i, j], (100+8 * i), (44 * j)- 7);
                }
            }

            for (int i = 0; i < 10; i++)
            {
                tiling.DrawImageUnscaled(grid[i, 18], (100+8 * i) - 44, (44 * 18) - 22 - 7);
            }
            return b;
        }
        static void Main(string[] args)
        {

//            processUnitOutlined("Swordsman");
            
            processUnitOutlined("Block");
            
            processUnitOutlined("Infantry");

            processUnitOutlined("Tank");
            
            processUnitOutlined("Artillery");
            
            processUnitOutlined("Artillery_S");
            processUnitOutlined("Artillery_P");
            processUnitOutlined("Artillery_T");
            
            processUnitOutlined("Supply");
            processUnitOutlined("Supply_T");
            
            processUnitOutlined("Helicopter");

            processUnitOutlined("Plane");
            
            processUnitOutlined("City");
            processUnitOutlined("Castle");
            processUnitOutlined("Factory");
            processUnitOutlined("Capital");
            
            /*
            processFloor("Grass");
            processFloor("Forest");
            processFloor("Jungle");
            
            processFloor("Road_Straight");
            processFloor("Road_Curve");
            processFloor("Road_Tee");
            processFloor("Road_End");
            processFloor("Road_Cross");

            processFloor("River_Straight");
            processFloor("River_Curve");
            processFloor("River_Tee");
            processFloor("River_End");
            processFloor("River_Cross");
*/            
            //processBases();
//            makeTiling().Save("tiling_large.png", ImageFormat.Png);
            /*
            Bitmap[] randomTilings = new Bitmap[18];
            for (int i = 0; i < 18; i++)
            {
                randomTilings[i] = makeTiling();
            }
            Bitmap b = new Bitmap(720 + 72, 720 + 72);
            Graphics g = Graphics.FromImage(b);
            for (int i = 0; i < 18; i++)
            {
                g.DrawImageUnscaled(randomTilings[i], 264 * (i % 3), 132 * (i / 3));
            }
            b.Save("tiling_large.png", ImageFormat.Png);*/
            /*
            ProcessStartInfo startInfo = new ProcessStartInfo(@"C:\Program Files\ImageMagick-6.8.9-Q16\convert.EXE");
            startInfo.UseShellExecute = false;
            startInfo.Arguments = "tiling_large.png -modulate 110,45 tiling_grayed.png";
            Process.Start(startInfo).WaitForExit();
            */

        }
    }
}
