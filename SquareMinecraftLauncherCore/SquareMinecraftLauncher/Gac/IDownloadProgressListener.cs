using System;
using System.Collections.Generic;
using System.Text;

namespace Gac
{
   public  interface  IDownloadProgressListener
    {
         void OnDownloadSize(long size);
    }
}
