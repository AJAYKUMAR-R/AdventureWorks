using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccess.Utlis.ImageHandling
{
    public class ImageProcessingHelper : IImageProcessingHelper
    {
        public  byte[] GetImageData(DbDataReader reader,int index)
        {
            long length = reader.GetBytes(index, 0, null, 0, 0); // First, get the length of the data
            byte[] buffer = new byte[length];                // Allocate buffer based on the actual length
            long bytesRead = reader.GetBytes(11, 0, buffer, 0, buffer.Length);

            if (bytesRead > 0)
            {
                return buffer;  // `ThumbNailPhoto` is now set to the byte array
            }

            return null;

        }
    }
}
