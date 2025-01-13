using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDataAccess.Utlis.ImageHandling
{
    public interface IImageProcessingHelper
    {
        public byte[] GetImageData(DbDataReader reader, int index);
    }
}
